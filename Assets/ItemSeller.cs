using UnityEngine;
using System.Collections;

public class ItemSeller : MonoBehaviour 
{
	private GameObject currentItem = null;
	private int quantity = 0;
	private int maxQuantity = 0;
	private GameObject textItem = null;

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
		if (currentItem != null)
		{
			PlayerCoin.Add((int)(currentItem.GetComponent<Item>().Cost * quantity * 0.1f));
			PlayerItems.Add(currentItem.name, -quantity);
			SetItem(currentItem.name);
		}
	}

	public void ClearItem()
	{
		if (currentItem != null)
		{
			quantity = 0;
			maxQuantity = 0;

			Destroy(currentItem);
			Destroy(textItem);

			currentItem = null;
			textItem = null;
		}

	}

	public void SetItem(string name)
	{
		ClearItem();

		foreach(GameObject item in Reference.Asset.itemTemplate)
		{
			if (item.name == name)
			{
				currentItem = (GameObject) Instantiate(item,
				                                       new Vector3(-0.57f,2.75f),
				                                       Quaternion.identity);
				currentItem.name = name;

				maxQuantity = PlayerItems.Get(name);
				quantity = 10;

				if (maxQuantity < 10)
					quantity = maxQuantity;

				textItem = (GameObject) Instantiate(Reference.Asset.textUtility,
				                                    new Vector3(-0.17f,2.9f),
				                                    Quaternion.identity);
				textItem.GetComponent<TextMesh>().color = new Color(0,0,0);
				textItem.GetComponent<TextMesh>().text = "x" + quantity + 
					" $"+(quantity*item.GetComponent<Item>().Cost * 0.1f);
				textItem.GetComponent<TextMesh>().fontSize = 24;
			}
		}
	}
}
