using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldGenerate : MonoBehaviour 
{
	[SerializeField]
	private GameObject areaDot;
	[SerializeField]
	private GameObject connectorDot;
	[SerializeField]
	private GameObject textTemplate;
	[SerializeField]
	private GameObject colliderTemplate;

	[SerializeField]
	private GameObject starFailedTemplate;
	[SerializeField]
	private GameObject star1Template;
	[SerializeField]
	private GameObject star2Template;
	[SerializeField]
	private GameObject star3Template;

	private List<GameObject> areaList = new List<GameObject>();
	private List<GameObject> colliderList = new List<GameObject>();
	private List<FieldStatus> fieldStats = new List<FieldStatus>();

	// Use this for initialization
	void Start () 
	{
		List<GameObject> templates = Reference.Asset.monsterTemplate;
		Vector3 offset = new Vector3(0,3f);

		bool empty = false;

		for (int i = 0 ; i < templates.Count-2 ; i++)
		{
			GameObject go = null;

			if (i % 2 == 0)
			{
				go = (GameObject) Instantiate(areaDot,
				                              new Vector3(-2,-i,0)+offset,
				                              Quaternion.identity);
			}
			else
			{
				go = (GameObject) Instantiate(areaDot,
				                              new Vector3(2,-i,0)+offset,
				                              Quaternion.identity);
			}

			go.transform.localScale = new Vector3(0.7f,0.7f);
			go.transform.parent = this.transform;

			if (empty)
			{
				go.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}

			FieldStatus fs = PlayerField.Get(i);
			switch(fs)
			{
			case FieldStatus.FIELD_EMPTY:
				if (!empty)
				{
					fs = FieldStatus.FIELD_FAILED;
				}
				empty = true;
				break;
			case FieldStatus.FIELD_FAILED:
				Instantiate(starFailedTemplate, 
				            go.transform.position + new Vector3(0,0.75f),
				            Quaternion.identity);
				break;
			case FieldStatus.FIELD_1STAR:
				Instantiate(star1Template, 
				            go.transform.position + new Vector3(0,0.75f),
				            Quaternion.identity);
				break;
			case FieldStatus.FIELD_2STAR:
				Instantiate(star2Template, 
				            go.transform.position + new Vector3(0,0.75f),
				            Quaternion.identity);
				break;
			case FieldStatus.FIELD_3STAR:
				Instantiate(star3Template, 
				            go.transform.position + new Vector3(0,0.75f),
				            Quaternion.identity);
				break;
			}

			fieldStats.Add(fs);
			areaList.Add(go);

			GameObject collider = (GameObject) Instantiate(colliderTemplate,
			                                               go.transform.position,
			                                               Quaternion.identity);
			collider.transform.parent = this.transform;
			colliderList.Add(collider);
		}

		// Generate path
		for (int i = 0 ; i < areaList.Count-1 ; i++)
		{
			GameObject from = areaList[i];
			GameObject to = areaList[i+1];
			Vector3 direction = to.transform.position - from.transform.position;

			for(int j = 2; j < 11 ; j++)
			{
				float randomVar = 0.2f * Mathf.Sin(Mathf.PI * (j+2)/10);
				Vector3 randomOffset = new Vector3(randomVar,-randomVar);
				GameObject go = (GameObject) Instantiate(connectorDot, 
				            from.transform.position + (direction.normalized*j)/3 + randomOffset,
				            Quaternion.identity);
				go.transform.parent = this.transform;
			}
		}
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
				int index = 0;
				foreach(GameObject coll in colliderList)
				{
					if (hit.transform.gameObject == coll && fieldStats[index] != FieldStatus.FIELD_EMPTY)
					{
						PlayerMonster.IndexEnemy = index;
						Application.LoadLevel(3);
					}

					index++;
				}
			}
		}
	}
}
