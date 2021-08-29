using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class KidnapperBehavior : MonoBehaviour
{
	public void triggerGotoSpawn()
	{
		base.transform.localPosition = this.defaultSpawnPOS;
	}

	public void triggerHeyKid()
	{
	}

	public void triggerWalkbackJumpIdle()
	{
		this.myAC.SetTrigger("endWalking");
		TweenExtensions.Kill(this.myMoveSeq, false);
		this.doFootSteps = false;
		base.transform.localRotation = Quaternion.Euler(this.walkBackJumpROT);
		base.transform.localPosition = new Vector3(this.walkBackJumpPOS.x, this.walkBackJumpPOS.y, this.walkBackJumpPOS.z);
	}

	public void triggerWalkBackJump()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.jumpSound1, 0.9f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.goodEveningClip, 1f, false, 0.5f);
		this.myAC.SetTrigger("wbJump");
	}

	public void triggerUsingComptuerJumpIlde()
	{
		base.transform.localPosition = this.usingCompJumpPOS;
	}

	public void triggerUsingComputerJump()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.jumpSound1, 1f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.goodEveningClip, 0.9f, false, 0.3f);
		this.myAC.SetTrigger("N2CJump");
		this.myMoveSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.myMoveSeq, 3.5f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-3.823f, -3.579f, 2.263f), 0.4f), 1));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 3.5f, TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(7.169f, base.transform.localRotation.eulerAngles.y, base.transform.localRotation.eulerAngles.z), 0.4f), 1));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 3.9f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, new Vector3(-3.903f, -3.579f, 2.263f), 1.5f), 1));
		TweenExtensions.Play<Sequence>(this.myMoveSeq);
	}

	public void triggerGotoClimbPoint()
	{
		this.endFootStepSounds();
		TweenExtensions.Kill(this.myMoveSeq, false);
		GameManager.TimeSlinger.FireTimer(0.2f, new Action(this.switchBackToIdle));
		this.myMoveSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.triggerClimbJump));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, this.climbJumpPOS, 0.3f), 1));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => base.transform.localRotation, delegate(Quaternion x)
		{
			base.transform.localRotation = x;
		}, new Vector3(this.climbJumpROT.x, this.climbJumpROT.y, this.climbJumpROT.z), 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.myMoveSeq);
	}

	public void triggerClimbJump()
	{
		base.transform.localPosition = this.climbJumpPOS;
		base.transform.localRotation = new Quaternion(this.climbJumpROT.x, this.climbJumpROT.y, this.climbJumpROT.z, 0f);
		this.myAC.SetTrigger("climbJump");
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.jumpSound1, 0.9f, false, 0.3f);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.indoorLand, 1f, false, 1.8f);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.indoorFootStep1, 1f, false, 3.4f);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.indoorFootStep2, 1f, false, 3.9f);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.indoorFootStep3, 1f, false, 4.55f);
		this.myMoveSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.myMoveSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.meshObject.transform.localPosition, delegate(Vector3 x)
		{
			this.meshObject.transform.localPosition = x;
		}, new Vector3(this.meshObject.transform.localPosition.x, this.meshObject.transform.localPosition.y, 1f), 0.1f), 1));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 0.28f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.meshObject.transform.localPosition, delegate(Vector3 x)
		{
			this.meshObject.transform.localPosition = x;
		}, new Vector3(0.4f, 5.9f, 2.57f), 2f), 1));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.meshObject.transform.localPosition, delegate(Vector3 x)
		{
			this.meshObject.transform.localPosition = x;
		}, new Vector3(1f, 7.2f, 2.8f), 0.4f), 1));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 2.7f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.meshObject.transform.localPosition, delegate(Vector3 x)
		{
			this.meshObject.transform.localPosition = x;
		}, new Vector3(1f, 1.2f, 5.4f), 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.myMoveSeq);
	}

	public void triggerRoamMode(float roamTime)
	{
		base.transform.localPosition = this.roamModeSpawnPOS;
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.theSource, 0.4f, false);
		this.myAC.SetTrigger("triggerWalking");
		this.myMoveSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.endFootStepSounds));
		TweenSettingsExtensions.Insert(this.myMoveSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => base.transform.localPosition, delegate(Vector3 x)
		{
			base.transform.localPosition = x;
		}, this.roamModeEndPOS, roamTime * 0.8f), 1));
		TweenExtensions.Play<Sequence>(this.myMoveSeq);
		this.roamTimeStamp = Time.time;
		this.doFootSteps = true;
		this.fi = 0;
	}

	public void endRoamMode()
	{
		this.myAC.SetTrigger("endWalking");
		TweenExtensions.Kill(this.myMoveSeq, false);
		this.triggerGotoSpawn();
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.nothingHere, 0.2f, false);
	}

	public void triggerDoorJump()
	{
		this.myAC.SetTrigger("doorJump");
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.gotoDoorJump));
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.jumpSound1, 0.9f, false, 0.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.SFX, this.indoorFootStep1, 0.65f, false, 0.66f);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.SFX, this.indoorFootStep2, 0.4f, false, 1.1f);
		GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.VOICE, this.goodEveningClip, 1f, false, 2f);
	}

	public void gotoDoorJump()
	{
		base.transform.localPosition = this.doorJumpPOS;
		base.transform.localRotation = Quaternion.Euler(this.doorJumpROT);
	}

	private void endFootStepSounds()
	{
		this.doFootSteps = false;
	}

	private void switchBackToIdle()
	{
		this.myAC.SetTrigger("endWalking");
	}

	private void Start()
	{
		this.myAC = base.GetComponent<Animator>();
		GameManager.SetKidnapper(this);
	}

	private void Update()
	{
		if (this.doFootSteps && Time.time - this.roamTimeStamp >= 1.1f)
		{
			if (this.fi == 0)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.outdoorFootStep1, 0.4f, false);
				this.fi += 1;
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.KIDNAPPER, AudioLayer.BACKGROUND, this.outdoorFootStep2, 0.4f, false);
				this.fi = 0;
			}
			this.roamTimeStamp = Time.time;
		}
	}

	public GameObject meshObject;

	public AudioClip jumpSound1;

	public AudioClip goodEveningClip;

	public AudioClip heyKid;

	public AudioClip theSource;

	public AudioClip nothingHere;

	public AudioClip indoorLand;

	public AudioClip indoorFootStep1;

	public AudioClip indoorFootStep2;

	public AudioClip indoorFootStep3;

	public AudioClip outdoorFootStep1;

	public AudioClip outdoorFootStep2;

	public Vector3 defaultSpawnPOS;

	public Vector3 walkBackJumpPOS;

	public Vector3 walkBackJumpROT;

	public Vector3 usingCompJumpPOS;

	public Vector3 usingCompJumpROT;

	public Vector3 climbJumpPOS;

	public Vector3 climbJumpROT;

	public Vector3 roamModeSpawnPOS;

	public Vector3 roamModeSpawnROT;

	public Vector3 roamModeEndPOS;

	public Vector3 doorJumpPOS;

	public Vector3 doorJumpROT;

	private Animator myAC;

	private Sequence myMoveSeq;

	private float roamTimeStamp;

	private bool doFootSteps;

	private short fi;
}
