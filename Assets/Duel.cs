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
	private bool gameEnd = false;
	private int defeatedEnemy = 0;
	private GameObject enemyMoving = null;
	private GameObject enemy = null;
	[SerializeField]
	private float health = 0;
	private float maxHealth = 0;
	[SerializeField]
	private float playerHealth = 0;
	private float playerMaxHealth = 0;
	[SerializeField]
	private GameObject gridCollider = null;
	[SerializeField]
	private GameObject textTemplate = null;
	[SerializeField]
	private GameObject messageBox = null;
	[SerializeField]
	private GameObject hpDisplay = null;
	[SerializeField]
	private GameObject blackBar = null;

	[SerializeField]
	private GameObject greenBar = null;
	[SerializeField]
	private GameObject redBar = null;
	[SerializeField]
	private GameObject yellowBar = null;
	private List<GameObject> barManaged = new List<GameObject>();
	private GameObject playerBarManager = null;

	private int damageCounter = 0;
	private float damageBuffer = 0;
	private float damageTimer = 0;

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
				                                        Quaternion.Euler(0,0,45f));
				go.transform.localScale = new Vector3(0.6f, 0.6f);

				GridInfo gi = new GridInfo();
				gi.gridObject = null;
				gi.collider = go;
				gi.marked = false;

				managedGrid[row].Add(gi);
			}
		}

		for (int i=0;i<PlayerMonster.Count && i < 3;i++)
		{
			GameObject go = PlayerMonster.Spawn(i, new Vector3(i*1.5f-(6/3)/2-0.5f, 1.95f));
			go.transform.localScale = new Vector3(0.3f, 0.3f);

			playerHealth += go.GetComponent<Monster>().MaxHealth;

			activeMonster.Add(go);
		}

		playerMaxHealth = playerHealth;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (movingList.Count == 0)
			damageCounter = 0;

		bool runBattle = EnemyApproach() && HealthRoutine();

		if (runBattle)
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

		ShowBlackBar(runBattle);

		ShowHealthBar();
		DamageBufferAction();
		MovingGrid();

		if (gameEnd)
			ShowGameEnd();
	}

	void OnGUI()
	{
		if (!gameEnd)
		{
			foreach(GameObject go in activeMonster)
			{
				Vector3 screenPos = Camera.main.WorldToScreenPoint(go.transform.position);

				GUI.Label(new Rect(screenPos.x, 
				                   Screen.height-screenPos.y-(Screen.height*0.05f), 
				                   65, 25),
				          "Lv." + go.GetComponent<Monster>().Level,
				          Reference.Asset.textStyle);
			}
		}
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
						new Color(0.5f,0.5f,0.5f,0.5f);
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
				damageCounter++;

				GameObject ori = movingList[i].GetComponent<Monster>().Original;
				ori.GetComponent<Monster>().Exp += 1;
				float damage = ori.GetComponent<Monster>().Damage * (1+(damageCounter/5.0f));
				health -= damage;
				damageBuffer += damage;

				Destroy(movingList[i]);
				movingList.RemoveAt(i);
				i--;
			}
		}
	}

	private bool HealthRoutine()
	{
		hpDisplay.GetComponent<TextMesh>().text = ((int)playerHealth).ToString();

		if (gameEnd)
			return false;

		if (enemy == null)
		{
			int indexRandom = PlayerMonster.IndexEnemy+defeatedEnemy;
			enemyMoving = (GameObject) Instantiate(Reference.Asset.monsterTemplate[indexRandom],
			                                 new Vector3(3,4,-5),
			                                 Quaternion.identity);

			enemyMoving.transform.localScale = new Vector3 (0.3f, 0.3f);
			health = enemyMoving.GetComponent<Monster>().MaxHealth * (3+PlayerMonster.IndexEnemy);
			maxHealth = enemyMoving.GetComponent<Monster>().MaxHealth * (3+PlayerMonster.IndexEnemy);
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
						defeatedEnemy++;
						gameEnd = true;
					}
				}

				return false;
			}
		}

		return true;
	}

	private void ShowHealthBar()
	{
		if (enemy == null)
		{
			foreach(GameObject go in barManaged)
				Destroy(go);
			barManaged.Clear();

			return;
		}

		int healthPerc = Mathf.CeilToInt((health/maxHealth)*10);

		if (barManaged.Count != healthPerc)
		{
			foreach(GameObject go in barManaged)
				Destroy(go);
			barManaged.Clear();

			Vector3 pos = enemy.transform.position + new Vector3(1,0);

			for (int i = 0 ; i < healthPerc ; i++)
			{
				GameObject bar = null;

				if (i > 5)
					bar = (GameObject)Instantiate(greenBar,
									              pos + new Vector3(i*0.2f,0),
									              Quaternion.identity);
				else if (i > 2)
					bar = (GameObject)Instantiate(yellowBar,
					                              pos + new Vector3(i*0.2f,0),
					                              Quaternion.identity);
				else
					bar = (GameObject)Instantiate(redBar,
					                              pos + new Vector3(i*0.2f,0),
					                              Quaternion.identity);

				bar.GetComponent<SpriteRenderer>().sortingOrder = 10;
				barManaged.Add(bar);
			}
		}

		if (playerBarManager == null)
		{
			playerBarManager = (GameObject) Instantiate( yellowBar,
			                                            new Vector3 (0,1.161f,0),
			                                            Quaternion.Euler(new Vector3(0,0,90)));
			playerBarManager.GetComponent<SpriteRenderer>().sortingOrder = 10;
		}
		else
		{
			playerBarManager.transform.localScale = new Vector3(1,15*Mathf.Max(playerHealth/playerMaxHealth, 0.0f),1);
		}

		if (enemy != null && !gameEnd && movingList.Count == 0)
		{
			playerHealth -= (enemy.GetComponent<Monster>().Damage * Time.deltaTime);
		}

		if (playerHealth <= 0)
			gameEnd = true;
	}

	private void ShowGameEnd()
	{
		if (!messageBox.activeSelf)
		{
			int bonusMin = (PlayerMonster.IndexEnemy+1) * 9;
			int bonusMax = (PlayerMonster.IndexEnemy+1) * 12;

			if (playerHealth <= 0)
			{
				bonusMin = (PlayerMonster.IndexEnemy+1) * 1;
				bonusMax = (PlayerMonster.IndexEnemy+1) * 3;
			}

			int goldBonus = Random.Range(bonusMin, bonusMax);

			PlayerCoin.Add(goldBonus);
			GameObject gold = messageBox.transform.FindChild("GoldDisplay").gameObject;
			gold.GetComponent<TextMesh>().text = "+" + goldBonus;

			// Apply database
			foreach(GameObject go in activeMonster)
			{
				PlayerMonster.Get(go.GetComponent<Monster>().StorageIndex).exp = 
					go.GetComponent<Monster>().Exp;
			}

			switch(defeatedEnemy)
			{
			case 0:
				if (PlayerField.Get(PlayerMonster.IndexEnemy) != FieldStatus.FIELD_3STAR &&
				    PlayerField.Get(PlayerMonster.IndexEnemy) != FieldStatus.FIELD_2STAR &&
				    PlayerField.Get(PlayerMonster.IndexEnemy) != FieldStatus.FIELD_1STAR)
				PlayerField.Add(PlayerMonster.IndexEnemy, FieldStatus.FIELD_FAILED);
				break;
			case 1:
				if (PlayerField.Get(PlayerMonster.IndexEnemy) != FieldStatus.FIELD_3STAR &&
				    PlayerField.Get(PlayerMonster.IndexEnemy) != FieldStatus.FIELD_2STAR)
					PlayerField.Add(PlayerMonster.IndexEnemy, FieldStatus.FIELD_1STAR);
				break;
			case 2:
				if (PlayerField.Get(PlayerMonster.IndexEnemy) != FieldStatus.FIELD_3STAR)
					PlayerField.Add(PlayerMonster.IndexEnemy, FieldStatus.FIELD_2STAR);
				break;
			case 3:
				PlayerField.Add(PlayerMonster.IndexEnemy, FieldStatus.FIELD_3STAR);
				break;
			}

			messageBox.SetActive(true);
		}

		GameObject title = messageBox.transform.FindChild("TextHelper").gameObject;

		if (playerHealth > 0)
		{
			title.GetComponent<TextMesh>().text = "Victory";
			title.GetComponent<TextMesh>().color = new Color(255,226,0);
		}
		else
		{
			title.GetComponent<TextMesh>().text = "Defeated";
			title.GetComponent<TextMesh>().color = new Color(233,0,0);
		}
	}

	private bool EnemyApproach()
	{
		if (enemyMoving != null)
		{
			ClearMark();
			Vector3 targetPos = new Vector3(0,4,0);
			float distance = Vector3.Distance(enemyMoving.transform.position, targetPos);

			if (distance > 0.05f)
			{
				Vector3 direction = targetPos - enemyMoving.transform.position;
				enemyMoving.transform.Translate(direction.normalized * 5 * Time.deltaTime);
			}
			else
			{
				enemy = enemyMoving;
				enemyMoving = null;
			}

			return false;
		}

		return true;
	}

	public void ShowBlackBar(bool b)
	{
		blackBar.SetActive(!b);
	}

	private void DamageBufferAction()
	{
		if (damageTimer > 0.1f)
		{
			if (damageBuffer > 0)
			{
				Vector3 targetPos = new Vector3(0,4,0);
				float dirRandom = Random.Range(-0.5f,0.5f);
				Vector3 targetOffsetPos = new Vector3(dirRandom,0);
				GameObject text = (GameObject) Instantiate(textTemplate,
				                                           targetPos+targetOffsetPos,
				                                           Quaternion.identity);
				text.GetComponent<TextMesh>().text = "-" + ((int)damageBuffer).ToString();
				text.GetComponent<TextMesh>().color = new Color(0.75f,0,0f);
				text.GetComponent<TextMesh>().offsetZ = -1f;
				text.GetComponent<TextMesh>().fontSize = 24;
				text.AddComponent<Lifetime>();
				text.GetComponent<Lifetime>().LifeTime = 1.2f;
				text.GetComponent<Lifetime>().FloatDirection = new Vector2(dirRandom, 0.5f);

				damageBuffer = 0;
			}

			damageTimer = 0;
		}

		damageTimer += Time.deltaTime;
	}
}
