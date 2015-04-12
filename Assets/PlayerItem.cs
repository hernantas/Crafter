using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlayerItem
{
	private static Dictionary<string, int> resource = new Dictionary<string, int>();
	
	public static void AddType(GameObject go)
	{
		if (!resource.ContainsKey(go.name))
			resource.Add (go.name, 0);
	}
	
	public static void AddType(string name)
	{
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
}
