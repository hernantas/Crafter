using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetsReference : MonoBehaviour 
{
	public GUIStyle textStyle = new GUIStyle();
	public List<GameObject> monsterTemplate = new List<GameObject>();
	public List<GameObject> itemTemplate = new List<GameObject>();
	public GameObject textUtility;

	void Start()
	{
		PlayerSave.Auto();

		if (monsterTemplate.Count > 0 && !PlayerMonster.IsLoaded)
		{
			PlayerMonster.Load();
			PlayerMonster.Add(monsterTemplate[0].name, 0);
			PlayerMonster.Add(monsterTemplate[1].name, 0);
			PlayerMonster.Add(monsterTemplate[2].name, 0);
		}
	}

	public GameObject GetMonster(string name)
	{
		foreach(GameObject go in monsterTemplate)
		{
			if (go.name == name)
			{
				return go;
			}
		}

		return null;
	}
}
