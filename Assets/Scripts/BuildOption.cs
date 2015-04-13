using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
class BuildingData
{
	[SerializeField]
	private GameObject buildingObject;
	public GameObject BuildingObject
	{
		set
		{
			buildingObject = value;
		}
		get
		{
			return buildingObject;
		}
	}
	private int count;
	public int Count
	{
		set
		{
			count = value;
		}
		get
		{
			return count;
		}
	}
	[SerializeField]
	private int maxCount;
	public int MaxCount
	{
		set
		{
			maxCount = value;
		}
		get
		{
			return maxCount;
		}
	}
}

public class BuildOption : MonoBehaviour 
{
	private GameObject ground = null;
	bool bActive = false;
	[SerializeField]
	private List<BuildingData> buildList = new List<BuildingData>();
	private List<GameObject> managedList = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		if (PlayerBuilding.HasBuilding())
		{
			for (int i=0;i<PlayerBuilding.Count;i++)
			{
				GroundBuildingRelation gbr = PlayerBuilding.GetByIndex(i);

				GameObject searchGround = GameObject.Find(gbr.ground);
				GameObject template = this.GetBuildObject(gbr.building);

				this.ground = searchGround;
				BuildBuilding(template, false);
			}
		}

		this.ground = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0) && bActive)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
			                                     Vector2.zero);
			
			if (hit.transform != null)
			{
				bool b = (hit.transform.gameObject == this.gameObject);

				foreach(GameObject go in managedList)
				{
					if (hit.transform.gameObject == go)
					{
						b = true;

						if (!IsBuildingMax(go.name) && go.GetComponent<Building>().isCostAvailable())
						{
							BuildBuilding(go);
							IncreaseBuilding(go.name);
						}

						break;
					}
				}

				if (!b)
					HideBuild();
			}
			else
			{
				HideBuild();
			}
		}
	}

	void LateUpdate()
	{
		if (ground == null)
		{
			bActive = false;
			GetComponent<SpriteRenderer> ().enabled = false;
		}
		else
		{
			bActive = true;
			GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	public void BuildBuilding(GameObject building)
	{
		BuildBuilding(building, true);
	}

	public void BuildBuilding(GameObject building, bool withCost)
	{
		Vector3 offset = new Vector3 (0, 0.6f, 0);
		
		GameObject go = (GameObject)Instantiate (this.GetBuildObject(building.name), 
		                                         ground.transform.position + offset, 
		                                         Quaternion.identity);
		
		go.name = building.name;
		go.transform.SetParent (this.transform.parent.parent);
		ground.GetComponent<Ground> ().Building = go;

		if (!withCost)
			go.GetComponent<Building> ().NoCost();
		else
			PlayerBuilding.Add(ground.name, go.name);

		go.GetComponent<Building> ().Build (false);
		this.HideBuild ();
	}

	public void ShowBuild(GameObject groundGo)
	{
		this.ground = groundGo;

		Vector3 offset = new Vector3 (-2.15f, 2.1f, 0);

		for (int i=0;i<buildList.Count;i++)
		{
			GameObject go = (GameObject)Instantiate(buildList[i].BuildingObject, 
			                                        (new Vector3((i%6)*0.85f,Mathf.CeilToInt(i/6)*1,0) + offset), 
			                                        Quaternion.identity);
			
			go.name = buildList[i].BuildingObject.name;
			go.transform.localScale = (new Vector3(0.5f,0.5f,1));
			
			go.GetComponent<SpriteRenderer>().sortingOrder = 
				this.GetComponent<SpriteRenderer>().sortingOrder + 1;
			go.GetComponent<Building>().Ground = ground;

			if (IsBuildingMax(go.name))
			{
				go.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.2f);
			}
			else
			{
				if (!go.GetComponent<Building>().isCostAvailable())
					go.GetComponent<SpriteRenderer>().color = new Color(1,0.25f, 0.25f, 0.75f);
			}

			go.transform.SetParent(this.gameObject.transform);

			managedList.Add(go);
		}
	}

	public void HideBuild()
	{
		this.ground = null;

		foreach (GameObject go in managedList)
		{
			Destroy(go);
		}

		managedList.Clear ();
	}

	public bool IsBuildingMax(string name)
	{
		foreach(BuildingData data in buildList)
		{
			if (data.BuildingObject.name == name)
			{
				return (data.Count >= data.MaxCount);
			}
		}

		return true;
	}

	public void IncreaseBuildingMax(string name, int value)
	{
		foreach(BuildingData data in buildList)
		{
			if (data.BuildingObject.name == name)
			{
				data.MaxCount+= value;
			}
		}
	}

	public void IncreaseBuilding(string name)
	{
		foreach(BuildingData data in buildList)
		{
			if (data.BuildingObject.name == name)
			{
				data.Count++;
			}
		}
	}

	public GameObject GetBuildObject(string name)
	{
		foreach(BuildingData data in buildList)
		{
			if (data.BuildingObject.name == name)
			{
				return data.BuildingObject;
			}
		}

		return null;
	}
}
