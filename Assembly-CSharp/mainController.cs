using System;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.ImageEffects;

public class mainController : MonoBehaviour
{
	public void switchToComputerView()
	{
		this.myActionController.lockAction = true;
		this.lockControls = true;
		this.aboutToUseComputer = true;
		this.myUIManager.hideCrossHair();
		this.mySwitchSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.switchToComputerCamera));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(5.8636f, 0f, 0f), 0.3f), 3), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -2.1562f, 0f), 0.3f), 3), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 26.7f, 0.3f), 2));
		TweenExtensions.Play<Sequence>(this.mySwitchSeq);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.OUTSIDE, 0.2f, 0.3f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.FRONT, 0.25f, 0.3f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.PLAYER, 0.5f, 0.3f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.PHONE, 0.3f, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.COMPUTER, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.COMPUTERHARDWARE, 0.3f);
	}

	public void switchToMainView()
	{
		this.mainCamera.enabled = true;
		this.computerCamera.enabled = false;
		this.isUsingComputer = false;
		this.myCursorManager.setCursorMoveable(false);
		this.myUIManager.hideCursorUI();
		this.myComputerController.showCursorIMG();
		this.mySwitchSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.switchToMainCamera));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(this.myDefaultROT.x, this.myDefaultROT.y, this.myDefaultROT.z), 0.3f), 3), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 60f, 0.3f), 2));
		TweenExtensions.Play<Sequence>(this.mySwitchSeq);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTER, 0.2f, 0.3f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTERHARDWARE, 0.75f, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.OUTSIDE, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.FRONT, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.PLAYER, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.PHONE, 0.3f);
	}

	public void performLightAction()
	{
		bool flag = true;
		this.lockControls = true;
		this.myActionController.lockAction = true;
		this.masterLock = true;
		this.isMovingAround = true;
		if (this.myTrackerManager.isTheCoastClear())
		{
			this.myLightSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.lightAniDone));
		}
		else
		{
			GameManager.FileSlinger.deleteFile("wttg2.gd");
			GameManager.TimeSlinger.FireTimer(1.6f, new Action(GameManager.GetTheKidnapper().triggerWalkbackJumpIdle));
			this.myLightSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.triggerWalkBackJump));
			flag = false;
		}
		GameManager.TimeSlinger.FireTimer(1.3f, new Action(this.myActionController.mySceneManager.triggerLights));
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 0.6f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 0.85f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 1f);
		if (flag)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 1.9f);
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 2.15f);
		}
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.5f, base.transform.localPosition.z), 0.6f), 1));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 208f, 0f), 0.3f), 1), true));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 0.3f), 3), true));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-7.4f, 3.5f, -1.12f), 0.5f), 1));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.6f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 233.4f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f), 1));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.725f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f), 1));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f), 1));
		TweenSettingsExtensions.Insert(this.myLightSeq, 0.975f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f), 1));
		TweenSettingsExtensions.Insert(this.myLightSeq, 1.1f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(15f, this.cameraDefaultROT.y, this.cameraDefaultROT.z), 0.2f), 1), true));
		TweenSettingsExtensions.Insert(this.myLightSeq, 1.6f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(this.cameraDefaultROT.x, this.cameraDefaultROT.y, this.cameraDefaultROT.z), 0.3f), 1), true));
		TweenSettingsExtensions.Insert(this.myLightSeq, 1.6f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 0.3f), 1), true));
		if (flag)
		{
			TweenSettingsExtensions.Insert(this.myLightSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(this.myDefaultPOS.x, 3.5f, this.myDefaultPOS.z), 0.5f), 1));
			TweenSettingsExtensions.Insert(this.myLightSeq, 1.9f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(this.myDefaultROT.x, this.myDefaultROT.y, this.myDefaultROT.z), 0.5f), 1), true));
			TweenSettingsExtensions.Insert(this.myLightSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f), 1));
			TweenSettingsExtensions.Insert(this.myLightSeq, 2.025f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f), 1));
			TweenSettingsExtensions.Insert(this.myLightSeq, 2.15f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f), 1));
			TweenSettingsExtensions.Insert(this.myLightSeq, 2.275f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f), 1));
			TweenSettingsExtensions.Insert(this.myLightSeq, 2.4f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z), 0.4f), 1));
		}
		else
		{
			TweenSettingsExtensions.Insert(this.myLightSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(-6.948f, 3.363f, -0.439f), 0.3f), 1));
		}
		TweenExtensions.Play<Sequence>(this.myLightSeq);
	}

	public void performCheckDoorAction()
	{
		GameManager.GetTheUIManager().myPauseManager.lockPause = true;
		this.masterLock = true;
		this.lockControls = true;
		this.myActionController.lockAction = true;
		this.isMovingAround = true;
		GameManager.GetTheBreatherManager().playerWentToDoor();
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 0.6f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 0.975f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 1.35f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 1.85f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 2.35f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 2.85f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 3.35f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 3.85f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 4.3f);
		this.myCheckDoorSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.unlockDoorControls));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.876f, base.transform.localPosition.z), 0.6f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -245f, 0f), 0.3f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 0.3f), 3), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-3.328f, 3.876f, -2.402f), 0.75f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.6f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -261f, 0f), 0.75f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(6.459f, 3.876f, -5.124f), 1f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.35f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 1f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(27.162f, 3.876f, -5.212f), 2f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.7875f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.975f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.1625f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 4.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 4.5f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(27.162f, 2.707f, -6.33f), 1.5f), 3));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 4.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 1.5f), 3), true));
		TweenExtensions.Play<Sequence>(this.myCheckDoorSeq);
	}

	public void performGoBackAction()
	{
		this.masterLock = true;
		this.lockControls = true;
		this.myActionController.lockAction = true;
		this.isMovingAround = true;
		this.inDoorAction = false;
		this.myChair.SetActive(true);
		GameManager.GetTheSceneManager().setDoorAction(false);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 0.6f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 1.1f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 1.6f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 2.1f);
		if (!this.doorKidnap)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 2.6f);
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 3.1f);
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 3.6f);
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 3.975f);
		}
		if (this.doorKidnap)
		{
			this.myGoBackSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.triggerDoorBackJump));
		}
		else
		{
			this.myGoBackSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.lightAniDone));
		}
		if (this.doorKidnap)
		{
			GameManager.TimeSlinger.FireTimer(1.9f, new Action(GameManager.GetTheKidnapper().triggerDoorJump));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(base.transform.localPosition.x, 3.576f, -5.212f), 0.6f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(0f, -90f, 0f), 0.3f), 1), true));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.mainCamera.transform.localRotation = x;
			}, new Vector3(0f, 0f, 0f), 0.3f), 3), true));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(10.989f, 3.576f, -5.124f), 2f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
		}
		else
		{
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(base.transform.localPosition.x, 3.876f, -5.212f), 0.6f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(0f, -90f, 0f), 0.3f), 1), true));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.mainCamera.transform.localRotation = x;
			}, new Vector3(0f, 0f, 0f), 0.3f), 3), true));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(6.459f, 3.876f, -5.124f), 2f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 2.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(-3.328f, 3.876f, -2.402f), 1f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 2.6f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(0f, -32f, 0f), 1f), 1), true));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 3.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(this.myDefaultPOS.x, 3.5f, this.myDefaultPOS.z), 0.75f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 3.6f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(this.myDefaultROT.x, this.myDefaultROT.y, this.myDefaultROT.z), 0.75f), 1), true));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 4.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(this.myDefaultPOS.x, this.myDefaultPOS.y, this.myDefaultPOS.z), 0.4f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 0.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 1.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 2.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 2.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 2.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 2.85f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 3.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myGoBackSeq, 3.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f), 1));
			TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.6f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f), 1));
			TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.7875f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f), 1));
			TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.975f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f), 1));
			TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 4.1625f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f), 1));
		}
		TweenExtensions.Play<Sequence>(this.myGoBackSeq);
	}

	public void triggerClimbJumpMovement()
	{
		GameManager.TimeSlinger.FireTimer(4.24f, new Action(this.enableLensBlur));
		GameManager.TimeSlinger.FireTimer(4.5f, new Action(this.triggerKidnapGameOver));
		this.masterLock = true;
		this.myJumpSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.myJumpSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 39f, 0f), 0.3f), 1), true));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(this.cameraDefaultROT.x, this.cameraDefaultROT.y, this.cameraDefaultROT.z), 0.3f), 1), true));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 0.3f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 4f), 2), true));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.25f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-8.111f, 2.293f, 2.557f), 0.35f), 9));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.25f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(-12.53f, 0f, 0f), 0.35f), 9), true));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.25f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 1.2f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.25f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 1.2f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.25f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 1.2f), 1));
		TweenExtensions.Play<Sequence>(this.myJumpSeq);
	}

	public void triggerBraceMovement()
	{
		this.masterLock = true;
		this.braceSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.braceSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(27.593f, this.duckLook.y, this.duckLook.z), 1f), 9));
		TweenSettingsExtensions.Insert(this.braceSeq, 0.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(14.397f, 21.701f, 2.537f), 1f), 3), true));
		TweenSettingsExtensions.Insert(this.braceSeq, 0.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 1f), 3), true));
		TweenExtensions.Play<Sequence>(this.braceSeq);
	}

	public void unlockBraceMovement()
	{
		this.masterLock = false;
		this.braceSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.braceSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, this.duckLook, 1f), 1));
		TweenSettingsExtensions.Insert(this.braceSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, Vector3.zero, 1f), 1), true));
		TweenExtensions.Play<Sequence>(this.braceSeq);
	}

	public void triggerBraceKnock()
	{
		TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-0.02f, 0f, 0f), 0.1f), 1), true), 0.2f);
		TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(0.02f, 0f, 0f), 0.25f), 5), true), 0.4f);
	}

	public void triggerDoorKnockedDownAni()
	{
		this.myCameraMB.enabled = true;
		this.masterLock = true;
		GameManager.TimeSlinger.FireTimer(4f, new Action(this.disableMotionBlur));
		GameManager.TimeSlinger.FireTimer(5.7f, new Action(this.enableLensBlur));
		this.doorKnockSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.doorKnockSeq, 0.35f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(17.966f, 1.193f, -5.606f), 1f), 8));
		TweenSettingsExtensions.Insert(this.doorKnockSeq, 0.35f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-26f, 90f, 0f), 1f), 9), true));
		TweenSettingsExtensions.Insert(this.doorKnockSeq, 0.35f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.5f), 3), true));
		TweenSettingsExtensions.Insert(this.doorKnockSeq, 5.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-55.31f, 90f, 0f), 0.7f), 1), true));
		TweenSettingsExtensions.Insert(this.doorKnockSeq, 5.8f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.doorKnockSeq, 5.8f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.doorKnockSeq, 5.8f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.doorKnockSeq);
	}

	public void triggerBumRush()
	{
		this.masterLock = true;
		GameManager.TimeSlinger.FireTimer(1.8f, new Action(this.enableLensBlur));
		this.bumSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.bumSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 107f, 0f), 0.2f), 8), true));
		TweenSettingsExtensions.Insert(this.bumSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.2f), 3), true));
		TweenSettingsExtensions.Insert(this.bumSeq, 0.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 161f, 0f), 1f), 1), true));
		TweenSettingsExtensions.Insert(this.bumSeq, 1.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-6.668f, 2.687f, 3.576f), 1f), 1));
		TweenSettingsExtensions.Insert(this.bumSeq, 1.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-14.569f, 180f, 0f), 1f), 1), true));
		TweenSettingsExtensions.Insert(this.bumSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.bumSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.bumSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.bumSeq);
	}

	public void triggerNightNight()
	{
		this.masterLock = true;
		GameManager.GetTheBreatherManager().sayNightNight();
		GameManager.TimeSlinger.FireTimer(3.8f, new Action(this.enableLensBlur));
		this.nightSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.nightSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -180f, 0f), 0.3f), 2), true));
		TweenSettingsExtensions.Insert(this.nightSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.3f), 3), true));
		TweenSettingsExtensions.Insert(this.nightSeq, 1.9f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-12.625f, -180f, 0f), 1f), 2), true));
		TweenSettingsExtensions.Insert(this.nightSeq, 3.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.nightSeq, 3.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.nightSeq, 3.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.nightSeq);
	}

	public void setNightNight()
	{
		this.nightNightActive = true;
	}

	public void forceNightNight()
	{
		this.switchToMainView();
		GameManager.TimeSlinger.FireTimer(0.4f, new Action(this.triggerNightNight));
	}

	public actionController getMyActionController()
	{
		return this.myActionController;
	}

	public void triggerNymphoEndingAni()
	{
		this.mainCamera.transform.localPosition = new Vector3(0f, 1.091f, 1.039f);
		this.mainCamera.useOcclusionCulling = false;
		this.masterLock = true;
		this.mainCamera.enabled = true;
		this.computerCamera.enabled = false;
		this.isUsingComputer = false;
		this.myCursorManager.setCursorMoveable(false);
		this.myUIManager.hideCursorUI();
		this.myComputerController.showCursorIMG();
		this.mySwitchSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, Vector3.zero, 0.3f), 3), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 83f, 0.3f), 2));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 0.3f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(18.111f, 16.201f, 2.585f), 0.6f), 2), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 1.9f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(20.429f, -13.129f, -3.046f), 0.6f), 3), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0f, 1.218f, 0.95f), 0.6f), 3));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 3f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 0.8f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 7f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0.066f, 1.218f, 1.002f), 0.8f), 1));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 7f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(0f, 69f, 0f), 1.5f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 11f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 2f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 15f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0.066f, 1.1f, 1.115f), 0.8f), 1));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 15f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(60.04f, 0f, 0f), 0.8f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 17f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0f, 1.218f, 0.95f), 0.8f), 1));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 17f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 0.8f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 22f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(-10.311f, -61.203f, 0f), 1.5f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 25.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 1f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 27.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(17.391f, 0f, 0f), 1f), 1), true));
		TweenSettingsExtensions.Insert(this.mySwitchSeq, 30.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 1f), 1), true));
		TweenExtensions.Play<Sequence>(this.mySwitchSeq);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTER, 0.2f, 0.3f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTERHARDWARE, 0.75f, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.OUTSIDE, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.FRONT, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.PLAYER, 0.3f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.PHONE, 0.3f);
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.enableOC));
	}

	public void triggerNymphoEndWalking()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 1.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 2.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 3.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 4.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 5.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 6.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.3f, false, 7.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound2, 0.3f, false, 8.95f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.1f, false, 9.75f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.footStepSound, 0.1f, false, 10f);
		this.myCheckDoorSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.876f, base.transform.localPosition.z), 0.6f), 2));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0f, 1f, 0f), 0.6f), 2));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 60f, 0.6f), 2));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.2f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -245f, 0f), 0.75f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.95f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-3.328f, 3.876f, -2.402f), 1f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.95f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -261f, 0f), 1f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.95f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(7.486f, 3.876f, -5.28f), 2f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.95f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 2f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 4.95f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(13.521f, 3.876f, -5.28f), 1f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 5.95f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, Vector3.zero, 1f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 6.95f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(13.521f, 3.876f, 3.086f), 3f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 12.96f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(-0.2299995f, 1.076f, 0.6381001f), 0.96f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 12.96f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(6.196f, 0f, 0f), 0.96f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 12.96f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 42f, 0.96f), 1));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 1.95f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.45f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 2.95f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.45f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 3.95f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 4.45f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 4.95f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 5.45f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 6.95f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 7.45f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 7.95f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 8.45f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 8.95f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.myCheckDoorSeq, 9.45f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenExtensions.Play<Sequence>(this.myCheckDoorSeq);
	}

	private void prepLooks()
	{
		this.leftLook = new Vector3(0f, this.maxLeftLook, 0f);
		this.rightLook = new Vector3(0f, this.maxRightLook, 0f);
		this.behindLook = new Vector3(0f, this.maxBehindLook, 0f);
		this.peakLook = new Vector3(27.789f, 4.4f, -5.397f);
		this.peakROT = new Vector3(0f, 90f, 0f);
		this.duckLook = new Vector3(27.162f, 2.707f, -6.33f);
		this.deskPivotROT = base.transform.localRotation;
	}

	private void RotateView()
	{
		this.myMouseCapture.LookCameraRotation(this.myCamera.transform);
	}

	private void protoPivot()
	{
		if (CrossPlatformInputManager.GetButton("Horizontal"))
		{
			if (CrossPlatformInputManager.GetAxis("Vertical") == 0f)
			{
				if (CrossPlatformInputManager.GetAxis("Horizontal") < 0f)
				{
					this.deskPivotROT *= Quaternion.Euler(0f, -this.pivotSpeed, 0f);
					this.deskPivotROT = this.myMouseCapture.ClampRotationAroundYAxis(this.deskPivotROT, this.maxLeftLook, this.maxLeftLook);
					base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.deskPivotROT, this.pivotSpeed * Time.deltaTime);
				}
				else if (CrossPlatformInputManager.GetAxis("Horizontal") > 0f)
				{
					this.deskPivotROT *= Quaternion.Euler(0f, this.pivotSpeed, 0f);
					this.deskPivotROT = this.myMouseCapture.ClampRotationAroundYAxis(this.deskPivotROT, this.maxRightLook, this.maxRightLook);
					base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.deskPivotROT, this.pivotSpeed * Time.deltaTime);
				}
			}
		}
		else if (CrossPlatformInputManager.GetButton("Vertical") && CrossPlatformInputManager.GetAxis("Vertical") < 0f)
		{
			if (base.transform.localRotation.eulerAngles.y > this.maxBehindLook)
			{
				this.deskPivotROT *= Quaternion.Euler(0f, -this.pivotSpeed, 0f);
				this.deskPivotROT = this.myMouseCapture.ClampRotationAroundYAxis(this.deskPivotROT, this.maxBehindLook, base.transform.localRotation.eulerAngles.y);
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.deskPivotROT, this.pivotSpeed * Time.deltaTime);
			}
			else
			{
				this.deskPivotROT *= Quaternion.Euler(0f, this.pivotSpeed, 0f);
				this.deskPivotROT = this.myMouseCapture.ClampRotationAroundYAxis(this.deskPivotROT, base.transform.localRotation.eulerAngles.y, this.maxBehindLook);
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.deskPivotROT, this.pivotSpeed * Time.deltaTime);
			}
		}
		else if (base.transform.localRotation.eulerAngles.y > 1f)
		{
			this.deskPivotROT *= Quaternion.Euler(0f, this.pivotSpeed, 0f);
			this.deskPivotROT = this.myMouseCapture.ClampRotationAroundYAxis(this.deskPivotROT, 0f, 1f);
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.deskPivotROT, this.pivotSpeed * Time.deltaTime);
		}
	}

	private void UpdatePivotTween()
	{
		if (this.nightNightActive)
		{
			if (CrossPlatformInputManager.GetButton("Horizontal") || CrossPlatformInputManager.GetButton("Vertical"))
			{
				this.triggerNightNight();
			}
		}
		else if (CrossPlatformInputManager.GetButton("Horizontal"))
		{
			if (CrossPlatformInputManager.GetAxis("Vertical") == 0f)
			{
				if (CrossPlatformInputManager.GetAxis("Horizontal") < 0f)
				{
					if (base.transform.localRotation.eulerAngles.y != this.maxLeftLook)
					{
						TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
						{
							base.transform.localRotation = x;
						}, this.leftLook, 0.5f), 3), true);
					}
				}
				else if (CrossPlatformInputManager.GetAxis("Horizontal") > 0f && base.transform.localRotation.eulerAngles.y != this.maxRightLook)
				{
					TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
					{
						base.transform.localRotation = x;
					}, this.rightLook, 0.5f), 3), true);
				}
			}
		}
		else if (CrossPlatformInputManager.GetButton("Vertical") && CrossPlatformInputManager.GetAxis("Vertical") < 0f)
		{
			if (base.transform.localRotation.eulerAngles.y != this.maxBehindLook)
			{
				TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
				{
					base.transform.localRotation = x;
				}, this.behindLook, 0.5f), 3), true);
			}
		}
		else if (base.transform.localRotation.eulerAngles.y != 0f)
		{
			TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, Vector3.zero, 0.5f), 3), true);
		}
	}

	private void UpdateDoorPivotTween()
	{
		if (!GameManager.GetTheBreatherManager().playerIsHoldingKnob)
		{
			if (CrossPlatformInputManager.GetButton("Horizontal"))
			{
				if (CrossPlatformInputManager.GetAxis("Vertical") == 0f)
				{
					if (CrossPlatformInputManager.GetAxis("Horizontal") < 0f)
					{
						GameManager.GetTheBreatherManager().playerCanBeSeen = false;
						if (base.transform.localRotation.eulerAngles.y != this.maxLeftLook)
						{
							TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
							{
								base.transform.localPosition = x;
							}, this.duckLook, 0.5f), 1);
							TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
							{
								base.transform.localRotation = x;
							}, this.leftLook, 0.5f), 3), true);
						}
					}
					else if (CrossPlatformInputManager.GetAxis("Horizontal") > 0f)
					{
						if (base.transform.localPosition.y >= 4f)
						{
							GameManager.GetTheBreatherManager().playerCanBeSeen = true;
						}
						else
						{
							GameManager.GetTheBreatherManager().playerCanBeSeen = false;
						}
						TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
						{
							base.transform.localPosition = x;
						}, this.peakLook, 1f), 1);
						TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
						{
							base.transform.localRotation = x;
						}, this.peakROT, 1f), 1), true);
					}
				}
			}
			else if (base.transform.localRotation.eulerAngles.y != 0f)
			{
				GameManager.GetTheBreatherManager().playerCanBeSeen = false;
				TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
				{
					base.transform.localPosition = x;
				}, this.duckLook, 0.5f), 1);
				TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
				{
					base.transform.localRotation = x;
				}, Vector3.zero, 0.5f), 3), true);
			}
		}
	}

	private void windowPeak()
	{
		if (CrossPlatformInputManager.GetButton("RightClick"))
		{
			if (CrossPlatformInputManager.GetAxis("RightClick") > 0f && !this.lockPivot)
			{
				this.lockPivot = true;
				this.windowPeakSeq = DOTween.Sequence();
				TweenSettingsExtensions.Insert(this.windowPeakSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
				{
					base.transform.localPosition = x;
				}, this.peakLook, 1f), 1));
				TweenSettingsExtensions.Insert(this.windowPeakSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
				{
					base.transform.localRotation = x;
				}, this.peakROT, 1f), 1), true));
				TweenExtensions.Play<Sequence>(this.windowPeakSeq);
			}
		}
		else if (this.lockPivot)
		{
			this.lockPivot = false;
			if (TweenExtensions.IsPlaying(this.windowPeakSeq))
			{
				TweenExtensions.Kill(this.windowPeakSeq, false);
			}
			this.windowPeakSeq = DOTween.Sequence();
			TweenSettingsExtensions.Insert(this.windowPeakSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, this.duckLook, 0.5f), 1));
			TweenSettingsExtensions.Insert(this.windowPeakSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, Vector2.zero, 0.5f), 1), true));
			TweenExtensions.Play<Sequence>(this.windowPeakSeq);
		}
	}

	private void switchToComputerCamera()
	{
		this.myComputerController.hideCursorIMG();
		this.myUIManager.showCursorUI();
		this.mainCamera.enabled = false;
		this.computerCamera.enabled = true;
		this.isUsingComputer = true;
		this.myCursorManager.setCursorMoveable(true);
	}

	private void switchToMainCamera()
	{
		if (!this.isDoomed)
		{
			this.myUIManager.showCrossHair();
			if (this.windowCreep)
			{
				GameManager.GetTheUIManager().myPauseManager.lockPause = true;
				GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.MOMENT, GameManager.GetTheBreatherManager().BreatherJump3, 1f, false, 0.25f);
				TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
				{
					this.myCamera.transform.localRotation = x;
				}, new Vector3(0f, 49.99f), 0.25f), 2);
				GameManager.TimeSlinger.FireTimer(0.5f, new Action(GameManager.GetTheBreatherManager().triggerBreatherWindowAway));
				GameManager.TimeSlinger.FireTimer(2.5f, new Action(this.unlockPostWindowCreep));
			}
			else
			{
				this.lockControls = false;
				this.myActionController.lockAction = false;
				this.aboutToUseComputer = false;
			}
		}
		else
		{
			this.myTrackerManager.cancelDoomClock();
			GameManager.TimeSlinger.FireTimer(0.2f, new Action(GameManager.GetTheKidnapper().triggerUsingComputerJump));
			GameManager.TimeSlinger.FireTimer(3.4f, new Action(this.enableLensBlur));
			GameManager.TimeSlinger.FireTimer(4.1f, new Action(this.triggerKidnapGameOver));
			this.myJumpSeq = DOTween.Sequence();
			TweenSettingsExtensions.Insert(this.myJumpSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(base.transform.localRotation.eulerAngles.x, 90f, base.transform.localRotation.eulerAngles.z), 0.3f), 3), true));
			TweenSettingsExtensions.Insert(this.myJumpSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(-6.886f, base.transform.localPosition.y, base.transform.localPosition.z), 0.3f), 3));
			TweenSettingsExtensions.Insert(this.myJumpSeq, 0f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.myCamera.transform.localRotation = x;
			}, new Vector3(-4.378f, base.transform.localRotation.eulerAngles.y, base.transform.localRotation.eulerAngles.z), 0.3f), 3), true));
			TweenSettingsExtensions.Insert(this.myJumpSeq, 3.8f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Distortion, delegate(float x)
			{
				this.myLDB.Distortion = x;
			}, 2f, 1.2f), 1));
			TweenSettingsExtensions.Insert(this.myJumpSeq, 3.8f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
			{
				this.myLDB.CubicDistortion = x;
			}, 1.3f, 1.2f), 1));
			TweenSettingsExtensions.Insert(this.myJumpSeq, 3.8f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Scale, delegate(float x)
			{
				this.myLDB.Scale = x;
			}, 0.75f, 1.2f), 1));
			TweenExtensions.Play<Sequence>(this.myJumpSeq);
		}
	}

	private void lightAniDone()
	{
		GameManager.GetTheUIManager().myPauseManager.lockPause = false;
		this.lockControls = false;
		this.myActionController.lockAction = false;
		this.masterLock = false;
		this.isMovingAround = false;
		this.myChair.SetActive(false);
	}

	private void unlockPostWindowCreep()
	{
		GameManager.GetTheUIManager().myPauseManager.lockPause = false;
		this.lockControls = false;
		this.myActionController.lockAction = false;
		this.aboutToUseComputer = false;
		this.windowCreep = false;
	}

	private void triggerWalkBackJump()
	{
		GameManager.GetTheKidnapper().triggerWalkBackJump();
		GameManager.TimeSlinger.FireTimer(2.94f, new Action(this.enableLensBlur));
		GameManager.TimeSlinger.FireTimer(3.84f, new Action(this.triggerKidnapGameOver));
		this.myJumpSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.myJumpSeq, 2.45f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.154f, base.transform.localPosition.z), 0.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 2.45f, TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(-7.211f, this.myCamera.transform.localRotation.eulerAngles.y, this.myCamera.transform.localRotation.eulerAngles.z), 0.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 2.85f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 1.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 2.85f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 1.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 2.85f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 1.5f), 1));
		TweenExtensions.Play<Sequence>(this.myJumpSeq);
	}

	private void triggerDoorBackJump()
	{
		GameManager.TimeSlinger.FireTimer(3.95f, new Action(this.enableLensBlur));
		GameManager.TimeSlinger.FireTimer(4.5f, new Action(this.triggerKidnapGameOver));
		this.myJumpSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.myJumpSeq, 3.65f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.154f, base.transform.localPosition.z), 0.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 3.65f, TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(-7.211f, this.myCamera.transform.localRotation.eulerAngles.y, this.myCamera.transform.localRotation.eulerAngles.z), 0.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.05f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 1.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.05f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 1.5f), 1));
		TweenSettingsExtensions.Insert(this.myJumpSeq, 4.05f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 1.5f), 1));
		TweenExtensions.Play<Sequence>(this.myJumpSeq);
	}

	private void enableLensBlur()
	{
		this.myLDB.enabled = true;
	}

	private void triggerKidnapGameOver()
	{
		this.myUIManager.triggerGameOver("You've been kidnapped...");
	}

	private void unlockDoorControls()
	{
		this.masterLock = false;
		this.lockControls = false;
		this.myActionController.lockAction = false;
		this.inDoorAction = true;
		this.isMovingAround = false;
		GameManager.GetTheSceneManager().setDoorAction(true);
	}

	private void disableMotionBlur()
	{
		this.myCameraMB.enabled = false;
	}

	private void enableOC()
	{
		this.mainCamera.useOcclusionCulling = true;
	}

	private void Awake()
	{
		GameManager.SetMainController(this);
	}

	private void Start()
	{
		this.mainCamera.enabled = true;
		this.computerCamera.enabled = false;
		this.myCamera = this.mainCamera;
		this.myDefaultPOS = base.transform.localPosition;
		this.myDefaultROT = base.transform.localRotation.eulerAngles;
		this.myLDB = this.myCamera.GetComponent<LensDistortionBlur>();
		this.myCameraMB = this.myCamera.GetComponent<CameraMotionBlur>();
		this.cameraDefaultPOS = this.mainCamera.transform.localPosition;
		this.cameraDefaultROT = this.mainCamera.transform.localRotation.eulerAngles;
		this.myMouseCapture.Init(base.transform, this.myCamera.transform);
		this.myActionController = base.GetComponent<actionController>();
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTER, 0.2f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTERHARDWARE, 0.75f);
		this.prepLooks();
	}

	private void Update()
	{
		if (!this.masterLock)
		{
			if (this.inDoorAction)
			{
				if (this.myPauseManager.iAmPaused)
				{
					this.lockControls = true;
				}
				else
				{
					this.lockControls = false;
				}
				if (!this.lockControls)
				{
					this.RotateView();
					this.myActionController.lockAction = false;
					this.UpdateDoorPivotTween();
				}
				else
				{
					this.myActionController.lockAction = true;
				}
			}
			else if (!this.isUsingComputer)
			{
				if (!this.aboutToUseComputer)
				{
					if (this.myPauseManager.iAmPaused)
					{
						this.lockControls = true;
					}
					else
					{
						this.lockControls = false;
					}
				}
				if (!this.lockControls)
				{
					this.UpdatePivotTween();
					this.RotateView();
					this.myActionController.lockAction = false;
				}
				else
				{
					this.myActionController.lockAction = true;
				}
			}
			else
			{
				this.myActionController.lockAction = true;
			}
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus && this.isUsingComputer)
		{
			this.myComputerController.hideCursorIMG();
			this.myUIManager.showCursorUI();
			this.myCursorManager.setCursorMoveable(true);
		}
	}

	public UIManager myUIManager;

	public PauseManager myPauseManager;

	public cursorManager myCursorManager;

	public computerController myComputerController;

	public TrackerManager myTrackerManager;

	public Camera mainCamera;

	public Camera computerCamera;

	public GameObject myChair;

	public bool isUsingComputer;

	public bool isDoomed;

	public bool masterLock;

	public bool isMovingAround;

	public bool doorKidnap;

	public bool windowCreep;

	public mouseCapture myMouseCapture;

	[Range(185f, 355f)]
	public float maxLeftLook = 290f;

	[Range(0f, 175f)]
	public float maxRightLook = 80f;

	[Range(180f, 260f)]
	public float maxBehindLook = 190f;

	[Range(0f, 10f)]
	public float lookSpeed = 5f;

	[Range(0f, 10f)]
	public float pivotSpeed = 5f;

	[Range(0f, 10f)]
	public float smoothTime = 5f;

	public AudioClip footStepSound;

	public AudioClip footStepSound2;

	private actionController myActionController;

	private Camera myCamera;

	private Sequence myLightSeq;

	private Sequence mySwitchSeq;

	private Sequence myJumpSeq;

	private Sequence myCheckDoorSeq;

	private Sequence myGoBackSeq;

	private Sequence windowPeakSeq;

	private Sequence myPivotSeq;

	private Sequence doorKnockSeq;

	private Sequence braceSeq;

	private Sequence bumSeq;

	private Sequence nightSeq;

	private bool lockControls;

	private bool aboutToUseComputer;

	private bool inDoorAction;

	private bool lockPivot;

	private bool nightNightActive;

	private Vector3 leftLook;

	private Vector3 rightLook;

	private Vector3 behindLook;

	private Vector3 peakLook;

	private Vector3 duckLook;

	private Vector3 peakROT;

	private Vector3 myDefaultPOS;

	private Vector3 myDefaultROT;

	private Vector3 cameraDefaultPOS;

	private Vector3 cameraDefaultROT;

	private LensDistortionBlur myLDB;

	private CameraMotionBlur myCameraMB;

	private Quaternion deskPivotROT;
}
