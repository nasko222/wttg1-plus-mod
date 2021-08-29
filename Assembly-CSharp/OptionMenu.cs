using System;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
	private void prepCurrentData()
	{
		GameManager.FileSlinger.loadOptionFile();
		this.currentScreenWidth = GameManager.FileSlinger.optData.setScreenWidth;
		this.currentScreenHeight = GameManager.FileSlinger.optData.setScreenHeight;
		this.fullScreenMode = GameManager.FileSlinger.optData.fullScreenMode;
		if (GameManager.FileSlinger.optData.vSyncOn)
		{
			this.vSyncOnLink.setActive();
			this.vSyncOffLink.setInactive();
		}
		else
		{
			this.vSyncOnLink.setInactive();
			this.vSyncOffLink.setActive();
		}
		if (GameManager.FileSlinger.optData.fullScreenMode)
		{
			this.windowModeOnLink.setInactive();
			this.windowModeOffLink.setActive();
		}
		else
		{
			this.windowModeOnLink.setActive();
			this.windowModeOffLink.setInactive();
		}
		if (GameManager.FileSlinger.optData.useTheMic)
		{
			this.micOnLink.setActive();
			this.micOffLink.setInactive();
		}
		else
		{
			this.micOnLink.setInactive();
			this.micOffLink.setActive();
		}
		int qLevel = GameManager.FileSlinger.optData.qLevel;
		if (qLevel != 0)
		{
			if (qLevel != 1)
			{
				if (qLevel == 2)
				{
					this.QHighLink.setInactive();
					this.QMedLink.setInactive();
					this.QLowLink.setActive();
				}
			}
			else
			{
				this.QHighLink.setInactive();
				this.QMedLink.setActive();
				this.QLowLink.setInactive();
			}
		}
		else
		{
			this.QHighLink.setActive();
			this.QMedLink.setInactive();
			this.QLowLink.setInactive();
		}
		if (GameManager.FileSlinger.optData.contentFilter)
		{
			this.nudityOnLink.setInactive();
			this.nudityOffLink.setActive();
		}
		else
		{
			this.nudityOnLink.setActive();
			this.nudityOffLink.setInactive();
		}
	}

	private void prepResoultions()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		this.resLinkObjs = new List<GameObject>();
		this.avResolutions = new List<Resolution>();
		Resolution[] resolutions = Screen.resolutions;
		for (int i = resolutions.Length - 1; i >= 0; i--)
		{
			this.avResolutions.Add(resolutions[i]);
		}
		for (int j = 0; j < this.avResolutions.Count; j++)
		{
			if (j != 0 && j % 5 == 0)
			{
				if (num4 > num3)
				{
					num3 = num4;
				}
				num2 -= 42f;
				num = 0f;
				num4 = 0f;
			}
			string setRes = this.avResolutions[j].width.ToString() + "X" + this.avResolutions[j].height.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.resLinkObject);
			gameObject.transform.SetParent(this.resHolder.transform);
			gameObject.GetComponent<ResLink>().myMenuManager = this.myMenuManager;
			gameObject.GetComponent<ResLink>().buildMe(new Action<int>(this.resHit), setRes, j, num, num2);
			if (this.avResolutions[j].width == GameManager.FileSlinger.optData.setScreenWidth && this.avResolutions[j].height == GameManager.FileSlinger.optData.setScreenHeight)
			{
				gameObject.GetComponent<ResLink>().setActive();
			}
			this.resLinkObjs.Add(gameObject);
			num = num + gameObject.GetComponent<ResLink>().mySetWidth + 15f;
			num4 = num4 + gameObject.GetComponent<ResLink>().mySetWidth + 15f;
		}
		this.resHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(num3, Math.Abs(num2) + 36f);
	}

	private void prepLinks()
	{
		this.vSyncOnLink.setAction(new Action(this.vSyncOnHit));
		this.vSyncOffLink.setAction(new Action(this.vSyncOffHit));
		this.windowModeOnLink.setAction(new Action(this.windowModeOnHit));
		this.windowModeOffLink.setAction(new Action(this.windowModeOffHit));
		this.micOnLink.setAction(new Action(this.micOnHit));
		this.micOffLink.setAction(new Action(this.micOffHit));
		this.QHighLink.setAction(new Action(this.qHighHit));
		this.QMedLink.setAction(new Action(this.qMedHit));
		this.QLowLink.setAction(new Action(this.qLowHit));
		this.doneLink.setAction(new Action(this.doneHit));
		this.nudityOnLink.setAction(new Action(this.nudityOnHit));
		this.nudityOffLink.setAction(new Action(this.nudityOffHit));
	}

	private void vSyncOnHit()
	{
		QualitySettings.vSyncCount = 1;
		this.vSyncOnLink.setActive();
		this.vSyncOffLink.setInactive();
		GameManager.FileSlinger.optData.vSyncOn = true;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void vSyncOffHit()
	{
		QualitySettings.vSyncCount = 0;
		this.vSyncOnLink.setInactive();
		this.vSyncOffLink.setActive();
		GameManager.FileSlinger.optData.vSyncOn = false;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void windowModeOnHit()
	{
		Screen.SetResolution(this.currentScreenWidth, this.currentScreenHeight, false);
		this.mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
		this.fullScreenMode = false;
		this.windowModeOnLink.setActive();
		this.windowModeOffLink.setInactive();
		GameManager.FileSlinger.optData.fullScreenMode = false;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void windowModeOffHit()
	{
		Screen.SetResolution(this.currentScreenWidth, this.currentScreenHeight, true);
		this.mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
		this.fullScreenMode = true;
		this.windowModeOnLink.setInactive();
		this.windowModeOffLink.setActive();
		GameManager.FileSlinger.optData.fullScreenMode = true;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void micOnHit()
	{
		this.micOnLink.setActive();
		this.micOffLink.setInactive();
		GameManager.FileSlinger.optData.useTheMic = true;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void micOffHit()
	{
		this.micOnLink.setInactive();
		this.micOffLink.setActive();
		GameManager.FileSlinger.optData.useTheMic = false;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void nudityOnHit()
	{
		this.nudityOnLink.setActive();
		this.nudityOffLink.setInactive();
		GameManager.FileSlinger.optData.contentFilter = false;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void nudityOffHit()
	{
		this.nudityOnLink.setInactive();
		this.nudityOffLink.setActive();
		GameManager.FileSlinger.optData.contentFilter = true;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void qHighHit()
	{
		QualitySettings.masterTextureLimit = 0;
		this.QHighLink.setActive();
		this.QMedLink.setInactive();
		this.QLowLink.setInactive();
		GameManager.FileSlinger.optData.qLevel = 0;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void qMedHit()
	{
		QualitySettings.masterTextureLimit = 1;
		this.QHighLink.setInactive();
		this.QMedLink.setActive();
		this.QLowLink.setInactive();
		GameManager.FileSlinger.optData.qLevel = 1;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void qLowHit()
	{
		QualitySettings.masterTextureLimit = 2;
		this.QHighLink.setInactive();
		this.QMedLink.setInactive();
		this.QLowLink.setActive();
		GameManager.FileSlinger.optData.qLevel = 2;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void resHit(int resIndex)
	{
		for (int i = 0; i < this.resLinkObjs.Count; i++)
		{
			if (i == resIndex)
			{
				this.resLinkObjs[i].GetComponent<ResLink>().setActive();
			}
			else
			{
				this.resLinkObjs[i].GetComponent<ResLink>().setInactive();
			}
		}
		this.currentScreenWidth = this.avResolutions[resIndex].width;
		this.currentScreenHeight = this.avResolutions[resIndex].height;
		Screen.SetResolution(this.currentScreenWidth, this.currentScreenHeight, this.fullScreenMode);
		this.mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
		GameManager.FileSlinger.optData.setScreenWidth = this.currentScreenWidth;
		GameManager.FileSlinger.optData.setScreenHeight = this.currentScreenHeight;
		GameManager.FileSlinger.saveOptionFile();
	}

	private void doneHit()
	{
		this.myMenuManager.hideOpts();
	}

	private void OnEnable()
	{
		this.prepCurrentData();
		this.prepResoultions();
		this.prepLinks();
	}

	private void OnDisable()
	{
		for (int i = 0; i < this.resLinkObjs.Count; i++)
		{
			UnityEngine.Object.Destroy(this.resLinkObjs[i]);
		}
		this.resLinkObjs.Clear();
	}

	public MenuManager myMenuManager;

	public Camera mainCamera;

	public GameObject resHolder;

	public GameObject resLinkObject;

	public OptLink vSyncOnLink;

	public OptLink vSyncOffLink;

	public OptLink windowModeOnLink;

	public OptLink windowModeOffLink;

	public OptLink micOnLink;

	public OptLink micOffLink;

	public OptLink QHighLink;

	public OptLink QMedLink;

	public OptLink QLowLink;

	public OptLink nudityOnLink;

	public OptLink nudityOffLink;

	public OptLink doneLink;

	private List<Resolution> avResolutions;

	private List<GameObject> resLinkObjs;

	private int currentScreenWidth;

	private int currentScreenHeight;

	private bool fullScreenMode;
}
