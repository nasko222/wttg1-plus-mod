using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhereAmIManager : MonoBehaviour
{
	private void triggerTheRun()
	{
		GameManager.TimeSlinger.FireTimer(13.2f, new Action(this.triggerWalkAni));
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 0.4f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false, 0.8f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 1.2f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false, 1.6f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 2f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false, 2.4f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 2.8f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false, 3.2f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 3.6f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false, 4f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 4.4f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false, 4.8f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 5.2f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.7f, false, 5.6f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.7f, false, 6f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.breath1, 0.3f, false, 0.9f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.breath1, 0.3f, false, 2.64f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.breath1, 0.3f, false, 4.38f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.breath2, 0.5f, false, 7.1f);
		TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.screenFade.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.screenFade.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f), 1), 6.1f);
		this.pantSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.pantSeq, 7.1f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.5f, 0f), 0.8f), 1), true));
		TweenSettingsExtensions.Insert(this.pantSeq, 7.9f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.5f, 0f), 0.4f), 1), true));
		TweenSettingsExtensions.Insert(this.pantSeq, 8.3f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.5f, 0f), 0.4f), 1), true));
		TweenSettingsExtensions.Insert(this.pantSeq, 8.7f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.5f, 0f), 0.4f), 1), true));
		TweenSettingsExtensions.Insert(this.pantSeq, 9.1f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.5f, 0f), 0.8f), 1), true));
		TweenSettingsExtensions.Insert(this.pantSeq, 9.9f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.5f, 0f), 0.4f), 1), true));
		TweenSettingsExtensions.Insert(this.pantSeq, 10.3f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.5f, 0f), 1.1f), 1), true));
		TweenSettingsExtensions.Insert(this.pantSeq, 11.4f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 1f, 0f), 0.8f), 1));
		TweenSettingsExtensions.Insert(this.pantSeq, 11.4f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCam.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCam.transform.localRotation = x;
		}, Vector3.zero, 0.8f), 1), true));
		TweenExtensions.Play<Sequence>(this.pantSeq);
	}

	private void triggerWalkAni()
	{
		this.walkSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.walkSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.whereAmIController.transform.localPosition, delegate(Vector3 x)
		{
			this.whereAmIController.transform.localPosition = x;
		}, this.endWalkPoint, 10f), 1));
		TweenSettingsExtensions.Insert(this.walkSeq, 0f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 0.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 1f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 1.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 2f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 2.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 3f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 3.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 4f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 4.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 5.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 6f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 6.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 7f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 7.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 8f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 8.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 9f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, -0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 9.5f, TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.mainCam.transform.localPosition, delegate(Vector3 x)
		{
			this.mainCam.transform.localPosition = x;
		}, new Vector3(0f, 0.25f, 0f), 0.5f), 1), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 2.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCam.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCam.transform.localRotation = x;
		}, new Vector3(-6.89f, 44.255f, 0f), 1f), 3), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 4f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCam.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCam.transform.localRotation = x;
		}, Vector3.zero, 0.75f), 2), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 8.5f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCam.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCam.transform.localRotation = x;
		}, new Vector3(0f, -53f, 0f), 0.65f), 3), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 10f, TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.mainCam.transform.localRotation, delegate(Quaternion x)
		{
			this.mainCam.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 2.5f), 6), true));
		TweenSettingsExtensions.Insert(this.walkSeq, 12.6f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainCam.fieldOfView, delegate(float x)
		{
			this.mainCam.fieldOfView = x;
		}, 27f, 1f), 1));
		TweenExtensions.Play<Sequence>(this.walkSeq);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.4f, false);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.4f, false, 1f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.4f, false, 2f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.4f, false, 3f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.4f, false, 4f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.4f, false, 5f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.4f, false, 6f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.4f, false, 7f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep1, 0.4f, false, 8f);
		GameManager.AudioSlinger.FireSound(AudioHubs.PLAYER, AudioLayer.SFX, this.footStep2, 0.4f, false, 9f);
		GameManager.TimeSlinger.FireTimer(3.5f, new Action(this.triggerDriveBy));
		GameManager.TimeSlinger.FireTimer(14f, new Action(this.triggerJump));
		GameManager.TimeSlinger.FireTimer(15.5f, new Action(this.triggerTeaser));
	}

	private void triggerDriveBy()
	{
		this.fatherCar.SetActive(true);
		this.fatherCar.GetComponent<extCarBehavior>().triggerDriveBy();
	}

	private void triggerJump()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.jumpSound, 1f, false, 0.1f);
		this.exeObject.transform.localPosition = new Vector3(65.846f, -0.949f, 35.281f);
		TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.exeObject.transform.localPosition, delegate(Vector3 x)
		{
			this.exeObject.transform.localPosition = x;
		}, new Vector3(65.846f, -0.437f, 35.281f), 0.1f), 1);
	}

	private void triggerTeaser()
	{
		GameManager.FileSlinger.deleteFile("wttg2.gd");
		GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.BACKGROUND, 0f);
		GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.MOMENT, this.rwsTune, 0.75f, false, 3f);
		GameManager.TimeSlinger.FireTimer(31f, new Action(this.triggerMainScreen));
		GameManager.TimeSlinger.FireTimer(1f, new Action(this.removeEnv));
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.screenFade.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.screenFade.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f), 1);
		TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.teaserText.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.teaserText.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 10f), 1), 4.6f);
	}

	private void removeEnv()
	{
		this.MainEnv.SetActive(false);
	}

	private void triggerMainScreen()
	{
		SceneManager.LoadScene(0);
	}

	private void Start()
	{
		Cursor.lockState = 1;
		Cursor.visible = false;
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.triggerTheRun));
	}

	public GameObject MainEnv;

	public GameObject whereAmIController;

	public Camera mainCam;

	public GameObject fatherCar;

	public GameObject exeObject;

	public GameObject screenFade;

	public GameObject teaserText;

	public Vector3 endWalkPoint;

	public AudioClip jumpSound;

	public AudioClip footStep1;

	public AudioClip footStep2;

	public AudioClip breath1;

	public AudioClip breath2;

	public AudioClip rwsTune;

	private Sequence walkSeq;

	private Sequence pantSeq;
}
