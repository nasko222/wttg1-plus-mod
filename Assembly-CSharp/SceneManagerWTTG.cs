using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerWTTG : MonoBehaviour
{
	public void checkAction(RaycastHit theRay)
	{
		if (this.readyForAction)
		{
			this.hasCurrentAction = true;
			this.currentActionRay = theRay;
			this.currentActionRay.collider.SendMessageUpwards("runAction");
		}
	}

	public void clearAction()
	{
		if (this.hasCurrentAction)
		{
			this.currentActionRay.collider.SendMessageUpwards("clearAction");
			this.hasCurrentAction = false;
		}
	}

	public void performAction(short clickIndex = 0)
	{
		if (this.hasCurrentAction)
		{
			this.currentActionRay.collider.SendMessageUpwards("performAction");
			this.hasCurrentAction = false;
		}
	}

	public void performHoldAction(short clickIndex = 0)
	{
		if (this.hasCurrentAction)
		{
			this.currentActionRay.collider.SendMessageUpwards("performHoldAction");
		}
	}

	public void clearHoldAction()
	{
		if (this.hasCurrentAction)
		{
			this.currentActionRay.collider.SendMessageUpwards("clearHoldAction");
		}
	}

	public void refreshReflectorProbes()
	{
		for (int i = 0; i < this.ReflectorProbes.Count; i++)
		{
			this.ReflectorProbes[i].RenderProbe();
		}
		this.readyForAction = true;
	}

	public void triggerLights()
	{
		this.switchSeq = DOTween.Sequence();
		GameManager.TimeSlinger.FireTimer(0.1f, new Action(this.switchLights));
		if (this.lightsOn)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.lightSwitchOffSound, 0.25f, false, 0.1f);
			GameManager.GetTheCloud().myTimeManager.freezeTime = true;
			TweenSettingsExtensions.Insert(this.switchSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.lightSwitchKnob.transform.localPosition, delegate(Vector3 x)
			{
				this.lightSwitchKnob.transform.localPosition = x;
			}, this.offSwitchPOS, 0.2f), 1));
			TweenSettingsExtensions.Insert(this.switchSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.lightSwitchKnob.transform.localRotation, delegate(Quaternion x)
			{
				this.lightSwitchKnob.transform.localRotation = x;
			}, this.offSwitchROT, 0.2f), 1));
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.lightSwitchOnSound, 0.25f, false, 0.1f);
			if (!GameManager.GetTheHackerManager().hackActive)
			{
				GameManager.GetTheCloud().myTimeManager.freezeTime = false;
			}
			TweenSettingsExtensions.Insert(this.switchSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.lightSwitchKnob.transform.localPosition, delegate(Vector3 x)
			{
				this.lightSwitchKnob.transform.localPosition = x;
			}, this.defaultLightSwitchKnobPOS, 0.2f), 1));
			TweenSettingsExtensions.Insert(this.switchSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => this.lightSwitchKnob.transform.localRotation, delegate(Quaternion x)
			{
				this.lightSwitchKnob.transform.localRotation = x;
			}, this.defaultLightSwitchKnobROT, 0.2f), 1));
		}
		TweenExtensions.Play<Sequence>(this.switchSeq);
	}

	public void triggerLightJump()
	{
		bool flag = true;
		if (!this.inDoorAction && !GameManager.GetTheMainController().isMovingAround && !GameManager.GetTheMainController().isUsingComputer)
		{
			flag = false;
			GameManager.GetLightSwitchAction().triggerLightSwitch();
			this.triggerLights();
		}
		if (flag)
		{
			GameManager.TimeSlinger.FireTimer(3f, new Action(this.triggerLightJump));
		}
	}

	public void ForceLights()
	{
		GameManager.GetLightSwitchAction().triggerLightSwitch();
		this.triggerLights();
	}

	public void setDoorAction(bool setValue)
	{
		this.inDoorAction = setValue;
	}

	public bool areTheLightsOn()
	{
		return this.lightsOn;
	}

	public bool isInDoorAction()
	{
		return this.inDoorAction;
	}

	public void triggerNymphoEnding()
	{
		GameManager.FileSlinger.deleteFile("wttg2.gd");
		this.NymphoObject.SetActive(true);
		this.WTTGPhone.transform.SetParent(this.NymphoWristJoint.transform);
		this.WTTGPhone.transform.localPosition = new Vector3(0.1981f, 0.1538f, 0.0271f);
		this.WTTGPhone.transform.localRotation = Quaternion.Euler(new Vector3(146.35f, 450.076f, 188.38f));
		this.WTTGPhone.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
		this.mirrorCam.SetActive(true);
		this.mirrorRT.SetActive(true);
		GameManager.GetTheUIManager().hideCrossHair();
		GameManager.GetTheUIManager().myPauseManager.lockPause = true;
		GameManager.GetTheCloud().myTimeManager.freezeTime = true;
		GameManager.GetTheBreatherManager().masterLock = true;
		GameManager.GetTheMainController().masterLock = true;
		GameManager.GetTheMainController().isUsingComputer = false;
		GameManager.GetTheMainController().myComputerController.lockControls = true;
		GameManager.GetThePhoneManager().triggerGentleCall(this.theConvo);
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.tirggerLeaveComputer));
		GameManager.TimeSlinger.FireTimer(49f, new Action(this.triggerWalkToMirror));
	}

	private void tirggerLeaveComputer()
	{
		GameManager.GetTheMainController().triggerNymphoEndingAni();
		GameManager.TimeSlinger.FireTimer(1.1f, new Action(this.triggerPickUpPhone));
	}

	private void triggerPickUpPhone()
	{
		this.NymphoObject.GetComponent<Animator>().SetTrigger("triggerPhonePickup");
	}

	private void triggerWalkToMirror()
	{
		this.WTTGPhone.SetActive(false);
		this.NymphoObject.transform.localPosition = new Vector3(0f, -3.843f, 0f);
		GameManager.GetTheMainController().triggerNymphoEndWalking();
		GameManager.TimeSlinger.FireTimer(5.95f, new Action(this.triggerNymphoWalkingAni));
		GameManager.TimeSlinger.FireTimer(9.9f, new Action(this.triggerNymphoMirrorAni));
		GameManager.TimeSlinger.FireTimer(20.75f, new Action(this.triggerTheTeaser));
	}

	private void triggerTheTeaser()
	{
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTER, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTERHARDWARE, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.MAINROOM, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.LEFTROOM, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.OUTSIDE, 0f);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.PLAYER, "Uneasy1.wav");
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.MOMENT, this.rwsTune, 0.75f, false, 0.5f);
		GameManager.GetTheUIManager().fadeScreenOutWithTime(0.2f);
		TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.teaserText.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.teaserText.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 10f), 1), 2.1f);
		TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => GameManager.GetTheMainController().mainCamera.transform.localPosition, delegate(Vector3 x)
		{
			GameManager.GetTheMainController().mainCamera.transform.localPosition = x;
		}, Vector3.zero, 0.1f), 0.3f);
		GameManager.TimeSlinger.FireTimer(28.5f, new Action(this.tirggerMainScreen));
	}

	private void tirggerMainScreen()
	{
		SceneManager.LoadScene(0);
	}

	private void switchLights()
	{
		for (int i = 0; i < this.Lights.Count; i++)
		{
			if (this.lightsOn)
			{
				this.Lights[i].gameObject.SetActive(false);
			}
			else
			{
				this.Lights[i].gameObject.SetActive(true);
			}
		}
		if (this.lightsOn)
		{
			this.lightsOn = false;
			this.ComputerScreen.SetActive(false);
			this.LampCover.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", new Color(0f, 0f, 0f, 0f));
			GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
		}
		else
		{
			this.lightsOn = true;
			this.ComputerScreen.SetActive(true);
			this.LampCover.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", this.defaultLampEmi);
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.SOFTWARESFX);
		}
		this.refreshReflectorProbes();
	}

	private void triggerNymphoWalkingAni()
	{
		this.NymphoObject.GetComponent<Animator>().SetTrigger("triggerWalk");
	}

	private void triggerNymphoMirrorAni()
	{
		this.NymphoObject.GetComponent<Animator>().SetTrigger("triggerMirror");
	}

	private void Awake()
	{
		GameManager.SetScenceManager(this);
	}

	private void Start()
	{
		this.rbTimeStamp = Time.time;
		this.reflectRBS = true;
		this.lightsOn = true;
		this.defaultLightSwitchKnobPOS = this.lightSwitchKnob.transform.localPosition;
		this.defaultLightSwitchKnobROT = this.lightSwitchKnob.transform.localRotation.eulerAngles;
		this.defaultLampEmi = this.LampCover.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor");
	}

	private void Update()
	{
		if (this.reflectRBS && Time.time - this.rbTimeStamp >= this.RBWaitTime)
		{
			this.reflectRBS = false;
			this.refreshReflectorProbes();
		}
	}

	public UIManager myUIManager;

	public PauseManager myPauseManager;

	public GameObject lightSwitchKnob;

	public GameObject ComputerScreen;

	public GameObject LampCover;

	public GameObject NymphoObject;

	public GameObject WTTGPhone;

	public GameObject NymphoWristJoint;

	public GameObject mirrorRT;

	public GameObject mirrorCam;

	public GameObject teaserText;

	public Vector3 offSwitchPOS;

	public Vector3 offSwitchROT;

	public AudioClip lightSwitchOnSound;

	public AudioClip lightSwitchOffSound;

	[Range(0f, 3f)]
	public float RBWaitTime = 0.5f;

	public List<ReflectionProbe> ReflectorProbes;

	public List<Light> Lights;

	public AudioClip theConvo;

	public AudioClip rwsTune;

	private RaycastHit currentActionRay;

	private bool reflectRBS;

	private bool readyForAction;

	private bool lightsOn = true;

	private bool hasCurrentAction;

	private bool inDoorAction;

	private float rbTimeStamp;

	private Vector3 defaultLightSwitchKnobPOS;

	private Vector3 defaultLightSwitchKnobROT;

	private Sequence switchSeq;

	private Color defaultLampEmi;
}
