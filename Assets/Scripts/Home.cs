using UnityEngine;
using System.Collections;

public class Home : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
			                                     Vector2.zero);
			
			if (hit.transform != null && hit.transform.gameObject == this.gameObject)
			{
				Application.LoadLevel(0);
			}
		}
	}
}
