using System;
using UnityEngine;

public class KnobAction : MonoBehaviour
{
	public void runAction()
	{
		if (GameManager.GetTheSceneManager().isInDoorAction() && GameManager.GetTheBreatherManager().isPlayerInBreatherAction() && !this.inAction && !this.actionShown)
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
	}

	public void performHoldAction()
	{
		if (!this.holdingSet)
		{
			this.holdingSet = true;
			this.knobSet = false;
			GameManager.GetTheBreatherManager().playerIsHoldingKnob = true;
			GameManager.GetTheUIManager().displayActionText("Holding...");
		}
	}

	public void clearHoldAction()
	{
		if (this.holdingSet)
		{
			GameManager.GetTheBreatherManager().playerIsHoldingKnob = false;
			this.holdingSet = false;
			GameManager.GetTheUIManager().hideActionText();
		}
		if (!this.knobSet)
		{
			this.knobSet = true;
			GameManager.GetTheUIManager().displayActionText("Hold Knob");
		}
	}

	private void showAction()
	{
		this.actionShown = true;
		GameManager.GetTheMainController().getMyActionController().holdActionEnabled = true;
		GameManager.GetTheUIManager().displayActionText("Hold Knob");
	}

	private void hideAction()
	{
		GameManager.GetTheMainController().getMyActionController().holdActionEnabled = false;
		GameManager.GetTheUIManager().hideActionText();
		this.actionShown = false;
	}

	private bool actionShown;

	private bool inAction;

	private bool holdingSet;

	private bool knobSet;
}
