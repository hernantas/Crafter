using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FieldStatus
{
	FIELD_EMPTY,
	FIELD_FAILED,
	FIELD_1STAR,
	FIELD_2STAR,
	FIELD_3STAR
}
public static class PlayerField 
{
	private static Dictionary<int, FieldStatus> fieldData = new Dictionary<int, FieldStatus>();

	public static void Add(int index, FieldStatus star)
	{
		fieldData[index] = star;
	}

	public static FieldStatus Get(int index)
	{
		if (!fieldData.ContainsKey(index))
			return FieldStatus.FIELD_EMPTY;

		return fieldData[index];
	}

	public static bool Has(int index)
	{
		return fieldData.ContainsKey(index);
	}
}
