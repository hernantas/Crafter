using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
class ResourceList
{
	public GameObject go = null;

	public GameObject req = null;
	[SerializeField]
	private int curCount = 0;
	public int reqCount = 0;

	public void IncreaseCount(int value)
	{
		curCount+=value;
	}

	public bool IsMatch(int req)
	{
		return (curCount >= req);
	}

	public void ClearCount(int req)
	{
		curCount -= req;

		if (curCount < 0)
		{
			Debug.LogWarning("Count is less than required");
		}
	}
}

class GridInfo
{
	public GameObject gameObject;
	public GameObject collider;
}

public class Puzzling : MonoBehaviour 
{
	[SerializeField]
	private int maxTurn = 12;
	[SerializeField]
	private List<ResourceList> farmList;
	private List<List<GridInfo>> gridList;
	private List<List<bool>> markList;
	[SerializeField]
	private GameObject blockerTemplate;
	private int latestMarkX = -1;
	private int latestMarkY = -1;
	private List<GameObject> displayCollected;
	private List<int> totalCollected = new List<int>();
	private List<GameObject> movingCollected = new List<GameObject>();
	private GUIStyle textScore;
	private GUIStyle textCollected;

	void OnGUI()
	{
		for (int i=0;i<totalCollected.Count;i++)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(displayCollected[i].transform.position);
			//i*74+35
			GUI.Label(new Rect(screenPos.x,16,50,32), 
			          totalCollected[i].ToString(), 
			          textCollected);
		}

