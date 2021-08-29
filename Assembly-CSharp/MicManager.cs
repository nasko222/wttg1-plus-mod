using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MicManager : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MicManager.MicDelegate playerHasNoMic;

	public void triggerPlayerListen()
	{
		if (this.foundMic)
		{
			this.startRecPlayer();
		}
		else if (this.playerHasNoMic != null)
		{
			this.playerHasNoMic();
		}
	}

	public void stopPlayerListen()
	{
		this.stopRecPlayer();
	}

	private void prepMics()
	{
		this.micDevs = new List<string>();
		foreach (string item in Microphone.devices)
		{
			this.micDevs.Add(item);
		}
		if (this.micDevs.Count > 0)
		{
			this.micIndex = 0;
			this.warmMic();
		}
	}

	private void warmMic()
	{
		if (this.micIndex >= this.micDevs.Count)
		{
			this.foundMic = false;
		}
		else
		{
			this.micDevToTest = this.micDevs[this.micIndex];
			this.micTimeStamp = Time.time;
			this.startRec();
			this.checkForMic = true;
			this.micIndex++;
		}
	}

	private void startRecPlayer()
	{
		this.listenToPlayer = true;
		int num = 0;
		int num2 = 0;
		Microphone.GetDeviceCaps(this.micDevToTest, ref num, ref num2);
		this.tmpAC = Microphone.Start(this.defaultMic, true, 300, num2);
		while (Microphone.GetPosition(this.defaultMic) <= 0)
		{
		}
		this.myAS.PlayOneShot(this.tmpAC);
	}

	private void stopRecPlayer()
	{
		if (this.foundMic)
		{
			Microphone.End(this.defaultMic);
			this.myAS.Stop();
		}
	}

	private void startRec()
	{
		int num = 0;
		int num2 = 0;
		Microphone.GetDeviceCaps(this.micDevToTest, ref num, ref num2);
		this.tmpAC = Microphone.Start(this.micDevToTest, true, 300, num2);
		while (Microphone.GetPosition(this.micDevToTest) <= 0)
		{
		}
		this.myAS.PlayOneShot(this.tmpAC);
	}

	private void stopRec()
	{
		Microphone.End(this.micDevToTest);
		this.myAS.Stop();
	}

	private float AnalyzeSound()
	{
		float[] array = new float[1024];
		float[] array2 = new float[1024];
		float[] array3 = new float[1024];
		this.myAS.GetOutputData(array, 0);
		this.myAS.GetOutputData(array2, 1);
		array3 = this.CombineChannels(array, array2);
		float num = 0f;
		for (int i = 0; i < array3.Length; i++)
		{
			num += array3[i] * array3[i];
		}
		this.RMS = Mathf.Sqrt(num / this.SampleSize);
		this.Decibels = 20f * Mathf.Log10(this.RMS / this.DecibelRef);
		if (this.Decibels < -160f)
		{
			this.Decibels = -160f;
		}
		return this.Decibels;
	}

	private float[] CombineChannels(float[] Left, float[] Right)
	{
		float[] array = new float[Left.Length];
		for (int i = 0; i < Left.Length; i++)
		{
			array[i] = (Left[i] + Right[i]) / 2f / 32768f;
		}
		return array;
	}

	private void Start()
	{
		this.foundMic = false;
		if (GameManager.FileSlinger.optData.useTheMic)
		{
			this.myAS = base.GetComponent<AudioSource>();
			this.prepMics();
		}
	}

	private void Update()
	{
		if (this.checkForMic)
		{
			if (Time.time - this.micTimeStamp >= this.micWarmCheckTime)
			{
				this.stopRec();
				this.checkForMic = false;
				this.warmMic();
			}
			else if (this.AnalyzeSound() > -155f)
			{
				this.stopRec();
				this.checkForMic = false;
				this.foundMic = true;
				this.defaultMic = this.micDevToTest;
			}
		}
		if (this.listenToPlayer && this.AnalyzeSound() > -this.triggerTalkedDBLevel)
		{
			this.listenToPlayer = false;
			this.stopRecPlayer();
			this.playerDidTalk();
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MicManager.MicDelegate playerDidTalk;

	[Range(0.5f, 5.5f)]
	public float micWarmCheckTime = 3f;

	[Range(0f, 160f)]
	public float triggerTalkedDBLevel = 145f;

	private List<string> micDevs;

	private AudioSource myAS;

	private AudioClip tmpAC;

	private int micIndex;

	private bool foundMic;

	private bool checkForMic;

	private bool listenToPlayer;

	private float micTimeStamp;

	private string micDevToTest;

	private string defaultMic;

	public float DecibelRef = 0.1f;

	private float RMS;

	private float Decibels;

	private float SampleSize = 1024f;

	private float[] Samples;

	private float[] Spectrum;

	public delegate void MicDelegate();
}
