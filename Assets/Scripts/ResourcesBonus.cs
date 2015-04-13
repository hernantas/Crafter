using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
class BonusInfo
{
	public GameObject resource = null;
	public int count = 0;
}

public class ResourcesBonus : MonoBehaviour {
	[SerializeField]
	private List<BonusInfo> bonusAtStart = new List<BonusInfo>();

	// Use this for initialization
	void Start () 
	{
		if (!PlayerResources.HasResource)
		{
			foreach (BonusInfo bi in bonusAtStart)
			{
				PlayerResources.Add(bi.resource.name, bi.count);
			}
		}
	}
}
