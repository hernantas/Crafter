using UnityEngine;
using System.Collections;

public static class PlayerCoin
{
	private static int coin = 30;

	public static void Add(int value)
	{
		coin += value;
	}

	public static int Get()
	{
		return coin;
	}

	public static void Spend(int value)
	{
		coin -= value;
	}
}
