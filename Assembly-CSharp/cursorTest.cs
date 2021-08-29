using System;
using UnityEngine;
using UnityEngine.UI;

public class cursorTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = this.cursorIcon.transform.position.z - Camera.main.transform.position.z;
		this.cursorIcon.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
	}

	public Image cursorIcon;
}
