using System;
using UnityEngine;

public class curtianScrub : MonoBehaviour
{
	public void openTheWindows(bool updateMuffleV = false)
	{
		this.updateMuffle = updateMuffleV;
		GameManager.TimeSlinger.FireTimer(1f, new Action(this.updateAudioSettings));
		this.leftWindow.transform.localPosition = this.leftWindowOpenPos;
		this.leftWindow.transform.localRotation = Quaternion.Euler(this.leftWindowOpenRot);
		this.rightWindow.transform.localPosition = this.rightWindowOpenPos;
		this.rightWindow.transform.localRotation = Quaternion.Euler(this.rightWindowOpenRot);
		this.setWindTime();
	}

	private void setWindTime()
	{
		this.windTime = Random.Range(this.windMinTime, this.windMaxTime);
		this.windTimeStamp = Time.time;
		this.windTimeActive = true;
		this.coolTimeActive = false;
	}

	private void updateAudioSettings()
	{
		GameManager.AudioSlinger.UpdateAudioHubSoundVolume(AudioHubs.OUTSIDE, "SummerNight", 0.9f);
		if (this.updateMuffle)
		{
			GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.OUTSIDE, 0.5f);
			this.updateMuffle = false;
		}
	}

	private void Start()
	{
		this.setWindTime();
	}

	private void Update()
	{
		if (this.windTimeActive)
		{
			if (Time.time - this.windTimeStamp >= this.windTime)
			{
				float num = Random.Range(this.minWindForce, this.maxWindForce);
				float num2 = num * 0.85f;
				if (num > 3f)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.OUTSIDE, AudioLayer.BACKGROUND, this.windSound, 0.1f, false);
				}
				this.leftCurtian.externalAcceleration = new Vector3(0f, 0f, -num);
				this.rightCurtian.externalAcceleration = new Vector3(0f, 0f, -num2);
				this.windTimeActive = false;
				this.coolTimeActive = true;
				this.coolTimeStamp = Time.time;
			}
		}
		else if (this.coolTimeActive && Time.time - this.coolTimeStamp >= this.coolOffTime)
		{
			this.leftCurtian.externalAcceleration = new Vector3(0f, 0f, 0f);
			this.rightCurtian.externalAcceleration = new Vector3(0f, 0f, 0f);
			this.setWindTime();
		}
	}

	public GameObject leftWindow;

	public GameObject rightWindow;

	public Cloth leftCurtian;

	public Cloth rightCurtian;

	public AudioClip windSound;

	public GameObject outSideHub1;

	public Vector3 leftWindowOpenPos;

	public Vector3 leftWindowOpenRot;

	public Vector3 rightWindowOpenPos;

	public Vector3 rightWindowOpenRot;

	[Range(3f, 10f)]
	public float maxWindForce = 6f;

	[Range(0.25f, 5f)]
	public float minWindForce = 1f;

	[Range(60f, 300f)]
	public float windMaxTime = 10f;

	[Range(1f, 60f)]
	public float windMinTime = 5f;

	[Range(0.5f, 15f)]
	public float coolOffTime = 5f;

	private bool windTimeActive;

	private bool coolTimeActive;

	private bool updateMuffle;

	private float windTime;

	private float windTimeStamp;

	private float coolTimeStamp;
}
