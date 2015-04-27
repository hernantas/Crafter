using UnityEngine;
using System.Collections;

public class ToolsClose : MonoBehaviour 
{
	void Start()
	{
	}

	void Update()
	{
	}

	void OnMouseUp()
	{
		ToolsDuel td = this.transform.parent.GetComponent<ToolsDuel>();
		td.Hide();
	}
}
