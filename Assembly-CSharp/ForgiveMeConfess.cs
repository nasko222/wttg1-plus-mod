using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class ForgiveMeConfess : MonoBehaviour
{
	private void submitAConfession()
	{
		if (!this.coolOffTimeActive)
		{
			if (this.canPostOnline)
			{
				if (this.confServerOnline)
				{
					if (this.confessInput.text.Length > 6)
					{
						this.addNewConf(this.confessInput.text);
					}
					else
					{
						GameManager.GetTheUIManager().displayMSGPopUp("Enter a full confession!");
					}
				}
				else
				{
					GameManager.GetTheUIManager().displayConfOffline();
					this.coolOffTimeActive = true;
					GameManager.TimeSlinger.FireTimer(7f, new Action(this.resetCoolOffTime));
				}
			}
			else
			{
				GameManager.GetTheUIManager().displayConfOffline();
				this.coolOffTimeActive = true;
				GameManager.TimeSlinger.FireTimer(7f, new Action(this.resetCoolOffTime));
			}
		}
	}

	private void resetCoolOffTime()
	{
		this.coolOffTimeActive = false;
	}

	private void checkConnection()
	{
		GameManager.GetTheHTTPPusher().connectionCheck(new Action(this.connectionGood), new Action(this.connectionBad));
	}

	private void connectionGood()
	{
		this.canPostOnline = true;
		this.checkConfServerOnline();
	}

	private void connectionBad()
	{
		this.canPostOnline = false;
		GameManager.GetTheUIManager().displayNoInet();
	}

	private void checkConfServerOnline()
	{
		string postURL = "http://wttg.reflectstudios.com/stats.php";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("action", "checkConfessServerStats");
		GameManager.GetTheHTTPPusher().submitPostText(postURL, dictionary, new Action<bool, WTTGJSONData>(this.respFromConfServer));
	}

	private void addNewConf(string confText)
	{
		string postURL = "http://wttg.reflectstudios.com/stats.php";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("action", "addNewConfession");
		dictionary.Add("confText", confText);
		GameManager.GetTheHTTPPusher().submitPostText(postURL, dictionary, new Action<bool, WTTGJSONData>(this.respFromAddingConf));
	}

	private void respFromConfServer(bool pass, WTTGJSONData rData)
	{
		if (pass)
		{
			if (rData.data != "1")
			{
				this.confServerOnline = false;
			}
		}
		else
		{
			this.confServerOnline = false;
		}
		if (!this.confServerOnline)
		{
			GameManager.GetTheUIManager().displayConfOffline();
		}
	}

	private void respFromAddingConf(bool pass, WTTGJSONData rData)
	{
		if (pass)
		{
			if (rData.data == "1")
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.confessSubmitSFX, 1f, false);
				this.confessFormSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.disableConfessInput));
				TweenSettingsExtensions.Insert(this.confessFormSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.confessFormHolder.alpha, delegate(float x)
				{
					this.confessFormHolder.alpha = x;
				}, 0f, 0.5f), 1));
				TweenExtensions.Play<Sequence>(this.confessFormSeq);
			}
			else
			{
				GameManager.GetTheUIManager().displayMSGPopUp(rData.message);
			}
		}
		else
		{
			GameManager.GetTheUIManager().displayMSGPopUp(rData.message);
		}
	}

	private void disableConfessInput()
	{
		this.confessInput.text = string.Empty;
		this.confessInput.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		this.submitBTN.tapAction = new Action(this.submitAConfession);
		this.checkConnection();
		this.confessInput.gameObject.SetActive(true);
		this.confessFormHolder.alpha = 1f;
	}

	private void OnDisable()
	{
	}

	public InputField confessInput;

	public LinkObject submitBTN;

	public CanvasGroup confessFormHolder;

	public AudioClip confessSubmitSFX;

	private bool canPostOnline = true;

	private bool confServerOnline = true;

	private bool coolOffTimeActive;

	private Sequence confessFormSeq;
}
