using System;
using UnityEngine;

[Serializable]
public class AudioBeatDefinition : Definition
{
	public AudioClip audioFile;

	public AudioLayer audioLayer;

	[Range(0f, 1f)]
	public float audioVolume;

	public float delay;

	public bool loop;
}
