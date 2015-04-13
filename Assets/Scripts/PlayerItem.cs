using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlayerItem
{
	private static Dictionary<string, int> items = new Dictionary<string, int>();
	private static bool hasItems = false;
	public static bool HasItems
	{
		get
		{
			return hasItems;
		}
	}
	
	public static void AddType(GameObject go)
	{
		hasItems = true;
		
		if (!items.ContainsKey(go.name))
			items.Add (go.name, 0);
	}
	
	public static void AddType(string name)
	{
		hasItems = true;
		
		if (!items.ContainsKey(name))
			items.Add (name, 0);
	}
	
	public static void Add(string name)
	{
		Add (name, 1);
	}
	
	public static void Add(string name, int value)
	{
		if (!items.ContainsKey (name))
			AddType (name);
		
		items [name] += value;
	}
	
	public static int Get(string name)
	{
		if (items.ContainsKey(name))
			return items [name];
		
		return 0;
	}
	
	public static Dictionary<string, int> Getitemss()
	{
		return items;
	}
}
