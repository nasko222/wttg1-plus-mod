using System;
using UnityEngine;

public class LightSwitchAction : MonoBehaviour
{
	public void runAction()
	{
		if (!GameManager.GetTheSceneManager().isInDoorAction() && !this.actionShown)
		{
			this.showAction();
		}
	}

	public void clearAction()
	{
		if (this.actionShown)
		{
			this.hideAction();
		}
	}

	public void performAction()
	{
		if (!GameManager.GetTheSceneManager().isInDoorAction())
		{
			this.hideAction();
			if (this.lightsOn)
			{
				this.lightsOn = false;
			}
			else
			{
				this.lightsOn = true;
			}
			this.mainController.performLightAction();
		}
	}

	public void triggerLightSwitch()
	{
		if (this.lightsOn)
		{
			this.lightsOn = false;
		}
		else
		{
			this.lightsOn = true;
		}
	}

	private void showAction()
	{
		this.actionShown = true;
		if (this.lightsOn)
		{
			this.myUIManager.displayActionText("Turn Lights Off");
		}
		else
		{
			this.myUIManager.displayActionText("Turn Lights On");
		}
	}

	private void hideAction()
	{
		this.myUIManager.hideActionText();
		this.actionShown = false;
	}

	private void Awake()
	{
		GameManager.SetLightSwitchAction(this);
	}

	public UIManager myUIManager;

	public mainController mainController;

	private bool actionShown;

	private bool lightsOn = true;
}
