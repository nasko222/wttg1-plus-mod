using System;
using UnityEngine;

[Serializable]
public class TutorialStepDefinition : Definition
{
	public string theStepText;

	public AudioClip theStepAudio;

	public bool hasActionIMG;

	public int actionIMG;

	public bool hasAttack;

	public string attackType;

	public bool dosAttack;

	public bool kAttack;
}
