using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		DisplayItem();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void DisplayItem()
	{
		Vector3 offset = new Vector3(-2.52f, 1.33f);

		List<GameObject> templates = Reference.Asset.itemTemplate;
		Debug.Log(templates.Count);
		for (int i=0;i<templates.Count;i++)
		{
			GameObject item = (GameObject) Instantiate(templates[i],
			                                           new Vector3(i%6,-i/6,0)+offset,
			                                           Quaternion.identity);
			//item.transform.parent = this.transform;
			item.GetComponent<SpriteRenderer>().sortingOrder = 1;
		}
	}
}
