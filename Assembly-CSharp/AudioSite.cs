using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSite : MonoBehaviour
{
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

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		this.prepSounds();
	}

	private void OnDisable()
	{
		for (int i = 0; i < this.AudioBeats.Count; i++)
		{
			GameManager.AudioSlinger.RemoveSound(this.AudioHub, this.AudioBeats[i].audioFile.name);
		}
	}

	public AudioHubs AudioHub;

	public List<AudioBeatDefinition> AudioBeats;
}
