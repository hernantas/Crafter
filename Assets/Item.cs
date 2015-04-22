using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	[SerializeField]
	private int cost = 0;
	public int Cost
	{
		set { cost = value; }
		get { return cost; }
	}



	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
