using System;
using UnityEngine;

public class jumpScrub : MonoBehaviour
{
	private void hideBadGuy()
	{
		this.badGuy.SetActive(false);
		this.badLight.SetActive(false);
	}

	private void Start()
	{
		this.theAs = base.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.badGuy.SetActive(true);
			this.badLight.SetActive(true);
			this.theAs.Play();
			GameManager.TimeSlinger.FireTimer(5f, new Action(this.hideBadGuy));
		}
	}

	public GameObject badGuy;

	public GameObject badLight;

	private AudioSource theAs;
}
