using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class computerController : MonoBehaviour
{
	public void showCursorIMG()
	{
		this.cursorIcon.enabled = true;
	}

	public void hideCursorIMG()
	{
		this.cursorIcon.enabled = false;
	}

	private void switchBackToMainView()
	{
		if (!this.lockOutRightClick && CrossPlatformInputManager.GetButtonDown("RightClick"))
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = this.cursorIcon.transform.position.z - this.computerUICamera.transform.position.z;
			this.cursorIcon.transform.position = this.computerUICamera.ScreenToWorldPoint(mousePosition);
			this.showCursorIMG();
			this.mainController.switchToMainView();
		}
	}

	private void moveCursorUI()
	{
		Vector3 mousePosition = Input.mousePosition;
		bool flag = false;
		bool flag2 = false;
		mousePosition.x += this.cursorUIIcon.rectTransform.sizeDelta.x * 0.25f;
		mousePosition.y -= this.cursorUIIcon.rectTransform.sizeDelta.y * 0.3f;
		this.cursorUIIcon.transform.position = mousePosition;
		if (mousePosition.x < this.cursorUIMinX || mousePosition.x > this.cursorUIMaxX)
		{
			flag = true;
		}
		if (mousePosition.y < this.cursorUIMinY || mousePosition.y > this.cursorUIMaxY)
		{
			flag2 = true;
		}
		if (flag)
		{
			float x;
			if (mousePosition.x < this.cursorUIMinX)
			{
				x = this.cursorUIMinX;
			}
			else
			{
				x = this.cursorUIMaxX;
			}
			Vector3 position = new Vector3(x, mousePosition.y, 0f);
			this.cursorUIIcon.transform.position = position;
		}
		if (flag2)
		{
			float y;
			if (mousePosition.y < this.cursorUIMinY)
			{
				y = this.cursorUIMinY;
			}
			else
			{
				y = this.cursorUIMaxY;
			}
			Vector3 position2 = new Vector3(this.cursorUIIcon.transform.position.x, y, 0f);
			this.cursorUIIcon.transform.position = position2;
		}
	}

	private void Start()
	{
		this.cursorUIMinX = GameManager.MagicSlinger.getScreenWidthPXByPerc(0.009f);
		this.cursorUIMaxX = GameManager.MagicSlinger.getScreenWidthPXByPerc(0.99f);
		this.cursorUIMinY = GameManager.MagicSlinger.getScreenHeightPXByPerc(0.02f);
		this.cursorUIMaxY = GameManager.MagicSlinger.getScreenHeightPXByPerc(0.9844f);
	}

	private void Update()
	{
		if (this.mainController.isUsingComputer)
		{
			if (this.myPauseManager.iAmPaused)
			{
				this.lockControls = true;
			}
			else
			{
				this.lockControls = false;
			}
			if (!this.lockControls)
			{
				this.switchBackToMainView();
				this.moveCursorUI();
			}
		}
	}

	public mainController mainController;

	public PauseManager myPauseManager;

	public cursorManager myCursorManager;

	public Camera computerCamera;

	public Camera computerUICamera;

	public Canvas computerUI;

	public Canvas screenUI;

	public Image cursorIcon;

	public Image cursorUIIcon;

	public AudioClip clickSound;

	public bool lockOutRightClick;

	public bool lockControls;

	private float cursorUIMinX;

	private float cursorUIMaxX;

	private float cursorUIMinY;

	private float cursorUIMaxY;
}
