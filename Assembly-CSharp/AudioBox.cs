using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioBox : MonoBehaviour
{
	public void setMeActive()
	{
		this.warmTimeStamp = Time.time;
		this.warmMeUp = true;
		this.iAmActive = true;
	}

	private void prepSounds()
	{
		for (int i = 0; i < this.AudioBeats.Count; i++)
		{
			if (this.AudioBeats[i].delay == 0f)
			{
				GameManager.AudioSlinger.DealSound(this.AudioHub, this.AudioBeats[i].audioLayer, this.AudioBeats[i].audioFile, this.AudioBeats[i].audioVolume, this.AudioBeats[i].loop);
			}
			else
			{
				GameManager.TimeSlinger.FireIntTimer(this.AudioBeats[i].delay, new Action<int>(this.delayFire), i);
			}
		}
	}

	private void delayFire(int audioIndex)
	{
		GameManager.AudioSlinger.DealSound(this.AudioHub, this.AudioBeats[audioIndex].audioLayer, this.AudioBeats[audioIndex].audioFile, this.AudioBeats[audioIndex].audioVolume, this.AudioBeats[audioIndex].loop);
	}

	private void prepMagicFire()
	{
		this.magicTimeWindow = Random.Range(this.magicWindowMin, this.magicWindowMax);
		this.magicTimeStamp = Time.time;
		this.magicFireActive = true;
	}

	private void fireMagic()
	{
		int index = Random.Range(0, this.AudioBeats.Count);
		GameManager.AudioSlinger.DealSound(this.AudioHub, this.AudioBeats[index].audioLayer, this.AudioBeats[index].audioFile, this.AudioBeats[index].audioVolume, this.AudioBeats[index].loop);
		this.prepMagicFire();
	}

	private void Start()
	{
		if (this.iAmActive)
		{
			this.warmTimeStamp = Time.time;
			this.warmMeUp = true;
		}
	}

	private void Update()
	{
		if (this.iAmActive)
		{
			if (this.warmMeUp && Time.time - this.warmTimeStamp >= 0.5f)
			{
				this.warmMeUp = false;
				if (this.MagicFire)
				{
					this.prepMagicFire();
				}
				else
				{
					this.prepSounds();
				}
			}
			if (this.magicFireActive && Time.time - this.magicTimeStamp >= this.magicTimeWindow)
			{
				this.magicFireActive = false;
				this.fireMagic();
			}
		}
	}

	public bool iAmActive = true;

	public AudioHubs AudioHub;

	public List<AudioBeatDefinition> AudioBeats;

	public bool MagicFire;

	[Range(5f, 300f)]
	public float magicWindowMin = 30f;

	[Range(30f, 900f)]
	public float magicWindowMax = 120f;

	private bool warmMeUp;

	private bool magicFireActive;

	private float warmTimeStamp;

	private float magicTimeStamp;

	private float magicTimeWindow;
}
