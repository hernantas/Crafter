using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundBuildingRelation
{
	public string ground;
	public string building;
}

public static class PlayerBuilding
{
	private static List<GroundBuildingRelation> groundBuilding = new List<GroundBuildingRelation>();
	public static int Count
	{
		get
		{
			return groundBuilding.Count;
		}
	}

	public static void Add(string ground, string building)
	{
		if (!HasBuilding(ground))
		{
			GroundBuildingRelation gbr = new GroundBuildingRelation();
			gbr.ground = ground;
			gbr.building = building;

			groundBuilding.Add(gbr);
		}
	}

	public static GroundBuildingRelation GetByIndex(int i)
	{
		return groundBuilding[i];
	}

	public static GroundBuildingRelation GetByGround(string ground)
	{
		foreach (GroundBuildingRelation gbr in groundBuilding)
		{
			if (gbr.ground == ground)
			{
				return gbr;
			}
		}

		return null;
	}

	public static GroundBuildingRelation GetByBuilding(string building)
	{
		foreach (GroundBuildingRelation gbr in groundBuilding)
		{
			if (gbr.ground == building)
			{
				return gbr;
			}
		}
		
		return null;
	}

	public static bool HasBuilding()
	{
		return (groundBuilding.Count > 0);
	}

	public static bool HasBuilding(string ground)
	{
		return (GetByGround(ground) != null);
	}
}
