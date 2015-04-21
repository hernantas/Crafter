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
	[SerializeField]
	private int storageIndex = -1;
	public int StorageIndex 
	{
		set { storageIndex = value; }
		get { return storageIndex; }
	}
	public GameObject Original
	{
		set; get;
	}

	[SerializeField]
	private int exp = 0;
	public int Exp 
	{ 
		set { exp = value; } 
		get { return exp; }
	}
	private const int baseExp = 25;

	public int Level
	{
		//set { exp = baseExp * (value-1); }
		get 
		{ 
			for (int i=1;true;i++)
			{
				if ((i*(i+1))/2 * baseExp > exp)
				{
					return i;
				}
			}
		}
	}

	[SerializeField]
	private Attr maxHealth;
	public float MaxHealth { get { return maxHealth.Get(Level-1); } }

	[SerializeField]
	private Attr damage;
	public float Damage { get { return damage.Get(Level-1); } }

	[SerializeField]
	private int cost = 0;
	public int Cost { get { return cost; } }

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void AddExp(int exp)
	{
		this.exp += exp;
	}
}
