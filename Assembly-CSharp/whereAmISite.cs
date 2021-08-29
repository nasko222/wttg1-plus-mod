using System;
using UnityEngine;
using UnityEngine.UI;

public class whereAmISite : MonoBehaviour
{
	private void prepAssets()
	{
		this.enterBTN.setMyAction(new Action(this.checkURL));
	}

	private void checkURL()
	{
		string text = this.urlText.text;
		if (GameManager.GetTheCloud().isThisTheRedRoomURL(text))
		{
			GameManager.FileSlinger.saveData.siteRateDisabled = true;
			GameManager.FileSlinger.saveFile("wttg2.gd");
			GameManager.GetTheHackerManager().ConnectToRedRoom();
		}
		else
		{
			this.urlText.text = string.Empty;
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SFX, this.wrongAnswer, 0.75f, false);
		}
	}

	private void OnEnable()
	{
		this.prepAssets();
	}

	public InputField urlText;

	public SiteBTN enterBTN;

	public AudioClip wrongAnswer;
}
