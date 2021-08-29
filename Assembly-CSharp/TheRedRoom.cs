using System;
using UnityEngine;

public class TheRedRoom : MonoBehaviour
{
	private void leaveLinkClicked()
	{
		string theURL = "http://" + GameManager.GetTheCloud().getWikiLink(0) + ".ann";
		GameManager.GetTheAnnBehavior().forceLoadURL(theURL, false);
	}

	private void enterLinkClicked()
	{
		if (GameManager.FileSlinger.saveData.N1 && GameManager.FileSlinger.saveData.N2 && GameManager.FileSlinger.saveData.N3 && GameManager.FileSlinger.saveData.N4)
		{
			GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_WHO_AM_I, true);
			GameManager.GetTheSceneManager().triggerNymphoEnding();
		}
		else
		{
			GameManager.FileSlinger.saveData.siteRateDisabled = false;
			GameManager.FileSlinger.saveFile("wttg2.gd");
			GameManager.GetTheHackerManager().ConnectToRedRoom();
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

	private void OnDisable()
	{
	}

	public LinkObject leaveLink;

	public LinkObject enterLink;
}
