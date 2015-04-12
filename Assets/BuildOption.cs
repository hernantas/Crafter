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

public class BuildOption : MonoBehaviour {
	private GameObject ground;
	public GameObject Ground
	{
		set
		{
			ground = value;
		}
	}
	[SerializeField]
	private List<BuildingData> buildList = new List<BuildingData>();
	private List<GameObject> managedList = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0) && this.gameObject.activeSelf)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if (hit.transform != null)
			{
				bool b = (hit.transform.gameObject == this.gameObject);

				foreach(GameObject go in managedList)
				{
					if (hit.transform.gameObject == go)
					{
						b = true;

						if (!IsBuildingMax(go.name))
						{
							go.GetComponent<Building>().BuildBuilding();
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

	public void ShowBuild()
	{
		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<BoxCollider2D> ().enabled = false;

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
				go.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.2f);

			go.transform.SetParent(this.gameObject.transform);

			managedList.Add(go);
		}
	}

	public void HideBuild()
	{
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;

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
