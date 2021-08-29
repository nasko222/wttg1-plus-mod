using System;
using System.Collections.Generic;
using UnityEngine;

public class timeSlinger
{
	public void clearTimers()
	{
		for (int i = 0; i < this.myTimers.Count; i++)
		{
			this.myTimers[i].Stop();
			this.myTimers[i] = null;
		}
		this.myTimers.Clear();
	}

	public void FireTimer(float duration, Action callBack)
	{
		Timer item = new Timer(duration, callBack);
		this.myTimers.Add(item);
	}

	public void FireTimer(float duration, Action callBack, string setID)
	{
		Timer item = new Timer(duration, callBack, setID);
		this.myTimers.Add(item);
	}

	public void FireTimer(float duration, Action callBack, int loopCount)
	{
		if (loopCount > 0)
		{
			Timer item = new Timer(duration, callBack, loopCount);
			this.myTimers.Add(item);
		}
		else
		{
			this.FireTimer(duration, callBack);
		}
	}

	public void FireTimer(float duration, Action callBack, int loopCount, string setID)
	{
		if (loopCount > 0)
		{
			Timer item = new Timer(duration, callBack, loopCount, setID);
			this.myTimers.Add(item);
		}
		else
		{
			this.FireTimer(duration, callBack);
		}
	}

	public void FireStringTimer(float duration, Action<string> callBack, string callValue)
	{
		Timer item = new Timer(duration, callBack, callValue);
		this.myTimers.Add(item);
	}

	public void FireIntTimer(float duration, Action<int> callBack, int callValue)
	{
		Timer item = new Timer(duration, callBack, callValue);
		this.myTimers.Add(item);
	}

	public void FireIntTimer(float duration, Action<int> callBack, int callValue, int loopCount)
	{
		Timer item = new Timer(duration, callBack, callValue, loopCount);
		this.myTimers.Add(item);
	}

	public void FireIntIntTimer(float duration, Action<int, int> callBack, int callValueA, int callValueB)
	{
		Timer item = new Timer(duration, callBack, callValueA, callValueB);
		this.myTimers.Add(item);
	}

	public void FireStrFloatTimer(float duration, Action<string, float> callBack, string strCallValue, float foatCallValue)
	{
		Timer item = new Timer(duration, callBack, strCallValue, foatCallValue);
		this.myTimers.Add(item);
	}

	public void FireBoolTimer(float duration, Action<bool> callBack, bool boolValue)
	{
		Timer item = new Timer(duration, callBack, boolValue);
		this.myTimers.Add(item);
	}

	public void KillTimerWithID(string theID)
	{
		for (int i = 0; i < this.myTimers.Count; i++)
		{
			if (this.myTimers[i].myID == theID)
			{
				this.myTimers[i].Stop();
				this.myTimers.RemoveAt(i);
				i--;
			}
		}
	}

	public void Update()
	{
		for (int i = 0; i < this.myTimers.Count; i++)
		{
			if (this.myTimers[i] != null && Time.time - this.myTimers[i].startTimeStamp >= this.myTimers[i].duration)
			{
				if (this.myTimers[i].isLooping)
				{
					this.myTimers[i].TriggerCallBack();
					this.myTimers[i].startTimeStamp = Time.time;
					this.myTimers[i].loopCount = this.myTimers[i].loopCount - 1;
					if (this.myTimers[i].loopCount <= 0)
					{
						if (this.myTimers.Count > 0)
						{
							this.myTimers.RemoveAt(i);
						}
						i--;
					}
				}
				else
				{
					this.myTimers[i].TriggerCallBack();
					if (this.myTimers.Count > 0)
					{
						this.myTimers.RemoveAt(i);
					}
					i--;
				}
			}
		}
	}

	private List<Timer> myTimers = new List<Timer>();
}
