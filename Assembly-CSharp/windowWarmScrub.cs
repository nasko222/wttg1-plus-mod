using System;
using System.Collections.Generic;
using UnityEngine;

public class windowWarmScrub : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.theWindows.Count; i++)
		{
			this.theWindows[i].SetActive(true);
		}
		this.windowTimeStamp = Time.time;
		this.warmitUp = true;
	}

	private void Update()
	{
		if (this.warmitUp && Time.time - this.windowTimeStamp >= this.windowUpTime)
		{
			this.warmitUp = false;
			for (int i = 0; i < this.theWindows.Count; i++)
			{
				this.theWindows[i].SetActive(false);
			}
		}
	}

	[Range(0f, 1f)]
	public float windowUpTime = 0.2f;

	public List<GameObject> theWindows;

	private bool warmitUp;

	private float windowTimeStamp;
}
