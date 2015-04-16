using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class GridInfo
{
	public GameObject gridObject = null;
	public GameObject collider = null;
	public bool marked = false;
}

public class Duel : MonoBehaviour 
{
	[SerializeField]
	private GameObject gridCollider = null;
	private List<List<GridInfo>> managedGrid = new List<List<GridInfo>>();
	private List<GameObject> movingList = new List<GameObject>();
	private Vector3 gridOffset = new Vector3(-2.14f,0.46f,0);
	private int lastMarkX = -1;
	private int lastMarkY = -1;

	// Use this for initialization
	void Start () 
	{
		for (int row = 0; row < 5; row++)
		{
			managedGrid.Add(new List<GridInfo>());

			for (int col = 0; col < 6; col++)
			{
				GameObject go = (GameObject)Instantiate(gridCollider,
				                                        new Vector3(col*0.86f,-row*0.86f,0)+gridOffset,
				                                        Quaternion.identity);
				GridInfo gi = new GridInfo();
				gi.gridObject = null;
				gi.collider = go;
				gi.marked = false;

				managedGrid[row].Add(gi);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveToFill();
		FillEmpty();
		ChangeGridDisplay();

		if (Input.GetMouseButton(0))
		{
			TestMouseClick();
		}

		if (Input.GetMouseButtonUp(0))
		{
			ClearMarked();
			ClearMark();
		}

		MovingGrid();
	}

	private void TestMouseClick()
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
		                                     Vector2.zero);
		if (hit.transform != null)
		{
			for (int row = 0; row < 5; row++)
			{
				for (int col = 0; col < 6; col++)
				{
					if (hit.transform.gameObject == managedGrid[row][col].collider)
					{
						Mark(row, col);
					}
				}
			}
		}
	}

	public void Mark(int row, int col)
	{
		if (lastMarkX == -1 || 
		    ((lastMarkX != row || lastMarkY != col) &&
		    (Mathf.Abs(lastMarkX-row) == 1 || Mathf.Abs(lastMarkY-col) == 1) &&
	 		(Mathf.Abs(lastMarkX-row) <= 1 && Mathf.Abs(lastMarkY-col) <= 1) &&
		 	!managedGrid[row][col].marked &&
	        managedGrid[lastMarkX][lastMarkY].gridObject.name == managedGrid[row][col].gridObject.name))
		{
			managedGrid[row][col].marked = true;
			lastMarkX = row;
			lastMarkY = col;
		}
	}

	public bool ClearMarked()
	{
		bool b = false;

		for (int row = 0; row < 5; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				if (managedGrid[row][col].marked)
				{
					GameObject go = managedGrid[row][col].gridObject;
					go.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);

					managedGrid[row][col].gridObject = null;
					movingList.Add(go);
				}
			}
		}

		return false;
	}

	public void ClearMark()
	{
		for (int row = 0; row < 5; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				managedGrid[row][col].marked = false;
			}
		}

		lastMarkX = -1;
		lastMarkY = -1;
	}

	public void MoveToFill()
	{
		for (int row = 4; row >= 1; row--)
		{
			for (int col = 0; col < 6; col++)
			{
				if (managedGrid[row][col].gridObject == null && 
				    managedGrid[row-1][col].gridObject != null)
				{
					managedGrid[row][col].gridObject = managedGrid[row-1][col].gridObject;
					managedGrid[row][col].gridObject.transform.position = 
						managedGrid[row][col].collider.transform.position;
					managedGrid[row-1][col].gridObject = null;
				}
			}
		}
	}

	public void FillEmpty()
	{
		for (int row = 0; row < 5; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				if (managedGrid[row][col].gridObject == null)
				{
					GameObject template = 
						Reference.Get.monsterTemplate[Random.Range(0, Reference.Get.monsterTemplate.Count-1)];
					GameObject go = (GameObject)Instantiate(template,
					                                        managedGrid[row][col].collider.transform.position,
					                                        Quaternion.identity);
					go.name = template.name;
					go.transform.localScale = new Vector3(0.5f,0.5f);
					managedGrid[row][col].gridObject = go;
				}
			}
		}
	}

	private void ChangeGridDisplay()
	{
		for (int row = 0; row < 5; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				if (managedGrid[row][col].marked)
					managedGrid[row][col].gridObject.GetComponent<SpriteRenderer>().color = 
						new Color(1,1,1,0.5f);
				else
					managedGrid[row][col].gridObject.GetComponent<SpriteRenderer>().color = 
						new Color(1,1,1,1);
			}
		}
	}

	private void MovingGrid()
	{
		for (int i=0;i<movingList.Count;i++)
		{
			Vector3 targetPos = new Vector3(0,4,0);
			float distance = Vector3.Distance(targetPos, movingList[i].transform.position);

			if (distance > 0.05f)
			{
				Vector3 direction = targetPos - movingList[i].transform.position;
				direction.Normalize();

				movingList[i].transform.Translate(direction * 5 * Time.deltaTime);
			}
			else
			{
				Destroy(movingList[i]);
				movingList.RemoveAt(i);
				i--;
			}
		}
	}
}
