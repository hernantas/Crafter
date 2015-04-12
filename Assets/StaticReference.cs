using UnityEngine;
using System.Collections;

public static class StaticReference 
{
	public static GameObject getGameObject(string name)
	{
		return Camera.main.GetComponent<Referencer>().getGameObject(name);
	}
}
