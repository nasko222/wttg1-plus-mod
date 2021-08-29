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
		this.mySwitchSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.switchToComputerCamera));
		this.mySwitchSeq.Insert(0f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(5.8636f, 0f, 0f), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		this.mySwitchSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -2.1562f, 0f), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		this.mySwitchSeq.Insert(0f, DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 26.7f, 0.3f).SetEase(Ease.InSine));
		this.mySwitchSeq.Play<Sequence>();
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
		this.mySwitchSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.switchToMainCamera));
		this.mySwitchSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(this.myDefaultROT.x, this.myDefaultROT.y, this.myDefaultROT.z), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		this.mySwitchSeq.Insert(0f, DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 60f, 0.3f).SetEase(Ease.InSine));
		this.mySwitchSeq.Play<Sequence>();
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
			this.myLightSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.lightAniDone));
		}
		else
		{
			GameManager.FileSlinger.deleteFile("wttg2.gd");
			GameManager.TimeSlinger.FireTimer(1.6f, new Action(GameManager.GetTheKidnapper().triggerWalkbackJumpIdle));
			this.myLightSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.triggerWalkBackJump));
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
		this.myLightSeq.Insert(0.2f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.5f, base.transform.localPosition.z), 0.6f).SetEase(Ease.Linear));
		this.myLightSeq.Insert(0.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 208f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		this.myLightSeq.Insert(0.2f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		this.myLightSeq.Insert(0.6f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-7.4f, 3.5f, -1.12f), 0.5f).SetEase(Ease.Linear));
		this.myLightSeq.Insert(0.6f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 233.4f, 0f), 0.5f).SetEase(Ease.Linear).SetOptions(true));
		this.myLightSeq.Insert(0.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
		this.myLightSeq.Insert(0.725f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
		this.myLightSeq.Insert(0.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
		this.myLightSeq.Insert(0.975f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
		this.myLightSeq.Insert(1.1f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(15f, this.cameraDefaultROT.y, this.cameraDefaultROT.z), 0.2f).SetEase(Ease.Linear).SetOptions(true));
		this.myLightSeq.Insert(1.6f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(this.cameraDefaultROT.x, this.cameraDefaultROT.y, this.cameraDefaultROT.z), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		this.myLightSeq.Insert(1.6f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		if (flag)
		{
			this.myLightSeq.Insert(1.9f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(this.myDefaultPOS.x, 3.5f, this.myDefaultPOS.z), 0.5f).SetEase(Ease.Linear));
			this.myLightSeq.Insert(1.9f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(this.myDefaultROT.x, this.myDefaultROT.y, this.myDefaultROT.z), 0.5f).SetEase(Ease.Linear).SetOptions(true));
			this.myLightSeq.Insert(1.9f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
			this.myLightSeq.Insert(2.025f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
			this.myLightSeq.Insert(2.15f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
			this.myLightSeq.Insert(2.275f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.125f).SetEase(Ease.Linear));
			this.myLightSeq.Insert(2.4f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z), 0.4f).SetEase(Ease.Linear));
		}
		else
		{
			this.myLightSeq.Insert(1.9f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(-6.948f, 3.363f, -0.439f), 0.3f).SetEase(Ease.Linear));
		}
		this.myLightSeq.Play<Sequence>();
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
		this.myCheckDoorSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.unlockDoorControls));
		this.myCheckDoorSeq.Insert(0.2f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.876f, base.transform.localPosition.z), 0.6f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(0.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -245f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(0.2f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		this.myCheckDoorSeq.Insert(0.6f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-3.328f, 3.876f, -2.402f), 0.75f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(0.6f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -261f, 0f), 0.75f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(1.35f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(6.459f, 3.876f, -5.124f), 1f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(1.35f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 1f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(2.35f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(27.162f, 3.876f, -5.212f), 2f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(0.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(0.7875f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(0.975f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(1.1625f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(1.35f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(1.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(1.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(2.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(2.35f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(2.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(2.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(3.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(3.35f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(3.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(3.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(4.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(4.5f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(27.162f, 2.707f, -6.33f), 1.5f).SetEase(Ease.OutSine));
		this.myCheckDoorSeq.Insert(4.5f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 0f, 0f), 1.5f).SetEase(Ease.OutSine).SetOptions(true));
		this.myCheckDoorSeq.Play<Sequence>();
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
			this.myGoBackSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.triggerDoorBackJump));
		}
		else
		{
			this.myGoBackSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.lightAniDone));
		}
		if (this.doorKidnap)
		{
			GameManager.TimeSlinger.FireTimer(1.9f, new Action(GameManager.GetTheKidnapper().triggerDoorJump));
			this.myGoBackSeq.Insert(0.2f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(base.transform.localPosition.x, 3.576f, -5.212f), 0.6f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(0.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(0f, -90f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
			this.myGoBackSeq.Insert(0.2f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.mainCamera.transform.localRotation = x;
			}, new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
			this.myGoBackSeq.Insert(0.6f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(10.989f, 3.576f, -5.124f), 2f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(0.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(0.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.35f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
		}
		else
		{
			this.myGoBackSeq.Insert(0.2f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(base.transform.localPosition.x, 3.876f, -5.212f), 0.6f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(0.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(0f, -90f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
			this.myGoBackSeq.Insert(0.2f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.mainCamera.transform.localRotation = x;
			}, new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
			this.myGoBackSeq.Insert(0.6f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(6.459f, 3.876f, -5.124f), 2f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(2.6f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(-3.328f, 3.876f, -2.402f), 1f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(2.6f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(0f, -32f, 0f), 1f).SetEase(Ease.Linear).SetOptions(true));
			this.myGoBackSeq.Insert(3.6f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(this.myDefaultPOS.x, 3.5f, this.myDefaultPOS.z), 0.75f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(3.6f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(this.myDefaultROT.x, this.myDefaultROT.y, this.myDefaultROT.z), 0.75f).SetEase(Ease.Linear).SetOptions(true));
			this.myGoBackSeq.Insert(4.35f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(this.myDefaultPOS.x, this.myDefaultPOS.y, this.myDefaultPOS.z), 0.4f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(0.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(0.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.35f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(1.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(2.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(2.35f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(2.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(2.85f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(3.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myGoBackSeq.Insert(3.35f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.25f).SetEase(Ease.Linear));
			this.myCheckDoorSeq.Insert(3.6f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
			this.myCheckDoorSeq.Insert(3.7875f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
			this.myCheckDoorSeq.Insert(3.975f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, 0.95f, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
			this.myCheckDoorSeq.Insert(4.1625f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, new Vector3(this.cameraDefaultPOS.x, this.cameraDefaultPOS.y, this.cameraDefaultPOS.z), 0.1875f).SetEase(Ease.Linear));
		}
		this.myGoBackSeq.Play<Sequence>();
	}

	public void triggerClimbJumpMovement()
	{
		GameManager.TimeSlinger.FireTimer(4.24f, new Action(this.enableLensBlur));
		GameManager.TimeSlinger.FireTimer(4.5f, new Action(this.triggerKidnapGameOver));
		this.masterLock = true;
		this.myJumpSeq = DOTween.Sequence();
		this.myJumpSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 39f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		this.myJumpSeq.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(this.cameraDefaultROT.x, this.cameraDefaultROT.y, this.cameraDefaultROT.z), 0.3f).SetEase(Ease.Linear).SetOptions(true));
		this.myJumpSeq.Insert(0.3f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 4f).SetEase(Ease.InSine).SetOptions(true));
		this.myJumpSeq.Insert(4.25f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-8.111f, 2.293f, 2.557f), 0.35f).SetEase(Ease.OutCubic));
		this.myJumpSeq.Insert(4.25f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(-12.53f, 0f, 0f), 0.35f).SetEase(Ease.OutCubic).SetOptions(true));
		this.myJumpSeq.Insert(4.25f, DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 1.2f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(4.25f, DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 1.2f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(4.25f, DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 1.2f).SetEase(Ease.Linear));
		this.myJumpSeq.Play<Sequence>();
	}

	public void triggerBraceMovement()
	{
		this.masterLock = true;
		this.braceSeq = DOTween.Sequence();
		this.braceSeq.Insert(0.5f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(27.593f, this.duckLook.y, this.duckLook.z), 1f).SetEase(Ease.OutCubic));
		this.braceSeq.Insert(0.5f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(14.397f, 21.701f, 2.537f), 1f).SetEase(Ease.OutSine).SetOptions(true));
		this.braceSeq.Insert(0.5f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 1f).SetEase(Ease.OutSine).SetOptions(true));
		this.braceSeq.Play<Sequence>();
	}

	public void unlockBraceMovement()
	{
		this.masterLock = false;
		this.braceSeq = DOTween.Sequence();
		this.braceSeq.Insert(0f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, this.duckLook, 1f).SetEase(Ease.Linear));
		this.braceSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, Vector3.zero, 1f).SetEase(Ease.Linear).SetOptions(true));
		this.braceSeq.Play<Sequence>();
	}

	public void triggerBraceKnock()
	{
		DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-0.02f, 0f, 0f), 0.1f).SetEase(Ease.Linear).SetRelative(true).SetDelay(0.2f);
		DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(0.02f, 0f, 0f), 0.25f).SetEase(Ease.InQuad).SetRelative(true).SetDelay(0.4f);
	}

	public void triggerDoorKnockedDownAni()
	{
		this.myCameraMB.enabled = true;
		this.masterLock = true;
		GameManager.TimeSlinger.FireTimer(4f, new Action(this.disableMotionBlur));
		GameManager.TimeSlinger.FireTimer(5.7f, new Action(this.enableLensBlur));
		this.doorKnockSeq = DOTween.Sequence();
		this.doorKnockSeq.Insert(0.35f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(17.966f, 1.193f, -5.606f), 1f).SetEase(Ease.InCubic));
		this.doorKnockSeq.Insert(0.35f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-26f, 90f, 0f), 1f).SetEase(Ease.OutCubic).SetOptions(true));
		this.doorKnockSeq.Insert(0.35f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.OutSine).SetOptions(true));
		this.doorKnockSeq.Insert(5.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-55.31f, 90f, 0f), 0.7f).SetEase(Ease.Linear).SetOptions(true));
		this.doorKnockSeq.Insert(5.8f, DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 0.5f).SetEase(Ease.Linear));
		this.doorKnockSeq.Insert(5.8f, DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 0.5f).SetEase(Ease.Linear));
		this.doorKnockSeq.Insert(5.8f, DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 0.5f).SetEase(Ease.Linear));
		this.doorKnockSeq.Play<Sequence>();
	}

	public void triggerBumRush()
	{
		this.masterLock = true;
		GameManager.TimeSlinger.FireTimer(1.8f, new Action(this.enableLensBlur));
		this.bumSeq = DOTween.Sequence();
		this.bumSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 107f, 0f), 0.2f).SetEase(Ease.InCubic).SetOptions(true));
		this.bumSeq.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.2f).SetEase(Ease.OutSine).SetOptions(true));
		this.bumSeq.Insert(0.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 161f, 0f), 1f).SetEase(Ease.Linear).SetOptions(true));
		this.bumSeq.Insert(1.2f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-6.668f, 2.687f, 3.576f), 1f).SetEase(Ease.Linear));
		this.bumSeq.Insert(1.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-14.569f, 180f, 0f), 1f).SetEase(Ease.Linear).SetOptions(true));
		this.bumSeq.Insert(1.9f, DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 0.5f).SetEase(Ease.Linear));
		this.bumSeq.Insert(1.9f, DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 0.5f).SetEase(Ease.Linear));
		this.bumSeq.Insert(1.9f, DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 0.5f).SetEase(Ease.Linear));
		this.bumSeq.Play<Sequence>();
	}

	public void triggerNightNight()
	{
		this.masterLock = true;
		GameManager.GetTheBreatherManager().sayNightNight();
		GameManager.TimeSlinger.FireTimer(3.8f, new Action(this.enableLensBlur));
		this.nightSeq = DOTween.Sequence();
		this.nightSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -180f, 0f), 0.3f).SetEase(Ease.InSine).SetOptions(true));
		this.nightSeq.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		this.nightSeq.Insert(1.9f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(-12.625f, -180f, 0f), 1f).SetEase(Ease.InSine).SetOptions(true));
		this.nightSeq.Insert(3.9f, DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 0.5f).SetEase(Ease.Linear));
		this.nightSeq.Insert(3.9f, DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 0.5f).SetEase(Ease.Linear));
		this.nightSeq.Insert(3.9f, DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 0.5f).SetEase(Ease.Linear));
		this.nightSeq.Play<Sequence>();
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
		this.mySwitchSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, Vector3.zero, 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		this.mySwitchSeq.Insert(0f, DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 83f, 0.3f).SetEase(Ease.InSine));
		this.mySwitchSeq.Insert(0.3f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(18.111f, 16.201f, 2.585f), 0.6f).SetEase(Ease.InSine).SetOptions(true));
		this.mySwitchSeq.Insert(1.9f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(20.429f, -13.129f, -3.046f), 0.6f).SetEase(Ease.OutSine).SetOptions(true));
		this.mySwitchSeq.Insert(1.9f, DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0f, 1.218f, 0.95f), 0.6f).SetEase(Ease.OutSine));
		this.mySwitchSeq.Insert(3f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 0.8f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(7f, DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0.066f, 1.218f, 1.002f), 0.8f).SetEase(Ease.Linear));
		this.mySwitchSeq.Insert(7f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(0f, 69f, 0f), 1.5f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(11f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 2f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(15f, DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0.066f, 1.1f, 1.115f), 0.8f).SetEase(Ease.Linear));
		this.mySwitchSeq.Insert(15f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(60.04f, 0f, 0f), 0.8f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(17f, DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0f, 1.218f, 0.95f), 0.8f).SetEase(Ease.Linear));
		this.mySwitchSeq.Insert(17f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 0.8f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(22f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(-10.311f, -61.203f, 0f), 1.5f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(25.5f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 1f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(27.5f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(17.391f, 0f, 0f), 1f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Insert(30.5f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, Vector3.zero, 1f).SetEase(Ease.Linear).SetOptions(true));
		this.mySwitchSeq.Play<Sequence>();
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
		this.myCheckDoorSeq.Insert(0.2f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.876f, base.transform.localPosition.z), 0.6f).SetEase(Ease.InSine));
		this.myCheckDoorSeq.Insert(0.2f, DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(0f, 1f, 0f), 0.6f).SetEase(Ease.InSine));
		this.myCheckDoorSeq.Insert(0.2f, DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 60f, 0.6f).SetEase(Ease.InSine));
		this.myCheckDoorSeq.Insert(1.2f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -245f, 0f), 0.75f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(1.95f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-3.328f, 3.876f, -2.402f), 1f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(1.95f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, -261f, 0f), 1f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(2.95f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(7.486f, 3.876f, -5.28f), 2f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(2.95f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 2f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(4.95f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(13.521f, 3.876f, -5.28f), 1f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(5.95f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, Vector3.zero, 1f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(6.95f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(13.521f, 3.876f, 3.086f), 3f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(12.96f, DOTween.To(() => this.mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCamera.transform.localPosition = x;
		}, new Vector3(-0.2299995f, 1.076f, 0.6381001f), 0.96f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(12.96f, DOTween.To(() => this.mainCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCamera.transform.localRotation = x;
		}, new Vector3(6.196f, 0f, 0f), 0.96f).SetEase(Ease.Linear).SetOptions(true));
		this.myCheckDoorSeq.Insert(12.96f, DOTween.To(() => this.mainCamera.fieldOfView, delegate(float x)
		{
			this.mainCamera.fieldOfView = x;
		}, 42f, 0.96f).SetEase(Ease.Linear));
		this.myCheckDoorSeq.Insert(1.95f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(2.45f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(2.95f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(3.45f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(3.95f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(4.45f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(4.95f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(5.45f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(6.95f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(7.45f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(7.95f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(8.45f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(8.95f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Insert(9.45f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f).SetEase(Ease.Linear).SetRelative(true));
		this.myCheckDoorSeq.Play<Sequence>();
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
						DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
						{
							base.transform.localRotation = x;
						}, this.leftLook, 0.5f).SetEase(Ease.OutSine).SetOptions(true);
					}
				}
				else if (CrossPlatformInputManager.GetAxis("Horizontal") > 0f && base.transform.localRotation.eulerAngles.y != this.maxRightLook)
				{
					DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
					{
						base.transform.localRotation = x;
					}, this.rightLook, 0.5f).SetEase(Ease.OutSine).SetOptions(true);
				}
			}
		}
		else if (CrossPlatformInputManager.GetButton("Vertical") && CrossPlatformInputManager.GetAxis("Vertical") < 0f)
		{
			if (base.transform.localRotation.eulerAngles.y != this.maxBehindLook)
			{
				DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
				{
					base.transform.localRotation = x;
				}, this.behindLook, 0.5f).SetEase(Ease.OutSine).SetOptions(true);
			}
		}
		else if (base.transform.localRotation.eulerAngles.y != 0f)
		{
			DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, Vector3.zero, 0.5f).SetEase(Ease.OutSine).SetOptions(true);
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
							DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
							{
								base.transform.localPosition = x;
							}, this.duckLook, 0.5f).SetEase(Ease.Linear);
							DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
							{
								base.transform.localRotation = x;
							}, this.leftLook, 0.5f).SetEase(Ease.OutSine).SetOptions(true);
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
						DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
						{
							base.transform.localPosition = x;
						}, this.peakLook, 1f).SetEase(Ease.Linear);
						DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
						{
							base.transform.localRotation = x;
						}, this.peakROT, 1f).SetEase(Ease.Linear).SetOptions(true);
					}
				}
			}
			else if (base.transform.localRotation.eulerAngles.y != 0f)
			{
				GameManager.GetTheBreatherManager().playerCanBeSeen = false;
				DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
				{
					base.transform.localPosition = x;
				}, this.duckLook, 0.5f).SetEase(Ease.Linear);
				DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
				{
					base.transform.localRotation = x;
				}, Vector3.zero, 0.5f).SetEase(Ease.OutSine).SetOptions(true);
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
				this.windowPeakSeq.Insert(0f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
				{
					base.transform.localPosition = x;
				}, this.peakLook, 1f).SetEase(Ease.Linear));
				this.windowPeakSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
				{
					base.transform.localRotation = x;
				}, this.peakROT, 1f).SetEase(Ease.Linear).SetOptions(true));
				this.windowPeakSeq.Play<Sequence>();
			}
		}
		else if (this.lockPivot)
		{
			this.lockPivot = false;
			if (this.windowPeakSeq.IsPlaying())
			{
				this.windowPeakSeq.Kill(false);
			}
			this.windowPeakSeq = DOTween.Sequence();
			this.windowPeakSeq.Insert(0f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, this.duckLook, 0.5f).SetEase(Ease.Linear));
			this.windowPeakSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, Vector2.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
			this.windowPeakSeq.Play<Sequence>();
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
				DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
				{
					this.myCamera.transform.localRotation = x;
				}, new Vector3(0f, 49.99f), 0.25f).SetEase(Ease.InSine);
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
			this.myJumpSeq.Insert(0f, DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
			{
				base.transform.localRotation = x;
			}, new Vector3(base.transform.localRotation.eulerAngles.x, 90f, base.transform.localRotation.eulerAngles.z), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
			this.myJumpSeq.Insert(0f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
			{
				base.transform.localPosition = x;
			}, new Vector3(-6.886f, base.transform.localPosition.y, base.transform.localPosition.z), 0.3f).SetEase(Ease.OutSine));
			this.myJumpSeq.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.myCamera.transform.localRotation = x;
			}, new Vector3(-4.378f, base.transform.localRotation.eulerAngles.y, base.transform.localRotation.eulerAngles.z), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
			this.myJumpSeq.Insert(3.8f, DOTween.To(() => this.myLDB.Distortion, delegate(float x)
			{
				this.myLDB.Distortion = x;
			}, 2f, 1.2f).SetEase(Ease.Linear));
			this.myJumpSeq.Insert(3.8f, DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
			{
				this.myLDB.CubicDistortion = x;
			}, 1.3f, 1.2f).SetEase(Ease.Linear));
			this.myJumpSeq.Insert(3.8f, DOTween.To(() => this.myLDB.Scale, delegate(float x)
			{
				this.myLDB.Scale = x;
			}, 0.75f, 1.2f).SetEase(Ease.Linear));
			this.myJumpSeq.Play<Sequence>();
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
		this.myJumpSeq.Insert(2.45f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.154f, base.transform.localPosition.z), 0.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(2.45f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(-7.211f, this.myCamera.transform.localRotation.eulerAngles.y, this.myCamera.transform.localRotation.eulerAngles.z), 0.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(2.85f, DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 1.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(2.85f, DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 1.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(2.85f, DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 1.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Play<Sequence>();
	}

	private void triggerDoorBackJump()
	{
		GameManager.TimeSlinger.FireTimer(3.95f, new Action(this.enableLensBlur));
		GameManager.TimeSlinger.FireTimer(4.5f, new Action(this.triggerKidnapGameOver));
		this.myJumpSeq = DOTween.Sequence();
		this.myJumpSeq.Insert(3.65f, DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(base.transform.localPosition.x, 3.154f, base.transform.localPosition.z), 0.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(3.65f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(-7.211f, this.myCamera.transform.localRotation.eulerAngles.y, this.myCamera.transform.localRotation.eulerAngles.z), 0.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(4.05f, DOTween.To(() => this.myLDB.Distortion, delegate(float x)
		{
			this.myLDB.Distortion = x;
		}, 2f, 1.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(4.05f, DOTween.To(() => this.myLDB.CubicDistortion, delegate(float x)
		{
			this.myLDB.CubicDistortion = x;
		}, 1.3f, 1.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Insert(4.05f, DOTween.To(() => this.myLDB.Scale, delegate(float x)
		{
			this.myLDB.Scale = x;
		}, 0.75f, 1.5f).SetEase(Ease.Linear));
		this.myJumpSeq.Play<Sequence>();
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
