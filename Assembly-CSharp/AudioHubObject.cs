using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class AudioHubObject : MonoBehaviour
{
	public void playSound(AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop)
	{
		if (!this.currentAudioSources.ContainsKey(setClip.name))
		{
			AudioSource audioSource;
			GameManager.AudioSlinger.BuildAudioSourceForHub(this, out audioSource);
			audioSource.clip = setClip;
			if (this.amMuffled)
			{
				audioSource.volume = setAudioLevel * this.muffledPerc;
			}
			else
			{
				audioSource.volume = setAudioLevel;
			}
			audioSource.loop = setLoop;
			audioSource.Play();
			this.currentAudioSources.Add(setClip.name, audioSource);
			this.currentAudioVolumes.Add(setClip.name, setAudioLevel);
			this.currentSoundsToLayers.Add(setClip.name, setLayer);
			if (this.currentGACs.ContainsKey(setLayer))
			{
				this.currentGACs[setLayer].AddNewAudioSource(audioSource, setAudioLevel);
				this.currentGACs[setLayer].UpdateMe();
			}
			else
			{
				GlobalAudioController globalAudioController = base.gameObject.AddComponent<GlobalAudioController>();
				globalAudioController.myHub = this;
				globalAudioController.audioLayer = setLayer;
				globalAudioController.AddNewAudioSource(audioSource, setAudioLevel);
				globalAudioController.UpdateMe();
				this.currentGACs.Add(setLayer, globalAudioController);
			}
			if (!setLoop)
			{
				GameManager.TimeSlinger.FireStringTimer(setClip.length, new Action<string>(this.removeSound), setClip.name);
			}
		}
	}

	public void playSound(AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop, float delay)
	{
		string text = setClip.name + ":" + delay;
		if (!this.currentAudioSources.ContainsKey(setClip.name))
		{
			AudioSource audioSource;
			GameManager.AudioSlinger.BuildAudioSourceForHub(this, out audioSource);
			audioSource.clip = setClip;
			if (this.amMuffled)
			{
				audioSource.volume = setAudioLevel * this.muffledPerc;
			}
			else
			{
				audioSource.volume = setAudioLevel;
			}
			audioSource.loop = setLoop;
			audioSource.PlayDelayed(delay);
			this.currentAudioSources.Add(text, audioSource);
			this.currentAudioVolumes.Add(text, setAudioLevel);
			this.currentSoundsToLayers.Add(text, setLayer);
			if (this.currentGACs.ContainsKey(setLayer))
			{
				this.currentGACs[setLayer].AddNewAudioSource(audioSource, setAudioLevel);
				this.currentGACs[setLayer].UpdateMe();
			}
			else
			{
				GlobalAudioController globalAudioController = base.gameObject.AddComponent<GlobalAudioController>();
				globalAudioController.myHub = this;
				globalAudioController.audioLayer = setLayer;
				globalAudioController.AddNewAudioSource(audioSource, setAudioLevel);
				globalAudioController.UpdateMe();
				this.currentGACs.Add(setLayer, globalAudioController);
			}
			if (!setLoop)
			{
				GameManager.TimeSlinger.FireStringTimer(setClip.length + delay, new Action<string>(this.removeSound), text);
			}
		}
	}

	public void playSound(AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop, bool delayLoop, float delay)
	{
		if (!delayLoop)
		{
			this.playSound(setLayer, setClip, setAudioLevel, setLoop, delay);
		}
		else
		{
			string text = setClip.name + ":" + delay;
			if (!this.currentAudioSources.ContainsKey(setClip.name))
			{
				AudioSource audioSource;
				GameManager.AudioSlinger.BuildAudioSourceForHub(this, out audioSource);
				audioSource.clip = setClip;
				if (this.amMuffled)
				{
					audioSource.volume = setAudioLevel * this.muffledPerc;
				}
				else
				{
					audioSource.volume = setAudioLevel;
				}
				audioSource.loop = false;
				audioSource.Play();
				this.currentAudioSources.Add(text, audioSource);
				this.currentAudioVolumes.Add(text, setAudioLevel);
				this.currentSoundsToLayers.Add(text, setLayer);
				if (this.currentGACs.ContainsKey(setLayer))
				{
					this.currentGACs[setLayer].AddNewAudioSource(audioSource, setAudioLevel);
					this.currentGACs[setLayer].UpdateMe();
				}
				else
				{
					GlobalAudioController globalAudioController = base.gameObject.AddComponent<GlobalAudioController>();
					globalAudioController.myHub = this;
					globalAudioController.audioLayer = setLayer;
					globalAudioController.AddNewAudioSource(audioSource, setAudioLevel);
					globalAudioController.UpdateMe();
					this.currentGACs.Add(setLayer, globalAudioController);
				}
				GameManager.TimeSlinger.FireStrFloatTimer(setClip.length + delay, new Action<string, float>(this.playDelayedLoopSound), text, setClip.length + delay);
			}
		}
	}

	public void fireSound(AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop)
	{
		AudioSource audioSource;
		GameManager.AudioSlinger.BuildAudioSourceForHub(this, out audioSource);
		audioSource.clip = setClip;
		if (this.amMuffled)
		{
			audioSource.volume = setAudioLevel * this.muffledPerc;
		}
		else
		{
			audioSource.volume = setAudioLevel;
		}
		audioSource.loop = setLoop;
		audioSource.Play();
		if (!setLoop)
		{
			GameManager.TimeSlinger.FireStringTimer(setClip.length, new Action<string>(this.removeSound), setClip.name);
		}
	}

	public void fireSound(AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop, float delay)
	{
		string callValue = setClip.name + ":" + delay;
		AudioSource audioSource;
		GameManager.AudioSlinger.BuildAudioSourceForHub(this, out audioSource);
		audioSource.clip = setClip;
		if (this.amMuffled)
		{
			audioSource.volume = setAudioLevel * this.muffledPerc;
		}
		else
		{
			audioSource.volume = setAudioLevel;
		}
		audioSource.loop = setLoop;
		audioSource.PlayDelayed(delay);
		if (!setLoop)
		{
			GameManager.TimeSlinger.FireStringTimer(setClip.length + delay, new Action<string>(this.removeSound), callValue);
		}
	}

	public void removeSound(string soundName = "")
	{
		if (this.currentAudioSources.ContainsKey(soundName))
		{
			AudioSource audioSource = this.currentAudioSources[soundName];
			AudioLayer key = this.currentSoundsToLayers[soundName];
			this.currentGACs[key].RemoveAudioSource(audioSource);
			this.currentAudioSources.Remove(soundName);
			this.currentAudioVolumes.Remove(soundName);
			this.currentSoundsToLayers.Remove(soundName);
			if (audioSource != null && audioSource.isPlaying)
			{
				audioSource.Stop();
			}
			UnityEngine.Object.Destroy(audioSource);
		}
	}

	public void playDelayedLoopSound(string soundName, float delayedValue)
	{
		if (this.currentAudioSources.ContainsKey(soundName))
		{
			AudioSource audioSource = this.currentAudioSources[soundName];
			audioSource.Play();
			GameManager.TimeSlinger.FireStrFloatTimer(delayedValue, new Action<string, float>(this.playDelayedLoopSound), soundName, delayedValue);
		}
	}

	public void adjustSoundVolumes(float setPercentage)
	{
		List<string> list = new List<string>(this.currentAudioSources.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			AudioLayer audioLayer = this.currentSoundsToLayers[list[i]];
			if (GameManager.AudioSlinger.IsLayerMuffeld(audioLayer))
			{
				this.currentAudioSources[list[i]].volume = this.currentAudioVolumes[list[i]] * GameManager.AudioSlinger.MuffeledLayerPerc(audioLayer);
			}
			else
			{
				this.currentAudioSources[list[i]].volume = this.currentAudioVolumes[list[i]] * setPercentage;
			}
		}
	}

	public void adjustSoundVolumes(float setPercentage, float setDuration)
	{
		List<string> list = new List<string>(this.currentAudioSources.Keys);
		this.volumeSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.clearVolumeSeq));
		for (int i = 0; i < list.Count; i++)
		{
			AudioLayer audioLayer = this.currentSoundsToLayers[list[i]];
			AudioSource tmpAS = this.currentAudioSources[list[i]];
			float num = this.currentAudioVolumes[list[i]] * setPercentage;
			if (GameManager.AudioSlinger.IsLayerMuffeld(audioLayer))
			{
				num *= GameManager.AudioSlinger.MuffeledLayerPerc(audioLayer);
			}
			this.volumeSeq.Insert(0f, DOTween.To(() => tmpAS.volume, delegate(float x)
			{
				tmpAS.volume = x;
			}, num, setDuration).SetEase(Ease.Linear));
		}
		this.volumeSeq.Play<Sequence>();
	}

	public void resetSoundVolumes()
	{
		List<string> list = new List<string>(this.currentAudioSources.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			AudioLayer audioLayer = this.currentSoundsToLayers[list[i]];
			if (GameManager.AudioSlinger.IsLayerMuffeld(audioLayer))
			{
				this.currentAudioSources[list[i]].volume = this.currentAudioVolumes[list[i]] * GameManager.AudioSlinger.MuffeledLayerPerc(audioLayer);
			}
			else
			{
				this.currentAudioSources[list[i]].volume = this.currentAudioVolumes[list[i]];
			}
		}
	}

	public void resetSoundVolumes(float setDuration)
	{
		List<string> list = new List<string>(this.currentAudioSources.Keys);
		this.volumeSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.clearVolumeSeq));
		for (int i = 0; i < list.Count; i++)
		{
			AudioLayer audioLayer = this.currentSoundsToLayers[list[i]];
			AudioSource tmpAS = this.currentAudioSources[list[i]];
			float num = this.currentAudioVolumes[list[i]];
			if (GameManager.AudioSlinger.IsLayerMuffeld(audioLayer))
			{
				num *= GameManager.AudioSlinger.MuffeledLayerPerc(audioLayer);
			}
			this.volumeSeq.Insert(0f, DOTween.To(() => tmpAS.volume, delegate(float x)
			{
				tmpAS.volume = x;
			}, num, setDuration).SetEase(Ease.Linear));
		}
		this.volumeSeq.Play<Sequence>();
	}

	public void updateClipVolume(string clipName, float newVolLevel)
	{
		if (this.currentAudioVolumes.ContainsKey(clipName))
		{
			this.currentAudioVolumes[clipName] = newVolLevel;
		}
	}

	private void clearVolumeSeq()
	{
		this.volumeSeq.Kill(true);
		this.volumeSeq = null;
	}

	private void Start()
	{
		this.currentAudioSources = new Dictionary<string, AudioSource>();
		this.currentAudioVolumes = new Dictionary<string, float>();
		this.currentGACs = new Dictionary<AudioLayer, GlobalAudioController>();
		this.currentSoundsToLayers = new Dictionary<string, AudioLayer>();
		GameManager.AudioSlinger.AddAudioHub(this);
	}

	private void OnDestory()
	{
		foreach (KeyValuePair<AudioLayer, GlobalAudioController> keyValuePair in this.currentGACs)
		{
			GameManager.AudioSlinger.RemoveGlobalAudioController(keyValuePair.Key, keyValuePair.Value.getMyKey());
		}
	}

	public AudioHubs myAudioHub;

	public bool amMuffled;

	public float muffledPerc;

	private Dictionary<string, AudioSource> currentAudioSources;

	private Dictionary<string, float> currentAudioVolumes;

	private Dictionary<AudioLayer, GlobalAudioController> currentGACs;

	private Dictionary<string, AudioLayer> currentSoundsToLayers;

	private Sequence volumeSeq;
}
