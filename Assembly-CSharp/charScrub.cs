using System;
using UnityEngine;

public class charScrub : MonoBehaviour
{
	public void activateMoveChair()
	{
		this.moveTimeStamp = Time.time;
		this.moveTimeWindow = UnityEngine.Random.Range(this.moveOpenWindow, this.moveCloseWindow);
		this.moveisActive = true;
	}

	private void moveTheChair()
	{
		if (this.myMainController.isUsingComputer)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.MAINROOM, AudioLayer.BACKGROUND, this.chairSound, 0.5f, false);
			base.transform.localPosition = this.chairMovedPos;
			base.transform.localRotation = Quaternion.Euler(this.chairMovedRot);
		}
		else
		{
			this.moveTimeStamp = Time.time;
			this.moveTimeWindow = 60f;
			this.moveisActive = true;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.moveisActive && Time.time - this.moveTimeStamp >= this.moveTimeWindow)
		{
			this.moveisActive = false;
			this.moveTheChair();
		}
	}

	public Vector3 chairMovedPos;

	public Vector3 chairMovedRot;

	public mainController myMainController;

	public AudioClip chairSound;

	[Range(0f, 600f)]
	public float moveOpenWindow;

	[Range(0f, 1200f)]
	public float moveCloseWindow;

	private float moveTimeStamp;

	private float moveTimeWindow;

	private bool moveisActive;
}
