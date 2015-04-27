using UnityEngine;
using System.Collections;

public enum ItemEffect
{
	EFFECT_NONE,
	EFFECT_HEAL,
	EFFECT_DAMAGE,
	EFFECT_DAMAGE_AMP
}

public class Item : MonoBehaviour 
{
	[SerializeField]
	private ItemEffect effect = new ItemEffect();
	public ItemEffect Effect
	{
		get	{ return effect; }
	}

	[SerializeField]
	private float value;
	public float Value
	{
		get { return value; }
	}

	[SerializeField]
	private float duration;
	public float Duration
	{
		get { return duration; }
	}
	public void SpendDuration(float t)
	{
		duration -= t;
	}

	[SerializeField]
	private int cost = 0;
	public int Cost
	{
		set { cost = value; }
		get { return cost; }
	}

	[SerializeField]
	private int dropWeight = 0;
	public int DropWeight
	{
		set { dropWeight = value; }
		get { return dropWeight; }
	}
}
