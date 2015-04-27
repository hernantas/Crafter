using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetManager : MonoBehaviour 
{
	[SerializeField]
	private Sprite usedTemplate = null;
	[SerializeField]
	private Sprite healthTemplate = null;
	[SerializeField]
	private Sprite damageTemplate = null;
	[SerializeField]
	private Sprite goldButton = null;
	[SerializeField]
	private Sprite sellButton = null;
	[SerializeField]
	private GameObject prevButton = null;
	[SerializeField]
	private GameObject nextButton = null;
	[SerializeField]
	private GameObject listCollider = null;
	private List<GameObject> colliderList = new List<GameObject>();
	private List<GameObject> petList = new List<GameObject>();

	private int currentPage = 0;
	private int maxPage = 0;

	// Use this for initialization
	void Start () 
	{
		RefreshDisplay();
		RefreshButtonPrevNext();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
			                                     Vector2.zero);
			bool found = false;
			if (hit.transform != null)
			{
				if (hit.transform.gameObject == prevButton)
				{
					Previous();
				}
				else if (hit.transform.gameObject == nextButton)
				{
					Next();
				}
				else
				{
					for (int i=1;i<colliderList.Count;i++)
					{
						if (hit.transform.gameObject == colliderList[i].gameObject)
						{
							found = true;
							PlayerMonster.Swap(i-1,i);
							break;
						}
					}
				}
			}

			if (found)
				RefreshDisplay();
		}
	}

	public void RefreshDisplay()
	{
		int oldPage = currentPage;
		currentPage = 0;
		for (int i=0;i<this.transform.childCount;i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}
		petList.Clear();
		colliderList.Clear();

		Vector3 offset = new Vector3(-1.38f, 0.77f);

		maxPage = Mathf.CeilToInt(PlayerMonster.Count/3.0f);
		for (int i = 0 ; i < PlayerMonster.Count ; i++)
		{
			int page = i/3;
			offset = new Vector3(-1.38f + (page * 6), 0.77f);
			GameObject go = PlayerMonster.Spawn(i, new Vector3(0,-i%3*2.20f,0)+offset);
			
			go.GetComponent<SpriteRenderer>().sortingOrder = 1;
			go.name = Reference.Asset.monsterTemplate[i].name;
			go.transform.localScale = new Vector3(0.3f, 0.3f);
			go.transform.parent = this.transform;
			
			GameObject collider = (GameObject) Instantiate(listCollider,
			                                               new Vector3(page*6, go.transform.position.y),
			                                               Quaternion.identity);
			collider.transform.parent = this.transform;
			colliderList.Add(collider);

			if (i<3)
			{
				GameObject usedText = new GameObject("usedText");
				usedText.AddComponent<SpriteRenderer>();
				usedText.GetComponent<SpriteRenderer>().sprite = usedTemplate;
				usedText.GetComponent<SpriteRenderer>().sortingOrder = 2;
				usedText.transform.position = new Vector3(-1,1+(-i%3*2.20f),0) + offset;
				usedText.transform.parent = this.transform;
			}
			
			Vector3 textOffset = new Vector3(1.25f,0.55f,0);
			GameObject text = (GameObject) Instantiate(Reference.Asset.textUtility,
			                                           new Vector3(0,-i%3*2.20f,-1)+offset+textOffset,
			                                           Quaternion.identity);
			text.GetComponent<TextMesh>().text = go.name + " Lv."+go.GetComponent<Monster>().Level;
			text.GetComponent<TextMesh>().color = new Color(0,0,0);
			text.transform.parent = this.transform;

			textOffset = new Vector3(1.3f,0f,0);
			ShowText(healthTemplate, 
			         go.GetComponent<Monster>().MaxHealth + 
			         " (+" + go.GetComponent<Monster>().MaxHealthInc + ")", 
			         new Vector3(0,-i%3*2.20f,0)+offset+textOffset);
			
			textOffset = new Vector3(1.3f,-0.55f,0);
			ShowText(damageTemplate, 
			         go.GetComponent<Monster>().Damage + 
			         " (+" + go.GetComponent<Monster>().DamageInc + ")", 
			         new Vector3(0,-i%3*2.20f,0)+offset+textOffset);

			textOffset = new Vector3(2.58f,-0.7f,0);
			ShowText(goldButton, 
			         (go.GetComponent<Monster>().Level * go.GetComponent<Monster>().Cost / 10).ToString(), 
			         new Vector3(0,-i%3*2.20f,0)+offset+textOffset);

			textOffset = new Vector3(3.6f,-0.7f,0);
			ShowText(sellButton, 
			         "", 
			         new Vector3(0,-i%3*2.20f,0)+offset+textOffset);
			
			petList.Add(go);
		}

		for (int i=0;i<oldPage;i++)
		{
			Next();
		}

		RefreshButtonPrevNext();
	}

	public void ShowText(Sprite icon, string text, Vector3 pos)
	{
		GameObject iconGold = null;
		if (icon != null)
		{
			iconGold = new GameObject("Text:" + text);
			iconGold.AddComponent<SpriteRenderer>();
			iconGold.GetComponent<SpriteRenderer>().sprite = icon;
			iconGold.GetComponent<SpriteRenderer>().sortingOrder = 1;
			iconGold.transform.position = pos;
			iconGold.transform.localScale = new Vector3(0.4f, 0.4f);
			iconGold.transform.parent = this.transform;
		}

		if (text != "")
		{
			pos += new Vector3(0.15f, 0.15f);
			GameObject textGold = (GameObject) Instantiate(Reference.Asset.textUtility,
			                                               pos,
			                                               Quaternion.identity);
			textGold.GetComponent<TextMesh>().text = text;
			textGold.GetComponent<TextMesh>().color = new Color(0,0,0);
			textGold.transform.parent = this.transform;
			textGold.transform.position = new Vector3(textGold.transform.position.x,
			                                          textGold.transform.position.y,
			                                          -5);
		}
		else
		{
			if (iconGold != null)
			{
				iconGold.transform.localScale = new Vector3(0.65f,0.65f,1);
			}
		}
	}

	private void Previous()
	{
		if (currentPage != 0)
		{
			this.transform.Translate(new Vector3(6,0,0));
			currentPage--;
		}

		RefreshButtonPrevNext();
	}

	private void Next()
	{
		if (currentPage != maxPage-1)
		{
			this.transform.Translate(new Vector3(-6,0,0));
			currentPage++;
		}

		RefreshButtonPrevNext();
	}

	private void RefreshButtonPrevNext()
	{
		if (currentPage == 0)
			prevButton.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.5f);
		else
			prevButton.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 1);
		
		if (currentPage == maxPage-1)
			nextButton.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.5f);
		else
			nextButton.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 1);
	}
}
