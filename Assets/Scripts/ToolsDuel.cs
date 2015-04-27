using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolsDuel : MonoBehaviour 
{
	[SerializeField]
	private GameObject colliderTemplate = null;

	private bool showing = false;

	private List<GameObject> childList = new List<GameObject>();
	private List<GameObject> colliderList = new List<GameObject>();
	private List<GameObject> itemList = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		Vector3 offset = new Vector3(-2.04f, 0.27f);
		
		for (int row = 0; row < 6; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				GameObject collider = (GameObject) Instantiate(colliderTemplate,
				                                               new Vector3(col*0.82f,-row*0.82f,-10)+offset,
				                                               Quaternion.identity);
				collider.transform.localScale = new Vector3(0.73f, 0.73f);
				collider.transform.parent = this.transform;
				colliderList.Add(collider);
			}
		}

		for (int i=0;i<this.transform.childCount;i++)
		{
			childList.Add(this.transform.GetChild(i).gameObject);
		}

		Hide();
	}

	void OnMouseUp()
	{
		for (int i=0;i<childList.Count;i++)
		{
			childList[i].SetActive(true);
		}

		showing = true;
		RefreshItem();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonUp(0))
		{
			ItemClickHander();
		}
	}

	public void Hide()
	{ 
		for (int i=0;i<childList.Count;i++)
		{
			childList[i].SetActive(false);
		}

		showing = false;
		RefreshItem();
	}

	private void RefreshItem()
	{
		if (showing)
		{
			Vector3 offset = new Vector3(-2.04f, 0.27f);
			int i = 0;

			List<GameObject> templates = Reference.Asset.itemTemplate;

			for (int j=0;j<templates.Count;j++)
			{
				if (templates[j].GetComponent<Item>().Effect != ItemEffect.EFFECT_NONE &&
				    PlayerItems.Get(templates[j].name) > 0)
				{
					int col = i%6;
					int row = i/6;
					GameObject item = (GameObject) Instantiate(templates[j],
				                                               new Vector3(col*0.82f,-row*0.82f,0)+offset,
				                                               Quaternion.identity);
					item.name = templates[i].name;
					item.transform.localScale = new Vector3(0.73f, 0.73f);
					item.transform.parent = this.transform;
					item.GetComponent<SpriteRenderer>().sortingOrder = 2;

					itemList.Add(item);

					i++;
				}
			}
		}
		else
		{
			for (int i=0;i<itemList.Count;i++)
			{
				Destroy(itemList[i].gameObject);
			}
			itemList.Clear();
		}
	}

	private void ItemClickHander()
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
		                                     Vector2.zero);
		
		if (hit.transform != null)
		{
			for (int i=0;i<colliderList.Count;i++)
			{
				if (colliderList[i] == hit.transform.gameObject)
				{
					GetComponent<BuffEffect>().AddBuff(itemList[i]);
					PlayerItems.Add(itemList[i].name, -1);
					itemList.RemoveAt(i);
					this.Hide();
					break;
				}
			}
		}

	}

	void OnGUI()
	{

	}
}
