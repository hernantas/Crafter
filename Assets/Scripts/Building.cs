using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuildCost
{
	public GameObject resource;
	public int count;
}

public class Building : MonoBehaviour 
{
	private bool buildMode = true;
	private GameObject ground = null;
	public GameObject Ground
	{
		set
		{
			ground = value;
		}
	}
	private bool noCost = false;
	[SerializeField]
	private List<BuildCost> cost = new List<BuildCost>();
	public List<BuildCost> Cost
	{
		get
		{
			return this.cost;
		}
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!buildMode)
		{
			if (Input.GetMouseButtonDown(0) && this.gameObject.activeSelf)
			{
				RaycastHit2D hit = Physics2D.Raycast(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				
				if (hit.transform != null && hit.transform.gameObject == this.gameObject)
				{
					BuildingClick();
				}
			}
		}
	}

	public void NoCost()
	{
		noCost = true;
	}

	public bool isCostAvailable()
	{
		bool b = true;
		foreach (BuildCost bc in cost)
		{
			if (PlayerResources.Get(bc.resource.name) < bc.count)
			{
				b = false;
				break;
			}
		}

		return b;
	}

	public void Build(bool b)
	{
		buildMode = b;

		if (!b) 
		{
			if (!noCost)
			{
				foreach(BuildCost bc in cost)
				{
					PlayerResources.Add(bc.resource.name, -bc.count);
	            }
			}
			OnBuild();
		}
	}

	public void OnBuild()
	{
		if (this.name == "cityhall")
		{
			GameObject go = GameObject.Find("Build Option");
			BuildOption opt = go.GetComponent<BuildOption>();

			opt.IncreaseBuildingMax("farm", 1);
			opt.IncreaseBuildingMax("blacksmith", 1);
			opt.IncreaseBuildingMax("mine", 1);
		}
	}

	public void BuildingClick()
	{
		if (this.name == "farm")
		{
			Application.LoadLevel(1);
		}
		else if (this.name == "mine")
		{
			Application.LoadLevel(2);
		}
		else if (this.name == "blacksmith")
		{
			Application.LoadLevel(3);
		}
	}
}
