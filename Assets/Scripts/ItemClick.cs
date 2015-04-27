using UnityEngine;
using System.Collections;

public class ItemClick : MonoBehaviour 
{
	private bool duelStage = false;
	public bool DuelStage 
	{ 
		set { duelStage = value; }
		get { return duelStage; }
	}

	public string ItemData { set; get; }

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnMouseUp()
	{

	}
}
