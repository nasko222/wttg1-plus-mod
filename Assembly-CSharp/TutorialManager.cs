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
		this.callSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.aniIdleCall));
		this.callSeq.Insert(0f, DOTween.To(() => this.TutorialHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TutorialHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f).SetEase(Ease.Linear));
		this.callSeq.Insert(0.5f, DOTween.To(() => this.CallWindow.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.CallWindow.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.25f).SetEase(Ease.OutSine));
		this.callSeq.Insert(0.5f, DOTween.To(() => this.CallWindow.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.CallWindow.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.OutSine));
		this.callSeq.Play<Sequence>();
	}

	private void aniIdleCall()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.phoneRingClip, 0.7f, true);
		this.idleSeq = DOTween.Sequence();
		this.idleSeq.Insert(0f, DOTween.To(() => this.AdamPic.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.AdamPic.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(0.9f, 0.9f, 0.9f), 0.75f).SetEase(Ease.InSine));
		this.idleSeq.Insert(0.75f, DOTween.To(() => this.AdamPic.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.AdamPic.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f).SetEase(Ease.OutSine));
		this.idleSeq.SetLoops(-1);
		this.idleSeq.Play<Sequence>();
	}

	private void acceptCall()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.phoneRingClip.name);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.acceptCallClip, 0.7f, false);
		this.idleSeq.Kill(false);
		this.callSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.processTutorialStep));
		this.callSeq.Insert(0f, DOTween.To(() => this.CallWindow.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.CallWindow.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(1.25f, 1.25f, 1.25f), 0.25f).SetEase(Ease.OutSine));
		this.callSeq.Insert(0f, DOTween.To(() => this.CallWindow.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.CallWindow.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.OutSine));
		this.callSeq.Play<Sequence>();
	}

	private void declineCall()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.phoneRingClip.name);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.declineCallClip, 0.7f, false);
		this.idleSeq.Kill(false);
		this.callSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.disableTutorial));
		this.callSeq.Insert(0f, DOTween.To(() => this.CallWindow.GetComponent<RectTransform>().localScale, delegate(Vector3 x)
		{
			this.CallWindow.GetComponent<RectTransform>().localScale = x;
		}, new Vector3(0.1f, 0.1f, 0.1f), 0.25f).SetEase(Ease.OutSine));
		this.callSeq.Insert(0f, DOTween.To(() => this.CallWindow.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.CallWindow.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.OutSine));
		this.callSeq.Insert(0.25f, DOTween.To(() => this.TutorialHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TutorialHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear));
		this.callSeq.Play<Sequence>();
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
				this.stepTextSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.endCurrentTutorialStep));
				this.stepTextSeq.Insert(0f, DOTween.To(() => this.currentStepTextLen, delegate(int x)
				{
					this.currentStepTextLen = x;
				}, this.tutorialSteps[this.currentStepIndex].theStepText.Length, this.tutorialSteps[this.currentStepIndex].theStepAudio.length).SetEase(Ease.Linear));
				this.stepTextSeq.Play<Sequence>();
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
		this.callSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.disableTutorial));
		this.callSeq.Insert(0f, DOTween.To(() => this.TutorialHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TutorialHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear));
		this.callSeq.Play<Sequence>();
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
		UnityEngine.Object.Destroy(this.TutorialHolder);
		for (int i = 0; i < this.tutorialImages.Count; i++)
		{
			UnityEngine.Object.Destroy(this.tutorialImages[i].gameObject);
		}
		this.tutorialActive = true;
		this.tutorialImages.Clear();
		this.tutorialSteps.Clear();
		this.stepIndex = 0;
		this.currentStepIndex = 0;
		this.stepUpdateTextActive = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void showTutorialImage(int imageIndex)
	{
		this.actionIMGSeq = DOTween.Sequence();
		this.actionIMGSeq.Insert(0f, DOTween.To(() => this.tutorialImages[imageIndex].color, delegate(Color x)
		{
			this.tutorialImages[imageIndex].color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.3f).SetEase(Ease.Linear));
		this.actionIMGSeq.Play<Sequence>();
	}

	private void clearLastImage(int imageIndex)
	{
		this.actionIMGSeq.Kill(false);
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
		this.stepTextSeq.Kill(false);
		this.currentStepTextLen = 0;
		this.stepTextSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.endCurrentTutorialStep));
		this.stepTextSeq.Insert(0f, DOTween.To(() => this.currentStepTextLen, delegate(int x)
		{
			this.currentStepTextLen = x;
		}, this.failedAttackText.Length, this.failedAttackAC.length).SetEase(Ease.Linear));
		this.stepTextSeq.Play<Sequence>();
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
			UnityEngine.Object.Destroy(this.TutorialHolder);
			UnityEngine.Object.Destroy(base.gameObject);
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
			UnityEngine.Object.Destroy(this.TutorialHolder);
			UnityEngine.Object.Destroy(base.gameObject);
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
					this.stepTextSeq.Kill(false);
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
