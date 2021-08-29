using System;
using UnityEngine;

public class rbScrub : MonoBehaviour
{
	private void captureProbe()
	{
		this.myRefProbe.RenderProbe();
	}

	private void checkCaptureTime()
	{
		if (Time.time - this.rbTimeStamp >= this.ReflectCaptureTime)
		{
			this.captureProbe();
			this.countDownActive = false;
		}
	}

	private void Start()
	{
		this.rbTimeStamp = Time.time;
		this.myRefProbe = base.GetComponent<ReflectionProbe>();
		this.countDownActive = true;
	}

	private void Update()
	{
		if (this.countDownActive)
		{
			this.checkCaptureTime();
		}
	}

	[Range(0f, 10f)]
	public float ReflectCaptureTime = 1f;

	private float rbTimeStamp;

	private ReflectionProbe myRefProbe;

	private bool countDownActive;
}
