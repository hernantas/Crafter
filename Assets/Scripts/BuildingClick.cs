using UnityEngine;
using System.Collections;

public class BuildingClick : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
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
				//Debug.Log("Unknow building " + this.name);

				if (this.name == "cityhall3")
				{
					Application.LoadLevel(4);
				}
				else if (this.name == "mine")
				{
					if (PlayerMonster.Count > 0)
					{
						Application.LoadLevel(1);
					}
					else
					{
						Debug.Log("You must buy pet first");
					}
				}
				else if (this.name == "blacksmith")
				{
					Application.LoadLevel(5);
				}
				else if (this.name == "petshop")
				{
					Application.LoadLevel(2);
				}
				else
				{
					Debug.LogWarning("Unknow building " + this.name);
				}
			}
		}
	}
}
