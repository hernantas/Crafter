using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour {
	private float lifeTime = 5f;
	public float LifeTime
	{
		set { lifeTime = value; }
	}
	public Vector2 FloatDirection 
	{
		set; get;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lifeTime <= 0)
			Destroy(this.gameObject);

		if (FloatDirection.magnitude > 0)
		{
			this.transform.Translate(new Vector3(FloatDirection.x, FloatDirection.y) * Time.deltaTime);
		}

		lifeTime -= Time.deltaTime;
	}
}
