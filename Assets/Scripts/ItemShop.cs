using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour 
{
	private List<GameObject> itemList = new List<GameObject>();
	private List<GameObject> colliderList = new List<GameObject>();
	[SerializeField]
	private GameObject colliderTemplate = null; 

	// Use this for initialization
	void Start () 
	{
		DisplayItem();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
			                                     Vector2.zero);

			if (hit.transform != null)
			{
				for (int i=0; i<colliderList.Count; i++)
				{
					if (hit.transform.gameObject == colliderList[i].gameObject)
					{
						if (PlayerCoin.Get() >= itemList[i].GetComponent<Item>().Cost)
						{
							PlayerCoin.Spend(itemList[i].GetComponent<Item>().Cost);
							PlayerItems.Add(itemList[i].name, 1);
						}
					}
				}
			}
		}
	}

	public void DisplayItem()
	{
		Vector3 offset = new Vector3(-2.52f, 1.33f);
		Vector3 textOffset = new Vector3(0.2f,-0.1f) + offset;

		List<GameObject> templates = Reference.Asset.itemTemplate;

		for (int i=0;i<templates.Count;i++)
		{
			GameObject item = (GameObject) Instantiate(templates[i],
			                                           new Vector3(i%6,-i/6,0)+offset,
			                                           Quaternion.identity);
			//item.transform.parent = this.transform;
			item.GetComponent<SpriteRenderer>().sortingOrder = 1;
			item.name = templates[i].name;

			GameObject cost = (GameObject) Instantiate(Reference.Asset.textUtility,
			                                           new Vector3(i%6,-i/6,-2)+textOffset,
			                                           Quaternion.identity);
			cost.GetComponent<TextMesh>().text = (item.GetComponent<Item>().Cost).ToString();
			cost.GetComponent<TextMesh>().fontSize = 22;
			cost.GetComponent<TextMesh>().color = new Color(0,0,0);

			GameObject collider = (GameObject) Instantiate(colliderTemplate,
			                                               new Vector3(i%6,-i/6,0)+offset,
			                                               Quaternion.identity);

			itemList.Add(item);
			colliderList.Add(collider);
		}


	}
}
