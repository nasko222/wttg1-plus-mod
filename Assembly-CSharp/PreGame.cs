using System;
using UnityEngine;

public class PreGame : MonoBehaviour
{
	private void setGameOptions()
	{
		if (GameManager.FileSlinger.optData.vSyncOn)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		QualitySettings.masterTextureLimit = GameManager.FileSlinger.optData.qLevel;
	}

	private void updateScreenRes()
	{
		Screen.SetResolution(GameManager.FileSlinger.optData.setScreenWidth, GameManager.FileSlinger.optData.setScreenHeight, GameManager.FileSlinger.optData.fullScreenMode);
		this.mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
	}

	private void Awake()
	{
		this.hasSavedData = GameManager.FileSlinger.loadOptionFile();
	}

	private void Start()
	{
		this.mainCamera = base.GetComponent<Camera>();
		bool flag = false;
		if (this.hasSavedData)
		{
			bool flag2 = false;
			for (int i = 0; i < Screen.resolutions.Length; i++)
			{
				if (Screen.resolutions[i].width == GameManager.FileSlinger.optData.setScreenWidth && Screen.resolutions[i].height == GameManager.FileSlinger.optData.setScreenHeight)
				{
					flag2 = true;
					i = Screen.resolutions.Length;
				}
			}
			if (!flag2)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			Resolution resolution = Screen.currentResolution;
			if (Screen.resolutions.Length > 0)
			{
				resolution = Screen.resolutions[Screen.resolutions.Length - 1];
			}
			GameManager.FileSlinger.optData.resetData();
			GameManager.FileSlinger.optData.setScreenWidth = resolution.width;
			GameManager.FileSlinger.optData.setScreenHeight = resolution.height;
			GameManager.FileSlinger.optData.fullScreenMode = true;
			GameManager.FileSlinger.saveOptionFile();
		}
		this.setGameOptions();
		this.updateScreenRes();
	}

	private void Update()
	{
	}

	private Camera mainCamera;

	private bool hasSavedData;
}
