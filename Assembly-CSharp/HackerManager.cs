using System;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HackerManager : MonoBehaviour
{
	public void launchTwitchHack(string hackerName, string hackType, string hackLevel)
	{
		if (!this.myTimeManager.freezeTime)
		{
			this.modemCoolOff = false;
			this.ipIsMasked = false;
			this.wasATwitchAttack = true;
			this.twitchAttackType = hackType;
			this.twitchAttackLevel = hackLevel;
			this.twitchHackerName = hackerName;
			this.hackActive = true;
			this.myPauseManager.lockPause = true;
			GameManager.GetDOSTwitch().PresentTwitchHacker(this.twitchHackerName);
			this.startHackAni();
		}
		else
		{
			this.twitchAttackType = hackType;
			this.twitchAttackLevel = hackLevel;
			this.twitchHackerName = hackerName;
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.launchTwitchHackDelayed));
		}
	}

	public void launchTwitchHackDelayed()
	{
		if (!this.myTimeManager.freezeTime)
		{
			this.modemCoolOff = false;
			this.ipIsMasked = false;
			this.wasATwitchAttack = true;
			this.hackActive = true;
			this.myPauseManager.lockPause = true;
			GameManager.GetDOSTwitch().PresentTwitchHacker(this.twitchHackerName);
			this.startHackAni();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.launchTwitchHackDelayed));
		}
	}

	public void launchTutorialHack(string attackType)
	{
		this.tutorialAttackType = attackType;
		this.inTutorialMode = true;
		this.myPauseManager.lockPause = true;
		this.startHackAni();
	}

	public void launchHack()
	{
		if (!GameManager.GetGameModeManager().getCasualMode() && !this.myTimeManager.freezeTime && !this.ipIsMasked && !this.modemCoolOff)
		{
			this.hackActive = true;
			this.myPauseManager.lockPause = true;
			this.startHackAni();
		}
	}

	public void DOSAttackBlocked()
	{
		this.DOSNodeHolder.GetComponent<CanvasGroup>().alpha = 0f;
		this.DOSAttackBlockedObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.DOSAttackBlockedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		if (this.inTutorialMode)
		{
			this.tutorialAttackPassed = true;
		}
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack3, 1f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack2, 1f, false);
		this.brightFX.enabled = true;
		this.brightFX2.enabled = true;
		this.DOSAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.DOSAttackBlockedAniDone));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSAttackBlockedObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.DOSAttackBlockedObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.75f), 9));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.DOSAttackBlockedObject.transform.localScale, delegate(Vector3 x)
		{
			this.DOSAttackBlockedObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f), 9));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt1.fillAmount, delegate(float x)
		{
			this.DOSPrompt1.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt2.fillAmount, delegate(float x)
		{
			this.DOSPrompt2.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt3.fillAmount, delegate(float x)
		{
			this.DOSPrompt3.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.DOSAniSeq);
	}

	public void DOSAttackPassed()
	{
		int num = Random.Range(1, this.daysToLooseMax);
		int num2 = Random.Range(1, 11);
		if (this.inTutorialMode)
		{
			this.tutorialAttackPassed = false;
			num = 0;
			num2 = 0;
		}
		if (num2 == 5)
		{
			this.DOSAttackHackedLostText.text = "Days Lost: " + num + "\nNotes    : Lost";
			this.myCould.clearNotes();
		}
		else
		{
			this.DOSAttackHackedLostText.text = "Days Lost: " + num + "\nNotes    : Kept";
		}
		this.myTimeManager.loseDays(num);
		this.brightFX.enabled = true;
		this.brightFX2.enabled = true;
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack4, 1f, false);
		this.DOSNodeHolder.GetComponent<CanvasGroup>().alpha = 0f;
		this.DOSAttackHackedObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.DOSAttackHackedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		this.DOSAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.DOSAttackBlockedAniDone));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.DOSAttackHackedObject.transform.localScale, delegate(Vector3 x)
		{
			this.DOSAttackHackedObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f), 9));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt1.fillAmount, delegate(float x)
		{
			this.DOSPrompt1.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt2.fillAmount, delegate(float x)
		{
			this.DOSPrompt2.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt3.fillAmount, delegate(float x)
		{
			this.DOSPrompt3.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSAttackHackedLostObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.DOSAttackHackedLostObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 3.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSAttackHackedLostObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.DOSAttackHackedLostObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 3.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSAttackHackedObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.DOSAttackHackedObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.DOSAniSeq);
	}

	public void KAttackBlocked()
	{
		this.KAttackBlockedObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.KAttackBlockedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		if (this.inTutorialMode)
		{
			this.tutorialAttackPassed = true;
		}
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack3, 1f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack2, 1f, false);
		this.brightFX.enabled = true;
		this.brightFX2.enabled = true;
		this.KAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.KAttackAniDone));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KAttackBlockedObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KAttackBlockedObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.75f), 9));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.KAttackBlockedObject.transform.localScale, delegate(Vector3 x)
		{
			this.KAttackBlockedObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f), 9));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt1.fillAmount, delegate(float x)
		{
			this.KPrompt1.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt2.fillAmount, delegate(float x)
		{
			this.KPrompt2.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt3.fillAmount, delegate(float x)
		{
			this.KPrompt3.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.KAniSeq);
	}

	public void KAttackPassed()
	{
		int num = Random.Range(1, this.daysToLooseMax);
		int num2 = Random.Range(1, 11);
		if (this.inTutorialMode)
		{
			this.tutorialAttackPassed = false;
			num = 0;
			num2 = 0;
		}
		if (num2 == 5)
		{
			this.KAttackHackedLostText.text = "Days Lost: " + num + "\nNotes    : Lost";
			this.myCould.clearNotes();
		}
		else
		{
			this.KAttackHackedLostText.text = "Days Lost: " + num + "\nNotes    : Kept";
		}
		this.myTimeManager.loseDays(num);
		this.brightFX.enabled = true;
		this.brightFX2.enabled = true;
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack4, 1f, false);
		this.KAttackHackedObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.KAttackHackedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		this.KAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.KAttackAniDone));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.KAttackHackedObject.transform.localScale, delegate(Vector3 x)
		{
			this.KAttackHackedObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f), 9));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt1.fillAmount, delegate(float x)
		{
			this.KPrompt1.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt2.fillAmount, delegate(float x)
		{
			this.KPrompt2.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt3.fillAmount, delegate(float x)
		{
			this.KPrompt3.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KAttackHackedLostObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KAttackHackedLostObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 3.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KAttackHackedLostObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KAttackHackedLostObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 3.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KAttackHackedObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KAttackHackedObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.KAniSeq);
	}

	public void VAttackBlocked()
	{
		this.VAttackBlockedObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.VAttackBlockedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack3, 1f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack2, 1f, false);
		if (this.inTutorialMode)
		{
			this.tutorialAttackPassed = true;
		}
		this.brightFX.enabled = true;
		this.brightFX2.enabled = true;
		this.VAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.VAttackAniDone));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VAttackBlockedObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.VAttackBlockedObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.75f), 9));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.VAttackBlockedObject.transform.localScale, delegate(Vector3 x)
		{
			this.VAttackBlockedObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f), 9));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt1.fillAmount, delegate(float x)
		{
			this.VPrompt1.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt2.fillAmount, delegate(float x)
		{
			this.VPrompt2.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt3.fillAmount, delegate(float x)
		{
			this.VPrompt3.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.VAniSeq);
	}

	public void VAttackedPassed()
	{
		int num = Random.Range(1, this.daysToLooseMax);
		int num2 = Random.Range(1, 11);
		if (this.inTutorialMode)
		{
			this.tutorialAttackPassed = false;
			num = 0;
			num2 = 0;
		}
		if (num2 == 5)
		{
			this.VAttackHackedLostText.text = "Days Lost: " + num + "\nNotes    : Lost";
			this.myCould.clearNotes();
		}
		else
		{
			this.VAttackHackedLostText.text = "Days Lost: " + num + "\nNotes    : Kept";
		}
		this.myTimeManager.loseDays(num);
		this.brightFX.enabled = true;
		this.brightFX2.enabled = true;
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack4, 1f, false);
		this.VAttackHackedObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.VAttackHackedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		this.VAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.VAttackAniDone));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.VAttackHackedObject.transform.localScale, delegate(Vector3 x)
		{
			this.VAttackHackedObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f), 9));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt1.fillAmount, delegate(float x)
		{
			this.VPrompt1.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt2.fillAmount, delegate(float x)
		{
			this.VPrompt2.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt3.fillAmount, delegate(float x)
		{
			this.VPrompt3.fillAmount = x;
		}, 0f, 0.75f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VAttackHackedLostObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.VAttackHackedLostObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 3.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VAttackHackedLostObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.VAttackHackedLostObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 3.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VAttackHackedObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.VAttackHackedObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 3.95f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.VAniSeq);
	}

	public void ConnectToRedRoom()
	{
		GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
		this.myTimeManager.freezeTime = true;
		this.myPauseManager.lockPause = true;
		this.myPauseManager.myCursorManager.disableCursor();
		this.myPauseManager.myUIManager.hideCursorUI();
		this.myMainController.masterLock = true;
		this.myMainController.myComputerController.lockControls = true;
		this.myMainController.myComputerController.lockOutRightClick = true;
		this.glitchFX.Mode = Glitch.GlitchingMode.Complete;
		this.glitchFX.SettingsInterferences.Speed = 0f;
		this.glitchFX.SettingsInterferences.Density = 4f;
		this.glitchFX.SettingsInterferences.MaxDisplacement = 8f;
		this.glitchFX.SettingsTearing.Speed = 0f;
		this.glitchFX.SettingsTearing.Intensity = 0.2f;
		this.glitchFX.enabled = true;
		this.glitchFX2.Mode = Glitch.GlitchingMode.Complete;
		this.glitchFX2.SettingsInterferences.Speed = 0f;
		this.glitchFX2.SettingsInterferences.Density = 4f;
		this.glitchFX2.SettingsInterferences.MaxDisplacement = 8f;
		this.glitchFX2.SettingsTearing.Speed = 0f;
		this.glitchFX2.SettingsTearing.Intensity = 0.2f;
		this.glitchFX2.enabled = true;
		this.analogFX.NoiseIntensity = 0.5f;
		this.analogFX.ScanlinesIntensity = 0.5f;
		this.analogFX.ScanlinesCount = 200;
		this.analogFX.Distortion = 0f;
		this.analogFX.CubicDistortion = 0f;
		this.analogFX.Scale = 1f;
		this.analogFX.enabled = false;
		this.analogFX2.NoiseIntensity = 0.5f;
		this.analogFX2.ScanlinesIntensity = 0.5f;
		this.analogFX2.ScanlinesCount = 200;
		this.analogFX2.Distortion = 0f;
		this.analogFX2.CubicDistortion = 0f;
		this.analogFX2.Scale = 1f;
		this.analogFX2.enabled = false;
		this.brightFX.Contrast = 0f;
		this.brightFX.Gamma = 1f;
		this.brightFX2.Contrast = 0f;
		this.brightFX2.Gamma = 1f;
		GameManager.TimeSlinger.FireTimer(1.9f, new Action(this.enableAnalogFX));
		GameManager.TimeSlinger.FireTimer(1.9f, new Action(this.enableBrightFX));
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.hackingSFX1, 1f, false);
		this.hackingAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.startRedRoomLoading));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX.SettingsInterferences.Speed, delegate(float x)
		{
			this.glitchFX.SettingsInterferences.Speed = x;
		}, 6f, 2f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX.SettingsTearing.Speed, delegate(float x)
		{
			this.glitchFX.SettingsTearing.Speed = x;
		}, 5f, 1.5f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX.SettingsTearing.Intensity, delegate(float x)
		{
			this.glitchFX.SettingsTearing.Intensity = x;
		}, 0.5f, 1f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX2.SettingsInterferences.Speed, delegate(float x)
		{
			this.glitchFX2.SettingsInterferences.Speed = x;
		}, 6f, 2f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX2.SettingsTearing.Speed, delegate(float x)
		{
			this.glitchFX2.SettingsTearing.Speed = x;
		}, 5f, 1.5f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX2.SettingsTearing.Intensity, delegate(float x)
		{
			this.glitchFX2.SettingsTearing.Intensity = x;
		}, 0.5f, 1f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.NoiseIntensity, delegate(float x)
		{
			this.analogFX.NoiseIntensity = x;
		}, 0.98f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.Distortion, delegate(float x)
		{
			this.analogFX.Distortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.CubicDistortion, delegate(float x)
		{
			this.analogFX.CubicDistortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.Scale, delegate(float x)
		{
			this.analogFX.Scale = x;
		}, 0.8f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.NoiseIntensity, delegate(float x)
		{
			this.analogFX2.NoiseIntensity = x;
		}, 0.98f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.Distortion, delegate(float x)
		{
			this.analogFX2.Distortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.CubicDistortion, delegate(float x)
		{
			this.analogFX2.CubicDistortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.Scale, delegate(float x)
		{
			this.analogFX2.Scale = x;
		}, 0.8f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.4f), 1));
		TweenExtensions.Play<Sequence>(this.hackingAniSeq);
	}

	public void MaskTheIP()
	{
		this.modemCoolOff = true;
		this.unMaskIPWindowTime = (float)Random.Range(this.maskIPOpen, this.maskIPClose) * 60f;
		this.unMaskIPTimeStamp = Time.time;
		this.ipIsMasked = true;
		GameManager.TimeSlinger.KillTimerWithID("modemCoolOffTimer");
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.subMaskIP));
	}

	private void subMaskIP()
	{
		GameManager.TimeSlinger.FireTimer(120f, new Action(this.modemCooledOff), "modemCoolOffTimer");
	}

	public void UnMaskTheIP()
	{
		this.ipIsMasked = false;
	}

	private void enableAnalogFX()
	{
		this.analogFX.enabled = true;
		this.analogFX2.enabled = true;
	}

	private void enableBrightFX()
	{
		this.brightFX.enabled = true;
		this.brightFX2.enabled = true;
	}

	private void startHackAni()
	{
		GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
		this.myTimeManager.freezeTime = true;
		this.glitchFX.Mode = Glitch.GlitchingMode.Complete;
		this.glitchFX.SettingsInterferences.Speed = 0f;
		this.glitchFX.SettingsInterferences.Density = 4f;
		this.glitchFX.SettingsInterferences.MaxDisplacement = 8f;
		this.glitchFX.SettingsTearing.Speed = 0f;
		this.glitchFX.SettingsTearing.Intensity = 0.2f;
		this.glitchFX.enabled = true;
		this.glitchFX2.Mode = Glitch.GlitchingMode.Complete;
		this.glitchFX2.SettingsInterferences.Speed = 0f;
		this.glitchFX2.SettingsInterferences.Density = 4f;
		this.glitchFX2.SettingsInterferences.MaxDisplacement = 8f;
		this.glitchFX2.SettingsTearing.Speed = 0f;
		this.glitchFX2.SettingsTearing.Intensity = 0.2f;
		this.glitchFX2.enabled = true;
		this.analogFX.NoiseIntensity = 0.5f;
		this.analogFX.ScanlinesIntensity = 0.5f;
		this.analogFX.ScanlinesCount = 200;
		this.analogFX.Distortion = 0f;
		this.analogFX.CubicDistortion = 0f;
		this.analogFX.Scale = 1f;
		this.analogFX.enabled = false;
		this.analogFX2.NoiseIntensity = 0.5f;
		this.analogFX2.ScanlinesIntensity = 0.5f;
		this.analogFX2.ScanlinesCount = 200;
		this.analogFX2.Distortion = 0f;
		this.analogFX2.CubicDistortion = 0f;
		this.analogFX2.Scale = 1f;
		this.analogFX2.enabled = false;
		this.brightFX.Contrast = 0f;
		this.brightFX.Gamma = 1f;
		this.brightFX2.Contrast = 0f;
		this.brightFX2.Gamma = 1f;
		GameManager.TimeSlinger.FireTimer(1.9f, new Action(this.enableAnalogFX));
		GameManager.TimeSlinger.FireTimer(1.9f, new Action(this.enableBrightFX));
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.hackingSFX1, 1f, false);
		this.hackingAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.startHackingLoadingAni));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX.SettingsInterferences.Speed, delegate(float x)
		{
			this.glitchFX.SettingsInterferences.Speed = x;
		}, 6f, 2f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX.SettingsTearing.Speed, delegate(float x)
		{
			this.glitchFX.SettingsTearing.Speed = x;
		}, 5f, 1.5f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX.SettingsTearing.Intensity, delegate(float x)
		{
			this.glitchFX.SettingsTearing.Intensity = x;
		}, 0.5f, 1f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX2.SettingsInterferences.Speed, delegate(float x)
		{
			this.glitchFX2.SettingsInterferences.Speed = x;
		}, 6f, 2f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX2.SettingsTearing.Speed, delegate(float x)
		{
			this.glitchFX2.SettingsTearing.Speed = x;
		}, 5f, 1.5f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.glitchFX2.SettingsTearing.Intensity, delegate(float x)
		{
			this.glitchFX2.SettingsTearing.Intensity = x;
		}, 0.5f, 1f), 3));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.NoiseIntensity, delegate(float x)
		{
			this.analogFX.NoiseIntensity = x;
		}, 0.98f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.Distortion, delegate(float x)
		{
			this.analogFX.Distortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.CubicDistortion, delegate(float x)
		{
			this.analogFX.CubicDistortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX.Scale, delegate(float x)
		{
			this.analogFX.Scale = x;
		}, 0.8f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.NoiseIntensity, delegate(float x)
		{
			this.analogFX2.NoiseIntensity = x;
		}, 0.98f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.Distortion, delegate(float x)
		{
			this.analogFX2.Distortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.CubicDistortion, delegate(float x)
		{
			this.analogFX2.CubicDistortion = x;
		}, 0.2f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.analogFX2.Scale, delegate(float x)
		{
			this.analogFX2.Scale = x;
		}, 0.8f, 0.4f), 27));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Contrast, delegate(float x)
		{
			this.brightFX.Contrast = x;
		}, -100f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX.Gamma, delegate(float x)
		{
			this.brightFX.Gamma = x;
		}, 9.9f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Contrast, delegate(float x)
		{
			this.brightFX2.Contrast = x;
		}, -100f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.hackingAniSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.brightFX2.Gamma, delegate(float x)
		{
			this.brightFX2.Gamma = x;
		}, 9.9f, 0.4f), 1));
		TweenExtensions.Play<Sequence>(this.hackingAniSeq);
	}

	private void startRedRoomLoading()
	{
		this.ConDesktop.SetActive(true);
		this.glitchFX.enabled = false;
		this.glitchFX2.enabled = false;
		this.brightFX.enabled = false;
		this.brightFX2.enabled = false;
		GameManager.TimeSlinger.FireTimer(1f, new Action(this.loadRedRoom1));
	}

	private void loadRedRoom1()
	{
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTERHARDWARE, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.OUTSIDE, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.MAINROOM, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.LEFTROOM, 0f);
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.loadRedRoom2));
	}

	private void loadRedRoom2()
	{
		SceneManager.LoadScene(2);
	}

	private void startHackingLoadingAni()
	{
		float y = this.SkullBottom.GetComponent<RectTransform>().localPosition.y;
		float num = y - 20f;
		this.HackerDesktop.SetActive(true);
		this.brightFX.enabled = false;
		this.pixelFX.enabled = true;
		this.brightFX2.enabled = false;
		this.pixelFX2.enabled = true;
		this.glitchFX.Mode = Glitch.GlitchingMode.Interferences;
		this.glitchFX.SettingsInterferences.Speed = 5f;
		this.glitchFX.SettingsInterferences.Density = 2f;
		this.glitchFX.SettingsInterferences.MaxDisplacement = 2f;
		this.glitchFX2.Mode = Glitch.GlitchingMode.Interferences;
		this.glitchFX2.SettingsInterferences.Speed = 5f;
		this.glitchFX2.SettingsInterferences.Density = 2f;
		this.glitchFX2.SettingsInterferences.MaxDisplacement = 2f;
		this.analogFX.NoiseIntensity = 0.5f;
		this.analogFX.ScanlinesIntensity = 2f;
		this.analogFX.ScanlinesCount = 400;
		this.analogFX.VerticalScanlines = false;
		this.analogFX2.NoiseIntensity = 0.5f;
		this.analogFX2.ScanlinesIntensity = 2f;
		this.analogFX2.ScanlinesCount = 400;
		this.analogFX2.VerticalScanlines = false;
		this.brightFX.Contrast = 0f;
		this.brightFX.Gamma = 1f;
		this.brightFX2.Contrast = 0f;
		this.brightFX2.Gamma = 1f;
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.hackingSFX2, 1f, false);
		this.hackingLoadingAniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.startLoadHackingGame));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.SkullObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.SkullObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 27));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullObject.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.SkullObject.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.5f), 27));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, num, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 1.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, y, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 1.4f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, num, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 1.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, y, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 1.8f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, num, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, y, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 2.3f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, num - 10f, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 2.9f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullBottom.GetComponent<RectTransform>().localPosition, delegate(Vector3 x)
		{
			this.SkullBottom.GetComponent<RectTransform>().localPosition = x;
		}, new Vector3(this.SkullBottom.GetComponent<RectTransform>().localPosition.x, y, 0f), 0.2f), 1));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 3.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.SkullObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.SkullObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 26));
		TweenSettingsExtensions.Insert(this.hackingLoadingAniSeq, 3.5f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.SkullObject.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.SkullObject.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1.5f, 1.5f, 1.5f), 0.5f), 26));
		TweenExtensions.Play<Sequence>(this.hackingLoadingAniSeq);
	}

	private void startLoadHackingGame()
	{
		bool inTutMode = false;
		this.brightFX.enabled = false;
		this.pixelFX.enabled = false;
		this.glitchFX.enabled = false;
		this.analogFX.enabled = false;
		this.brightFX2.enabled = false;
		this.pixelFX2.enabled = false;
		this.glitchFX2.enabled = false;
		this.analogFX2.enabled = false;
		this.SkullObject.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
		int num = Random.Range(0, 9);
		if (this.inTutorialMode)
		{
			inTutMode = true;
			if (this.tutorialAttackType == "DOS")
			{
				num = 2;
			}
			else if (this.tutorialAttackType == "KERNAL")
			{
				num = 5;
			}
			else if (this.tutorialAttackType == "CLOUD")
			{
				num = 8;
			}
		}
		else if (this.wasATwitchAttack)
		{
			if (this.twitchAttackType == "DOS")
			{
				num = 2;
			}
			else if (this.twitchAttackType == "KERNAL")
			{
				num = 5;
			}
			else
			{
				num = 8;
			}
		}
		if (num < 3)
		{
			this.buildDOSPuzzle(inTutMode);
		}
		else if (num < 6)
		{
			this.buildKAttack(inTutMode);
		}
		else
		{
			this.buildVAttack(inTutMode);
		}
	}

	private void buildDOSPuzzle(bool inTutMode = false)
	{
		this.DOSAttackHolder.SetActive(true);
		if (this.wasATwitchAttack)
		{
			this.myDOSAttack.prepDOSTwitchAttack(this.twitchHackerName, this.twitchAttackLevel);
			this.wasATwitchAttack = false;
			this.twitchHackerName = string.Empty;
			this.twitchAttackType = string.Empty;
			this.twitchAttackLevel = string.Empty;
		}
		else
		{
			this.myDOSAttack.prepDOSAttack(inTutMode);
		}
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.myDOSAttack.warmDOSAttack));
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack1, 1f, false);
		this.DOSAniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt1.fillAmount, delegate(float x)
		{
			this.DOSPrompt1.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt2.fillAmount, delegate(float x)
		{
			this.DOSPrompt2.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 1.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSPrompt3.fillAmount, delegate(float x)
		{
			this.DOSPrompt3.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.DOSAniSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.DOSNodeHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.DOSNodeHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 6));
		TweenExtensions.Play<Sequence>(this.DOSAniSeq);
	}

	private void DOSAttackBlockedAniDone()
	{
		this.HackerDesktop.SetActive(false);
		this.DOSAttackHolder.SetActive(false);
		this.brightFX.enabled = false;
		this.brightFX.Contrast = 0f;
		this.brightFX.Gamma = 1f;
		this.brightFX2.enabled = false;
		this.brightFX2.Contrast = 0f;
		this.brightFX2.Gamma = 1f;
		this.myTimeManager.freezeTime = false;
		this.myPauseManager.lockPause = false;
		this.hackActive = false;
		if (this.inTutorialMode)
		{
			this.myTimeManager.freezeTime = true;
			this.myPauseManager.lockPause = true;
			this.inTutorialMode = false;
			if (this.tutorialAttackPassed)
			{
				this.tutorialPass.DynamicInvoke(new object[0]);
			}
			else
			{
				this.tutorialFail.DynamicInvoke(new object[0]);
			}
		}
		if (!GameManager.GetTheUIManager().myMuteBehavior.iAmMuted)
		{
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0.3f);
		}
	}

	private void buildKAttack(bool inTutMode = false)
	{
		this.KAttackHolder.SetActive(true);
		if (this.wasATwitchAttack)
		{
			this.myKAttack.PrepTwitchAttack(this.twitchHackerName, this.twitchAttackLevel);
			this.wasATwitchAttack = false;
			this.twitchHackerName = string.Empty;
			this.twitchAttackType = string.Empty;
			this.twitchAttackLevel = string.Empty;
		}
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack1, 1f, false);
		GameManager.TimeSlinger.FireBoolTimer(1.6f, new Action<bool>(this.myKAttack.startKAttack), inTutMode);
		this.KAniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.KAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt1.fillAmount, delegate(float x)
		{
			this.KPrompt1.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt2.fillAmount, delegate(float x)
		{
			this.KPrompt2.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.KAniSeq, 1.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.KPrompt3.fillAmount, delegate(float x)
		{
			this.KPrompt3.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenExtensions.Play<Sequence>(this.KAniSeq);
	}

	private void KAttackAniDone()
	{
		this.HackerDesktop.SetActive(false);
		this.KAttackHolder.SetActive(false);
		this.brightFX.enabled = false;
		this.brightFX.Contrast = 0f;
		this.brightFX.Gamma = 1f;
		this.brightFX2.enabled = false;
		this.brightFX2.Contrast = 0f;
		this.brightFX2.Gamma = 1f;
		this.myTimeManager.freezeTime = false;
		this.myPauseManager.lockPause = false;
		this.hackActive = false;
		if (this.inTutorialMode)
		{
			this.myTimeManager.freezeTime = true;
			this.myPauseManager.lockPause = true;
			this.inTutorialMode = false;
			if (this.tutorialAttackPassed)
			{
				this.tutorialPass.DynamicInvoke(new object[0]);
			}
			else
			{
				this.tutorialFail.DynamicInvoke(new object[0]);
			}
		}
		if (!GameManager.GetTheUIManager().myMuteBehavior.iAmMuted)
		{
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0.3f);
		}
	}

	private void buildVAttack(bool inTutMode = false)
	{
		this.VAttackHolder.SetActive(true);
		if (this.wasATwitchAttack)
		{
			this.myVapeAttack.prepTwitchVapeAttack(this.twitchHackerName, this.twitchAttackLevel);
			this.wasATwitchAttack = false;
			this.twitchHackerName = string.Empty;
			this.twitchAttackType = string.Empty;
			this.twitchAttackLevel = string.Empty;
		}
		else
		{
			this.myVapeAttack.prepVapeAttack(inTutMode);
		}
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.DOSAttack1, 1f, false);
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.myVapeAttack.warmVapeAttack));
		this.VAniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.VAniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt1.fillAmount, delegate(float x)
		{
			this.VPrompt1.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt2.fillAmount, delegate(float x)
		{
			this.VPrompt2.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.VAniSeq, 1.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.VPrompt3.fillAmount, delegate(float x)
		{
			this.VPrompt3.fillAmount = x;
		}, 1f, 0.2f), 1));
		TweenExtensions.Play<Sequence>(this.VAniSeq);
	}

	private void VAttackAniDone()
	{
		this.HackerDesktop.SetActive(false);
		this.VAttackHolder.SetActive(false);
		this.brightFX.enabled = false;
		this.brightFX.Contrast = 0f;
		this.brightFX.Gamma = 1f;
		this.brightFX2.enabled = false;
		this.brightFX2.Contrast = 0f;
		this.brightFX2.Gamma = 1f;
		this.myTimeManager.freezeTime = false;
		this.myPauseManager.lockPause = false;
		this.hackActive = false;
		if (this.inTutorialMode)
		{
			this.myTimeManager.freezeTime = true;
			this.myPauseManager.lockPause = true;
			this.inTutorialMode = false;
			if (this.tutorialAttackPassed)
			{
				this.tutorialPass.DynamicInvoke(new object[0]);
			}
			else
			{
				this.tutorialFail.DynamicInvoke(new object[0]);
			}
		}
		if (!GameManager.GetTheUIManager().myMuteBehavior.iAmMuted)
		{
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0.3f);
		}
	}

	private void generateLaunchHack()
	{
		float num = Random.Range(this.launchHackOpen, this.launchHackClose) * 60f;
		float num2 = num * ((float)this.myCould.getRedRoomKeyVistCount() / 10f);
		if (num2 != 0f)
		{
			num2 -= num2 * 0.1f;
		}
		float num3 = num - num2;
		this.launchHackTimeStamp = Time.time;
		this.launchHackWindowTime = num3;
		this.checkLaunchHack = true;
	}

	private void modemCooledOff()
	{
		this.modemCoolOff = false;
	}

	private void Awake()
	{
		GameManager.SetTheHackerManager(this);
	}

	private void Start()
	{
		this.glitchFX = this.computerCamera.gameObject.GetComponent<Glitch>();
		this.analogFX = this.computerCamera.gameObject.GetComponent<AnalogTV>();
		this.brightFX = this.computerCamera.gameObject.GetComponent<BrightnessContrastGamma>();
		this.pixelFX = this.computerCamera.gameObject.GetComponent<PixelMatrix>();
		this.glitchFX2 = this.computerCameraUse.gameObject.GetComponent<Glitch>();
		this.analogFX2 = this.computerCameraUse.gameObject.GetComponent<AnalogTV>();
		this.brightFX2 = this.computerCameraUse.gameObject.GetComponent<BrightnessContrastGamma>();
		this.pixelFX2 = this.computerCameraUse.gameObject.GetComponent<PixelMatrix>();
		this.generateLaunchHack();
		this.MaskTheIP();
	}

	private void Update()
	{
		if (this.checkLaunchHack && Time.time - this.launchHackTimeStamp >= this.launchHackWindowTime)
		{
			if (!this.myTimeManager.freezeTime && !GameManager.GetTheBreatherManager().isPlayerInBreatherAction())
			{
				this.checkLaunchHack = false;
				this.launchHack();
				this.generateLaunchHack();
			}
			else
			{
				this.checkLaunchHack = false;
				this.generateLaunchHack();
			}
		}
	}

	public bool hackActive;

	public bool ipIsMasked;

	public bool modemCoolOff;

	[Range(0f, 10f)]
	public float launchHackOpen = 2f;

	[Range(2f, 20f)]
	public float launchHackClose = 10f;

	[Range(1f, 10f)]
	public int maskIPOpen = 8;

	[Range(2f, 20f)]
	public int maskIPClose = 16;

	public mainController myMainController;

	public TheCloud myCould;

	public TimeManager myTimeManager;

	public PauseManager myPauseManager;

	public GameObject Desktop;

	public GameObject HackerDesktop;

	public GameObject ConDesktop;

	public GameObject SkullObject;

	public GameObject DOSAttackHolder;

	public GameObject KAttackHolder;

	public GameObject VAttackHolder;

	public Image SkullBottom;

	public Camera computerCamera;

	public Camera computerCameraUse;

	[Range(0f, 8f)]
	public int daysToLooseMax = 8;

	public AudioClip hackingSFX1;

	public AudioClip hackingSFX2;

	public AudioClip DOSAttack1;

	public AudioClip DOSAttack2;

	public AudioClip DOSAttack3;

	public AudioClip DOSAttack4;

	public DOSAttack myDOSAttack;

	public GameObject DOSNodeHolder;

	public Image DOSPrompt1;

	public Image DOSPrompt2;

	public Image DOSPrompt3;

	public GameObject DOSAttackBlockedObject;

	public GameObject DOSAttackHackedObject;

	public GameObject DOSAttackHackedLostObject;

	public Text DOSAttackHackedLostText;

	public KAttack myKAttack;

	public Image KPrompt1;

	public Image KPrompt2;

	public Image KPrompt3;

	public GameObject KAttackBlockedObject;

	public GameObject KAttackHackedObject;

	public GameObject KAttackHackedLostObject;

	public Text KAttackHackedLostText;

	public vapeAttack myVapeAttack;

	public Image VPrompt1;

	public Image VPrompt2;

	public Image VPrompt3;

	public GameObject VAttackBlockedObject;

	public GameObject VAttackHackedObject;

	public GameObject VAttackHackedLostObject;

	public Text VAttackHackedLostText;

	private Sequence hackingAniSeq;

	private Sequence hackingLoadingAniSeq;

	private Sequence DOSAniSeq;

	private Sequence KAniSeq;

	private Sequence VAniSeq;

	private Glitch glitchFX;

	private Glitch glitchFX2;

	private AnalogTV analogFX;

	private AnalogTV analogFX2;

	private BrightnessContrastGamma brightFX;

	private BrightnessContrastGamma brightFX2;

	private PixelMatrix pixelFX;

	private PixelMatrix pixelFX2;

	private float launchHackTimeStamp;

	private float launchHackWindowTime;

	private float unMaskIPTimeStamp;

	private float unMaskIPWindowTime;

	private bool checkLaunchHack;

	private bool inTutorialMode;

	private bool wasATwitchAttack;

	public Action tutorialPass;

	public Action tutorialFail;

	private bool tutorialAttackPassed;

	private string tutorialAttackType;

	private string twitchAttackType;

	private string twitchAttackLevel;

	private string twitchHackerName;
}
