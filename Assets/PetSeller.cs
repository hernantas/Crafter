using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetSeller : MonoBehaviour 
{
	private List<GameObject> petList = new List<GameObject>();
	private List<GameObject> colliderList = new List<GameObject>();
	[SerializeField]
	private Sprite goldTemplate;
	[SerializeField]
	private GameObject listCollider;

	// Use this for initialization
	void Start () 
	{
		Vector3 offset = new Vector3(-2.1f, 1.3f);
		for (int i = 0 ; i < Reference.Asset.monsterTemplate.Count ; i++)
		{
			GameObject go = (GameObject) Instantiate(Reference.Asset.monsterTemplate[i],
			                                         new Vector3(0,-i*1.5f,0)+offset,
			                                         Quaternion.identity);

			go.name = Reference.Asset.monsterTemplate[i].name;
			go.transform.localScale = new Vector3(0.3f, 0.3f);
			go.transform.parent = this.transform;

			GameObject collider = (GameObject) Instantiate(listCollider,
			                                              new Vector3(0, go.transform.position.y),
			                                              Quaternion.identity);
			collider.transform.parent = this.transform;
			colliderList.Add(collider);

			Vector3 textOffset = new Vector3(0.73f,0.55f,0);
			GameObject text = (GameObject) Instantiate(Reference.Asset.textUtility,
			                                           new Vector3(0,-i*1.5f,0)+offset+textOffset,
			                                           Quaternion.identity);
			text.GetComponent<TextMesh>().text = go.name;
			text.GetComponent<TextMesh>().color = new Color(0,0,0);

			// Create Icon Gold
			textOffset = new Vector3(0.9f,0,0);
			GameObject iconGold = new GameObject("goldIcon");
			iconGold.AddComponent<SpriteRenderer>();
			iconGold.GetComponent<SpriteRenderer>().sprite = goldTemplate;
			iconGold.transform.position = new Vector3(0,-i*1.5f,0)+offset+textOffset;
			iconGold.transform.localScale = new Vector3(0.4f, 0.4f);
			iconGold.transform.parent = this.transform;

			textOffset = new Vector3(1.10f,0.15f,0);
			GameObject textGold = (GameObject) Instantiate(Reference.Asset.textUtility,
				                                           new Vector3(0,-i*1.5f,0)+offset+textOffset,
				                                           Quaternion.identity);
			textGold.GetComponent<TextMesh>().text = go.GetComponent<Monster>().Cost.ToString();
			textGold.GetComponent<TextMesh>().color = new Color(0,0,0);
			textGold.transform.parent = this.transform;

			petList.Add(go);
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
			                                     Vector2.zero);

			if (hit.transform != null)
			{
				for (int i = 0; i<colliderList.Count;i++)
				{
					if (colliderList[i].gameObject == hit.transform.gameObject)
					{
						if (!Buy(petList[i].name, petList[i].GetComponent<Monster>().Cost))
						{
							Debug.Log("Player doesn't have enough gold");
						}
					}
				}
			}
		}
	}

	private bool Buy(string name, int cost)
	{
		if (PlayerCoin.Get() > cost)
		{
			Debug.Log("Player buy: " + name + " at cost " + cost);
			PlayerMonster.Add(name,0);
			PlayerCoin.Spend(cost);

			return true;
		}

		return false;
	}
}
