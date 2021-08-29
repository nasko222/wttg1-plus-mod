using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
	public void loseDays(int dayCount = 0)
	{
		this.gameDay += dayCount;
		this.DayText.text = this.gameDay.ToString();
		if (dayCount != 0)
		{
			GameManager.FileSlinger.saveData.playerResetStats = true;
		}
		GameManager.FileSlinger.saveData.gameDay = this.gameDay;
		GameManager.FileSlinger.saveFile("wttg2.gd");
		this.gameMin = 0;
		this.gameHour = 0;
		this.curSecond = 0f;
		this.curTimeStamp = Time.time;
		this.isPM = true;
		this.hourIndex = this.MinTimePM;
		GameManager.TimeSlinger.FireTimer(10f, new Action(this.checkDayLimit));
	}

	public bool isWindowOpen(SiteDefinition siteDef)
	{
		return siteDef.windowUp <= this.gameHour && siteDef.windowDown > this.gameHour;
	}

	public void shutItDown()
	{
		this.gameDay++;
		this.DayText.text = this.gameDay.ToString();
		GameManager.FileSlinger.saveData.gameDay = this.gameDay;
		GameManager.FileSlinger.saveFile("wttg2.gd");
	}

	public int getDayCount()
	{
		return this.gameDay;
	}

	public bool isItOpen()
	{
		return this.gameHour == 5 && this.gameMin >= 2;
	}

	private void updateClock()
	{
		this.curSecond = Time.time - this.curTimeStamp;
		if (this.curSecond >= this.MinPerHour * 60f / 4f)
		{
			this.gameMin++;
			this.curSecond = 0f;
			this.curTimeStamp = Time.time;
			this.updateTheCT = true;
		}
		if (this.gameMin >= 4)
		{
			this.gameHour++;
			this.gameMin = 0;
			this.hourIndex++;
			this.updateTheCT = true;
		}
		if (this.hourIndex >= this.hourArray.Count)
		{
			this.hourIndex = 0;
			if (this.isPM)
			{
				this.isPM = false;
			}
			else
			{
				this.isPM = true;
			}
			this.updateTheCT = true;
		}
		if (this.gameHour >= this.hoursInDay)
		{
			this.endOfDayCycle();
			this.updateTheCT = true;
		}
		if (this.updateTheCT)
		{
			this.updateClocksText();
			this.updateTheCT = false;
		}
	}

	private void updateClocksText()
	{
		this.TimeText.text = string.Concat(new string[]
		{
			this.getHourValue(),
			":",
			this.getMinValue(),
			" ",
			this.getAMPM()
		});
		this.phoneLockClock.text = string.Concat(new string[]
		{
			this.getHourValue(),
			":",
			this.getMinValue(),
			" ",
			this.getAMPM()
		});
	}

	private void endOfDayCycle()
	{
		this.gameDay++;
		this.DayText.text = this.gameDay.ToString();
		this.myUIManager.flashSaveIcon();
		GameManager.FileSlinger.saveData.gameDay = this.gameDay;
		GameManager.FileSlinger.saveFile("wttg2.gd");
		this.gameMin = 0;
		this.gameHour = 0;
		this.curSecond = 0f;
		this.curTimeStamp = Time.time;
		this.isPM = true;
		this.hourIndex = this.MinTimePM;
		if (!GameManager.GetGameModeManager().getCasualMode() && this.gameDay == 2)
		{
			GameManager.GetTheCloud().addTxtDoc("readme.txt", "Hey,\n\nIt’s Adam again. I just found out that the Red Room is only open for 30 days at a time. Look at the calendar count at the top right corner of your screen. Do you see that? Once that hits 30.. GAME OVER.");
		}
		GameManager.TimeSlinger.FireTimer(10f, new Action(this.checkDayLimit));
	}

	private void checkDayLimit()
	{
		if (!GameManager.GetGameModeManager().getCasualMode() && this.gameDay >= this.maxDays)
		{
			this.myUIManager.triggerGameOver("You ran out of time, the Red Room went offline...");
		}
	}

	private string getHourValue()
	{
		return this.hourArray[this.hourIndex];
	}

	private string getMinValue()
	{
		string result = string.Empty;
		switch (this.gameMin)
		{
		case 0:
			result = "00";
			break;
		case 1:
			result = "15";
			break;
		case 2:
			result = "30";
			break;
		case 3:
			result = "45";
			break;
		}
		return result;
	}

	private string getAMPM()
	{
		if (this.isPM)
		{
			return "PM";
		}
		return "AM";
	}

	private void Start()
	{
		this.updateTheClock = true;
		this.isPM = true;
		this.gameDay = GameManager.FileSlinger.saveData.gameDay;
		this.DayText.text = this.gameDay.ToString();
		this.hourArray.Add("12");
		this.hourArray.Add("01");
		this.hourArray.Add("02");
		this.hourArray.Add("03");
		this.hourArray.Add("04");
		this.hourArray.Add("05");
		this.hourArray.Add("06");
		this.hourArray.Add("07");
		this.hourArray.Add("08");
		this.hourArray.Add("09");
		this.hourArray.Add("10");
		this.hourArray.Add("11");
		this.curTimeStamp = Time.time;
		this.hoursInDay = this.MaxTimeAM + 12 - this.MinTimePM;
		this.hourIndex = this.MinTimePM;
		GameManager.TimeSlinger.FireTimer(10f, new Action(this.checkDayLimit));
	}

	private void Update()
	{
		if (!this.freezeTime)
		{
			if (this.updateTheClock)
			{
				this.updateClock();
			}
		}
		else
		{
			this.curTimeStamp = Time.time;
		}
	}

	public TheCloud myCould;

	public UIManager myUIManager;

	public Text TimeText;

	public Text DayText;

	public Text phoneLockClock;

	public bool freezeTime;

	[Range(5f, 60f)]
	public int maxDays = 30;

	[Range(1f, 12f)]
	public int MinTimePM = 10;

	[Range(1f, 12f)]
	public int MaxTimeAM = 4;

	[Range(0.15f, 10f)]
	public float MinPerHour = 2f;

	private int gameDay = 1;

	private int gameHour;

	private int gameMin;

	private int hoursInDay;

	private int hourIndex;

	private float curTimeStamp;

	private float curSecond;

	private bool updateTheClock = true;

	private bool isPM = true;

	private bool updateTheCT;

	private List<string> hourArray = new List<string>();
}
