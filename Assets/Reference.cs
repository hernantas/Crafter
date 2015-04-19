using UnityEngine;
using System.Collections;

public static class Reference
{
	public static AssetsReference Asset
	{
		get
		{
			return GameObject.FindGameObjectWithTag("Reference").GetComponent<AssetsReference>();
		}
	}

	public static MonsterCreator Creator
	{
		get
		{
			return  GameObject.FindGameObjectWithTag("Reference").GetComponent<MonsterCreator>();
		}
	}
}
