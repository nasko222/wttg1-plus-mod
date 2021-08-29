using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class rrLampScrub : MonoBehaviour
{
	private void setFadeLightTime()
	{
		this.fadeTimeWindow = Random.Range(this.minWaitTime, this.maxWaitTime);
		this.fadeTimeStamp = Time.time;
		this.fadeTheLight = true;
	}

	private void fadeLight()
	{
		this.lightFadeSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.lightFadeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainLight.intensity, delegate(float x)
		{
			this.mainLight.intensity = x;
		}, this.mainLightDefaultValue * 0.65f, 0.8f), 1));
		TweenSettingsExtensions.Insert(this.lightFadeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ambLight.intensity, delegate(float x)
		{
			this.ambLight.intensity = x;
		}, this.ambLightDefaultValue * 0.4f, 0.8f), 1));
		TweenSettingsExtensions.Insert(this.lightFadeSeq, 0.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.mainLight.intensity, delegate(float x)
		{
			this.mainLight.intensity = x;
		}, this.mainLightDefaultValue, 0.7f), 1));
		TweenSettingsExtensions.Insert(this.lightFadeSeq, 0.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ambLight.intensity, delegate(float x)
		{
			this.ambLight.intensity = x;
		}, this.ambLightDefaultValue, 0.7f), 1));
		TweenExtensions.Play<Sequence>(this.lightFadeSeq);
		this.setFadeLightTime();
	}

	private void Start()
	{
		this.mainLightDefaultValue = this.mainLight.intensity;
		this.ambLightDefaultValue = this.ambLight.intensity;
		this.setFadeLightTime();
	}

	private void Update()
	{
		if (this.fadeTheLight && Time.time - this.fadeTimeStamp >= this.fadeTimeWindow)
		{
			this.fadeTheLight = false;
			this.fadeLight();
		}
	}

	[Range(0.5f, 15f)]
	public float minWaitTime = 8f;

	[Range(15f, 45f)]
	public float maxWaitTime = 15f;

	public Light mainLight;

	public Light ambLight;

	public GameObject bulbHolder;

	private Sequence lightFadeSeq;

	private float mainLightDefaultValue;

	private float ambLightDefaultValue;

	private float fadeTimeStamp;

	private float fadeTimeWindow;

	private bool fadeTheLight;
}
