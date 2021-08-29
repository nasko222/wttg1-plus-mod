using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class BreatherManager : MonoBehaviour
{
	public void releaseTheBreather()
	{
		if (!this.breatherWindowActive)
		{
			this.earlyBreather = true;
			this.callWindowActive = false;
			this.setBreatherWindow();
		}
	}

	public void triggerBreatherCount(int visitCount)
	{
		if (visitCount >= (int)this.creepCallCount && visitCount < (int)this.triggerCallCount)
		{
			if (!this.callWindowActive && !this.earlyBreather)
			{
				this.setCallWindow();
			}
		}
		else if (visitCount >= (int)this.triggerCallCount && !this.breatherWindowActive)
		{
			this.callWindowActive = false;
			this.setBreatherWindow();
		}
	}

	public void callWithWindowTime(float setTime)
	{
		this.setCallWindowSetTime(setTime);
	}

	public void callWithWindowTime(float setTime, bool addHangUp)
	{
		if (addHangUp)
		{
			if (!this.earlyBreather)
			{
				this.currentHangUpCount += 1;
				if (this.currentHangUpCount >= this.maxHangUps)
				{
					this.earlyBreather = true;
					this.callWindowActive = false;
					this.setBreatherWindow();
				}
				else
				{
					this.setCallWindowSetTime(setTime);
				}
			}
		}
		else
		{
			this.setCallWindowSetTime(setTime);
		}
	}

	public void endOfAniClip(string clipTriggerName)
	{
		if (clipTriggerName == "doorAppEnd")
		{
			this.myMicManager.triggerPlayerListen();
			DOTween.To(() => this.BreatherObject.transform.localPosition, delegate(Vector3 x)
			{
				this.BreatherObject.transform.localPosition = x;
			}, this.doorPOS, 0.5f).SetEase(Ease.Linear);
		}
		else if (clipTriggerName == "seenEnterAgrMode")
		{
			this.setBreachAttempts();
		}
		else if (clipTriggerName == "walkAway")
		{
			this.setBreatherWindow();
			this.inBreatherAction = false;
		}
		else if (clipTriggerName == "windowWalkAway")
		{
			this.SetBackToDefaultSpawn();
			this.masterLock = false;
		}
	}

	public void playerWentToDoor()
	{
		if (this.inBreatherAction && this.breatherReactionActive)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.VOICE, this.Breathing, 0.01f, true);
			this.breatherReactionActive = false;
			this.BreatherObject.transform.localPosition = this.preDoorPOS;
			this.BreatherObject.transform.localRotation = Quaternion.Euler(this.preDoorROT);
			GameManager.TimeSlinger.FireTimer(5.5f, new Action(this.triggerPreDoorSeq));
			GameManager.TimeSlinger.FireTimer(14.2f, new Action(this.setDoorKnobAttempts));
		}
	}

	public bool isPlayerInBreatherAction()
	{
		return this.inBreatherAction;
	}

	public void sayNightNight()
	{
		GameManager.TimeSlinger.KillTimerWithID("forceNightNight");
		this.BreatherAC.SetTrigger("breatherNightNight");
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.MUSIC, this.BreatherJump1, 1f, false, 0.3f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.VOICE, this.NightNight, 0.35f, false, 1.2f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.KnifeStab2, 0.75f, false, 3.8f);
		GameManager.TimeSlinger.FireTimer(4f, new Action(this.triggerGameOver));
	}

	private void triggerPreDoorSeq()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep1, 0.15f, false, 3.43f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep2, 0.1725f, false, 4.1f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep3, 0.2f, false, 4.66f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep4, 0.225f, false, 5.3f);
		this.BreatherAC.SetTrigger("breatherToDoor");
	}

	private void setCallWindow()
	{
		this.callTimeWindow = UnityEngine.Random.Range(this.callWindowOpen, this.callWindowClose) * 60f;
		this.callTimeStamp = Time.time;
		this.callWindowActive = true;
	}

	private void setCallWindowSetTime(float setTime)
	{
		this.callTimeWindow = setTime;
		this.callTimeStamp = Time.time;
		this.callWindowActive = true;
	}

	private void setBreatherWindow()
	{
		this.breatherTimeWindow = UnityEngine.Random.Range(this.breatherWindowOpen, this.breatherWindowClose) * 60f;
		this.breatherTimeStamp = Time.time;
		this.breatherWindowActive = true;
	}

	private void setBreatherWindow(float setTime)
	{
		this.breatherTimeWindow = setTime;
		this.breatherTimeStamp = Time.time;
		this.breatherWindowActive = true;
	}

	private void triggerCall()
	{
		if (GameManager.GetTheCloud().getRedRoomKeyVistCount() >= (int)this.creepCallCount && GameManager.GetTheCloud().getRedRoomKeyVistCount() < (int)this.threatCallCount)
		{
			GameManager.GetThePhoneManager().setIncomingCall(this.creepAudioClips[UnityEngine.Random.Range(0, this.creepAudioClips.Count)]);
		}
		else if (GameManager.GetTheCloud().getRedRoomKeyVistCount() >= (int)this.threatCallCount && GameManager.GetTheCloud().getRedRoomKeyVistCount() < (int)this.triggerCallCount)
		{
			GameManager.GetThePhoneManager().setIncomingCall(this.threatAudioClips[UnityEngine.Random.Range(0, this.threatAudioClips.Count)]);
		}
		this.triggerBreatherCount(GameManager.GetTheCloud().getRedRoomKeyVistCount());
	}

	private void triggerBreatherReact()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.FRONT, AudioLayer.SFX, this.soundCues[UnityEngine.Random.Range(0, this.soundCues.Count)], 0.25f, false);
		this.setReactionWindowTime = this.breatherReactionWindow;
		this.inBreatherAction = true;
		this.breatherReactionTimeStamp = Time.time;
		this.breatherReactionActive = true;
	}

	private void setDoorKnobAttempts()
	{
		this.currentKnobAttemptIndex = (short)UnityEngine.Random.Range((int)this.knobAttemptsMin, (int)this.knobAttemptsMax);
		this.fireDoorKnobAttempt();
	}

	private void setBreachAttempts()
	{
		short num = (short)UnityEngine.Random.Range((int)this.breachAttemptsMin, (int)this.breachAttemptsMax);
		this.currentBreachAttemptIndex = num;
		this.inBreachMode = true;
		this.countClicks = true;
		this.microTimpStamp = Time.time;
		this.checkMicroTime = true;
	}

	private void stagePeaks()
	{
		short num = (short)UnityEngine.Random.Range(1, 4);
		for (short num2 = 0; num2 < num; num2 += 1)
		{
			GameManager.TimeSlinger.FireTimer((float)(num2 + 1) * 5f, new Action(this.firePeakAttempt), "breatherPeak" + num2);
		}
	}

	private void SetMicroTime()
	{
		this.microTimpStamp = Time.time;
		this.checkMicroTime = true;
	}

	private void SetCheckToBeSeen()
	{
		this.seenTimeStamp = Time.time;
		this.checkToBeSeen = true;
	}

	private void SetBackToDefaultSpawn()
	{
		this.BreatherObject.transform.localPosition = this.defaultSpawnPOS;
		this.BreatherObject.transform.localRotation = Quaternion.Euler(this.defaultSpawnROT);
	}

	private void fireDoorKnobAttempt()
	{
		if (this.currentKnobAttemptIndex > 0)
		{
			this.knobWindowTime = UnityEngine.Random.Range(this.knobWindowMin, this.knobWindowMax);
			GameManager.AudioSlinger.DealSound(AudioHubs.FRONT, AudioLayer.SFX, this.DoorKnob1, 0.15f, false);
			GameManager.TimeSlinger.FireTimer(1.5f, new Action(this.SetMicroTime));
			this.BreatherAC.SetTrigger("breatherTryKnob");
			DOTween.To(() => this.TheDoorKnob.transform.localRotation, delegate(Quaternion x)
			{
				this.TheDoorKnob.transform.localRotation = x;
			}, new Vector3(90f, 0f, 0f), 1.4f).SetEase(Ease.Linear).SetRelative(true);
			this.doorKnobTimeStamp = Time.time;
			this.doorKnobAction = true;
			this.currentKnobAttemptIndex -= 1;
		}
		else
		{
			this.triggerWalkAway();
		}
	}

	private void fireBreachAttempt()
	{
		this.countClicks = false;
		this.checkMicroTime = false;
		if (this.currentClickCount >= this.neededAmountOfClicks)
		{
			if (this.currentBreachAttemptIndex > 0)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.VOICE, this.gruntClips[UnityEngine.Random.Range(0, this.gruntClips.Count)], 0.25f, false);
				GameManager.AudioSlinger.DealSound(AudioHubs.FRONT, AudioLayer.SFX, this.BangDoor, 1f, false, 0.2f);
				GameManager.GetTheMainController().triggerBraceKnock();
				DOTween.To(() => this.TheDoor.transform.localPosition, delegate(Vector3 x)
				{
					this.TheDoor.transform.localPosition = x;
				}, new Vector3(-0.02f, 0f, 0f), 0.1f).SetEase(Ease.Linear).SetRelative(true).SetDelay(0.2f);
				DOTween.To(() => this.TheDoor.transform.localPosition, delegate(Vector3 x)
				{
					this.TheDoor.transform.localPosition = x;
				}, new Vector3(0.02f, 0f, 0f), 0.25f).SetEase(Ease.InQuad).SetRelative(true).SetDelay(0.4f);
				this.currentClickCount = 0;
				this.microTimpStamp = Time.time;
				this.countClicks = true;
				this.checkMicroTime = true;
				this.currentBreachAttemptIndex -= 1;
			}
			else
			{
				this.currentClickCount = 0;
				this.inBreachMode = false;
				this.triggerWalkAway();
				GameManager.GetTheUIManager().hideBrace();
				GameManager.GetTheMainController().unlockBraceMovement();
			}
		}
		else
		{
			GameManager.GetTheUIManager().hideBrace();
			this.triggerKnobDoom();
		}
	}

	private void firePeakAttempt()
	{
		this.BreatherAC.SetTrigger("breatherPeakIn");
		GameManager.TimeSlinger.FireTimer(0.8f, new Action(this.SetCheckToBeSeen));
	}

	private void triggerWalkAway()
	{
		this.myMicManager.stopPlayerListen();
		GameManager.TimeSlinger.FireTimer(5.15f, new Action(this.SetBackToDefaultSpawn));
		GameManager.GetTheSceneManager().clearAction();
		GameManager.AudioSlinger.RemoveSound(AudioHubs.BREATHER, this.Breathing.name);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep4, 0.225f, false, 0.7f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep3, 0.2f, false, 1.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep2, 0.1725f, false, 2.06f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.OUTSIDE, this.OutsideFootStep1, 0.15f, false, 2.7f);
		DOTween.To(() => this.BreatherObject.transform.localPosition, delegate(Vector3 x)
		{
			this.BreatherObject.transform.localPosition = x;
		}, this.preDoorPOS, 0.25f).SetEase(Ease.Linear);
		this.BreatherAC.SetTrigger("breatherWalkAway");
	}

	private void triggerSeenAgressiveMode()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.BREATHER, this.Breathing.name);
		this.myMicManager.stopPlayerListen();
		this.doorKnobAction = false;
		this.inBreachMode = true;
		this.countClicks = true;
		GameManager.TimeSlinger.KillTimerWithID("breatherPeak0");
		GameManager.TimeSlinger.KillTimerWithID("breatherPeak1");
		GameManager.TimeSlinger.KillTimerWithID("breatherPeak2");
		GameManager.TimeSlinger.KillTimerWithID("breatherKnobCoolOff");
		this.BreatherAC.SetTrigger("breatherSeenArgMode");
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.VOICE, this.SeeYouInThere, 0.15f, false);
		GameManager.GetTheMainController().triggerBraceMovement();
		GameManager.TimeSlinger.FireTimer(1f, new Action(GameManager.GetTheUIManager().showBrace));
	}

	private void triggerHearAgressiveMode()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.BREATHER, this.Breathing.name);
		this.myMicManager.stopPlayerListen();
		this.inBreachMode = true;
		this.countClicks = true;
		this.BreatherAC.SetTrigger("breatherSeenArgMode");
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.VOICE, this.HearYouInThere, 0.15f, false);
		GameManager.GetTheMainController().triggerBraceMovement();
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(GameManager.GetTheUIManager().showBrace));
	}

	private void triggerBreatherReactionDoom()
	{
		GameManager.FileSlinger.deleteFile("wttg2.gd");
		GameManager.GetTheUIManager().myPauseManager.lockPause = true;
		this.doomIsActive = true;
		if (GameManager.GetTheMainController().isUsingComputer)
		{
			this.triggerNightNight();
		}
		else
		{
			this.triggerBumRush();
		}
	}

	private void triggerKnobDoom()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.BREATHER, this.Breathing.name);
		GameManager.FileSlinger.deleteFile("wttg2.gd");
		GameManager.GetTheUIManager().myPauseManager.lockPause = true;
		this.doomIsActive = true;
		GameManager.TimeSlinger.KillTimerWithID("breatherPeak0");
		GameManager.TimeSlinger.KillTimerWithID("breatherPeak1");
		GameManager.TimeSlinger.KillTimerWithID("breatherPeak2");
		GameManager.TimeSlinger.KillTimerWithID("breatherKnobCoolOff");
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.MUSIC, this.BreatherJump1, 1f, false, 0.35f);
		GameManager.AudioSlinger.DealSound(AudioHubs.FRONT, AudioLayer.SFX, this.DoorSlamOpen, 0.9f, false, 0.35f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.InsideFootStep1, 0.4f, false, 1.26f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.InsideFootStep1, 0.5f, false, 1.9f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.VOICE, this.Laugh1, 0.65f, false, 2.8f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.InsideFootStep1, 0.8f, false, 5.2f);
		GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.InsideFootStep1, 0.9f, false, 5.7f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.KnifeStab1, 0.25f, false, 5.8f);
		this.BreatherAC.SetTrigger("breatherKickDoor");
		GameManager.GetTheMainController().triggerDoorKnockedDownAni();
		DOTween.To(() => this.TheDoor.transform.localRotation, delegate(Quaternion x)
		{
			this.TheDoor.transform.localRotation = x;
		}, new Vector3(0f, -271f, 0f), 0.165f).SetEase(Ease.Linear).SetOptions(true).SetDelay(0.35f);
		GameManager.TimeSlinger.FireTimer(6.4f, new Action(this.triggerGameOver));
	}

	private void triggerBumRush()
	{
		if (GameManager.GetTheMainController().isMovingAround)
		{
			GameManager.TimeSlinger.FireTimer(6f, new Action(this.triggerBumRush));
		}
		else
		{
			GameManager.GetTheMainController().triggerBumRush();
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.MUSIC, this.BreatherJump1, 0.7f, false, 0.2f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.VOICE, this.Laugh2, 1f, false, 0.2f);
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.KnifeStab1, 0.8f, false, 1.9f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.FastStep1, 1f, false, 0.23f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.FastStep2, 1f, false, 0.5f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.FastStep3, 1f, false, 0.73f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.FastStep4, 1f, false, 0.93f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.FastStep1, 1f, false, 1.2f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.FastStep2, 1f, false, 1.56f);
			GameManager.AudioSlinger.DealSound(AudioHubs.BREATHER, AudioLayer.SFX, this.FastStep3, 1f, false, 1.96f);
			this.BreatherObject.transform.localPosition = this.rushSpawnPOS;
			this.BreatherObject.transform.localRotation = Quaternion.Euler(this.rushSpawnROT);
			this.BreatherAC.SetTrigger("breatherBumRush");
			GameManager.TimeSlinger.FireTimer(2.7f, new Action(this.triggerGameOver));
		}
	}

	private void triggerNightNight()
	{
		GameManager.TimeSlinger.FireTimer(120f, new Action(GameManager.GetTheMainController().forceNightNight), "forceNightNight");
		GameManager.GetTheCloud().myTimeManager.freezeTime = true;
		GameManager.GetTheMainController().setNightNight();
		GameManager.GetTheTrackerManager().spoofTracker();
		this.BreatherObject.transform.localPosition = this.nightSpawnPOS;
		this.BreatherObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	private void noMicFound()
	{
	}

	private void playerDidTalk()
	{
		if (this.inBreatherAction)
		{
			this.doorKnobAction = false;
			this.checkMicroTime = false;
			this.playerIsHoldingKnob = false;
			GameManager.TimeSlinger.KillTimerWithID("breatherPeak0");
			GameManager.TimeSlinger.KillTimerWithID("breatherPeak1");
			GameManager.TimeSlinger.KillTimerWithID("breatherPeak2");
			GameManager.TimeSlinger.KillTimerWithID("breatherKnobCoolOff");
			this.triggerHearAgressiveMode();
		}
	}

	private void triggerGameOver()
	{
		GameManager.GetTheUIManager().triggerGameOver("You were killed...");
	}

	public void triggerBreatherWindowJump()
	{
		bool flag = true;
		if (!GameManager.GetTheHackerManager().myTimeManager.freezeTime && !this.masterLock && !this.breatherReactionActive && !this.inBreatherAction && !GameManager.GetTheTrackerManager().isPlayerInKidnapperAction() && !GameManager.GetTheMainController().isMovingAround && GameManager.GetTheMainController().isUsingComputer)
		{
			GameManager.GetTheMainController().windowCreep = true;
			flag = false;
			this.masterLock = true;
			this.BreatherObject.transform.localPosition = this.WindowPeakPOS;
			this.BreatherObject.transform.localRotation = Quaternion.Euler(this.WindowPeakROT);
		}
		if (flag)
		{
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.triggerBreatherWindowJump));
		}
	}

	public void triggerBreatherWindowAway()
	{
		this.BreatherAC.SetTrigger("breatherWindowWalkAway");
	}

	private void Awake()
	{
		GameManager.SetBreatherManager(this);
	}

	private void Start()
	{
		this.myMicManager.playerHasNoMic += this.noMicFound;
		this.myMicManager.playerDidTalk += this.playerDidTalk;
	}

	private void Update()
	{
		if (!GameManager.GetGameModeManager().getCasualMode() && !this.masterLock)
		{
			if (this.callWindowActive && Time.time - this.callTimeStamp >= this.callTimeWindow)
			{
				this.callWindowActive = false;
				this.triggerCall();
			}
			if (this.breatherWindowActive && Time.time - this.breatherTimeStamp >= this.breatherTimeWindow)
			{
				this.breatherWindowActive = false;
				if (!GameManager.GetTheTrackerManager().isPlayerInKidnapperAction())
				{
					if (!GameManager.GetTheSceneManager().isInDoorAction())
					{
						if (!GameManager.GetTheHackerManager().myTimeManager.freezeTime)
						{
							if (!GameManager.GetTheMainController().isMovingAround)
							{
								this.triggerBreatherReact();
							}
							else
							{
								this.setBreatherWindow(15f);
							}
						}
						else
						{
							this.setBreatherWindow(45f);
						}
					}
					else
					{
						this.setBreatherWindow(30f);
					}
				}
				else
				{
					this.setBreatherWindow(30f);
				}
			}
			if (this.breatherReactionActive && Time.time - this.breatherReactionTimeStamp >= this.setReactionWindowTime)
			{
				this.breatherReactionActive = false;
				this.triggerBreatherReactionDoom();
			}
			if (this.inBreatherAction)
			{
				if (this.doorKnobAction)
				{
					if (Time.time - this.doorKnobTimeStamp >= this.knobWindowTime)
					{
						this.checkMicroTime = false;
						this.doorKnobAction = false;
						this.BreatherAC.SetTrigger("breatherKnobCoolIdle");
						this.stagePeaks();
						GameManager.TimeSlinger.FireTimer(this.knobCoolOffTime, new Action(this.fireDoorKnobAttempt), "breatherKnobCoolOff");
					}
					if (this.checkMicroTime && Time.time - this.microTimpStamp >= 0.5f)
					{
						if (this.playerIsHoldingKnob)
						{
							DOTween.To(() => this.TheDoorKnob.transform.localRotation, delegate(Quaternion x)
							{
								this.TheDoorKnob.transform.localRotation = x;
							}, new Vector3(60f, 0f, 0f), 0.2f).SetEase(Ease.OutSine).SetRelative(true);
							DOTween.To(() => this.TheDoorKnob.transform.localRotation, delegate(Quaternion x)
							{
								this.TheDoorKnob.transform.localRotation = x;
							}, new Vector3(-60f, 0f, 0f), 0.2f).SetEase(Ease.InSine).SetRelative(true).SetDelay(0.2f);
							GameManager.AudioSlinger.DealSound(AudioHubs.FRONT, AudioLayer.SFX, this.DoorKnob2, 0.1f, false);
							this.microTimpStamp = Time.time;
							return;
						}
						GameManager.AudioSlinger.DealSound(AudioHubs.FRONT, AudioLayer.SFX, this.DoorKnob3, 0.2f, false);
						this.doorKnobAction = false;
						this.triggerKnobDoom();
						return;
					}
				}
				else if (this.inBreachMode)
				{
					if (this.countClicks && Input.GetMouseButton(0))
					{
						this.currentClickCount++;
					}
					if (this.checkMicroTime && Time.time - this.microTimpStamp >= this.breachCoolTime)
					{
						this.fireBreachAttempt();
						return;
					}
				}
				else if (!this.doomIsActive && this.checkToBeSeen)
				{
					if (this.playerCanBeSeen)
					{
						this.checkToBeSeen = false;
						this.triggerSeenAgressiveMode();
					}
					if (Time.time - this.seenTimeStamp >= 3f)
					{
						this.checkToBeSeen = false;
					}
				}
			}
		}
	}

	public MicManager myMicManager;

	public bool playerIsHoldingKnob;

	public bool playerCanBeSeen;

	public bool masterLock;

	[Range(1f, 7f)]
	public short creepCallCount = 2;

	[Range(1f, 7f)]
	public short threatCallCount = 4;

	[Range(1f, 7f)]
	public short triggerCallCount = 5;

	[Range(1f, 20f)]
	public short maxHangUps = 7;

	[Range(1f, 15f)]
	public float callWindowOpen = 8f;

	[Range(6f, 25f)]
	public float callWindowClose = 15f;

	[Range(1f, 15f)]
	public float breatherWindowOpen = 6f;

	[Range(6f, 25f)]
	public float breatherWindowClose = 15f;

	[Range(10f, 300f)]
	public float breatherReactionWindow = 15f;

	[Range(2f, 5f)]
	public short knobAttemptsMin = 2;

	[Range(4f, 10f)]
	public short knobAttemptsMax = 6;

	[Range(2f, 12f)]
	public float knobWindowMin = 2f;

	[Range(12f, 30f)]
	public float knobWindowMax = 12f;

	[Range(1f, 60f)]
	public float knobCoolOffTime = 30f;

	[Range(2f, 8f)]
	public short breachAttemptsMin = 2;

	[Range(8f, 20f)]
	public short breachAttemptsMax = 8;

	[Range(1f, 5f)]
	public float breachCoolTime = 1.5f;

	public int neededAmountOfClicks = 20;

	public GameObject BreatherObject;

	public Animator BreatherAC;

	public GameObject TheDoorKnob;

	public GameObject TheDoor;

	public Vector3 defaultSpawnPOS;

	public Vector3 defaultSpawnROT;

	public Vector3 preDoorPOS;

	public Vector3 preDoorROT;

	public Vector3 doorPOS;

	public Vector3 rushSpawnPOS;

	public Vector3 rushSpawnROT;

	public Vector3 nightSpawnPOS;

	public Vector3 WindowPeakPOS;

	public Vector3 WindowPeakROT;

	public Vector3 WindowPeakAwayPOS;

	public AudioClip BreatherJump1;

	public AudioClip BreatherJump2;

	public AudioClip BreatherJump3;

	public AudioClip DoorKnob1;

	public AudioClip DoorKnob2;

	public AudioClip DoorKnob3;

	public AudioClip DoorSlamOpen;

	public AudioClip BangDoor;

	public AudioClip InsideFootStep1;

	public AudioClip InsideFootStep2;

	public AudioClip FastStep1;

	public AudioClip FastStep2;

	public AudioClip FastStep3;

	public AudioClip FastStep4;

	public AudioClip KnifeStab1;

	public AudioClip KnifeStab2;

	public AudioClip Laugh1;

	public AudioClip Laugh2;

	public AudioClip NightNight;

	public AudioClip Breathing;

	public List<AudioClip> creepAudioClips;

	public List<AudioClip> threatAudioClips;

	public List<AudioClip> gruntClips;

	public List<AudioClip> soundCues;

	public AudioClip SeeYouInThere;

	public AudioClip HearYouInThere;

	public AudioClip OutsideFootStep1;

	public AudioClip OutsideFootStep2;

	public AudioClip OutsideFootStep3;

	public AudioClip OutsideFootStep4;

	private float callTimeStamp;

	private float callTimeWindow;

	private float breatherTimeStamp;

	private float breatherTimeWindow;

	private float breatherReactionTimeStamp;

	private float doorKnobTimeStamp;

	private float microTimpStamp;

	private float seenTimeStamp;

	private float knobWindowTime;

	private float setReactionWindowTime;

	private bool callWindowActive;

	private bool breatherWindowActive;

	private bool breatherReactionActive;

	private bool inBreatherAction;

	private bool doorKnobAction;

	private bool checkMicroTime;

	private bool doomIsActive;

	private bool checkToBeSeen;

	private bool inBreachMode;

	private bool countClicks;

	private bool earlyBreather;

	private short currentKnobAttemptIndex;

	private short currentBreachAttemptIndex;

	private short currentHangUpCount;

	private int currentClickCount;
}
