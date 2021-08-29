using System;
using System.Collections.Generic;
using UnityEngine;

public class audioSlinger
{
	public audioSlinger()
	{
		this.currentGlobalAudioControllers = new Dictionary<AudioLayer, List<audioSlinger.GACHolder>>();
		this.currentAudioHubs = new Dictionary<AudioHubs, AudioHubObject>();
		this.currentGACHolders = new Dictionary<string, int>();
		this.currentMuffeldLayers = new Dictionary<AudioLayer, float>();
	}

	public void ClearAudioHubs()
	{
		this.currentGlobalAudioControllers.Clear();
		this.currentAudioHubs.Clear();
		this.currentGACHolders.Clear();
		this.currentMuffeldLayers.Clear();
	}

	public void AddGlobalAudioController(string key, GlobalAudioController theGAC)
	{
		if (!this.currentGlobalAudioControllers.ContainsKey(theGAC.audioLayer))
		{
			this.currentGlobalAudioControllers.Add(theGAC.audioLayer, new List<audioSlinger.GACHolder>());
		}
		if (!this.currentGACHolders.ContainsKey(key))
		{
			this.currentGlobalAudioControllers[theGAC.audioLayer].Add(new audioSlinger.GACHolder(key, theGAC));
			this.currentGACHolders.Add(key, this.currentGlobalAudioControllers[theGAC.audioLayer].Count - 1);
		}
	}

	public void RemoveGlobalAudioController(AudioLayer setAL, string setKey)
	{
		if (this.currentGACHolders.ContainsKey(setKey) && this.currentGACHolders[setKey] < this.currentGlobalAudioControllers[setAL].Count)
		{
			this.currentGlobalAudioControllers[setAL][this.currentGACHolders[setKey]].myGAC.KillMe();
			this.currentGlobalAudioControllers[setAL].RemoveAt(this.currentGACHolders[setKey]);
			this.currentGACHolders.Remove(setKey);
		}
	}

	public void MasterMuteAll()
	{
		this.MuffleGlobalVolume(AudioLayer.BACKGROUND, 0f);
		this.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
		this.MuffleGlobalVolume(AudioLayer.MOMENT, 0f);
		this.MuffleGlobalVolume(AudioLayer.VOICE, 0f);
		this.MuffleGlobalVolume(AudioLayer.CONTROLLED, 0f);
		this.MuffleGlobalVolume(AudioLayer.HACKINGSFX, 0f);
		this.MuffleGlobalVolume(AudioLayer.COMPUTERSFX, 0f);
		this.MuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
		this.MuffleGlobalVolume(AudioLayer.MENU, 0f);
		this.MuffleGlobalVolume(AudioLayer.OUTSIDE, 0f);
		this.MuffleGlobalVolume(AudioLayer.PHONE, 0f);
	}

	public void MasterUnMuteAll()
	{
		this.UnMuffleGlobalVolume(AudioLayer.BACKGROUND, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.MUSIC, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.MOMENT, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.VOICE, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.CONTROLLED, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.HACKINGSFX, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.COMPUTERSFX, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.MENU, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.OUTSIDE, 0f);
		this.UnMuffleGlobalVolume(AudioLayer.PHONE, 0f);
	}

	public void MuffleGlobalVolume(AudioLayer audioLayer, float percentage)
	{
		if (!this.currentMuffeldLayers.ContainsKey(audioLayer))
		{
			this.currentMuffeldLayers.Add(audioLayer, percentage);
			this.AdjustGlobalVolume(audioLayer, percentage);
		}
	}

	public void MuffleGlobalVolume(AudioLayer audioLayer, float percentage, float duration)
	{
		if (!this.currentMuffeldLayers.ContainsKey(audioLayer))
		{
			this.currentMuffeldLayers.Add(audioLayer, percentage);
			this.AdjustGlobalVolume(audioLayer, percentage, duration);
		}
	}

	public void UnMuffleGlobalVolume(AudioLayer audioLayer)
	{
		if (this.currentMuffeldLayers.ContainsKey(audioLayer))
		{
			this.ResetGlobalVolume(audioLayer);
			this.currentMuffeldLayers.Remove(audioLayer);
		}
	}

	public void UnMuffleGlobalVolume(AudioLayer audioLayer, float duration)
	{
		if (this.currentMuffeldLayers.ContainsKey(audioLayer))
		{
			this.ResetGlobalVolume(audioLayer, duration);
			this.currentMuffeldLayers.Remove(audioLayer);
		}
	}

