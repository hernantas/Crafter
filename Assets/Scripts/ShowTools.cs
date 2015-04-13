using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
class ToolsList
{
	public GameObject tools;
	public int count;
}

public class ShowTools : MonoBehaviour 
{
	private bool isShow = false;
	private bool isMoving = false;
	[SerializeField]
	private List<ToolsList> toolsList = new List<ToolsList>();
	private List<GameObject> managedList = new List<GameObject>();
	private Vector3 originalPos;

	// Use this for initialization
	void Start () 
	{
		originalPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isMoving && Input.GetMouseButton(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
			                                     Vector2.zero);

			if (hit.transform != null && hit.transform.gameObject == this.gameObject)
			{
				if (!isShow)
					Show();
				else
					Hide();
			}
		}
		else
		{
			Moving();
		}
	}

	private void Show()
	{
		isMoving = true;

		int i=0;
		foreach(ToolsList tl in toolsList)
		{
			GameObject go = (GameObject)Instantiate(tl.tools,
			                                        new Vector3(this.transform.position.x+1+i, 
			            										this.transform.position.y),
			                                        Quaternion.identity);

			go.name = tl.tools.name;
			go.transform.parent = this.transform;
			go.GetComponent<SpriteRenderer>().sortingOrder = 
				this.GetComponent<SpriteRenderer>().sortingOrder+1;

			if (PlayerItem.Get(go.name) == 0)
			{
				go.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.2f);
				go.GetComponent<ItemTools>().IsDisplayOnly = true;
			}

			managedList.Add(go);
			i++;
		}

		isShow = true;
	}

	private void Hide()
	{
		isMoving = true;

		foreach(GameObject go in managedList)
		{
			Destroy(go.gameObject);
		}
		managedList.Clear();

		isShow = false;
	}

	private void Moving()
	{
		Vector3 targetPos = originalPos;

		if (isShow)
			targetPos = new Vector3(-2.5f,this.transform.position.y,1);

		Vector3 direction = targetPos - this.transform.position;
		direction.Normalize();
		float distance = Vector3.Distance(targetPos, this.transform.position);

		if (distance > 0.05f)
		{
			this.transform.Translate(direction * Mathf.Clamp(distance*3, 1, 10) * Time.deltaTime);
		}
		else
		{
			isMoving = false;
		}
	}
}
