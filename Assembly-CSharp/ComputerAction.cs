using System;
using UnityEngine;

public class ComputerAction : MonoBehaviour
{
	public void runAction()
	{
		if (!GameManager.GetTheSceneManager().isInDoorAction() && GameManager.GetTheSceneManager().areTheLightsOn() && !this.actionShown)
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
		if (!GameManager.GetTheSceneManager().isInDoorAction() && GameManager.GetTheSceneManager().areTheLightsOn())
		{
			this.mainController.switchToComputerView();
			this.hideAction();
		}
	}

	private void showAction()
	{
		this.actionShown = true;
		this.myUIManager.displayActionText("Use Computer");
	}

	private void hideAction()
	{
		this.myUIManager.hideActionText();
		this.actionShown = false;
	}

	public UIManager myUIManager;

	public mainController mainController;

	private bool actionShown;
}
