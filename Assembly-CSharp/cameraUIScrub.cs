using System;
using UnityEngine;

public class cameraUIScrub : MonoBehaviour
{
	private void Start()
	{
		this.myCamera = base.GetComponent<Camera>();
		int width = Screen.width;
		int height = Screen.height;
		this.myCamera.orthographicSize = (float)height / 2f;
		base.transform.localPosition = new Vector3((float)width / 2f, (float)(-(float)(height / 2)), base.transform.localPosition.z);
	}

	private Camera myCamera;
}
