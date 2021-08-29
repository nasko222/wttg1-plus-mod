using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialManager : MonoBehaviour
{
	public void startTutorial()
	{
		GameManager.GetTheCloud().myTimeManager.freezeTime = true;
		this.myPauseManager.lockPause = true;
		this.TutorialHolder.SetActive(true);
		this.declineCallBTN.hasAction = true;
		this.declineCallBTN.setAction = new Action(this.declineCall);
		this.acceptCallBTN.hasAction = true;
		this.acceptCallBTN.setAction = new Action(this.acceptCall);
		this.callSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.aniIdleCall));
		TweenSettingsExtensions.Insert(this.callSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.TutorialHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TutorialHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.callSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.CallWindow.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.CallWindow.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.25f), 3));
		TweenSettingsExtensions.Insert(this.callSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.CallWindow.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.CallWindow.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f), 3));
		TweenExtensions.Play<Sequence>(this.callSeq);
	}

	private void aniIdleCall()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.phoneRingClip, 0.7f, true);
		this.idleSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.idleSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.AdamPic.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.AdamPic.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(0.9f, 0.9f, 0.9f), 0.75f), 2));
		TweenSettingsExtensions.Insert(this.idleSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.AdamPic.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.AdamPic.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f), 3));
		TweenSettingsExtensions.SetLoops<Sequence>(this.idleSeq, -1);
		TweenExtensions.Play<Sequence>(this.idleSeq);
	}

	private void acceptCall()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.phoneRingClip.name);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.acceptCallClip, 0.7f, false);
		TweenExtensions.Kill(this.idleSeq, false);
		this.callSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.processTutorialStep));
		TweenSettingsExtensions.Insert(this.callSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.CallWindow.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.CallWindow.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1.25f, 1.25f, 1.25f), 0.25f), 3));
		TweenSettingsExtensions.Insert(this.callSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.CallWindow.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.CallWindow.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f), 3));
		TweenExtensions.Play<Sequence>(this.callSeq);
	}

	private void declineCall()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.phoneRingClip.name);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.declineCallClip, 0.7f, false);
		TweenExtensions.Kill(this.idleSeq, false);
		this.callSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.disableTutorial));
		TweenSettingsExtensions.Insert(this.callSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.CallWindow.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.CallWindow.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(0.1f, 0.1f, 0.1f), 0.25f), 3));
		TweenSettingsExtensions.Insert(this.callSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.CallWindow.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.CallWindow.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f), 3));
		TweenSettingsExtensions.Insert(this.callSeq, 0.25f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.TutorialHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TutorialHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.callSeq);
	}

	private void processTutorialStep()
	{
		this.tutorialActive = true;
		this.CallWindow.SetActive(false);
		if (this.stepIndex < this.tutorialSteps.Count)
		{
			if (this.tutorialSteps[this.currentStepIndex].hasActionIMG)
			{
				this.clearLastImage(this.tutorialSteps[this.currentStepIndex].actionIMG);
			}
			this.currentStepIndex = this.stepIndex;
			this.currentStepTextLen = 0;
			this.tutorialStepText.text = string.Empty;
			if (this.tutorialSteps[this.currentStepIndex].hasAttack)
			{
				this.myHackerManager.tutorialPass = new Action(this.passedTutorialAttack);
				this.myHackerManager.tutorialFail = new Action(this.failedTutorialAttack);
				this.myHackerManager.launchTutorialHack(this.tutorialSteps[this.currentStepIndex].attackType);
			}
			else
			{
				if (this.tutorialSteps[this.currentStepIndex].hasActionIMG)
				{
					this.showTutorialImage(this.tutorialSteps[this.currentStepIndex].actionIMG);
				}
				this.stepTextSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.endCurrentTutorialStep));
				TweenSettingsExtensions.Insert(this.stepTextSeq, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(() => this.currentStepTextLen, delegate(int x)
				{
					this.currentStepTextLen = x;
				}, this.tutorialSteps[this.currentStepIndex].theStepText.Length, this.tutorialSteps[this.currentStepIndex].theStepAudio.length), 1));
				TweenExtensions.Play<Sequence>(this.stepTextSeq);
				this.stepUpdateTextActive = true;
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.tutorialSteps[this.currentStepIndex].theStepAudio, 1f, false);
				this.stepIndex++;
			}
		}
		else
		{
			this.endOfTutorial();
		}
	}

	private void endCurrentTutorialStep()
	{
		if (this.showFailedAttackText)
		{
			this.showFailedAttackText = false;
			this.stepUpdateTextActive = false;
			this.lockInput = false;
			this.tutorialStepText.text = this.failedAttackText;
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.failedAttackAC.name);
		}
		else
		{
			this.stepUpdateTextActive = false;
			this.lockInput = false;
			this.tutorialStepText.text = this.tutorialSteps[this.currentStepIndex].theStepText;
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.tutorialSteps[this.currentStepIndex].theStepAudio.name);
		}
	}

	private void endOfTutorial()
	{
		this.mainHub.setMeActive();
		this.leftHub.setMeActive();
		this.myChairScrub.activateMoveChair();
		this.myPauseManager.lockPause = false;
		this.callSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.disableTutorial));
		TweenSettingsExtensions.Insert(this.callSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.TutorialHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TutorialHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.callSeq);
		GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_GOOD_GUY_ADAM, true);
	}

	private void disableTutorial()
	{
		this.mainHub.setMeActive();
		this.leftHub.setMeActive();
		this.myChairScrub.activateMoveChair();
		GameManager.GetTheCloud().myTimeManager.freezeTime = false;
		this.myPauseManager.lockPause = false;
		this.TutorialHolder.SetActive(false);
		Object.Destroy(this.TutorialHolder);
		for (int i = 0; i < this.tutorialImages.Count; i++)
		{
			Object.Destroy(this.tutorialImages[i].gameObject);
		}
		this.tutorialActive = true;
		this.tutorialImages.Clear();
		this.tutorialSteps.Clear();
		this.stepIndex = 0;
		this.currentStepIndex = 0;
		this.stepUpdateTextActive = false;
		Object.Destroy(base.gameObject);
	}

	private void showTutorialImage(int imageIndex)
	{
		this.actionIMGSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.actionIMGSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.tutorialImages[imageIndex].color, delegate(Color x)
		{
			this.tutorialImages[imageIndex].color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.actionIMGSeq);
	}

	private void clearLastImage(int imageIndex)
	{
		TweenExtensions.Kill(this.actionIMGSeq, false);
		this.tutorialImages[imageIndex].color = new Color(1f, 1f, 1f, 0f);
	}

	private void passedTutorialAttack()
	{
		this.stepIndex++;
		this.processTutorialStep();
	}

	private void failedTutorialAttack()
	{
		this.showFailedAttackText = true;
		this.stepUpdateTextActive = true;
		TweenExtensions.Kill(this.stepTextSeq, false);
		this.currentStepTextLen = 0;
		this.stepTextSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.endCurrentTutorialStep));
		TweenSettingsExtensions.Insert(this.stepTextSeq, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(() => this.currentStepTextLen, delegate(int x)
		{
			this.currentStepTextLen = x;
		}, this.failedAttackText.Length, this.failedAttackAC.length), 1));
		TweenExtensions.Play<Sequence>(this.stepTextSeq);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.failedAttackAC, 1f, false);
	}

	private void checkTutorialIsValid()
	{
		if (GameManager.GetTheCloud().myTimeManager.getDayCount() == 1)
		{
			this.startTutorial();
			this.lockInput = true;
		}
		else
		{
			this.mainHub.setMeActive();
			this.leftHub.setMeActive();
			this.myChairScrub.activateMoveChair();
			this.TutorialHolder.SetActive(false);
			Object.Destroy(this.TutorialHolder);
			Object.Destroy(base.gameObject);
		}
	}

	private void unlockInput()
	{
		this.lockInput = false;
	}

	private void Start()
	{
		if (GameManager.GetGameModeManager().getCasualMode())
		{
			this.mainHub.setMeActive();
			this.leftHub.setMeActive();
			this.myChairScrub.activateMoveChair();
			this.TutorialHolder.SetActive(false);
			Object.Destroy(this.TutorialHolder);
			Object.Destroy(base.gameObject);
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(4f, new Action(this.checkTutorialIsValid));
		}
	}

	private void Update()
	{
		if (this.tutorialActive)
		{
			bool flag = false;
			if (this.stepUpdateTextActive)
			{
				if (CrossPlatformInputManager.GetButtonDown("LeftClick"))
				{
					TweenExtensions.Kill(this.stepTextSeq, false);
					this.tutorialStepText.text = this.tutorialSteps[this.currentStepIndex].theStepText;
					this.stepUpdateTextActive = false;
					flag = true;
					GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.tutorialSteps[this.currentStepIndex].theStepAudio.name);
					GameManager.TimeSlinger.FireTimer(1f, new Action(this.unlockInput));
				}
				if (!flag)
				{
					if (this.showFailedAttackText)
					{
						this.tutorialStepText.text = this.failedAttackText.Substring(0, this.currentStepTextLen);
					}
					else
					{
						this.tutorialStepText.text = this.tutorialSteps[this.currentStepIndex].theStepText.Substring(0, this.currentStepTextLen);
					}
				}
			}
			else if (!this.lockInput && CrossPlatformInputManager.GetButtonDown("LeftClick"))
			{
				this.lockInput = true;
				this.processTutorialStep();
			}
		}
	}

	public HackerManager myHackerManager;

	public PauseManager myPauseManager;

	public AudioBox mainHub;

	public AudioBox leftHub;

	public charScrub myChairScrub;

	public GameObject TutorialHolder;

	public GameObject CallWindow;

	public GameObject AdamPic;

	public btnBehavior acceptCallBTN;

	public btnBehavior declineCallBTN;

	public Text tutorialStepText;

	public AudioClip phoneRingClip;

	public AudioClip acceptCallClip;

	public AudioClip declineCallClip;

	public string failedAttackText = string.Empty;

	public AudioClip failedAttackAC;

	public List<Image> tutorialImages;

	public List<TutorialStepDefinition> tutorialSteps;

	private Sequence callSeq;

	private Sequence idleSeq;

	private Sequence stepTextSeq;

	private Sequence actionIMGSeq;

	private int stepIndex;

	private int currentStepIndex;

	private int currentStepTextLen;

	private bool stepUpdateTextActive;

	private bool lockInput;

	private bool tutorialActive;

	private bool showFailedAttackText;
}
