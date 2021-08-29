using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
	public void turnPhoneOn()
	{
		this.offStateCG.alpha = 1f;
		this.PhoneScreen.gameObject.SetActive(true);
		DOTween.To(() => this.offStateCG.alpha, delegate(float x)
		{
			this.offStateCG.alpha = x;
		}, 0f, 0.4f).SetEase(Ease.Linear);
		this.phoneLockOnTimeStamp = Time.time;
		this.phoneLockOn = true;
	}

	public void turnPhoneOff()
	{
		DOTween.To(() => this.offStateCG.alpha, delegate(float x)
		{
			this.offStateCG.alpha = x;
		}, 1f, 0.4f).SetEase(Ease.Linear);
		GameManager.TimeSlinger.FireTimer(0.4f, new Action(this.setPhoneScreenOff));
	}

	public void hardTurnPhoneOff()
	{
		this.PhoneScreen.SetActive(false);
	}

	public void setIncomingCall(AudioClip setCallClip)
	{
		this.currentCallClip = setCallClip;
		this.triggerIncomingCall();
	}

	public void answerPhone()
	{
		this.incomingCall = false;
		this.onCall = true;
		this.phoneAniSeq.Kill(true);
		GameManager.TimeSlinger.KillTimerWithID("cellPhoneVib");
		GameManager.TimeSlinger.KillTimerWithID("cellPhoneRing");
		GameManager.TimeSlinger.KillTimerWithID("missedCall");
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.phoneVibrateSFX.name);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.phoneRingSFX.name + ":1.2");
		GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phonePickUpSFX, 0.2f, false);
		DOTween.To(() => this.incomingCallCG.alpha, delegate(float x)
		{
			this.incomingCallCG.alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear);
		this.phoneClockCG.alpha = 0f;
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.playCallAudio));
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.myPhoneAction.phoneDoneAction));
		GameManager.TimeSlinger.FireTimer(2.5f, new Action(GameManager.GetTheTrackerManager().playerNoiseHasHappend), "answerPhoneNoise");
	}

	public void hangUpPhone()
	{
		this.onCall = false;
		this.myPhoneAction.clearAction();
		this.myPhoneAction.phoneDoneAction();
		GameManager.TimeSlinger.KillTimerWithID("answerPhoneNoise");
		GameManager.TimeSlinger.FireTimer(5.1f, new Action(this.triggerPhoneScreenOff));
		GameManager.TimeSlinger.FireTimer(6.5f, new Action(this.resetPhoneCGs));
		GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phoneHangUpSFX, 0.2f, false);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.currentCallClip.name);
		DOTween.To(() => this.onCallCG.alpha, delegate(float x)
		{
			this.onCallCG.alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear);
		DOTween.To(() => this.phoneClockCG.alpha, delegate(float x)
		{
			this.phoneClockCG.alpha = x;
		}, 1f, 0.5f).SetEase(Ease.Linear);
	}

	public void hangUpPhone(bool forcedHangUp)
	{
		this.onCall = false;
		this.myPhoneAction.clearAction();
		this.myPhoneAction.phoneDoneAction();
		GameManager.TimeSlinger.KillTimerWithID("answerPhoneNoise");
		if (forcedHangUp)
		{
			GameManager.TimeSlinger.KillTimerWithID("autoHangUp");
			GameManager.GetTheBreatherManager().callWithWindowTime(30f, true);
		}
		GameManager.TimeSlinger.FireTimer(5.1f, new Action(this.triggerPhoneScreenOff));
		GameManager.TimeSlinger.FireTimer(6.5f, new Action(this.resetPhoneCGs));
		GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phoneHangUpSFX, 0.2f, false);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.currentCallClip.name);
		DOTween.To(() => this.onCallCG.alpha, delegate(float x)
		{
			this.onCallCG.alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear);
		DOTween.To(() => this.phoneClockCG.alpha, delegate(float x)
		{
			this.phoneClockCG.alpha = x;
		}, 1f, 0.5f).SetEase(Ease.Linear);
	}

	public void killPhone()
	{
		this.stopIncomingCall();
	}

	public bool isPhoneRinging()
	{
		return this.incomingCall;
	}

	public bool isOnPhoneCall()
	{
		return this.onCall;
	}

	public void triggerGentleCall(AudioClip setCallClip)
	{
		this.currentCallClip = setCallClip;
		GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phoneRingSFX, 0.75f, true);
		GameManager.TimeSlinger.FireTimer(3f, new Action(this.answerGentleCall));
	}

	private void prepPhone()
	{
		this.defaultPhonePOS = this.PhoneObject.transform.localPosition;
		this.defaultPhoneROT = this.PhoneObject.transform.localRotation.eulerAngles;
	}

	private void setPhoneScreenOff()
	{
		this.PhoneScreen.gameObject.SetActive(false);
		this.myPhoneAction.phoneDoneAction();
	}

	private void triggerIncomingCall()
	{
		this.incomingCall = true;
		this.triggerPhoneScreenOn();
		this.triggerPhoneVibrate();
		GameManager.TimeSlinger.FireTimer(this.phoneVibrateSFX.length + 1f, new Action(this.triggerPhoneVibrate), 1, "cellPhoneVib");
		GameManager.TimeSlinger.FireTimer((this.phoneVibrateSFX.length + 1f) * 2f, new Action(this.triggerPhoneRing), "cellPhoneRing");
		GameManager.TimeSlinger.FireTimer((this.phoneVibrateSFX.length + 1f) * 2f + (this.phoneRingSFX.length + 1.2f) * 4f, new Action(this.triggerMissedCall), "missedCall");
	}

	private void triggerMissedCall()
	{
		if (this.incomingCall)
		{
			this.triggerPhoneScreenOff();
			this.incomingCall = false;
			GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.phoneRingSFX.name + ":1.2");
			GameManager.GetTheBreatherManager().callWithWindowTime(30f, true);
		}
	}

	private void triggerPhoneVibrate()
	{
		if (this.incomingCall)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phoneVibrateSFX, 0.25f, false);
			this.phoneAniSeq = DOTween.Sequence();
			this.phoneAniSeq.Insert(0f, DOTween.To(() => this.PhoneObject.transform.localRotation, delegate(Quaternion x)
			{
				this.PhoneObject.transform.localRotation = x;
			}, new Vector3(this.defaultPhoneROT.x, this.defaultPhoneROT.y * 1.02f, this.defaultPhoneROT.z), 0.5385f).SetEase(Ease.Linear));
			this.phoneAniSeq.Insert(0f, DOTween.To(() => this.PhoneObject.transform.localPosition, delegate(Vector3 x)
			{
				this.PhoneObject.transform.localPosition = x;
			}, new Vector3(this.defaultPhonePOS.x + this.defaultPhonePOS.x * 0.001f, this.defaultPhonePOS.y, this.defaultPhonePOS.z - this.defaultPhonePOS.z * 0.001f), 0.5385f).SetEase(Ease.Linear));
			this.phoneAniSeq.Insert(0.5385f, DOTween.To(() => this.PhoneObject.transform.localRotation, delegate(Quaternion x)
			{
				this.PhoneObject.transform.localRotation = x;
			}, new Vector3(this.defaultPhoneROT.x, this.defaultPhoneROT.y, this.defaultPhoneROT.z), 0.5385f).SetEase(Ease.Linear));
			this.phoneAniSeq.Insert(0.5385f, DOTween.To(() => this.PhoneObject.transform.localPosition, delegate(Vector3 x)
			{
				this.PhoneObject.transform.localPosition = x;
			}, new Vector3(this.defaultPhonePOS.x, this.defaultPhonePOS.y, this.defaultPhonePOS.z), 0.5385f).SetEase(Ease.Linear));
			this.phoneAniSeq.Play<Sequence>();
		}
	}

	private void triggerPhoneRing()
	{
		if (this.incomingCall)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phoneRingSFX, 0.75f, true, true, 1.2f);
			GameManager.GetTheTrackerManager().playerNoiseHasHappend();
		}
	}

	private void triggerPhoneScreenOn()
	{
		this.offStateCG.alpha = 1f;
		this.PhoneScreen.gameObject.SetActive(true);
		DOTween.To(() => this.offStateCG.alpha, delegate(float x)
		{
			this.offStateCG.alpha = x;
		}, 0f, 0.4f).SetEase(Ease.Linear);
	}

	private void triggerPhoneScreenOff()
	{
		DOTween.To(() => this.offStateCG.alpha, delegate(float x)
		{
			this.offStateCG.alpha = x;
		}, 1f, 0.4f).SetEase(Ease.Linear);
		GameManager.TimeSlinger.FireTimer(0.4f, new Action(this.setPhoneScreenOff));
	}

	private void stopIncomingCall()
	{
		this.incomingCall = false;
		this.phoneAniSeq.Kill(true);
		GameManager.TimeSlinger.KillTimerWithID("cellPhoneVib");
		GameManager.TimeSlinger.KillTimerWithID("cellPhoneRing");
		GameManager.TimeSlinger.KillTimerWithID("missedCall");
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.phoneVibrateSFX.name);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.phoneRingSFX.name + ":1.2");
		this.triggerPhoneScreenOff();
	}

	private void resetPhoneCGs()
	{
		this.incomingCallCG.alpha = 1f;
		this.onCallCG.alpha = 1f;
	}

	private void playCallAudio()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.currentCallClip, 0.4f, false);
		GameManager.TimeSlinger.FireTimer(this.currentCallClip.length + 1f, new Action(this.hangUpPhone), "autoHangUp");
	}

	private void answerGentleCall()
	{
		this.PhoneScreen.gameObject.SetActive(true);
		this.offStateCG.alpha = 0f;
		this.phoneClockCG.alpha = 0f;
		this.incomingCallCG.alpha = 0f;
		this.gObject.SetActive(true);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PHONE, this.phoneRingSFX.name);
		GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phonePickUpSFX, 0.2f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.PHONE, this.currentCallClip, 0.4f, false, 1.8f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PHONE, AudioLayer.PHONE, this.phoneHangUpSFX, 0.1f, false, this.currentCallClip.length + 2.5f);
	}

	private void Awake()
	{
		GameManager.SetPhoneManager(this);
	}

	private void Start()
	{
		this.hardTurnPhoneOff();
		this.prepPhone();
	}

	private void Update()
	{
		if (this.phoneLockOn && Time.time - this.phoneLockOnTimeStamp >= this.onIdleTime)
		{
			this.phoneLockOn = false;
			this.turnPhoneOff();
		}
	}

	[Range(1f, 20f)]
	public float onIdleTime = 10f;

	public GameObject PhoneObject;

	public GameObject PhoneScreen;

	public Canvas PhoneCanvas;

	public CanvasGroup offStateCG;

	public CanvasGroup incomingCallCG;

	public CanvasGroup onCallCG;

	public CanvasGroup phoneClockCG;

	public GameObject gObject;

	public PhoneAction myPhoneAction;

	public AudioClip phoneVibrateSFX;

	public AudioClip phoneRingSFX;

	public AudioClip phonePickUpSFX;

	public AudioClip phoneHangUpSFX;

	private Sequence phoneOffStateSeq;

	private Sequence phoneAniSeq;

	private bool phoneLockOn;

	private bool incomingCall;

	private bool onCall;

	private float phoneLockOnTimeStamp;

	private Vector3 defaultPhonePOS;

	private Vector3 defaultPhoneROT;

	private AudioClip currentCallClip;
}
