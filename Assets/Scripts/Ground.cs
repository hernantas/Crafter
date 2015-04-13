using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
	private Camera mainCamera;
	public GameObject Building 
	{
		set;
		get;
	}

	// Use this for initialization
	void Start () 
	{
		Building = null;
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.transform != null && 
				hit.transform.gameObject == this.gameObject)
			{
				GroundClickEvent();
			}
		}
	}

	void GroundClickEvent()
	{
		if (Building == null)
		{
			GameObject go = GameObject.Find("Build Option");
			BuildOption buildOption = go.GetComponent<BuildOption>();
			buildOption.ShowBuild(this.gameObject);
		}
	}
}
