using UnityEngine;
using System.Collections;

[System.Serializable]
public class Attr
{
	[SerializeField]
	private float val = 0;
	public float BaseValue { get { return val; } }

	[SerializeField]
	private float inc = 0;
	public float Increase { get { return inc; } }

	public float Get(int level)
	{
		return val + (level * inc);
	}
}

public class Monster : MonoBehaviour 
{
	private int exp = 0;
	public int Exp { set { exp = value; } }
	private const int baseExp = 50;

	public int Level
	{
		//set { exp = baseExp * (value-1); }
		get { return (exp/baseExp)+1; }
	}

	[SerializeField]
	private Attr maxHealth;
	public float MaxHealth { get { return maxHealth.Get(Level-1); } }

	[SerializeField]
	private Attr damage;
	public float Damage { get { return damage.Get(Level-1); } }

	[SerializeField]
	private float cost = 0;
	public float Cost { get { return cost; } }

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
