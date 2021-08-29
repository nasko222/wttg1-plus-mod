using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering;

public class extCarBehavior : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event extCarBehavior.CarDelegate TriggerRespawn;

	private void aniWheels()
	{
		if (this.hasWheels)
		{
			this.frontLeftWheel.transform.Rotate(Vector3.right * this.WHEEL_SPEED * Time.deltaTime);
			this.frontRightWheel.transform.Rotate(Vector3.right * this.WHEEL_SPEED * Time.deltaTime);
			this.rearLeftWheel.transform.Rotate(Vector3.right * this.WHEEL_SPEED * Time.deltaTime);
			this.rearRightWheel.transform.Rotate(Vector3.right * this.WHEEL_SPEED * Time.deltaTime);
		}
	}

	private void moveCarForward()
	{
		if ((double)base.transform.position.y < 0.01)
		{
			base.transform.Translate(Vector3.up * extCarBehavior.SPAWN_SPEED * Time.deltaTime);
		}
		if (!this.comingFromPullOver)
		{
			base.transform.Translate(Vector3.forward * this.CAR_SPEED * Time.deltaTime);
		}
	}

	private void setStopBehavior()
	{
		this.comingFromPullOver = true;
		this.setZValue = base.transform.position.z;
		this.pullOverAudioSource.Play();
		if (this.myDriveAwaySeq != null && this.myDriveAwaySeq != null)
		{
			this.myDriveAwaySeq.Complete();
		}
		this.turnRightBlinkerOn();
		this.myDriveAwaySeq = DOTween.Sequence().OnComplete(new TweenCallback(this.presentPulledOver));
		this.myDriveAwaySeq.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(this.hitchStopLocation.x * 0.99f, base.transform.position.y, this.hitchStopLocation.z), 7.826f).SetEase(Ease.OutQuad));
		this.myDriveAwaySeq.Insert(0f, DOTween.To(() => this.driveByAudioSource.volume, delegate(float x)
		{
			this.driveByAudioSource.volume = x;
		}, 0f, 6f).SetEase(Ease.Linear));
		this.myDriveAwaySeq.Insert(0f, DOTween.To(() => this.pullOverAudioSource.volume, delegate(float x)
		{
			this.pullOverAudioSource.volume = x;
		}, 0.85f, 2f).SetEase(Ease.Linear));
		this.myDriveAwaySeq.Insert(5f, DOTween.To(() => this.WHEEL_SPEED, delegate(float x)
		{
			this.WHEEL_SPEED = x;
		}, 0f, 2.826f).SetEase(Ease.Linear));
		this.myDriveAwaySeq.Play<Sequence>();
	}

	private void destroyMe()
	{
		if (this.TriggerRespawn != null)
		{
			this.TriggerRespawn();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void dismissPullOverAniDone()
	{
		this.WHEEL_SPEED *= 0.8f;
		this.pullAwayAudioSource.Play();
		this.turnRightBlinkerOff();
		this.turnLeftBlinkerOn();
		if (this.myDriveAwaySeq != null && this.myDriveAwaySeq != null)
		{
			this.myDriveAwaySeq.Complete();
		}
		this.myDriveAwaySeq = DOTween.Sequence().OnComplete(new TweenCallback(this.theCarIsOff));
		this.myDriveAwaySeq.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(base.transform.position.x * 1.2f, base.transform.position.y, this.setZValue), 4.5f).SetEase(Ease.InSine));
		this.myDriveAwaySeq.Play<Sequence>();
		Tweener t = DOTween.To(() => this.driveByAudioSource.volume, delegate(float x)
		{
			this.driveByAudioSource.volume = x;
		}, 1f, 7f).SetEase(Ease.Linear);
		t.Play<Tweener>();
		this.comingFromPullOver = true;
		this.carIsMoving = true;
	}

	private void theCarIsOff()
	{
		this.turnLeftBlinkerOff();
		this.comingFromPullOver = false;
	}

	private void prepSounds()
	{
		this.driveByAudioSource = base.gameObject.AddComponent<AudioSource>();
		this.driveByAudioSource.clip = this.driveBySound;
		this.driveByAudioSource.spatialBlend = 1f;
		this.driveByAudioSource.loop = true;
		this.driveByAudioSource.maxDistance = 55f;
		this.pullOverAudioSource = base.gameObject.AddComponent<AudioSource>();
		this.pullOverAudioSource.clip = this.pullOverSound;
		this.pullOverAudioSource.loop = false;
		this.pullOverAudioSource.spatialBlend = 1f;
		this.pullOverAudioSource.volume = 0f;
		this.engineIdleAudioSource = base.gameObject.AddComponent<AudioSource>();
		this.engineIdleAudioSource.clip = this.engineIdleSound;
		this.engineIdleAudioSource.loop = true;
		this.engineIdleAudioSource.spatialBlend = 1f;
		this.engineIdleAudioSource.volume = 0.15f;
		this.engineIdleAudioSource.maxDistance = 1f;
		this.windowDownAudioSource = base.gameObject.AddComponent<AudioSource>();
		this.windowDownAudioSource.clip = this.windowDownSound;
		this.windowDownAudioSource.loop = false;
		this.windowDownAudioSource.spatialBlend = 1f;
		this.windowDownAudioSource.maxDistance = 4f;
		this.windowUpAudioSource = base.gameObject.AddComponent<AudioSource>();
		this.windowUpAudioSource.clip = this.windowUpSound;
		this.windowUpAudioSource.loop = false;
		this.windowUpAudioSource.spatialBlend = 1f;
		this.windowUpAudioSource.maxDistance = 4f;
		this.pullAwayAudioSource = base.gameObject.AddComponent<AudioSource>();
		this.pullAwayAudioSource.clip = this.pullAwaySound;
		this.pullAwayAudioSource.loop = false;
		this.pullAwayAudioSource.spatialBlend = 1f;
		this.driveByAudioSource.Play();
	}

	private void prepCornerLights()
	{
		Material[] materials = this.carExterior.GetComponent<Renderer>().materials;
		for (int i = 0; i < materials.Length; i++)
		{
			string name = materials[i].name;
			if (name != null)
			{
				if (!(name == "Front_Left_Corner_Light (Instance)"))
				{
					if (!(name == "Front_Right_Corner_Light (Instance)"))
					{
						if (!(name == "Rear_Right_Corner_Light (Instance)"))
						{
							if (name == "Rear_Left_Corner_Light (Instance)")
							{
								this.rearLeftCornerLight = materials[i];
								this.rearLeftCLColor = materials[i].GetColor("_EmissionColor");
							}
						}
						else
						{
							this.rearRightCornerLight = materials[i];
							this.rearRightCLColor = materials[i].GetColor("_EmissionColor");
						}
					}
					else
					{
						this.frontRightCornerLight = materials[i];
						this.frontRightCLColor = materials[i].GetColor("_EmissionColor");
					}
				}
				else
				{
					this.frontLeftCornerLight = materials[i];
					this.frontLeftCLColor = materials[i].GetColor("_EmissionColor");
				}
			}
		}
	}

	private void turnRightBlinkerOn()
	{
		this.leftBlinkerOn = false;
		this.rightBlinkerOn = true;
		this.RightBlinkerTurnOn();
	}

	private void turnLeftBlinkerOn()
	{
		this.leftBlinkerOn = true;
		this.rightBlinkerOn = false;
		this.LeftBlinkerTurnOn();
	}

	private void turnRightBlinkerOff()
	{
		this.rightBlinkerOn = false;
	}

	private void turnLeftBlinkerOff()
	{
		this.leftBlinkerOn = false;
	}

	private void RightBlinkerTurnOn()
	{
		this.frontRightCornerLight.SetColor("_EmissionColor", this.frontRightCLColor * 0.25f);
		this.rearRightCornerLight.SetColor("_EmissionColor", this.rearRightCLColor * 0.25f);
		GameManager.TimeSlinger.FireTimer(0.45f, new Action(this.RightBlinkerTurnOff));
	}

	private void RightBlinkerTurnOff()
	{
		this.frontRightCornerLight.SetColor("_EmissionColor", this.frontRightCLColor);
		this.rearRightCornerLight.SetColor("_EmissionColor", this.rearRightCLColor);
		if (this.rightBlinkerOn)
		{
			GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.RightBlinkerTurnOn));
		}
	}

	private void LeftBlinkerTurnOn()
	{
		this.frontLeftCornerLight.SetColor("_EmissionColor", this.frontLeftCLColor * 0.25f);
		this.rearLeftCornerLight.SetColor("_EmissionColor", this.rearLeftCLColor * 0.25f);
		GameManager.TimeSlinger.FireTimer(0.45f, new Action(this.LeftBlinkerTurnOff));
	}

	private void LeftBlinkerTurnOff()
	{
		this.frontLeftCornerLight.SetColor("_EmissionColor", this.frontLeftCLColor);
		this.rearLeftCornerLight.SetColor("_EmissionColor", this.rearLeftCLColor);
		if (this.leftBlinkerOn)
		{
			GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.LeftBlinkerTurnOn));
		}
	}

	public void triggerDriveBy()
	{
		this.hasWheels = false;
		this.carIsMoving = true;
		if (this.frontLeftWheel != null && this.frontRightWheel != null && this.rearLeftWheel != null && this.rearRightWheel != null)
		{
			this.hasWheels = true;
		}
		this.defaultPassWindowPos = this.passWindow.transform.localPosition;
		this.defaultPassWindowRot = this.passWindow.transform.localRotation;
		this.prepSounds();
		this.prepCornerLights();
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.setStopBehavior));
	}

	public void presentPulledOver()
	{
		this.carIsMoving = false;
		this.engineIdleAudioSource.Play();
		this.windowDownAudioSource.Play();
		if (this.myPullOverSeq != null && this.myPullOverSeq != null)
		{
			this.myPullOverSeq.Complete();
		}
		this.myPullOverSeq = DOTween.Sequence();
		this.myPullOverSeq.Insert(0f, DOTween.To(() => this.domeLight.intensity, delegate(float x)
		{
			this.domeLight.intensity = x;
		}, 1.5f, 0.55f).SetEase(Ease.Linear));
		this.myPullOverSeq.Insert(0f, DOTween.To(() => this.passWindow.transform.localPosition, delegate(Vector3 x)
		{
			this.passWindow.transform.localPosition = x;
		}, new Vector3(this.passWindow.transform.localPosition.x, 0.252f, this.passWindow.transform.localPosition.z), 3.9f).SetEase(Ease.Linear));
		this.myPullOverSeq.Insert(0f, DOTween.To(() => this.passWindow.transform.localRotation, delegate(Quaternion x)
		{
			this.passWindow.transform.localRotation = x;
		}, new Vector3(0f, 0f, -9.8f), 3.9f).SetEase(Ease.Linear));
		this.myPullOverSeq.Play<Sequence>();
	}

	public void dismissPulledOver()
	{
		this.windowUpAudioSource.Play();
		if (this.myPullOverSeq != null && this.myPullOverSeq != null)
		{
			this.myPullOverSeq.Complete();
		}
		this.myPullOverSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.dismissPullOverAniDone));
		this.myPullOverSeq.Insert(0f, DOTween.To(() => this.passWindow.transform.localPosition, delegate(Vector3 x)
		{
			this.passWindow.transform.localPosition = x;
		}, this.defaultPassWindowPos, 2.8f).SetEase(Ease.Linear));
		this.myPullOverSeq.Insert(0f, DOTween.To(() => this.passWindow.transform.localRotation, delegate(Quaternion x)
		{
			this.passWindow.transform.localRotation = x;
		}, new Vector3(this.defaultPassWindowRot.eulerAngles.x, this.defaultPassWindowRot.eulerAngles.y, this.defaultPassWindowRot.eulerAngles.z), 2.8f).SetEase(Ease.Linear));
		this.myPullOverSeq.Insert(2.8f, DOTween.To(() => this.domeLight.intensity, delegate(float x)
		{
			this.domeLight.intensity = x;
		}, 0f, 0.55f).SetEase(Ease.Linear));
		this.myPullOverSeq.Play<Sequence>();
	}

	public void refreshReflectProbe()
	{
		if (this.reflectProbe != null && this.reflectProbe != null)
		{
			this.reflectProbe.refreshMode = ReflectionProbeRefreshMode.EveryFrame;
			this.reflectProbe.timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
		}
	}

	public void openPassDoor()
	{
		if (this.myPullOverSeq != null && this.myOpenDoorSeq != null)
		{
			this.myOpenDoorSeq.Complete();
		}
		this.myOpenDoorSeq = DOTween.Sequence();
		this.myOpenDoorSeq.Insert(0f, DOTween.To(() => this.passDoor.transform.localRotation, delegate(Quaternion x)
		{
			this.passDoor.transform.localRotation = x;
		}, new Vector3(this.passDoor.transform.localRotation.eulerAngles.x, -15f, this.passDoor.transform.localRotation.eulerAngles.z), 1.3f).SetEase(Ease.OutCubic));
		this.myOpenDoorSeq.Play<Sequence>();
	}

	private void Update()
	{
		if (this.carIsMoving)
		{
			this.aniWheels();
			this.moveCarForward();
		}
	}

	public GameObject carExterior;

	public GameObject frontLeftWheel;

	public GameObject frontRightWheel;

	public GameObject rearLeftWheel;

	public GameObject rearRightWheel;

	public GameObject passWindow;

	public GameObject passDoor;

	public Light domeLight;

	public ReflectionProbe reflectProbe;

	public float WHEEL_SPEED = 235f;

	public float CAR_SPEED = 11f;

	public Vector3 hitchStopLocation;

	public float hitchStopLocMinX;

	public AudioClip driveBySound;

	public AudioClip pullOverSound;

	public AudioClip pullAwaySound;

	public AudioClip engineIdleSound;

	public AudioClip windowDownSound;

	public AudioClip windowUpSound;

	private static float SPAWN_SPEED = 1f;

	private bool hasWheels;

	private bool carIsMoving;

	private bool comingFromPullOver;

	private bool rightBlinkerOn;

	private bool leftBlinkerOn;

	private float setZValue;

	private Sequence myPullOverSeq;

	private Sequence myDriveAwaySeq;

	private Sequence myOpenDoorSeq;

	private Vector3 defaultPassWindowPos;

	private Quaternion defaultPassWindowRot;

	private Material frontLeftCornerLight;

	private Material frontRightCornerLight;

	private Material rearLeftCornerLight;

	private Material rearRightCornerLight;

	private Color frontLeftCLColor;

	private Color frontRightCLColor;

	private Color rearLeftCLColor;

	private Color rearRightCLColor;

	private AudioSource driveByAudioSource;

	private AudioSource pullOverAudioSource;

	private AudioSource pullAwayAudioSource;

	private AudioSource engineIdleAudioSource;

	private AudioSource windowDownAudioSource;

	private AudioSource windowUpAudioSource;

	public delegate void CarDelegate();
}
