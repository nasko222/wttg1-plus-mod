using System;
using UnityEngine;
using UnityEngine.UI;

public class TwitchMenu : MonoBehaviour
{
	private void prepTwitchData()
	{
		if (!GameManager.FileSlinger.wildLoadFile<TwitchData>("twitchData.gd", out this.myTwitchData))
		{
			this.myTwitchData = new TwitchData();
			GameManager.FileSlinger.wildSaveFile<TwitchData>("twitchData.gd", this.myTwitchData);
		}
		this.oAuthInput.text = this.myTwitchData.twitchOAuth;
		this.userNameInput.text = this.myTwitchData.twitchUserName;
		this.prepLinks();
	}

	private void prepLinks()
	{
		if (this.myTwitchData.isActive)
		{
			this.activeYESLink.setActive();
			this.activeNOLink.setInactive();
		}
		else
		{
			this.activeYESLink.setInactive();
			this.activeNOLink.setActive();
		}
		this.activeYESLink.setAction(new Action(this.activeYESHit));
		this.activeNOLink.setAction(new Action(this.activeNOHit));
		this.doneLink.setAction(new Action(this.doneHit));
	}

	private void activeYESHit()
	{
		this.activeYESLink.setActive();
		this.activeNOLink.setInactive();
		this.myTwitchData.isActive = true;
		GameManager.FileSlinger.wildSaveFile<TwitchData>("twitchData.gd", this.myTwitchData);
	}

	private void activeNOHit()
	{
		this.activeYESLink.setInactive();
		this.activeNOLink.setActive();
		this.myTwitchData.isActive = false;
		GameManager.FileSlinger.wildSaveFile<TwitchData>("twitchData.gd", this.myTwitchData);
	}

	private void doneHit()
	{
		this.myTwitchData.twitchOAuth = this.oAuthInput.text;
		this.myTwitchData.twitchUserName = this.userNameInput.text.ToLower();
		GameManager.FileSlinger.wildSaveFile<TwitchData>("twitchData.gd", this.myTwitchData);
		this.myMenuManager.hideTwitch();
	}

	private void OnEnable()
	{
		this.prepTwitchData();
	}

	public MenuManager myMenuManager;

	public OptLink activeYESLink;

	public OptLink activeNOLink;

	public OptLink doneLink;

	public InputField oAuthInput;

	public InputField userNameInput;

	private TwitchData myTwitchData;
}
