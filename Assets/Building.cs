using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour 
{
	private bool buildMode = true;
	private GameObject ground;
	public GameObject Ground
	{
		set
		{
			ground = value;
		}
	}

	private Camera mainCamera;

	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!buildMode)
		{
			if (Input.GetMouseButtonDown(0) && this.gameObject.activeSelf)
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				
				if (hit.transform != null && hit.transform.gameObject == this.gameObject)
				{
					BuildingClick();
				}
			}
		}
	}

	public void Build(bool b)
	{
		buildMode = b;

		if (!b) 
		{
			OnBuild();
		}
	}

	public void BuildBuilding()
	{
		Vector3 offset = new Vector3 (0, 0.6f, 0);

		GameObject bo = GameObject.Find("Build Option");
		BuildOption bOpt = bo.GetComponent<BuildOption> ();

		GameObject go = (GameObject)Instantiate (bOpt.GetBuildObject(this.name), 
		                                         ground.transform.position + offset, 
		                                         Quaternion.identity);

		go.name = this.name;
		go.transform.SetParent (this.transform.parent.parent);
		ground.GetComponent<Ground> ().Building = go;

		Building bu = go.GetComponent<Building> ();
		bu.Build (false);
		bOpt.HideBuild ();
	}

	public void OnBuild()
	{
		if (this.name == "cityhall")
		{

			GameObject go = GameObject.Find("Build Option");
			BuildOption opt = go.GetComponent<BuildOption>();

			opt.IncreaseBuildingMax("farm", 1);
			opt.IncreaseBuildingMax("blacksmith", 1);
		}
	}

	public void BuildingClick()
	{
		if (this.name == "farm")
		{
			GameObject home = GameObject.Find("Home");
			GameObject game = GameObject.Find("Game");

			for (int i=0;i<home.transform.childCount;i++)
			{
				home.transform.GetChild(i).gameObject.SetActive(false);
			}

			for (int i=0;i<game.transform.childCount;i++)
			{
				game.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		else if (this.name == "blacksmith")
		{

		}
	}
}
