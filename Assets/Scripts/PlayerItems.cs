using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlayerItems 
{
	private static Dictionary<string, int> items = new Dictionary<string, int>();
	public static Dictionary<string, int> Items
	{
		set { items = value; }
		get { return items; }
	}
	public static int Count
	{
		get { return items.Count; }
	}

	public static void Add(string itemName, int quantity)
	{
		if (!items.ContainsKey(itemName))
		{
			items[itemName] = 0;
		}

		items[itemName] += quantity; 
	}

	public static int Get(string itemName)
	{
		if (items.ContainsKey(itemName))
			return items[itemName];

		return 0;
	}
}
