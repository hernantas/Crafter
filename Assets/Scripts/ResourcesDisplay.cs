using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesDisplay : MonoBehaviour 
{
	[SerializeField]
	private Dictionary<string, GameObject> displayedObject = new Dictionary<string, GameObject>();
	private GUIStyle textStyle;

	// Use this for initialization
	void Start () 
	{
		textStyle = new GUIStyle (StaticReference.Get ().TextStyle);
		textStyle.fontSize = 16;
	}

	void OnGUI()
	{
		foreach(KeyValuePair<string, GameObject> kvp in displayedObject)
		{
			Vector3 displayPos = Camera.main.WorldToScreenPoint(kvp.Value.transform.position);

			GUI.Label(new Rect(displayPos.x,Screen.height-displayPos.y,32,32), PlayerResources.Get(kvp.Key).ToString(), textStyle);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 offset = new Vector3 (-2.6f, -3.8f, 0);

		int i = 0;
		foreach(KeyValuePair<string, int> kvp in PlayerResources.GetResources())
		{
			if (!displayedObject.ContainsKey(kvp.Key))
			{
				GameObject template = StaticReference.getGameObject(kvp.Key);

				GameObject go = (GameObject)Instantiate(template,
				                                        new Vector3(i,0,0)+offset,
				                                        Quaternion.identity);
				go.name = template.name;
				go.transform.localScale = new Vector3(0.4f,0.4f);
				displayedObject[go.name] = go;
			}
			i++;
		}
	}
}
