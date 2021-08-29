using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class GlobalAudioController : MonoBehaviour
{
	public GlobalAudioController()
	{
		this.audioSources = new List<AudioSource>();
		this.asDPList = new List<GlobalAudioController.defaultSDP>();
	}

	public GlobalAudioController(AudioLayer AL)
	{
		this.audioLayer = AL;
		this.audioSources = new List<AudioSource>();
		this.asDPList = new List<GlobalAudioController.defaultSDP>();
	}

	public void AdjustMyPitchTo(float pitch)
	{
		for (int i = 0; i < this.audioSources.Count; i++)
		{
			this.audioSources[i].pitch = pitch;
		}
	}

	public void AdjustMyVolumeTo(float percentage)
	{
		if (percentage > 1f)
		{
			percentage = 1f;
		}
		else if (percentage < 0f)
		{
			percentage = 0f;
		}
		for (int i = 0; i < this.audioSources.Count; i++)
		{
			float num = this.asDPList[i].myVolume;
			if (this.myHub != null && this.myHub.amMuffled)
			{
				num *= this.myHub.muffledPerc;
			}
			if (this.audioSources[i] != null)
			{
				this.audioSources[i].volume = num * percentage;
			}
		}
	}

	public void AdjustMyVolumeTo(float percentage, float duration)
	{
		if (percentage > 1f)
		{
			percentage = 1f;
		}
		else if (percentage < 0f)
		{
			percentage = 0f;
		}
		AudioSource tmpAudioSource;
		for (int i = 0; i < this.audioSources.Count; i++)
		{
			tmpAudioSource = this.audioSources[i];
			float num = this.asDPList[i].myVolume;
			if (this.myHub != null && this.myHub.amMuffled)
			{
				num *= this.myHub.muffledPerc;
			}
			DOTween.To(() => tmpAudioSource.volume, delegate(float x)
			{
				tmpAudioSource.volume = x;
			}, num * percentage, duration).SetEase(Ease.Linear);
		}
	}

	public void ResetMyVolume()
	{
		for (int i = 0; i < this.audioSources.Count; i++)
		{
			float num = this.asDPList[i].myVolume;
			if (this.myHub != null && this.myHub.amMuffled)
			{
				num *= this.myHub.muffledPerc;
			}
			this.audioSources[i].volume = num;
		}
	}

	public void ResetMyVolume(float duration)
	{
		AudioSource tmpAudioSource;
		for (int i = 0; i < this.audioSources.Count; i++)
		{
			float num = this.asDPList[i].myVolume;
			tmpAudioSource = this.audioSources[i];
			if (this.myHub != null && this.myHub.amMuffled)
			{
				num *= this.myHub.muffledPerc;
			}
			DOTween.To(() => tmpAudioSource.volume, delegate(float x)
			{
				tmpAudioSource.volume = x;
			}, num, duration).SetEase(Ease.Linear);
		}
	}

	public void AddNewAudioSource(AudioSource newAudioSource, float setVol)
	{
		if (GameManager.AudioSlinger.IsLayerMuffeld(this.audioLayer))
		{
			newAudioSource.volume = setVol * GameManager.AudioSlinger.MuffeledLayerPerc(this.audioLayer);
		}
		this.audioSources.Add(newAudioSource);
		this.asDPList.Add(new GlobalAudioController.defaultSDP(setVol));
	}

	public void RemoveAudioSource(AudioSource asToRemove)
	{
		int num = this.audioSources.IndexOf(asToRemove);
		if (num != -1)
		{
			this.asDPList.RemoveAt(num);
			this.audioSources.RemoveAt(num);
		}
	}

	public void UpdateMe()
	{
		if (this.audioSources.Count > 0)
		{
			GameManager.AudioSlinger.AddGlobalAudioController(this.myKey, this);
		}
	}

	public string getMyKey()
	{
		return this.myKey;
	}

	public void KillMe()
	{
		UnityEngine.Object.Destroy(this);
	}

	private void Awake()
	{
		this.myKey = Guid.NewGuid().ToString();
	}

	public AudioLayer audioLayer;

	public List<AudioSource> audioSources;

	public AudioHubObject myHub;

	private List<GlobalAudioController.defaultSDP> asDPList;

	private string myKey;

	private struct defaultSDP
	{
		public defaultSDP(float myVol)
		{
			this.myVolume = myVol;
		}

		public float myVolume;
	}
}
