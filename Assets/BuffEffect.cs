using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffEffect : MonoBehaviour 
{
	private List<GameObject> buffItems = new List<GameObject>();
	[SerializeField]
	private GameObject manager;
	private Duel duelMgr;

	// Use this for initialization
	void Start () 
	{
		duelMgr = manager.GetComponent<Duel>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i=0;i<buffItems.Count;i++)
		{
			buffItems[i].transform.position = new Vector3(3,-i,0);
			buffItems[i].GetComponent<Item>().SpendDuration(Time.deltaTime);

			DoBuffEffect(buffItems[i].GetComponent<Item>());

			if (buffItems[i].GetComponent<Item>().Duration <= 0)
			{
				Destroy(buffItems[i]);
				buffItems.RemoveAt(i);
				i--;
			}
		}
	}

	public void AddBuff(GameObject item)
	{
		buffItems.Add(item);
	}

	private void DoBuffEffect(Item item)
	{
		switch (item.Effect)
		{
		case ItemEffect.EFFECT_HEAL: duelMgr.AddHealth(item.Value * Time.deltaTime); break;
		case ItemEffect.EFFECT_DAMAGE: duelMgr.Damage(item.Value * Time.deltaTime); break;
		case ItemEffect.EFFECT_DAMAGE_AMP: duelMgr.AddHealth(item.Value * Time.deltaTime); break;
		}
	}
}
