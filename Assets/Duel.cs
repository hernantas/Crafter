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
	private int defeatedEnemy = 0;
	private GameObject enemy = null;
	[SerializeField]
	private float health = 0;
	[SerializeField]
	private GameObject gridCollider = null;
	[SerializeField]
	private GameObject textTemplate = null;
	private List<List<GridInfo>> managedGrid = new List<List<GridInfo>>();
	private List<GameObject> activeMonster = new List<GameObject>();
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

		for (int i=0;i<PlayerMonster.Count;i++)
		{
			GameObject go = PlayerMonster.Spawn(i, new Vector3(i*1.5f-(6/PlayerMonster.Count)/2, 1.95f));
			go.transform.localScale = new Vector3(0.3f, 0.3f);

			activeMonster.Add(go);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (HealthRoutine())
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

	public int ClearMarked()
	{
		int count = 0;
		for (int row = 0; row < 5; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				if (managedGrid[row][col].marked)
				{
					count++;

					GameObject go = managedGrid[row][col].gridObject;
					go.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);

					managedGrid[row][col].gridObject = null;
					movingList.Add(go);
				}
			}
		}

		return count;
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
					GameObject template = activeMonster[Random.Range(0, activeMonster.Count)];
						//Reference.Asset.monsterTemplate[Random.Range(0, Reference.Asset.monsterTemplate.Count-1)];
					GameObject go = (GameObject)Instantiate(template,
					                                        managedGrid[row][col].collider.transform.position,
					                                        Quaternion.identity);
					go.name = template.name;
					go.transform.localScale = new Vector3(0.2f,0.2f);
					go.GetComponent<Monster>().Original = template;
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

				GameObject ori = movingList[i].GetComponent<Monster>().Original;
				ori.GetComponent<Monster>().Exp += 1;
				health -= ori.GetComponent<Monster>().Damage;

				float dirRandom = Random.Range(-0.5f,0.5f);
				Vector3 targetOffsetPos = new Vector3(dirRandom,0);
				GameObject text = (GameObject) Instantiate(textTemplate,
				                                           targetPos+targetOffsetPos,
				                                           Quaternion.identity);
				text.GetComponent<TextMesh>().text = "-"+ori.GetComponent<Monster>().Damage.ToString();
				text.GetComponent<TextMesh>().color = new Color(0.75f,0,0f);
				text.GetComponent<TextMesh>().offsetZ = -1f;
				text.GetComponent<TextMesh>().fontSize = 24;
				text.AddComponent<Lifetime>();
				text.GetComponent<Lifetime>().LifeTime = 1f;
				text.GetComponent<Lifetime>().FloatDirection = new Vector2(dirRandom, 0.5f);

				Destroy(movingList[i]);
				movingList.RemoveAt(i);
				i--;
			}
		}
	}

	private bool HealthRoutine()
	{
		if (enemy == null)
		{
			int indexRandom = PlayerMonster.IndexEnemy+defeatedEnemy;
			enemy = (GameObject) Instantiate(Reference.Asset.monsterTemplate[indexRandom],
			                                 new Vector3(0,4,0),
			                                 Quaternion.identity);
			enemy.transform.localScale = new Vector3 (0.3f, 0.3f);
			health = enemy.GetComponent<Monster>().MaxHealth;
		}
		else
		{
			if (health <= 0)
			{
				if (movingList.Count == 0)
				{
					if (defeatedEnemy < 2)
					{
						Destroy(enemy.gameObject);
						enemy = null;
						defeatedEnemy++;
					}
					else
					{
						// Apply database

						foreach(GameObject go in activeMonster)
						{
							PlayerMonster.Get(go.GetComponent<Monster>().StorageIndex).exp = 
								go.GetComponent<Monster>().Exp;
						}
						
						Application.LoadLevel(0);
					}
				}

				return false;
			}
		}

		return true;
	}
}
