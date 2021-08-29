using System;
using UnityEngine;
using UnityEngine.UI;

public class TextRoller : MonoBehaviour
{
	public void Fire(int setFromValue, int setToValue, float setDelayPerUnit, float setMaxDuration)
	{
		this.fromValue = setFromValue;
		this.toValue = setToValue;
		this.delayPerUnit = setDelayPerUnit;
		this.maxRollDuration = setMaxDuration;
		this.tempValue = -1;
		this.setRollUp();
	}

	public void Fire(int setFromValue, int setToValue, float setDelayPerUnit, float setMaxDuration, AudioClip setAudioClip, AudioHubs setAudioHub, AudioLayer setAudioLayer, float setAudioVolume)
	{
		this.fromValue = setFromValue;
		this.toValue = setToValue;
		this.delayPerUnit = setDelayPerUnit;
		this.maxRollDuration = setMaxDuration;
		this.rollSFX = setAudioClip;
		this.myAudioHub = setAudioHub;
		this.myAudioLayer = setAudioLayer;
		this.audioVolume = setAudioVolume;
		this.tempValue = -1;
		this.setRollUp();
	}

	private void setRollUp()
	{
		this.rollDuration = Mathf.Min((float)Mathf.Abs(this.toValue - this.fromValue) * this.delayPerUnit, this.maxRollDuration);
		this.startTimeStamp = (this.stepTimeStamp = Time.time);
		this.rollActive = true;
	}

	private void killMe()
	{
		this.rollActive = false;
		UnityEngine.Object.Destroy(this);
	}

	private void Awake()
	{
		this.myTextObject = base.GetComponent<Text>();
		if (this.myTextObject == null)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void Update()
	{
		if (this.rollActive)
		{
			if (Time.time - this.stepTimeStamp >= this.delayPerUnit && this.tempValue != this.toValue)
			{
				this.stepTimeStamp = Time.time;
				this.tempValue = this.fromValue + Mathf.RoundToInt(Mathf.Lerp(0f, (float)(this.toValue - this.fromValue), (Time.time - this.startTimeStamp) / this.rollDuration));
				this.myTextObject.text = this.tempValue.ToString();
				if (this.rollSFX != null)
				{
					GameManager.AudioSlinger.DealSound(this.myAudioHub, this.myAudioLayer, this.rollSFX, this.audioVolume, false);
				}
			}
			else if (this.tempValue == this.toValue)
			{
				this.killMe();
			}
		}
	}

	private Text myTextObject;

	private float delayPerUnit;

	private float startTimeStamp;

	private float stepTimeStamp;

	private float rollDuration;

	private float maxRollDuration;

	private float audioVolume;

	private int fromValue;

	private int toValue;

	private int tempValue;

	private bool rollActive;

	private AudioHubs myAudioHub;

	private AudioLayer myAudioLayer;

	private AudioClip rollSFX;
}
