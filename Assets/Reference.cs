using UnityEngine;
using System.Collections;

public static class Reference
{
	public static AssetsReference Get
	{
		get
		{
			return GameObject.FindGameObjectWithTag("Reference").GetComponent<AssetsReference>();
		}
	}
}
