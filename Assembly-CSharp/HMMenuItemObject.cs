using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HMMenuItemObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void buildMe(HackerModeManager setHMM, float setWidth, int itemIndex, string setMenuText, string setAction, float myX, float myY, float delayIndex)
	{
		this.myHMM = setHMM;
		this.menuText = setMenuText;
		this.myAction = setAction;
		this.iWasFired = false;
		base.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 4f, base.GetComponent<RectTransform>().sizeDelta.y);
		base.GetComponent<RectTransform>().transform.localPosition = new Vector3(myX, myY, 0f);
		base.GetComponent<RectTransform>().transform.localScale = new Vector3(1f, 1f, 1f);
		this.menuHighlight.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 40f, this.menuHighlight.GetComponent<RectTransform>().sizeDelta.y);
		this.menuLineText.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 60f, this.menuLineText.GetComponent<RectTransform>().sizeDelta.y);
		this.boxHighlight.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 4f, this.boxHighlight.GetComponent<RectTransform>().sizeDelta.y);
		GameManager.TimeSlinger.FireTimer((float)itemIndex * 0.2f + delayIndex, new Action(this.showMe));
	}

	public void lockMe()
	{
		this.iAmLocked = true;
		this.menuLineText.fontStyle = 2;
	}

	public void unLockMe()
	{
		this.iAmLocked = false;
		this.menuLineText.fontStyle = 0;
	}

	private void showMe()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.showSFX, 0.3f, false);
		this.boxHighlight.color = this.boxHighlightColor;
		this.menuLineText.text = this.menuText;
		this.showMeSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.showMeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.boxHighlight.color, delegate(Color x)
		{
			this.boxHighlight.color = x;
		}, new Color(this.boxHighlightColor.r, this.boxHighlightColor.g, this.boxHighlightColor.b, 0f), 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.showMeSeq);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.iAmLocked)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.showSFX, 0.6f, false);
			TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.menuHighlight.color, delegate(Color x)
			{
				this.menuHighlight.color = x;
			}, new Color(this.menuHighlight.color.r, this.menuHighlight.color.g, this.menuHighlight.color.b, 1f), 0.3f), 1);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.iAmLocked)
		{
			TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.menuHighlight.color, delegate(Color x)
			{
				this.menuHighlight.color = x;
			}, new Color(this.menuHighlight.color.r, this.menuHighlight.color.g, this.menuHighlight.color.b, 0f), 0.3f), 1);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.iAmLocked && !this.iWasFired)
		{
			TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.menuHighlight.color, delegate(Color x)
			{
				this.menuHighlight.color = x;
			}, new Color(this.menuHighlight.color.r, this.menuHighlight.color.g, this.menuHighlight.color.b, 0f), 0.3f), 1);
			this.myHMM.performMenuAction(this.myAction);
			this.iWasFired = true;
		}
	}

	public Text menuLineText;

	public Image menuHighlight;

	public Image boxHighlight;

	public Color boxHighlightColor;

	public AudioClip showSFX;

	private HackerModeManager myHMM;

	private string menuText;

	private string myAction;

	private Sequence showMeSeq;

	private bool iWasFired;

	private bool iAmLocked;
}