	public bool IsLayerMuffeld(AudioLayer audioLayer)
	{
		return this.currentMuffeldLayers.ContainsKey(audioLayer);
	}

	public float MuffeledLayerPerc(AudioLayer audioLayer)
	{
		return this.currentMuffeldLayers[audioLayer];
	}

	public void ChangeGlobalPitch(AudioLayer audioLayer, float toPitch)
	{
		if (this.currentGlobalAudioControllers.ContainsKey(audioLayer))
		{
			for (int i = 0; i < this.currentGlobalAudioControllers[audioLayer].Count; i++)
			{
				if (this.currentGlobalAudioControllers[audioLayer][i].myGAC != null)
				{
					this.currentGlobalAudioControllers[audioLayer][i].myGAC.AdjustMyPitchTo(toPitch);
				}
			}
		}
	}

	public void AdjustGlobalVolume(AudioLayer audioLayer, float percentage)
	{
		if (this.currentGlobalAudioControllers.ContainsKey(audioLayer))
		{
			for (int i = 0; i < this.currentGlobalAudioControllers[audioLayer].Count; i++)
			{
				if (this.currentGlobalAudioControllers[audioLayer][i].myGAC != null)
				{
					this.currentGlobalAudioControllers[audioLayer][i].myGAC.AdjustMyVolumeTo(percentage);
				}
			}
		}
	}

	public void AdjustGlobalVolume(AudioLayer audioLayer, float percentage, float duration)
	{
		if (this.currentGlobalAudioControllers.ContainsKey(audioLayer))
		{
			for (int i = 0; i < this.currentGlobalAudioControllers[audioLayer].Count; i++)
			{
				if (this.currentGlobalAudioControllers[audioLayer][i].myGAC != null)
				{
					this.currentGlobalAudioControllers[audioLayer][i].myGAC.AdjustMyVolumeTo(percentage);
				}
			}
		}
	}

	public void ResetGlobalVolume(AudioLayer audioLayer)
	{
		if (this.currentGlobalAudioControllers.ContainsKey(audioLayer))
		{
			for (int i = 0; i < this.currentGlobalAudioControllers[audioLayer].Count; i++)
			{
				if (this.currentGlobalAudioControllers[audioLayer][i].myGAC != null)
				{
					this.currentGlobalAudioControllers[audioLayer][i].myGAC.ResetMyVolume();
				}
			}
		}
	}

	public void ResetGlobalVolume(AudioLayer audioLayer, float duration)
	{
		if (this.currentGlobalAudioControllers.ContainsKey(audioLayer))
		{
			for (int i = 0; i < this.currentGlobalAudioControllers[audioLayer].Count; i++)
			{
				if (this.currentGlobalAudioControllers[audioLayer][i].myGAC != null)
				{
					this.currentGlobalAudioControllers[audioLayer][i].myGAC.ResetMyVolume(duration);
				}
			}
		}
	}

	public void BuildAudioSourceForHub(AudioHubObject sourceObject, out AudioSource outAudioSource)
	{
		outAudioSource = sourceObject.gameObject.AddComponent<AudioSource>();
		switch (sourceObject.myAudioHub)
		{
		case AudioHubs.COMPUTER:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 1f;
			outAudioSource.maxDistance = 1.5f;
			break;
		case AudioHubs.COMPUTERHARDWARE:
			outAudioSource.panStereo = -0.75f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 0.5f;
			outAudioSource.maxDistance = 1f;
			break;
		case AudioHubs.OUTSIDE:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 3f;
			outAudioSource.maxDistance = 25f;
			break;
		case AudioHubs.PLAYER:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 0.5f;
			outAudioSource.maxDistance = 1.5f;
			break;
		case AudioHubs.LEFTROOM:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 1f;
			outAudioSource.maxDistance = 10f;
			break;
		case AudioHubs.MAINROOM:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 0.5f;
			outAudioSource.maxDistance = 8f;
			break;
		case AudioHubs.MENU:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 0f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 1f;
			outAudioSource.maxDistance = 500f;
			break;
		case AudioHubs.KIDNAPPER:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 1f;
			outAudioSource.maxDistance = 7f;
			break;
		case AudioHubs.RRBG:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 0.6f;
			outAudioSource.maxDistance = 6.9f;
			break;
		case AudioHubs.VICTIM:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 3.5f;
			outAudioSource.maxDistance = 15f;
			break;
		case AudioHubs.EXE:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 4.5f;
			outAudioSource.maxDistance = 15f;
			break;
		case AudioHubs.PHONE:
			outAudioSource.panStereo = 0.35f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 0.45f;
			outAudioSource.maxDistance = 0.9f;
			break;
		case AudioHubs.HACKERMODE:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 0f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 1f;
			outAudioSource.maxDistance = 500f;
			break;
		case AudioHubs.FRONT:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 1.5f;
			outAudioSource.maxDistance = 20f;
			break;
		case AudioHubs.BREATHER:
			outAudioSource.panStereo = 0f;
			outAudioSource.spatialBlend = 1f;
			outAudioSource.reverbZoneMix = 1f;
			outAudioSource.dopplerLevel = 1f;
			outAudioSource.spread = 0f;
			outAudioSource.rolloffMode = 0;
			outAudioSource.minDistance = 3f;
			outAudioSource.maxDistance = 9f;
			break;
		}
	}

