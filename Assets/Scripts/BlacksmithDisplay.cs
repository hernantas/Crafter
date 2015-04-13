using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class BlacksmithGUIInfo
{
	public GameObject ui;
	public int xCost;
}

class BlacksmithItemInfo
{
	public GameObject itm;
	public GameObject collider;
}

public class BlacksmithDisplay : MonoBehaviour 
{
	[SerializeField]
	private GameObject colliderBoxTemplate;
	[SerializeField]
	private List<GameObject> toolsList = new List<GameObject>();
	private List<BlacksmithGUIInfo> uiList = new List<BlacksmithGUIInfo>();
	private List<BlacksmithItemInfo> itemsInfo = new List<BlacksmithItemInfo>();
	private GUIStyle textStyle;

	// Use this for initialization
	void Start () 
	{
		textStyle = new GUIStyle(StaticReference.Get().TextStyle);

		Vector3 offset = new Vector3(-2.45f,2.65f,0);
		int i=0;
		foreach(GameObject go in toolsList)
		{
			ItemTools its = go.GetComponent<ItemTools>();

			if (its != null)
			{
				GameObject newDis = (GameObject)Instantiate(go,
				                                            new Vector3(0,-i,0)+offset,
				                                            Quaternion.identity);
				newDis.GetComponent<ItemTools>().IsDisplayOnly = true;
				newDis.name = go.name;

				if (!newDis.GetComponent<ItemTools>().IsCostAvailable())
					newDis.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.2f);

				GameObject boxColl = (GameObject) Instantiate(colliderBoxTemplate,
				                                              new Vector3(0,-i+offset.y,0),
				                                              Quaternion.identity);

				BlacksmithItemInfo bii = new BlacksmithItemInfo();
				bii.itm = newDis;
				bii.collider = boxColl;

				itemsInfo.Add(bii);

				int j=0;
				foreach(ItemCost ic in its.CreateCost)
				{
					BlacksmithGUIInfo bd = new BlacksmithGUIInfo();
					bd.ui = (GameObject)Instantiate(ic.resource,
					                    new Vector3(newDis.transform.position.x+j+1, 
													newDis.transform.position.y),
					            		Quaternion.identity);
					bd.ui.transform.localScale = new Vector3(0.5f,0.5f);
					bd.xCost = ic.count;
					uiList.Add(bd);
					j++;
				}

				i++;
			}
			else
			{
				Debug.LogWarning("Object is tools");
			}
		}
	}

	void OnGUI()
	{
		foreach(BlacksmithGUIInfo bgui in uiList)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(bgui.ui.transform.position);
			GUI.Label(new Rect(screenPos.x, screenPos.y, 50, 32),
			          "x" + bgui.xCost.ToString(),
			          textStyle);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
		                                     Vector2.zero);

		if (hit.transform != null && Input.GetMouseButtonUp(0))
		{
			foreach(BlacksmithItemInfo bInfo in itemsInfo)
			{
				if (bInfo.collider.gameObject == hit.transform.gameObject &&
				    bInfo.itm.GetComponent<ItemTools>().IsCostAvailable())
				{
					bInfo.itm.GetComponent<ItemTools>().DoCost();
					PlayerItem.Add(bInfo.itm.name);
				}

				if (!bInfo.itm.GetComponent<ItemTools>().IsCostAvailable())
				{
					bInfo.itm.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.2f);
				}
			}
		}
	}
}
