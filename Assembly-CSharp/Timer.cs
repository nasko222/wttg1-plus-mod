using System;
using UnityEngine;

public class Timer
{
	public Timer(float timerDuration, Action callBackFunction)
	{
		this.duration = timerDuration;
		this.myCallBack = callBackFunction;
		this.startTimeStamp = Time.time;
		this.timerIsActive = true;
	}

	public Timer(float timerDuration, Action callBackFunction, string setID)
	{
		this.duration = timerDuration;
		this.myCallBack = callBackFunction;
		this.startTimeStamp = Time.time;
		this.myID = setID;
		this.timerIsActive = true;
	}

	public Timer(float timerDuration, Action callBackFunction, int setLoops)
	{
		if (setLoops > 0)
		{
			this.duration = timerDuration;
			this.myCallBack = callBackFunction;
			this.startTimeStamp = Time.time;
			this.isLooping = true;
			this.timerIsActive = true;
			this.loopCount = setLoops;
		}
		else
		{
			this.duration = timerDuration;
			this.myCallBack = callBackFunction;
			this.startTimeStamp = Time.time;
			this.timerIsActive = true;
		}
	}

	public Timer(float timerDuration, Action callBackFunction, int setLoops, string setID)
	{
		if (setLoops > 0)
		{
			this.duration = timerDuration;
			this.myCallBack = callBackFunction;
			this.startTimeStamp = Time.time;
			this.myID = setID;
			this.isLooping = true;
			this.timerIsActive = true;
			this.loopCount = setLoops;
		}
		else
		{
			this.duration = timerDuration;
			this.myCallBack = callBackFunction;
			this.startTimeStamp = Time.time;
			this.timerIsActive = true;
		}
	}

	public Timer(float timerDuration, Action<string> callBackFunction, string callBackValue)
	{
		this.duration = timerDuration;
		this.myStrCallBack = callBackFunction;
		this.myStrValue = callBackValue;
		this.startTimeStamp = Time.time;
		this.timerIsActive = true;
	}

	public Timer(float timerDuration, Action<int> callBackFunction, int callBackValue)
	{
		this.duration = timerDuration;
		this.myIntCallBack = callBackFunction;
		this.myIntValue = callBackValue;
		this.startTimeStamp = Time.time;
		this.timerIsActive = true;
	}

	public Timer(float timerDuration, Action<int> callBackFunction, int callBackValue, int setLoops)
	{
		if (setLoops > 0)
		{
			this.duration = timerDuration;
			this.myIntCallBack = callBackFunction;
			this.myIntValue = callBackValue;
			this.startTimeStamp = Time.time;
			this.isLooping = true;
			this.timerIsActive = true;
			this.loopCount = setLoops;
		}
		else
		{
			this.duration = timerDuration;
			this.myIntCallBack = callBackFunction;
			this.myIntValue = callBackValue;
			this.startTimeStamp = Time.time;
			this.timerIsActive = true;
		}
	}

	public Timer(float timerDuration, Action<int, int> callBackFunction, int callBackValueA, int callBackValueB)
	{
		this.duration = timerDuration;
		this.myIntIntCallBack = callBackFunction;
		this.myIntValue = callBackValueA;
		this.myIntValueB = callBackValueB;
		this.startTimeStamp = Time.time;
		this.timerIsActive = true;
	}

	public Timer(float timerDuration, Action<string, float> callBackFunction, string stringCallBackValue, float floatCallBackValue)
	{
		this.duration = timerDuration;
		this.myStrFloatCallBack = callBackFunction;
		this.myStrValue = stringCallBackValue;
		this.myFloatValue = floatCallBackValue;
		this.startTimeStamp = Time.time;
		this.timerIsActive = true;
	}

	public Timer(float timerDuration, Action<bool> callBackFunction, bool setBoolValue)
	{
		this.duration = timerDuration;
		this.myBoolCallBack = callBackFunction;
		this.myBoolValue = setBoolValue;
		this.startTimeStamp = Time.time;
		this.timerIsActive = true;
	}

	public void Stop()
	{
		this.timerIsActive = false;
	}

	public void TriggerCallBack()
	{
		if (this.timerIsActive)
		{
			if (this.myCallBack != null)
			{
				this.myCallBack.DynamicInvoke(new object[0]);
			}
			else if (this.myStrCallBack != null)
			{
				this.myStrCallBack(this.myStrValue);
			}
			else if (this.myIntCallBack != null)
			{
				this.myIntCallBack(this.myIntValue);
			}
			else if (this.myIntIntCallBack != null)
			{
				this.myIntIntCallBack(this.myIntValue, this.myIntValueB);
			}
			else if (this.myStrFloatCallBack != null)
			{
				this.myStrFloatCallBack(this.myStrValue, this.myFloatValue);
			}
			else if (this.myBoolCallBack != null)
			{
				this.myBoolCallBack(this.myBoolValue);
			}
		}
	}

	public float duration;

	public float startTimeStamp;

	public bool isLooping;

	public int loopCount;

	public string myID;

	private Action myCallBack;

	private Action<string> myStrCallBack;

	private string myStrValue;

	private Action<int> myIntCallBack;

	public Action<int, int> myIntIntCallBack;

	private int myIntValue;

	private int myIntValueB;

	private bool timerIsActive;

	private Action<string, float> myStrFloatCallBack;

	private float myFloatValue;

	private Action<bool> myBoolCallBack;

	private bool myBoolValue;
}
