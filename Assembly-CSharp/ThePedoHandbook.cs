using System;
using UnityEngine;

public class ThePedoHandbook : MonoBehaviour
{
	private void leaveLinkClicked()
	{
		string theURL = "http://" + GameManager.GetTheCloud().getWikiLink(0) + ".ann";
		GameManager.GetTheAnnBehavior().forceLoadURL(theURL, false);
	}

	private void enterLinkClicked()
	{
		if (!this.kidnapperSent)
		{
			this.kidnapperSent = true;
			this.myTrackerMan.KidnapNow();
		}
	}

	private void OnEnable()
	{
		this.leaveLink.deadUnlessTapped = false;
		this.leaveLink.hasTapAction = true;
		this.leaveLink.tapAction = new Action(this.leaveLinkClicked);
		this.enterLink.deadUnlessTapped = false;
		this.enterLink.hasTapAction = true;
		this.enterLink.tapAction = new Action(this.enterLinkClicked);
	}

	public LinkObject leaveLink;

	public LinkObject enterLink;

	public TrackerManager myTrackerMan;

	private bool kidnapperSent;
}
