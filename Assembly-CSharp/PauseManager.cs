using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PauseManager : MonoBehaviour
{
	private void triggerPause()
	{
		if (this.iAmPaused)
		{
			Time.timeScale = 1f;
			if (this.mySceneManager.areTheLightsOn())
			{
				this.myTimeManager.freezeTime = false;
			}
			this.iAmPaused = false;
			this.myCursorManager.disableCursor();
			this.myUIManager.unPauseMe();
			GameManager.AudioSlinger.MasterUnMuteAll();
			if (GameManager.GetTheUIManager().myMuteBehavior.iAmMuted)
			{
				GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
			}
		}
		else
		{
			GameManager.AudioSlinger.MasterMuteAll();
			this.myCursorManager.enableCursor();
			this.myUIManager.pauseMe();
			this.myTimeManager.freezeTime = true;
			this.iAmPaused = true;
			Time.timeScale = 0f;
		}
	}

	private void Start()
	{
		this.iAmPaused = false;
	}

	private void Update()
	{
		if (!this.lockPause && CrossPlatformInputManager.GetButtonDown("Cancel"))
		{
			this.triggerPause();
		}
	}

	public cursorManager myCursorManager;

	public TimeManager myTimeManager;

	public UIManager myUIManager;

	public SceneManagerWTTG mySceneManager;

	public bool lockPause;

	public bool iAmPaused;
}
