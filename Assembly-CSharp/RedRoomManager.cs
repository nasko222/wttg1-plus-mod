using System;
using System.Collections.Generic;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class RedRoomManager : MonoBehaviour
{
	private void prepRoom()
	{
		if (this.sEnding)
		{
			this.playerHubObject.transform.SetParent(this.redController.transform);
			this.playerHubObject.transform.localPosition = new Vector3(0f, 0.862f, 0.044f);
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.playMusic));
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.startRedEndAni));
		}
		else
		{
			this.fadeScreenIn();
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.playMusic));
			GameManager.TimeSlinger.FireTimer(this.screenFadeTime, new Action(this.fadeStatsIn));
			GameManager.TimeSlinger.FireTimer(this.screenFadeTime + 1f, new Action(this.startStatInfo));
			GameManager.TimeSlinger.FireTimer(this.screenFadeTime + 16f, new Action(this.enableRedRoomAni));
		}
	}

	private void fadeStatsIn()
	{
		this.statFaderSeq = DOTween.Sequence();
		this.statFaderSeq.Insert(0f, DOTween.To(() => this.StatHolder.alpha, delegate(float x)
		{
			this.StatHolder.alpha = x;
		}, 1f, 1f).SetEase(Ease.Linear));
		this.statFaderSeq.Play<Sequence>();
	}

	private void fadeScreenIn()
	{
		this.screenFadeSeq = DOTween.Sequence();
		this.screenFadeSeq.Insert(0f, DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 0f, this.screenFadeTime).SetEase(Ease.Linear));
		this.screenFadeSeq.Play<Sequence>();
	}

	private void fadeScreenOut()
	{
		this.screenFadeSeq = DOTween.Sequence();
		this.screenFadeSeq.Insert(0f, DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 1f, this.screenFadeTime).SetEase(Ease.Linear));
		this.screenFadeSeq.Play<Sequence>();
	}

	private void aniRec()
	{
		this.recSeq = DOTween.Sequence();
		this.recSeq.Insert(0f, DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 0.2f), 1f).SetEase(Ease.Linear));
		this.recSeq.Insert(1f, DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 1f).SetEase(Ease.Linear));
		this.recSeq.SetLoops(-1);
		this.recSeq.Play<Sequence>();
	}

	private void startStatInfo()
	{
		this.aniRec();
		this.addViewerCount();
	}

	private void addViewerCount()
	{
		if (this.viewerCount > 9000)
		{
			this.viewerCountText.text = "9,057";
		}
		else
		{
			this.viewerCountText.text = this.viewerCount.ToString("N0");
			this.viewerCountWindow = UnityEngine.Random.Range(1f, 5f);
			this.viewerCountTimeStamp = Time.time;
			this.viewerCountActive = true;
			this.viewerCount += UnityEngine.Random.Range(4, 25);
		}
	}

	private void prepBuffer()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bufferClip, 0.75f, false);
		this.cameraGlitch.enabled = true;
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.showBuffer));
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.RRBG, 0f);
		GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
	}

	private void showBuffer()
	{
		this.cameraGlitch.enabled = false;
		this.StatHolder.alpha = 0f;
		this.ScreenFadeHolder.alpha = 1f;
		this.BufferHolder.alpha = 1f;
		this.bufferSeq = DOTween.Sequence();
		this.bufferSeq.Insert(0f, DOTween.To(() => this.BufferHolder.alpha, delegate(float x)
		{
			this.BufferHolder.alpha = x;
		}, 0.1f, 0.5f).SetEase(Ease.Linear));
		this.bufferSeq.Insert(0.5f, DOTween.To(() => this.BufferHolder.alpha, delegate(float x)
		{
			this.BufferHolder.alpha = x;
		}, 1f, 0.5f).SetEase(Ease.Linear));
		this.bufferSeq.SetLoops(-1);
		this.bufferSeq.Play<Sequence>();
		GameManager.TimeSlinger.FireTimer(this.setBufferTime, new Action(this.hideBuffer));
	}

	private void hideBuffer()
	{
		GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.MUSIC);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.RRBG);
		this.bufferSeq.Kill(false);
		this.StatHolder.alpha = 1f;
		this.BufferHolder.alpha = 0f;
		this.ScreenFadeHolder.alpha = 0f;
	}

	private void startEndAni()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.VICTIM, AudioLayer.SFX, this.slapClip, 0.2f, false, 10.4f);
		GameManager.AudioSlinger.DealSound(AudioHubs.VICTIM, AudioLayer.VOICE, this.vicLine1, 1f, false, 14f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.wakeyWakey, 0.5f, false, 11.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.exeLine1, 0.8f, false, 20.41f);
		this.setBufferTime = 5f;
		GameManager.TimeSlinger.FireTimer(30.41f, new Action(this.prepBuffer));
		GameManager.TimeSlinger.FireTimer(35.51f, new Action(this.endAniPart2));
		this.whistleSource.Play();
		this.trolley.GetComponent<AudioSource>().Play();
		this.trolleySeq = DOTween.Sequence().OnComplete(new TweenCallback(this.endAniPart1));
		this.trolleySeq.Insert(0f, DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyStartPoint, 1f).SetEase(Ease.Linear));
		this.trolleySeq.Play<Sequence>();
	}

	private void startRedEndAni()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.slapClip, 1f, false, 10.4f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.VOICE, this.vicLine1, 1f, false, 15f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.wakeyWakey, 0.7f, false, 11.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.redEXELine1, 0.45f, false, 20.41f);
		GameManager.TimeSlinger.FireTimer(12.5f, new Action(this.redLookUpAfterSlap));
		GameManager.TimeSlinger.FireTimer(10.6f, new Action(this.fadeScreenIn));
		GameManager.TimeSlinger.FireTimer(10.8f, new Action(this.redSeeStraight));
		GameManager.TimeSlinger.FireTimer(21f, new Action(this.redLookAtDoor));
		GameManager.TimeSlinger.FireTimer(28.6f, new Action(this.redPresentBreakFree));
		this.slapSeq = DOTween.Sequence();
		this.slapSeq.Insert(10.3f, DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(47.528f, -33.733f, -33.493f), 0.55f).SetEase(Ease.InCubic).SetOptions(true));
		this.slapSeq.Insert(10.85f, DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(65.02f, 0f, -0f), 0.8f).SetEase(Ease.InCubic).SetOptions(true));
		this.slapSeq.Play<Sequence>();
		this.whistleSource.Play();
		this.trolley.GetComponent<AudioSource>().Play();
		this.trolleySeq = DOTween.Sequence().OnComplete(new TweenCallback(this.redEndAniPart1));
		this.trolleySeq.Insert(0f, DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyStartPoint, 1f).SetEase(Ease.Linear));
		this.trolleySeq.Play<Sequence>();
	}

	private void redLookUpAfterSlap()
	{
		DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 2.6f).SetEase(Ease.InCubic);
		DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 2.6f).SetOptions(true).SetEase(Ease.InCubic);
	}

	private void endAniPart1()
	{
		this.exeAC.SetTrigger("startEnd1");
		this.vicAC.SetTrigger("triggerEnd1");
		this.trolleySeq = DOTween.Sequence();
		this.trolleySeq.Insert(0f, DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyEndPoint, 6.85f).SetEase(Ease.OutSine));
		this.trolleySeq.Play<Sequence>();
	}

	private void redEndAniPart1()
	{
		this.exeAC.SetTrigger("startRedEnd1");
		this.vicAC.SetTrigger("triggerRedEnd1");
		this.trolleySeq = DOTween.Sequence();
		this.trolleySeq.Insert(0f, DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyEndPoint, 6.85f).SetEase(Ease.OutSine));
		this.trolleySeq.Play<Sequence>();
	}

	private void endAniPart2()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.exeLine2, 0.45f, false, 0.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.VICTIM, AudioLayer.VOICE, this.vicLine2, 0.1f, false, 1.5f);
		this.setBufferTime = 5f;
		GameManager.TimeSlinger.FireTimer(5.3f, new Action(this.prepBuffer));
		GameManager.TimeSlinger.FireTimer(10.5f, new Action(this.endAniPart3));
		this.plyerTool.SetActive(true);
		this.exeAC.SetTrigger("startEnd2");
		this.vicAC.SetTrigger("triggerEnd2");
	}

	private void endAniPart3()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.SFX, this.cutterClip, 0.7f, false, 0.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.exeLine3, 1f, false, 0.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.vicLine3, 0.7f, false, 0.7f);
		Material[] materials = this.vicBody.GetComponent<Renderer>().materials;
		materials[1] = this.vicBloodyBody;
		this.vicBody.GetComponent<Renderer>().materials = materials;
		this.fingerBloodSpot.SetActive(true);
		this.vicFinger.SetActive(false);
		this.cutterTool.SetActive(true);
		this.trolleyCutter.SetActive(false);
		this.plyerTool.SetActive(false);
		this.setBufferTime = 5f;
		GameManager.TimeSlinger.FireTimer(2.3f, new Action(this.prepBuffer));
		GameManager.TimeSlinger.FireTimer(7.2f, new Action(this.endAniPart4));
		this.exeAC.SetTrigger("startEnd3");
		this.vicAC.SetTrigger("triggerEnd3");
	}

	private void endAniPart4()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.VICTIM, AudioLayer.VOICE, this.vicLine4, 0.8f, false, 0.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.SFX, this.eyeGougeClip, 0.5f, false, 0.5f);
		this.earBloodSpot.SetActive(true);
		this.vicEar.SetActive(false);
		this.cutterTool.SetActive(false);
		this.trolleyCutter.SetActive(true);
		this.setBufferTime = 7f;
		GameManager.TimeSlinger.FireTimer(2.1f, new Action(this.prepBuffer));
		GameManager.TimeSlinger.FireTimer(9.1f, new Action(this.endAniPart5));
		this.exeAC.SetTrigger("startEnd4");
		this.vicAC.SetTrigger("triggerEnd4");
	}

	private void endAniPart5()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.VICTIM, AudioLayer.VOICE, this.vicLine5, 0.8f, false, 0.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.exeLine4, 0.7f, false, 4.1f);
		GameManager.AudioSlinger.DealSound(AudioHubs.EXE, AudioLayer.VOICE, this.exeLine5, 0.7f, false, 15.1f);
		GameManager.TimeSlinger.FireTimer(20f, new Action(this.rollCredits));
		Material[] materials = this.vicBody.GetComponent<Renderer>().materials;
		materials[0] = this.vicBloodyHead;
		this.vicBody.GetComponent<Renderer>().materials = materials;
		this.vicEye.SetActive(false);
		this.exeObject.transform.localPosition = this.exeLastSpawnPOS;
		this.knifeTool.SetActive(true);
		this.exeAC.SetTrigger("startEnd5");
		this.vicAC.SetTrigger("triggerEnd5");
	}

	public void rollCredits()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.MOMENT, this.creditMusic, 0.2f, false);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.RRBG, 0f);
		GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
		GameManager.TimeSlinger.FireTimer(27f, new Action(this.enableQuit));
		this.StatHolder.alpha = 0f;
		this.ScreenFadeHolder.alpha = 1f;
		this.TheRedRoom.SetActive(false);
		this.titleSeq = DOTween.Sequence();
		this.titleSeq.Insert(3.1f, DOTween.To(() => this.TitleHolder.alpha, delegate(float x)
		{
			this.TitleHolder.alpha = x;
		}, 1f, 1f)).SetEase(Ease.Linear);
		this.titleSeq.Insert(14.8f, DOTween.To(() => this.TitleHolder.transform.localPosition, delegate(Vector3 x)
		{
			this.TitleHolder.transform.localPosition = x;
		}, new Vector3(0f, (float)Screen.height / 2f - 105f, 0f), 5f)).SetEase(Ease.Linear).SetRelative(true);
		this.titleSeq.Insert(18.8f, DOTween.To(() => this.CreditsHolder.alpha, delegate(float x)
		{
			this.CreditsHolder.alpha = x;
		}, 1f, 3f)).SetEase(Ease.Linear);
		this.titleSeq.Insert(26.8f, DOTween.To(() => this.QuitHolder.alpha, delegate(float x)
		{
			this.QuitHolder.alpha = x;
		}, 1f, 1f)).SetEase(Ease.Linear);
		this.titleSeq.Play<Sequence>();
	}

	private void enableQuit()
	{
		this.canQuitToMainMenu = true;
	}

	private void enableRedRoomAni()
	{
		this.startRedRoomAni = true;
	}

	private void redSeeStraight()
	{
		DOTween.To(() => this.cameraDV.Amount, delegate(float x)
		{
			this.cameraDV.Amount = x;
		}, 0f, 5f).SetEase(Ease.Linear);
	}

	private void redLookAtDoor()
	{
		DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-1.781f, -48.215f, 3.419f), 1f).SetOptions(true).SetEase(Ease.OutSine);
		DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.549f, 0f, 0f), 1f).SetOptions(true).SetEase(Ease.InSine).SetDelay(1.25f);
		DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 0.876f, 0.395f), 0.75f).SetEase(Ease.OutSine).SetDelay(2.85f);
		DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(50.585f, 0f, 0f), 0.75f).SetEase(Ease.OutSine).SetDelay(2.85f);
		DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(47.886f, -27.142f, -20.499f), 0.75f).SetOptions(true).SetEase(Ease.Linear).SetDelay(4f);
		DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(48.668f, 20.567f, 15.641f), 0.75f).SetOptions(true).SetEase(Ease.Linear).SetDelay(5f);
		DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 1.2f).SetEase(Ease.Linear).SetDelay(6f);
		DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 1.2f).SetOptions(true).SetEase(Ease.Linear).SetDelay(6f);
	}

	private void redPresentBreakFree()
	{
		DOTween.To(() => this.breakFreeHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.breakFreeHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.Linear);
		this.breakSeq = DOTween.Sequence();
		this.breakSeq.Insert(0f, DOTween.To(() => this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear));
		this.breakSeq.Insert(0.2f, DOTween.To(() => this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		this.breakSeq.SetLoops(-1);
		this.breakSeq.Play<Sequence>();
		this.redCheckClickCountTimeStamp = Time.time;
		this.redTotalClickCheckTimeStamp = Time.time;
		this.currentClickCount = 0;
		this.totalClickCount = 0;
		this.redCheckClickCount = true;
		this.redTotalClickCheck = true;
	}

	private void redDismissBreakFree()
	{
		if (this.attemptBreakSeq != null)
		{
			this.attemptBreakSeq.Kill(true);
		}
		if (this.breakSeq != null)
		{
			this.breakSeq.Kill(false);
		}
		DOTween.To(() => this.breakFreeHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.breakFreeHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear);
		if (this.totalClickCount >= 100)
		{
			GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.redTriggerBreakFree));
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.redTriggerGameOver));
		}
	}

	private void redTriggerAttemptBreakAni()
	{
		this.attemptBreakSeq = DOTween.Sequence();
		this.attemptBreakSeq.Insert(0f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.12f, 0f), 0.2f).SetRelative(true).SetEase(Ease.InOutBounce));
		this.attemptBreakSeq.Insert(0.2f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.12f, 0f), 0.2f).SetRelative(true).SetEase(Ease.InOutBounce));
		this.attemptBreakSeq.Insert(0.1f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.02f, 0f, 0f), 0.2f).SetRelative(true).SetEase(Ease.Linear));
		this.attemptBreakSeq.Insert(0.3f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0.02f, 0f, 0f), 0.2f).SetRelative(true).SetEase(Ease.Linear));
		this.attemptBreakSeq.SetLoops(-1);
		this.attemptBreakSeq.Play<Sequence>();
	}

	private void redTriggerBreakFree()
	{
		this.vicObject.SetActive(false);
		this.leftArmStrap.SetActive(false);
		this.rightArmStrap.SetActive(false);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.leatherSnap, 0.5f, false);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.leatherSnap, 0.5f, false, 2.3f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bareFootStep1, 0.5f, false, 5.85f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bareFootStep2, 0.5f, false, 6.1f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bareFootStep3, 0.5f, false, 6.35f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bareFootStep4, 0.5f, false, 6.6f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bareFootStep1, 0.5f, false, 6.85f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bareFootStep2, 0.5f, false, 7.1f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.doorOpenSFX, 1f, false, 7.6f);
		GameManager.TimeSlinger.FireTimer(7.2f, new Action(this.fadeScreenOut));
		GameManager.TimeSlinger.FireTimer(9.5f, new Action(this.redGotoRWSEnding));
		this.breakFreeSeq = DOTween.Sequence();
		this.breakFreeSeq.Insert(0f, DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(21.903f, -18.819f, -4.422f), 0.65f).SetEase(Ease.OutSine).SetOptions(true));
		this.breakFreeSeq.Insert(0f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0.199f, 1.223f, -0.425f), 1.6f).SetEase(Ease.OutBack));
		this.breakFreeSeq.Insert(1.7f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 0.6f).SetEase(Ease.InSine));
		this.breakFreeSeq.Insert(1.7f, DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 0.6f).SetOptions(true).SetEase(Ease.InSine));
		this.breakFreeSeq.Insert(2.3f, DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(21.686f, 32.111f, -0.791f), 0.65f).SetEase(Ease.OutSine).SetOptions(true));
		this.breakFreeSeq.Insert(2.3f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.226f, 1.184f, -0.105f), 1.6f).SetEase(Ease.OutBack));
		this.breakFreeSeq.Insert(4f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 0.6f).SetEase(Ease.InSine));
		this.breakFreeSeq.Insert(4f, DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 0.6f).SetOptions(true).SetEase(Ease.InSine));
		this.breakFreeSeq.Insert(4.6f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 1f, 0f), 0.6f).SetEase(Ease.InOutSine));
		this.breakFreeSeq.Insert(4.6f, DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, Vector3.zero, 0.6f).SetOptions(true).SetEase(Ease.InOutSine));
		this.breakFreeSeq.Insert(4.6f, DOTween.To(() => this.redController.transform.localPosition, delegate(Vector3 x)
		{
			this.redController.transform.localPosition = x;
		}, new Vector3(3.585f, 3.008f, 3.726f), 0.75f).SetEase(Ease.InOutBack));
		this.breakFreeSeq.Insert(5.35f, DOTween.To(() => this.redController.transform.localRotation, delegate(Quaternion x)
		{
			this.redController.transform.localRotation = x;
		}, new Vector3(0f, 2.954f, 0f), 0.3f).SetEase(Ease.InSine).SetOptions(true));
		this.breakFreeSeq.Insert(5.85f, DOTween.To(() => this.redController.transform.localPosition, delegate(Vector3 x)
		{
			this.redController.transform.localPosition = x;
		}, new Vector3(2.846f, 3.008f, 20.431f), 1.5f).SetEase(Ease.Linear));
		this.breakFreeSeq.Insert(5.85f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.45f, 0f), 0.25f).SetEase(Ease.Linear).SetRelative(true));
		this.breakFreeSeq.Insert(6.1f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.45f, 0f), 0.25f).SetEase(Ease.Linear).SetRelative(true));
		this.breakFreeSeq.Insert(6.35f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.45f, 0f), 0.25f).SetEase(Ease.Linear).SetRelative(true));
		this.breakFreeSeq.Insert(6.6f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.45f, 0f), 0.25f).SetEase(Ease.Linear).SetRelative(true));
		this.breakFreeSeq.Insert(6.85f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.45f, 0f), 0.25f).SetEase(Ease.Linear).SetRelative(true));
		this.breakFreeSeq.Insert(7.1f, DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.45f, 0f), 0.25f).SetEase(Ease.Linear).SetRelative(true));
		this.breakFreeSeq.Play<Sequence>();
	}

	private void redTriggerGameOver()
	{
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.EXE, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.RRBG, 0f);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PLAYER, this.redCameraMusic.name);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.bufferClip, 0.75f, false);
		this.ScreenFadeHolder.alpha = 1f;
		this.redGameOverHolder.SetActive(true);
		GameManager.TimeSlinger.FireTimer(4f, new Action(this.gotoMainMenu));
	}

	private void gotoMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	private void redGotoRWSEnding()
	{
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.EXE, 0f, 0.5f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.RRBG, 0f);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PLAYER, this.redCameraMusic.name);
		SceneManager.LoadScene(3);
	}

	private void unlockAch()
	{
		if (this.sEnding)
		{
			GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_WHERE_AM_I, true);
		}
		else
		{
			GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_REDRUM, true);
		}
		if (!GameManager.FileSlinger.saveData.playerResetStats)
		{
			GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_1337, true);
		}
		if (GameManager.FileSlinger.saveData.gameDay <= 7)
		{
			GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_DEEP_WEB_OUTLAW, true);
		}
	}

	private void playMusic()
	{
		if (this.sEnding)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.MUSIC, this.redCameraMusic, 0.3f, true);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.MUSIC, this.iBedMusic, 0.15f, true);
		}
	}

	private void Awake()
	{
		if (GameManager.FileSlinger.saveData.siteRateDisabled)
		{
			this.sEnding = true;
		}
		else
		{
			this.sEnding = false;
		}
		if (this.sEnding)
		{
			this.mainCamera.enabled = false;
			this.mainCamera.GetComponent<AudioListener>().enabled = false;
			this.redCamera.enabled = true;
			this.vicLeftEye.SetActive(false);
			this.vicLeftEyeLashLower.SetActive(false);
			this.vicLeftEyeLashUpper.SetActive(false);
			this.vicLeftEyeShell.SetActive(false);
			this.vicRightEye.SetActive(false);
			this.vicRightEyeLashLower.SetActive(false);
			this.vicRightEyeLashUpper.SetActive(false);
			this.vicRightEyeShell.SetActive(false);
			this.vicUpperTeeth.SetActive(false);
			this.vicLowerTeeth.SetActive(false);
			this.vicTongue.SetActive(false);
			this.vicHair.SetActive(false);
			this.cameraDV = this.redCamera.GetComponent<DoubleVision>();
		}
		else
		{
			GameManager.FileSlinger.deleteFile("wttg2.gd");
			this.redCamera.enabled = false;
			this.redCamera.GetComponent<AudioListener>().enabled = false;
		}
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		this.prepRoom();
		this.cameraGlitch = this.mainCamera.GetComponent<Glitch>();
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.unlockAch));
	}

	private void Update()
	{
		if (this.viewerCountActive && Time.time - this.viewerCountTimeStamp >= this.viewerCountWindow)
		{
			this.viewerCountActive = false;
			this.addViewerCount();
		}
		if (this.startRedRoomAni)
		{
			this.startRedRoomAni = false;
			this.startEndAni();
		}
		if (this.canQuitToMainMenu && Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(0);
		}
		if (this.redCheckClickCount)
		{
			if (CrossPlatformInputManager.GetButton("LeftClick") || CrossPlatformInputManager.GetButton("RightAlt"))
			{
				this.currentClickCount++;
			}
			if (Time.time - this.redCheckClickCountTimeStamp >= 0.5f)
			{
				if (this.currentClickCount >= 20)
				{
					if (this.attemptBreakSeq == null)
					{
						this.redTriggerAttemptBreakAni();
					}
					else if (!this.attemptBreakSeq.IsPlaying())
					{
						this.redTriggerAttemptBreakAni();
					}
					GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.leatherSFXS[UnityEngine.Random.Range(0, this.leatherSFXS.Count)], 0.1f, false);
					this.totalClickCount += this.currentClickCount;
				}
				else if (this.attemptBreakSeq != null)
				{
					this.attemptBreakSeq.Kill(true);
				}
				this.currentClickCount = 0;
				this.redCheckClickCountTimeStamp = Time.time;
			}
		}
		if (this.redTotalClickCheck && Time.time - this.redTotalClickCheckTimeStamp >= 6f)
		{
			this.redCheckClickCount = false;
			this.redTotalClickCheck = false;
			this.redDismissBreakFree();
		}
	}

	public bool sEnding;

	[Range(0.5f, 3.5f)]
	public float screenFadeTime = 1.5f;

	public Camera mainCamera;

	public Canvas UICanvas;

	public CanvasGroup ScreenFadeHolder;

	public CanvasGroup StatHolder;

	public CanvasGroup BufferHolder;

	public CanvasGroup TitleHolder;

	public CanvasGroup CreditsHolder;

	public CanvasGroup QuitHolder;

	public Image recLiveImage;

	public Text viewerCountText;

	public GameObject TheRedRoom;

	public Animator exeAC;

	public Animator vicAC;

	public GameObject exeObject;

	public GameObject vicObject;

	public Vector3 exeLastSpawnPOS;

	public GameObject trolley;

	public GameObject trolleyCutter;

	public Vector3 trolleyStartPoint;

	public Vector3 trolleyEndPoint;

	public GameObject plyerTool;

	public GameObject cutterTool;

	public GameObject knifeTool;

	public GameObject vicEar;

	public GameObject vicFinger;

	public GameObject vicEye;

	public GameObject vicBody;

	public Material vicBloodyBody;

	public Material vicBloodyHead;

	public GameObject fingerBloodSpot;

	public GameObject earBloodSpot;

	public AudioSource whistleSource;

	public AudioClip bufferClip;

	public AudioClip slapClip;

	public AudioClip cutterClip;

	public AudioClip eyeGougeClip;

	public AudioClip wakeyWakey;

	public AudioClip exeLine1;

	public AudioClip exeLine2;

	public AudioClip exeLine3;

	public AudioClip exeLine4;

	public AudioClip exeLine5;

	public AudioClip vicLine1;

	public AudioClip vicLine2;

	public AudioClip vicLine3;

	public AudioClip vicLine4;

	public AudioClip vicLine5;

	public AudioClip creditMusic;

	public GameObject redController;

	public Camera redCamera;

	public GameObject vicLeftEye;

	public GameObject vicLeftEyeLashLower;

	public GameObject vicLeftEyeLashUpper;

	public GameObject vicLeftEyeShell;

	public GameObject vicRightEye;

	public GameObject vicRightEyeLashLower;

	public GameObject vicRightEyeLashUpper;

	public GameObject vicRightEyeShell;

	public GameObject vicUpperTeeth;

	public GameObject vicLowerTeeth;

	public GameObject vicTongue;

	public GameObject vicHair;

	public GameObject breakFreeHolder;

	public GameObject leftClickIMG;

	public GameObject redGameOverHolder;

	public GameObject leftArmStrap;

	public GameObject rightArmStrap;

	public AudioClip redCameraMusic;

	public AudioClip redEXELine1;

	public List<AudioClip> leatherSFXS;

	public AudioClip leatherSnap;

	public AudioClip bareFootStep1;

	public AudioClip bareFootStep2;

	public AudioClip bareFootStep3;

	public AudioClip bareFootStep4;

	public AudioClip doorOpenSFX;

	public AudioClip iBedMusic;

	public GameObject playerHubObject;

	private Glitch cameraGlitch;

	private DoubleVision cameraDV;

	private Sequence screenFadeSeq;

	private Sequence statFaderSeq;

	private Sequence recSeq;

	private Sequence trolleySeq;

	private Sequence bufferSeq;

	private Sequence titleSeq;

	private Sequence slapSeq;

	private Sequence attemptBreakSeq;

	private Sequence breakSeq;

	private Sequence breakFreeSeq;

	private int viewerCount = 4587;

	private float viewerCountTimeStamp;

	private float viewerCountWindow;

	private float setBufferTime;

	private bool viewerCountActive;

	private bool canQuitToMainMenu;

	private bool startRedRoomAni;

	private bool redCheckClickCount;

	private bool redTotalClickCheck;

	private float redCheckClickCountTimeStamp;

	private float redTotalClickCheckTimeStamp;

	private int currentClickCount;

	private int totalClickCount;
}
