using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Illuminati : MonoBehaviour
{
	private void prepAssets()
	{
		this.secretLink.hasTapAction = true;
		this.secretLink.tapAction = new Action(this.checkForTheInput);
		this.foo1 = true;
		this.foo2 = false;
		this.secretHash = string.Empty;
	}

	private void iWasTapped()
	{
		int num = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		this.setIndex = num;
		this.setHash = empty;
		this.hotLink.deadUnlessTapped = false;
		this.hotLink.hasTapAction = true;
		this.hotLink.tapAction = new Action(this.generateTxtDoc);
	}

	private void generateTxtDoc()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.clickSound, 0.8f, false);
		string txtDocText = this.setIndex.ToString() + " - " + this.setHash;
		GameManager.GetTheCloud().addTxtDoc("confirmed.txt", txtDocText);
	}

	private void checkForTheInput()
	{
		if (this.foo1 && GameManager.GetTheCloud().myTimeManager.isItOpen())
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SFX, this.inputClick, 1f, false);
			this.foo1 = false;
			this.foo2 = true;
		}
	}

	private void checkForProperCode()
	{
		if (this.secretHash == "666")
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SFX, this.showPasscode, 1f, false);
			this.foo2 = false;
			this.secretHash = string.Empty;
			DOTween.To(() => this.secretHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.secretHolder.GetComponent<CanvasGroup>().alpha = x;
			}, 1f, 1f).SetEase(Ease.Linear);
		}
	}

	private void OnEnable()
	{
		if (base.GetComponent<SiteHolder>().wasITapped())
		{
			this.iWasTapped();
		}
		this.prepAssets();
	}

	private void Update()
	{
		if (this.foo2 && Input.GetKeyDown(KeyCode.Alpha6))
		{
			this.secretHash += "6";
			this.checkForProperCode();
		}
	}

	public GameObject secretHolder;

	public LinkObject secretLink;

	public LinkObject hotLink;

	public AudioClip clickSound;

	public AudioClip inputClick;

	public AudioClip showPasscode;

	private int setIndex;

	private string setHash;

	private string secretHash;

	private bool foo1;

	private bool foo2;
}
