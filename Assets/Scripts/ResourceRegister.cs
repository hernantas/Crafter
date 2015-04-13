using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceRegister : MonoBehaviour 
{
	[SerializeField]
	private List<GameObject> resources;

	private 
	// Use this for initialization
	void Start () 
	{
		for (int i=0;i<resources.Count;i++)
		{
			PlayerResources.AddType (resources [i].name);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
