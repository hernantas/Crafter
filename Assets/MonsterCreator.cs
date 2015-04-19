using UnityEngine;
using System.Collections;

public class MonsterCreator : MonoBehaviour {

	public GameObject CreateFromTemplate(string templateName)
	{
		foreach(GameObject template in Reference.Asset.monsterTemplate)
		{
			if (template.name == templateName)
			{
				GameObject go = (GameObject) Instantiate(template);
				return go;
			}
		}

		return null;
	}
}
