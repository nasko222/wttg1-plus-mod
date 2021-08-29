using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HMBTNObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void setMyAction(Action setAction)
	{
		this.myCallBackAction = setAction;
	}

	public void setMyAction(Action setAction, bool hasHoldAction)
	{
		this.myCallBackAction = setAction;
		this.holdAction = hasHoldAction;
	}

	public void setMyAction(Action setAction, bool hasHoldAction, string setHoldActionText)
	{
		this.myCallBackAction = setAction;
		this.holdAction = hasHoldAction;
		this.holdActionText = setHoldActionText;
	}

	public void releaseHold()
	{
		if (this.iWasFired)
		{
			if (this.buttonText != null)
			{
				this.buttonText.text = this.defaultTextValue.ToString();
				DOTween.To(() => this.buttonText.color, delegate(Color x)
				{
					this.buttonText.color = x;
				}, this.defaultStateTextColor, 0.3f).SetEase(Ease.Linear);
			}
			DOTween.To(() => this.buttonIMG.color, delegate(Color x)
			{
				this.buttonIMG.color = x;
			}, this.defaultStateBTNColor, 0.3f).SetEase(Ease.Linear);
			GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.resetFire));
		}
	}

	public void lockMe()
	{
		if (!this.iWasFired)
		{
			this.iAmLocked = true;
			this.buttonIMG.color = this.lockStateBTNColor;
			if (this.buttonText != null)
			{
				this.buttonText.color = this.lockStateTextColor;
				this.buttonText.fontStyle = FontStyle.Italic;
			}
		}
	}

	public void unLockMe()
	{
		if (!this.iWasFired)
		{
			this.iAmLocked = false;
			this.buttonIMG.color = this.defaultStateBTNColor;
			if (this.buttonText != null)
			{
				this.buttonText.color = this.defaultStateTextColor;
				this.buttonText.fontStyle = FontStyle.Normal;
			}
		}
	}

	private void resetFire()
	{
		this.iWasFired = false;
	}

	public void Start()
	{
		if (this.buttonText != null)
		{
			this.defaultTextValue = this.buttonText.text;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.iAmLocked)
		{
			if (this.holdAction)
			{
				if (!this.iWasFired)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.hoverSFX, 0.6f, false);
					DOTween.To(() => this.buttonIMG.color, delegate(Color x)
					{
						this.buttonIMG.color = x;
					}, this.hoverStateBTNColor, 0.3f).SetEase(Ease.Linear);
					if (this.buttonText != null)
					{
						DOTween.To(() => this.buttonText.color, delegate(Color x)
						{
							this.buttonText.color = x;
						}, this.hoverStateTextColor, 0.3f).SetEase(Ease.Linear);
					}
				}
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.hoverSFX, 0.6f, false);
				DOTween.To(() => this.buttonIMG.color, delegate(Color x)
				{
					this.buttonIMG.color = x;
				}, this.hoverStateBTNColor, 0.3f).SetEase(Ease.Linear);
				if (this.buttonText != null)
				{
					DOTween.To(() => this.buttonText.color, delegate(Color x)
					{
						this.buttonText.color = x;
					}, this.hoverStateTextColor, 0.3f).SetEase(Ease.Linear);
				}
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.iAmLocked)
		{
			if (this.holdAction)
			{
				if (!this.iWasFired)
				{
					DOTween.To(() => this.buttonIMG.color, delegate(Color x)
					{
						this.buttonIMG.color = x;
					}, this.defaultStateBTNColor, 0.3f).SetEase(Ease.Linear);
					if (this.buttonText != null)
					{
						DOTween.To(() => this.buttonText.color, delegate(Color x)
						{
							this.buttonText.color = x;
						}, this.defaultStateTextColor, 0.3f).SetEase(Ease.Linear);
					}
				}
			}
			else
			{
				DOTween.To(() => this.buttonIMG.color, delegate(Color x)
				{
					this.buttonIMG.color = x;
				}, this.defaultStateBTNColor, 0.3f).SetEase(Ease.Linear);
				if (this.buttonText != null)
				{
					DOTween.To(() => this.buttonText.color, delegate(Color x)
					{
						this.buttonText.color = x;
					}, this.defaultStateTextColor, 0.3f).SetEase(Ease.Linear);
				}
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.iAmLocked && !this.iWasFired)
		{
			this.iWasFired = true;
			this.myCallBackAction();
			if (!this.holdAction)
			{
				DOTween.To(() => this.buttonIMG.color, delegate(Color x)
				{
					this.buttonIMG.color = x;
				}, this.defaultStateBTNColor, 0.3f).SetEase(Ease.Linear);
				if (this.buttonText != null)
				{
					DOTween.To(() => this.buttonText.color, delegate(Color x)
					{
						this.buttonText.color = x;
					}, this.defaultStateTextColor, 0.3f).SetEase(Ease.Linear);
				}
				GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.resetFire));
			}
			else if (this.holdActionText != null && this.buttonText != null)
			{
				this.buttonText.text = this.holdActionText;
			}
		}
	}

	public Image buttonIMG;

	public Text buttonText;

	public Color defaultStateBTNColor;

	public Color defaultStateTextColor;

	public Color hoverStateBTNColor;

	public Color hoverStateTextColor;

	public Color lockStateBTNColor;

	public Color lockStateTextColor;

	public AudioClip hoverSFX;

	private bool iWasFired;

	private bool holdAction;

	private bool iAmLocked;

	private Action myCallBackAction;

	private string defaultTextValue;

	private string holdActionText;
}
