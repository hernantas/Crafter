using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour {
	[SerializeField]
	private string resName;
	public string ResourceName
	{
		get
		{
			return resName;
		}
	}
}
