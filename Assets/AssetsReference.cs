using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetsReference : MonoBehaviour 
{
	public GUIStyle textStyle = new GUIStyle();
	public List<GameObject> monsterTemplate = new List<GameObject>();

	public AssetsReference()
	{
		if (monsterTemplate.Count > 0 && !PlayerMonster.IsLoaded)
		{
			PlayerMonster.Load();
			PlayerMonster.Add(monsterTemplate[0].name, 1);
		}
	}
}
