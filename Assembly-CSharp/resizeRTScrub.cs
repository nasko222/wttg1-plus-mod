using System;
using UnityEngine;

public class resizeRTScrub : MonoBehaviour
{
	private void Start()
	{
		this.myCamera = base.GetComponent<Camera>();
		this.myRenderTexture.width = Screen.width;
		this.myRenderTexture.height = Screen.height;
		this.myCamera.targetTexture = this.myRenderTexture;
	}

	private void Update()
	{
	}

	public RenderTexture myRenderTexture;

	private Camera myCamera;
}