	public void AddAudioHub(AudioHubObject theHub)
	{
		if (this.currentAudioHubs.ContainsKey(theHub.myAudioHub))
		{
			this.currentAudioHubs.Remove(theHub.myAudioHub);
		}
		this.currentAudioHubs.Add(theHub.myAudioHub, theHub);
	}

	public void FireSound(AudioHubs setHub, AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].fireSound(setLayer, setClip, setAudioLevel, setLoop);
		}
	}

	public void FireSound(AudioHubs setHub, AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop, float delay)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].fireSound(setLayer, setClip, setAudioLevel, setLoop, delay);
		}
	}

	public void DealSound(AudioHubs setHub, AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].playSound(setLayer, setClip, setAudioLevel, setLoop);
		}
	}

	public void DealSound(AudioHubs setHub, AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop, float delay)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].playSound(setLayer, setClip, setAudioLevel, setLoop, delay);
		}
	}

	public void DealSound(AudioHubs setHub, AudioLayer setLayer, AudioClip setClip, float setAudioLevel, bool setLoop, bool delayLoop, float delay)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].playSound(setLayer, setClip, setAudioLevel, setLoop, delayLoop, delay);
		}
	}

	public void RemoveSound(AudioHubs setHub, string soundName)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].removeSound(soundName);
		}
	}

	public void AdjustHubSoundVolume(AudioHubs setHub, float setPerValue)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].adjustSoundVolumes(setPerValue);
		}
	}

	public void AdjustHubSoundVolume(AudioHubs setHub, float setPerValue, float setDuration)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].adjustSoundVolumes(setPerValue, setDuration);
		}
	}

	public void ResetHubSoundVolume(AudioHubs setHub)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].resetSoundVolumes();
		}
	}

	public void ResetHubSoundVolume(AudioHubs setHub, float setDuration)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].resetSoundVolumes(setDuration);
		}
	}

	public void MuffleAudioHub(AudioHubs setHub, float setPerc)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].amMuffled = true;
			this.currentAudioHubs[setHub].muffledPerc = setPerc;
			this.AdjustHubSoundVolume(setHub, setPerc);
		}
	}

	public void MuffleAudioHub(AudioHubs setHub, float setPerc, float setDur)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].amMuffled = true;
			this.currentAudioHubs[setHub].muffledPerc = setPerc;
			this.AdjustHubSoundVolume(setHub, setPerc, setDur);
		}
	}

	public void UnMuffleAudioHub(AudioHubs setHub)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].amMuffled = false;
			this.currentAudioHubs[setHub].muffledPerc = 1f;
			this.ResetHubSoundVolume(setHub);
		}
	}

	public void UnMuffleAudioHub(AudioHubs setHub, float setDur)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].amMuffled = false;
			this.currentAudioHubs[setHub].muffledPerc = 1f;
			this.ResetHubSoundVolume(setHub, setDur);
		}
	}

	public void UpdateAudioHubSoundVolume(AudioHubs setHub, string soundClipName, float newVolLevel)
	{
		if (this.currentAudioHubs.ContainsKey(setHub))
		{
			this.currentAudioHubs[setHub].updateClipVolume(soundClipName, newVolLevel);
		}
	}

	private Dictionary<AudioLayer, List<audioSlinger.GACHolder>> currentGlobalAudioControllers;

	private Dictionary<AudioHubs, AudioHubObject> currentAudioHubs;

	private Dictionary<string, int> currentGACHolders;

	private Dictionary<AudioLayer, float> currentMuffeldLayers;

	public struct GACHolder
	{
		public GACHolder(string setKey, GlobalAudioController setGAC)
		{
			this.myKey = setKey;
			this.myGAC = setGAC;
		}

		public string myKey;

		public GlobalAudioController myGAC;
	}
}
