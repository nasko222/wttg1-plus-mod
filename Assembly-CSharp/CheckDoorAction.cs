using System;
using UnityEngine;

public class CheckDoorAction : MonoBehaviour
{
	public void runAction()
	{
		if (GameManager.GetTheSceneManager().areTheLightsOn() && !this.actionShown)
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
		if (GameManager.GetTheSceneManager().areTheLightsOn())
		{
			if (GameManager.GetTheSceneManager().isInDoorAction())
			{
				if (!GameManager.GetTheBreatherManager().isPlayerInBreatherAction())
				{
					this.myMainController.performGoBackAction();
				}
			}
			else
			{
				this.myMainController.performCheckDoorAction();
			}
			this.hideAction();
		}
	}

	private void showAction()
	{
		this.actionShown = true;
		if (GameManager.GetTheSceneManager().isInDoorAction())
		{
			if (!GameManager.GetTheBreatherManager().isPlayerInBreatherAction())
			{
				GameManager.GetTheUIManager().displayActionText("Go Back");
			}
		}
		else
		{
			GameManager.GetTheUIManager().displayActionText("Check Door");
		}
	}

	private void hideAction()
	{
		GameManager.GetTheUIManager().hideActionText();
		this.actionShown = false;
	}

	public mainController myMainController;

	private bool actionShown;
}
