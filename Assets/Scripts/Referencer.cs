using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Referencer : MonoBehaviour {
	[SerializeField]
	private List<GameObject> gameObjectList = new List<GameObject>();
	public GUIStyle TextStyle = new GUIStyle();

	public GameObject getGameObject(string name)
	{
		foreach(GameObject go in gameObjectList)
		{
			if (go.name == name)
				return go;
		}

		return null;
	}
}
