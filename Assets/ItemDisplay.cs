using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDisplay : MonoBehaviour 
{
	private List<GameObject> itemList = new List<GameObject>();
	private List<GameObject> colliderList = new List<GameObject>();
	private GUIStyle textStyle = null;
	[SerializeField]
	private GameObject colliderTemplate = null;

	[SerializeField]
	private GameObject seller = null;

	// Use this for initialization
	void Start () 
	{
		textStyle = Reference.Asset.textStyle;

		int i=0;
		Vector3 offset = new Vector3(-2.04f,1.3f);
		foreach(GameObject item in Reference.Asset.itemTemplate)
		{
			if (item.GetComponent<Item>().Effect != ItemEffect.EFFECT_NONE &&
			    PlayerItems.Get(item.name) > 0)
			{
				int col = i%6;
				int row = i/6;

				GameObject newItem = (GameObject) Instantiate(item,
				                                              new Vector3(col*0.82f,-row*0.82f,0)+offset,
				                                              Quaternion.identity);
				newItem.name = item.name;
				newItem.transform.parent = this.transform;

				GameObject collider = (GameObject) Instantiate(colliderTemplate,
				                                               newItem.transform.position,
				                                               Quaternion.identity);

				colliderList.Add(collider);

				GameObject text = (GameObject) Instantiate(Reference.Asset.textUtility,
				                                           new Vector3(0.22f,-0.20f,0)+
				                                           newItem.transform.position,
				                                           Quaternion.identity);
				text.transform.parent = this.transform;
				text.GetComponent<TextMesh>().text = PlayerItems.Get(item.name).ToString();
				text.GetComponent<TextMesh>().color = new Color(0,0,0);

				itemList.Add(newItem);
				i++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonUp(0))
		{
			ItemClick();
		}
	}

	private void ItemClick()
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
		                                     Vector2.zero);
		
		if (hit.transform != null)
		{
			for (int i=0;i<colliderList.Count;i++)
			{
				if (colliderList[i] == hit.transform.gameObject)
				{
					seller.GetComponent<ItemSeller>().SetItem(itemList[i].name);
				}
			}
		}
		else
		{
			seller.GetComponent<ItemSeller>().ClearItem();
		}
	}
}
