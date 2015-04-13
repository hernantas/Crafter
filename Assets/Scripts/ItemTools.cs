using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemCost
{
	public GameObject resource = null;
	public int count = 0;
}

public class ItemTools : MonoBehaviour 
{
	private bool displayOnly = false;
	public bool IsDisplayOnly
	{
		set
		{
			displayOnly = value;
		}
		get
		{
			return displayOnly;
		}
	}

	//private GameObject resourceDestroy = null;
	[SerializeField]
	private GameObject resourceCollect = null;
	[SerializeField]
	private List<ItemCost> createCost = new List<ItemCost>();
	public List<ItemCost> CreateCost
	{
		get
		{
			return createCost;
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Only usable if not for display purpose
		if (!displayOnly && resourceCollect != null && Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
			                                         Vector2.zero);
			    
		    if (hit.transform != null && hit.transform.gameObject == this.gameObject)
		    {
				GameObject puzzle = GameObject.Find("Background-Puzzle");

				if (puzzle != null)
				{
					Puzzling puzz = puzzle.GetComponent<Puzzling>();
					if (puzz.MarkAll(resourceCollect.name))
					{
						PlayerItem.Add(this.gameObject.name, -1);
					}
				}
			}
		}
	}

	public bool IsCostAvailable()
	{
		bool b = true;

		foreach(ItemCost cost in createCost)
		{
			if (PlayerResources.Get(cost.resource.name) < cost.count)
			{
				b = false;
				break;
			}
		}

		return b;
	}

	public void DoCost()
	{
		foreach(ItemCost cost in createCost)
		{
			PlayerResources.Add(cost.resource.name, -cost.count);
		}
	}
}
