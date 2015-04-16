using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoredMonster
{
	public string monsterName = "";
	public int exp = 0;
}

public static class PlayerMonster
{
	private static bool isLoaded = false;
	public static bool IsLoaded { get { return isLoaded; } }

	private static List<StoredMonster> storage = new List<StoredMonster>();
	public static int Count { get { return storage.Count; } }

	public static void Load()
	{
		isLoaded = true;
	}

	public static void Add(string name, int exp)
	{
		StoredMonster sm = new StoredMonster();
		sm.monsterName = name;
		sm.exp = exp;
		storage.Add(sm);
	}

	public static StoredMonster Get(int index)
	{
		return storage[index];
	}

	public static void Remove(int index)
	{
		storage.RemoveAt(index);
	}
}
