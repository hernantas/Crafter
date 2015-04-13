using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlayerResources 
{
	private static Dictionary<string, int> resource = new Dictionary<string, int>();
	private static bool hasResource = false;
	public static bool HasResource
	{
		get
		{
			return hasResource;
		}
	}

	public static void AddType(GameObject go)
	{
		hasResource = true;

		if (!resource.ContainsKey(go.name))
			resource.Add (go.name, 0);
	}

	public static void AddType(string name)
	{
		hasResource = true;

		if (!resource.ContainsKey(name))
			resource.Add (name, 0);
	}

	public static void Add(string name)
	{
		Add (name, 1);
	}

	public static void Add(string name, int value)
	{
		if (!resource.ContainsKey (name))
			AddType (name);

		resource [name] += value;
	}

	public static int Get(string name)
	{
		if (resource.ContainsKey(name))
			return resource [name];

		return 0;
	}

	public static Dictionary<string, int> GetResources()
	{
		return resource;
	}
}
