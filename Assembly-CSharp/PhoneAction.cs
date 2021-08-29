using System;
using UnityEngine;

public class PhoneAction : MonoBehaviour
{
	public void runAction()
	{
		if (!GameManager.GetTheSceneManager().isInDoorAction() && !this.inAction && !this.actionShown)
		{
			this.showAction();
		}
	}

	public void clearAction()
	{
		if (!this.inAction && this.actionShown)
		{
			this.hideAction();
		}
	}

	public void performAction()
	{
		if (!GameManager.GetTheSceneManager().isInDoorAction() && !this.inAction)
		{
			if (GameManager.GetThePhoneManager().isPhoneRinging())
			{
				GameManager.GetThePhoneManager().answerPhone();
				this.inAction = true;
				this.hideAction();
			}
			else if (GameManager.GetThePhoneManager().isOnPhoneCall())
			{
				GameManager.GetThePhoneManager().hangUpPhone(true);
				this.inAction = true;
				this.hideAction();
			}
		}
	}

	public void phoneDoneAction()
	{
		this.actionShown = false;
		this.inAction = false;
	}

	private void showAction()
	{
		if (GameManager.GetThePhoneManager().isPhoneRinging())
		{
			this.actionShown = true;
			this.myUIManager.displayActionText("Answer Phone");
		}
		else if (GameManager.GetThePhoneManager().isOnPhoneCall())
		{
			this.actionShown = true;
			this.myUIManager.displayActionText("Hang Up");
		}
	}

	private void hideAction()
	{
		this.myUIManager.hideActionText();
		this.actionShown = false;
	}

	public UIManager myUIManager;

	private bool actionShown;

	private bool inAction;
}
