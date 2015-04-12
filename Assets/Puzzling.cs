using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
class ResourceList
{
	public GameObject go;

	public GameObject req;
	[SerializeField]
	private int curCount;
	public int reqCount;

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
	private float timer = 0;
	private float maxTimer = 0.2f;
	private int maxTurn = 12;
	[SerializeField]
	private List<ResourceList> farmList;
	private List<List<GridInfo>> gridList;
	private List<List<bool>> markList;
	[SerializeField]
	private GameObject blockerTemplate;
	private int latestMarkX = -1;
	private int latestMarkY = -1;

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width/2-100,Screen.height/4,100,32), maxTurn.ToString());
	}

	// Use this for initialization
	void Start () 
	{
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
				                                        Quaternion.identity);

				go.transform.SetParent(this.transform);

				gridList[i][j].collider = go;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		Vector3 offset = new Vector3 (-2.14f,0.5f);

		for(int i=gridList.Count-1;i>=0;i--)
		{
			for (int j=0;j<gridList[i].Count;j++)
			{
				if (gridList[i][j].gameObject == null)
				{
					if (i > 0 && gridList[i-1][j].gameObject != null)
					{
						gridList[i][j].gameObject = gridList[i-1][j].gameObject;
						gridList[i][j].gameObject.transform.position = new Vector3(j*0.85f, i*-0.85f, 1)+offset;
						gridList[i-1][j].gameObject = null;
					}
				}
				else
				{
					if (hit.transform != null && 
					    hit.transform.gameObject == gridList[i][j].collider &&
					    (latestMarkX != i || latestMarkY != j) &&
					    Input.GetMouseButton(0))
					{
						if ((latestMarkX == -1 && latestMarkY == -1) ||
						    (Mathf.RoundToInt(Mathf.Abs(latestMarkX-i)) <= 1 &&
						 	 Mathf.RoundToInt(Mathf.Abs(latestMarkY-j)) <= 1 &&
							 gridList[i][j].gameObject.name == gridList[latestMarkX][latestMarkY].gameObject.name))
						{
							Debug.Log("BBBBB");
							markList[i][j] = true;
							latestMarkX = i;
							latestMarkY = j;
						}
					}
					if (markList[i][j])
					{
						gridList[i][j].gameObject.GetComponent<SpriteRenderer>().color = 
							new Color(1,1,1, 0.5f);
					}
					else
					{
						gridList[i][j].gameObject.GetComponent<SpriteRenderer>().color = 
							new Color(1,1,1, 1);
					}
				}
			}
		}

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

		if (Input.GetMouseButtonUp(0))
		{
			ClearMarked();
			ClearMark();
		}

		timer += Time.deltaTime;
	}

	public bool CheckMarkNearby(int x, int y)
	{
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
					Destroy(gridList[i][j].gameObject);
					gridList[i][j].gameObject = null;
					count ++;
				}
			}
		}

		for (int i=0;i<farmList.Count;i++)
		{
			if (farmList[i].go.name == clearedObject)
			{
				farmList[i].IncreaseCount(count);
			}
		}

		return count;
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
