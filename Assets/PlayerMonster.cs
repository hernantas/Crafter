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
	private static int indexEnemny = -1;
	public static int IndexEnemy
	{
		set { indexEnemny = value; }
		get { return indexEnemny; }
	}

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

	public static GameObject Spawn(int index, Vector3 spawnPos)
	{
		GameObject created = Reference.Creator.CreateFromTemplate(storage[index].monsterName);

		if (created != null)
		{
			created.name = storage[index].monsterName;
			created.transform.position = spawnPos;
			created.GetComponent<Monster>().Exp = storage[index].exp;
			created.GetComponent<Monster>().StorageIndex = index;
		}

		return created;
	}

	public static void Swap(int index1, int index2)
	{
		StoredMonster temp = storage[index1];
		storage[index1] = storage[index2];
		storage[index2] = temp;
	}

	public static GameObject Spawn(int index)
	{
		return Spawn(index, new Vector3(0,0,0));
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
