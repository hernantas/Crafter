using UnityEngine;
using System.Collections;

public class CoinDisplay : MonoBehaviour 
{
	private Vector3 screenPos;
	private GUIStyle textStyle;

	// Use this for initialization
	void Start () 
	{
		screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
		textStyle = Reference.Asset.textStyle;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(screenPos.x, Screen.height-screenPos.y, 40, 25), 
		          PlayerCoin.Get().ToString(), 
		          textStyle);
	}
}
