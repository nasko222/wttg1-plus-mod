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
		TweenSettingsExtensions.Insert(this.statFaderSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.StatHolder.alpha, delegate(float x)
		{
			this.StatHolder.alpha = x;
		}, 1f, 1f), 1));
		TweenExtensions.Play<Sequence>(this.statFaderSeq);
	}

	private void fadeScreenIn()
	{
		this.screenFadeSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.screenFadeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 0f, this.screenFadeTime), 1));
		TweenExtensions.Play<Sequence>(this.screenFadeSeq);
	}

	private void fadeScreenOut()
	{
		this.screenFadeSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.screenFadeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 1f, this.screenFadeTime), 1));
		TweenExtensions.Play<Sequence>(this.screenFadeSeq);
	}

	private void aniRec()
	{
		this.recSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.recSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 0.2f), 1f), 1));
		TweenSettingsExtensions.Insert(this.recSeq, 1f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 1f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.recSeq, -1);
		TweenExtensions.Play<Sequence>(this.recSeq);
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
			this.viewerCountWindow = Random.Range(1f, 5f);
			this.viewerCountTimeStamp = Time.time;
			this.viewerCountActive = true;
			this.viewerCount += Random.Range(4, 25);
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
		TweenSettingsExtensions.Insert(this.bufferSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.BufferHolder.alpha, delegate(float x)
		{
			this.BufferHolder.alpha = x;
		}, 0.1f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.bufferSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.BufferHolder.alpha, delegate(float x)
		{
			this.BufferHolder.alpha = x;
		}, 1f, 0.5f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.bufferSeq, -1);
		TweenExtensions.Play<Sequence>(this.bufferSeq);
		GameManager.TimeSlinger.FireTimer(this.setBufferTime, new Action(this.hideBuffer));
	}

	private void hideBuffer()
	{
		GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.MUSIC);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.RRBG);
		TweenExtensions.Kill(this.bufferSeq, false);
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
		this.trolleySeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.endAniPart1));
		TweenSettingsExtensions.Insert(this.trolleySeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyStartPoint, 1f), 1));
		TweenExtensions.Play<Sequence>(this.trolleySeq);
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
		TweenSettingsExtensions.Insert(this.slapSeq, 10.3f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(47.528f, -33.733f, -33.493f), 0.55f), 8), true));
		TweenSettingsExtensions.Insert(this.slapSeq, 10.85f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(65.02f, 0f, -0f), 0.8f), 8), true));
		TweenExtensions.Play<Sequence>(this.slapSeq);
		this.whistleSource.Play();
		this.trolley.GetComponent<AudioSource>().Play();
		this.trolleySeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.redEndAniPart1));
		TweenSettingsExtensions.Insert(this.trolleySeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyStartPoint, 1f), 1));
		TweenExtensions.Play<Sequence>(this.trolleySeq);
	}

	private void redLookUpAfterSlap()
	{
		TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 2.6f), 8);
		TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 2.6f), true), 8);
	}

	private void endAniPart1()
	{
		this.exeAC.SetTrigger("startEnd1");
		this.vicAC.SetTrigger("triggerEnd1");
		this.trolleySeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.trolleySeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyEndPoint, 6.85f), 3));
		TweenExtensions.Play<Sequence>(this.trolleySeq);
	}

	private void redEndAniPart1()
	{
		this.exeAC.SetTrigger("startRedEnd1");
		this.vicAC.SetTrigger("triggerRedEnd1");
		this.trolleySeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.trolleySeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.trolley.transform.localPosition, delegate(Vector3 x)
		{
			this.trolley.transform.localPosition = x;
		}, this.trolleyEndPoint, 6.85f), 3));
		TweenExtensions.Play<Sequence>(this.trolleySeq);
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
		TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Insert(this.titleSeq, 3.1f, DOTween.To(() => this.TitleHolder.alpha, delegate(float x)
		{
			this.TitleHolder.alpha = x;
		}, 1f, 1f)), 1);
		TweenSettingsExtensions.SetRelative<Sequence>(TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Insert(this.titleSeq, 14.8f, DOTween.To(() => this.TitleHolder.transform.localPosition, delegate(Vector3 x)
		{
			this.TitleHolder.transform.localPosition = x;
		}, new Vector3(0f, (float)Screen.height / 2f - 105f, 0f), 5f)), 1), true);
		TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Insert(this.titleSeq, 18.8f, DOTween.To(() => this.CreditsHolder.alpha, delegate(float x)
		{
			this.CreditsHolder.alpha = x;
		}, 1f, 3f)), 1);
		TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Insert(this.titleSeq, 26.8f, DOTween.To(() => this.QuitHolder.alpha, delegate(float x)
		{
			this.QuitHolder.alpha = x;
		}, 1f, 1f)), 1);
		TweenExtensions.Play<Sequence>(this.titleSeq);
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
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.cameraDV.Amount, delegate(float x)
		{
			this.cameraDV.Amount = x;
		}, 0f, 5f), 1);
	}

	private void redLookAtDoor()
	{
		TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-1.781f, -48.215f, 3.419f), 1f), true), 3);
		TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.549f, 0f, 0f), 1f), true), 2), 1.25f);
		TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 0.876f, 0.395f), 0.75f), 3), 2.85f);
		TweenSettingsExtensions.SetDelay<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(50.585f, 0f, 0f), 0.75f), 3), 2.85f);
		TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(47.886f, -27.142f, -20.499f), 0.75f), true), 1), 4f);
		TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(48.668f, 20.567f, 15.641f), 0.75f), true), 1), 5f);
		TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 1.2f), 1), 6f);
		TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 1.2f), true), 1), 6f);
	}

	private void redPresentBreakFree()
	{
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.breakFreeHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.breakFreeHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f), 1);
		this.breakSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.breakSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.breakSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.leftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.breakSeq, -1);
		TweenExtensions.Play<Sequence>(this.breakSeq);
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
			TweenExtensions.Kill(this.attemptBreakSeq, true);
		}
		if (this.breakSeq != null)
		{
			TweenExtensions.Kill(this.breakSeq, false);
		}
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.breakFreeHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.breakFreeHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f), 1);
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
		TweenSettingsExtensions.Insert(this.attemptBreakSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.12f, 0f), 0.2f), true), 31));
		TweenSettingsExtensions.Insert(this.attemptBreakSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.12f, 0f), 0.2f), true), 31));
		TweenSettingsExtensions.Insert(this.attemptBreakSeq, 0.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.02f, 0f, 0f), 0.2f), true), 1));
		TweenSettingsExtensions.Insert(this.attemptBreakSeq, 0.3f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0.02f, 0f, 0f), 0.2f), true), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.attemptBreakSeq, -1);
		TweenExtensions.Play<Sequence>(this.attemptBreakSeq);
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
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(21.903f, -18.819f, -4.422f), 0.65f), 3), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0.199f, 1.223f, -0.425f), 1.6f), 27));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 1.7f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 0.6f), 2));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 1.7f, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 0.6f), true), 2));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 2.3f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(21.686f, 32.111f, -0.791f), 0.65f), 3), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 2.3f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.226f, 1.184f, -0.105f), 1.6f), 27));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 4f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(-0.043f, 1.053f, 0.25f), 0.6f), 2));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 4f, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, new Vector3(-5.594f, 0f, 0f), 0.6f), true), 2));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 4.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 1f, 0f), 0.6f), 4));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 4.6f, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => this.redCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.redCamera.transform.localRotation = x;
		}, Vector3.zero, 0.6f), true), 4));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 4.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redController.transform.localPosition, delegate(Vector3 x)
		{
			this.redController.transform.localPosition = x;
		}, new Vector3(3.585f, 3.008f, 3.726f), 0.75f), 28));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 5.35f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.redController.transform.localRotation, delegate(Quaternion x)
		{
			this.redController.transform.localRotation = x;
		}, new Vector3(0f, 2.954f, 0f), 0.3f), 2), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 5.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redController.transform.localPosition, delegate(Vector3 x)
		{
			this.redController.transform.localPosition = x;
		}, new Vector3(2.846f, 3.008f, 20.431f), 1.5f), 1));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 5.85f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.45f, 0f), 0.25f), 1), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 6.1f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.45f, 0f), 0.25f), 1), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 6.35f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.45f, 0f), 0.25f), 1), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 6.6f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.45f, 0f), 0.25f), 1), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 6.85f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.45f, 0f), 0.25f), 1), true));
		TweenSettingsExtensions.Insert(this.breakFreeSeq, 7.1f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.redCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.redCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.45f, 0f), 0.25f), 1), true));
		TweenExtensions.Play<Sequence>(this.breakFreeSeq);
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
		Cursor.lockState = 1;
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
		if (this.canQuitToMainMenu && Input.GetKeyDown(32))
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
					else if (!TweenExtensions.IsPlaying(this.attemptBreakSeq))
					{
						this.redTriggerAttemptBreakAni();
					}
					GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.leatherSFXS[Random.Range(0, this.leatherSFXS.Count)], 0.1f, false);
					this.totalClickCount += this.currentClickCount;
				}
				else if (this.attemptBreakSeq != null)
				{
					TweenExtensions.Kill(this.attemptBreakSeq, true);
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