		GUI.Label(new Rect(Screen.width/2-23,Screen.height/4,50,32), 
		          maxTurn.ToString(), 
		          textScore);
	}

	// Use this for initialization
	void Start () 
	{
		textScore = new GUIStyle(StaticReference.Get().TextStyle);
		textScore.fontSize = 32;
		textScore.fontStyle = FontStyle.Bold;
		textCollected = new GUIStyle(StaticReference.Get().TextStyle);
		textCollected.normal.textColor = new Color(0,0,0);

		gridList = new List<List<GridInfo>> (5);
		markList = new List<List<bool>> (5);

		for (int i=0;i<5;i++)
		{
			gridList.Add(new List<GridInfo>(6));
			markList.Add(new List<bool>(6));
		}
		for(int i=0;i<gridList.Count;i++)
		{
			for (int j=0;j<6;j++)
			{
				gridList[i].Add(new GridInfo());
				markList[i].Add(false);
			}
		}

		Vector3 offset = new Vector3 (-2.14f,0.5f);
		for(int i=0;i<gridList.Count;i++)
		{
			for (int j=0;j<gridList[i].Count;j++)
			{
				GameObject go = (GameObject)Instantiate(blockerTemplate,
				                                        new Vector3(j*0.85f, i*-0.85f, 1)+offset,
				                                        Quaternion.Euler(new Vector3(0,0,-45)));

				go.transform.SetParent(this.transform);

				gridList[i][j].collider = go;
			}
		}

		// Create collected
		Vector3 dOffset = new Vector3(-2.3f,4.7f,0);
		displayCollected = new List<GameObject>();
		for (int i=0;i<farmList.Count;i++)
		{
			GameObject go = (GameObject)Instantiate(farmList[i].go,
			                                        new Vector3(i*1.2f,0,0)+dOffset,
			                                        Quaternion.identity);
			go.transform.parent = this.transform;
			go.name = farmList[i].go.name;
			displayCollected.Add(go);
			totalCollected.Add(0);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (maxTurn > 0)
		{
			RaycastHit2D hit = Physics2D.Raycast(
				Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			for(int i=gridList.Count-1;i>=0;i--)
			{
				for (int j=0;j<gridList[i].Count;j++)
				{
					if (gridList[i][j].gameObject == null)
					{
						if (i > 0 && gridList[i-1][j].gameObject != null)
						{
							PushToBottom(i, j);
						}
					}
					else
					{
						if ((hit.transform != null && 
						    hit.transform.gameObject == gridList[i][j].collider &&
						    (latestMarkX != i || latestMarkY != j) && Input.GetMouseButton(0)) 
						    && 
						    ((latestMarkX == -1 && latestMarkY == -1) ||
						    (!markList[i][j] && Mathf.RoundToInt(Mathf.Abs(latestMarkX-i)) <= 1 &&
						 	Mathf.RoundToInt(Mathf.Abs(latestMarkY-j)) <= 1 &&
							gridList[i][j].gameObject.name == gridList[latestMarkX][latestMarkY].gameObject.name)))
						{
							markList[i][j] = true;
							latestMarkX = i;
							latestMarkY = j;
						}

						if (markList[i][j])
						{
							gridList[i][j].gameObject.GetComponent<SpriteRenderer>().color = 
								new Color(0.5f,0.5f,0.5f, 1);
						}
						else
						{
							gridList[i][j].gameObject.GetComponent<SpriteRenderer>().color = 
								new Color(1,1,1, 1);
						}
					}
				}
			}

			FillEmptyGrid();

			if (Input.GetMouseButtonUp(0))
			{
				if (ClearMarked() > 0)
					maxTurn --;
				ClearMark();
			}
		}
		else if (movingCollected.Count == 0)
		{
			Application.LoadLevel(0);
		}

		MovingCollected();
	}

	private void PushToBottom(int i, int j)
	{
		Vector3 offset = new Vector3 (-2.14f,0.5f);
		gridList[i][j].gameObject = gridList[i-1][j].gameObject;
		gridList[i][j].gameObject.transform.position = new Vector3(j*0.85f, i*-0.85f, 1)+offset;
		gridList[i-1][j].gameObject = null;
	}

	private void FillEmptyGrid()
	{
		Vector3 offset = new Vector3 (-2.14f,0.5f);

		for (int j=0;j<gridList[0].Count;j++)
		{
			if (gridList[0][j].gameObject == null)
			{
				GameObject template = GetGameObject();
				GameObject go = (GameObject)Instantiate(template,
				                                        new Vector3(j*0.85f, 0*-0.85f, 1)+offset, 
				                                        Quaternion.identity);
				go.name = template.name;
				
				go.GetComponent<SpriteRenderer>().sortingOrder = 
					this.GetComponent<SpriteRenderer>().sortingOrder+1;
				go.transform.parent = this.transform;
				gridList[0][j].gameObject = go;
			}
		}
	}

	public bool MarkAll(string resourceName)
	{
		for (int i=0;i<gridList.Count;i++)
		{
			for (int j=0;j<gridList[i].Count;j++)
			{
				if (gridList[i][j].gameObject != null && 
				    gridList[i][j].gameObject.name == resourceName)
				{
					markList[i][j] = true;
				}
			}
		}

		int count = ClearMarked();
		ClearMark();

		if (count > 0)
		{
			return true;
		}
		return false;
	}

	public int ClearMarked()
	{
		int count = 0;
		string clearedObject = "";
		for (int i=0;i<gridList.Count;i++)
		{
			for (int j=0;j<gridList[i].Count;j++)
			{
				if (markList[i][j])
				{
					clearedObject = gridList[i][j].gameObject.name;
					//Destroy(gridList[i][j].gameObject);
					movingCollected.Add(gridList[i][j].gameObject);
					gridList[i][j].gameObject = null;
					count ++;
				}
			}
		}

		for (int i=0;i<farmList.Count;i++)
		{
			if (farmList[i].go.name == clearedObject)
			{
				PlayerResources.Add(farmList[i].go.name, count+(count/6));
				farmList[i].IncreaseCount(count);
			}
		}

		return count;
	}

	private Vector3 GetDisplayResourcePos(string name)
	{
		foreach(GameObject go in displayCollected)
		{
			if (go.name == name)
				return go.transform.position;
		}

		return new Vector3(0,0,0);
	}

	private int GetDisplayResourceIndex(string name)
	{
		for(int i=0;i<displayCollected.Count;i++)
		{
			if (displayCollected[i].name == name)
				return i;
		}

		return -1;
	}

	public void MovingCollected()
	{
		for(int i=0;i<movingCollected.Count;i++)
		{
			movingCollected[i].GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
			Vector3 pos = GetDisplayResourcePos(movingCollected[i].name);
			Vector3 direction = pos - movingCollected[i].transform.position;
			float distance = Vector3.Distance(pos, movingCollected[i].transform.position);

			if (distance > 0.05f)
			{
				movingCollected[i].transform.Translate(direction.normalized*5*Time.deltaTime);
			}
			else
			{
				totalCollected[GetDisplayResourceIndex(movingCollected[i].name)]++;

				Destroy(movingCollected[i]);
				movingCollected.RemoveAt(i);
				i--;
			}
		}
	}

	public void ClearMark()
	{
		for (int i=0;i<gridList.Count;i++)
		{
			for (int j=0;j<gridList[i].Count;j++)
			{
				markList[i][j] = false;
			}
		}

		latestMarkX = -1;
		latestMarkY = -1;
	}

	public int GetRequirement(string name)
	{
		foreach(ResourceList rl in farmList)
		{
			if (rl.go.name == name)
			{
				return rl.reqCount;
			}
		}

		return 0;
	}

	public bool CheckRequirement(string name, int req)
	{
		foreach(ResourceList rl in farmList)
		{
			if (rl.go.name == name)
			{
				return rl.IsMatch(req);
			}
		}

		return false;
	}

	public void DecreaseCountReq(string name, int val)
	{
		foreach(ResourceList rl in farmList)
		{
			if (rl.go.name == name)
			{
				rl.IncreaseCount(val);
			}
		}
	}

	private GameObject GetGameObject()
	{
		int firstZero = -1;
		int lastZero = -1;

		for (int i=farmList.Count-1;i>=0;i--)
		{
			if (farmList[i].reqCount == 0)
			{
				if (lastZero < 0)
					lastZero = i;
				else
					firstZero = i;
			}
			else if (CheckRequirement(farmList[i].req.name, farmList[i].reqCount))
			{
				//Debug.Log("Requirement Match, return " + farmList[i].req.name);
				DecreaseCountReq(farmList[i].req.name, -farmList[i].reqCount);
				return farmList[i].go;
			}
		}

		if (firstZero < 0)
			firstZero = 0;
		if (lastZero < 0)
			lastZero = 0;

		int rand = Random.Range (firstZero, lastZero+1);

		return farmList [rand].go;
	}
}
