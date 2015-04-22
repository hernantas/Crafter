using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlayerItems 
{
	private static Dictionary<string, int> items = new Dictionary<string, int>();

	public static void Add(string itemName, int quantity)
	{
		if (!items.ContainsKey(itemName))
		{
			items[itemName] = 0;
		}

		items[itemName] += quantity; 
	}
}
