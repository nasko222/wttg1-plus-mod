using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class actionController : MonoBehaviour
{
	private void Update()
	{
		if (!this.lockAction)
		{
			if (this.holdActionEnabled)
			{
				if (CrossPlatformInputManager.GetAxis("LeftClick") >= 1f)
				{
					this.mySceneManager.performHoldAction(1);
				}
				else
				{
					this.mySceneManager.clearHoldAction();
				}
			}
			else if (CrossPlatformInputManager.GetButtonDown("LeftClick"))
			{
				this.mySceneManager.performAction(1);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!this.lockAction)
		{
			if (Physics.Raycast(this.myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), ref this.actionHit, float.PositiveInfinity, 8192))
			{
				this.mySceneManager.checkAction(this.actionHit);
			}
			else
			{
				this.mySceneManager.clearAction();
			}
		}
	}

	public SceneManagerWTTG mySceneManager;

	public Camera myCamera;

	public bool lockAction;

	public bool holdActionEnabled;

	private RaycastHit actionHit;
}
