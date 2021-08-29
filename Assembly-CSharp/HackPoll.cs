using System;
using System.Collections.Generic;
using UnityEngine;

public class HackPoll : MonoBehaviour
{
	public void BeginVote()
	{
		this.pickedBlackHatHacker = string.Empty;
		this.pickHackType = string.Empty;
		if (this.myDOSTwitch != null)
		{
			GameManager.GetTheUIManager().displayMSGPopUp("Hacker Poll In Progress");
			this.currentVotes = new Dictionary<string, string>();
			this.myDOSTwitch.myTwitchIRC.SendMsg("WTTG Hacker Poll!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Who will win, the black hats or white hats! - VOTE!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!WHITEHAT");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!BLACKHAT");
			this.voteIsLive = true;
			this.pollHasRan = false;
			GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), "WTTGTwitchHackerPoll");
		}
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive)
		{
			if (theVote.Contains("!"))
			{
				string text = theVote.Replace("!", string.Empty);
				if (userName == "reflectstudios" && text == "FORCE")
				{
					GameManager.TimeSlinger.KillTimerWithID("WTTGTwitchHackerPoll");
					this.myDOSTwitch.myTwitchIRC.SendMsg("Sorry, but the game dev had to flex his puny muscles and become the 1337 black hat hacker!");
					this.voteIsLive = false;
					this.pollHasRan = true;
					this.pickedBlackHatHacker = userName;
					this.BlackHatPickHackType();
				}
				else if (text == "WHITEHAT" || text == "BLACKHAT")
				{
					this.currentVotes.Add(userName, text);
				}
			}
		}
		else if (this.bhPickType)
		{
			if (userName == this.pickedBlackHatHacker && theVote.Contains("!"))
			{
				string a = theVote.Replace("!", string.Empty);
				if (a == "DOS" || a == "KERNAL" || a == "VAPE")
				{
					GameManager.TimeSlinger.KillTimerWithID("wttgBHPickType");
					this.pickHackType = a;
					this.PollEndHackType();
				}
			}
		}
		else if (this.bhPickDiff && userName == this.pickedBlackHatHacker && theVote.Contains("!"))
		{
			string a2 = theVote.Replace("!", string.Empty);
			if (a2 == "1337" || a2 == "SCRIPT" || a2 == "NOOB")
			{
				GameManager.TimeSlinger.KillTimerWithID("wttgBGPickDiff");
				this.pickHackDiff = a2;
				this.PollEndHackDiff();
			}
		}
	}

	private void BlackHatPickHackType()
	{
		this.myDOSTwitch.myTwitchIRC.SendMsg("It is time! - @" + this.pickedBlackHatHacker + " Pick The Hack!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!DOS");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!KERNAL");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!VAPE");
		this.bhPickType = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEndHackType), "wttgBHPickType");
	}

	private void BlackHatPickHackDiff()
	{
		this.myDOSTwitch.myTwitchIRC.SendMsg("@" + this.pickedBlackHatHacker + " Select your skill set!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!1337");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!SCRIPT");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!NOOB");
		this.bhPickDiff = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEndHackDiff), "wttgBGPickDiff");
	}

	private void PollEnd()
	{
		if (!this.pollHasRan)
		{
			this.pollHasRan = true;
			int num = 0;
			int num2 = 0;
			List<string> list = new List<string>();
			this.voteIsLive = false;
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Hacker Poll Has Ended!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
			foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
			{
				if (keyValuePair.Value == "WHITEHAT")
				{
					num++;
				}
				else
				{
					num2++;
					list.Add(keyValuePair.Key);
				}
			}
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("WHITEHAT: " + num.ToString());
			this.myDOSTwitch.myTwitchIRC.SendMsg("BLACKHAT: " + num2.ToString());
			if (num == 0 && num2 == 0)
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted.. Story of my life");
				this.myDOSTwitch.setPollInactive();
			}
			else if (num > num2)
			{
				this.myDOSTwitch.setPollInactive();
				GameManager.GetTheHackerManager().MaskTheIP();
				GameManager.GetTheUIManager().displayMSGPopUp("The WHITEHATS Have Won! - Instant Modem Reset!");
				this.myDOSTwitch.myTwitchIRC.SendMsg("The WHITEHATS Have Won!", 2f);
				this.myDOSTwitch.myTwitchIRC.SendMsg("HACK BLOCKED!", 2f);
			}
			else if (num == num2)
			{
				this.currentVotes.Clear();
				this.currentVotes = new Dictionary<string, string>();
				this.pickedBlackHatHacker = string.Empty;
				this.pickHackDiff = string.Empty;
				this.pickHackType = string.Empty;
				GameManager.TimeSlinger.FireTimer(10f, new Action(this.BeginVote));
				this.myDOSTwitch.myTwitchIRC.SendMsg("IT IS A TIE!");
				this.myDOSTwitch.myTwitchIRC.SendMsg("RE-VOTE IN 10 SECONDS!");
			}
			else
			{
				string str = string.Empty;
				for (int i = 0; i < 10; i++)
				{
					str = list[Random.Range(0, list.Count)];
				}
				this.pickedBlackHatHacker = str;
				this.myDOSTwitch.myTwitchIRC.SendMsg("The BLACKHATS Have Won!");
				this.myDOSTwitch.myTwitchIRC.SendMsg("The 1337 Hacker Is @" + str);
				this.BlackHatPickHackType();
			}
		}
	}

	private void PollEndHackType()
	{
		this.bhPickType = false;
		if (this.pickHackType != string.Empty)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg(string.Concat(new string[]
			{
				"@",
				this.pickedBlackHatHacker,
				" has picked a ",
				this.pickHackType,
				" attack!"
			}));
			this.BlackHatPickHackDiff();
		}
		else
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("@" + this.pickedBlackHatHacker + " has failed to pick a hack!");
			this.myDOSTwitch.setPollInactive();
		}
	}

	private void PollEndHackDiff()
	{
		this.bhPickDiff = false;
		if (this.pickHackDiff != string.Empty)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg(string.Concat(new string[]
			{
				"Launching a ",
				this.pickHackDiff,
				" ",
				this.pickHackType,
				" Attack!"
			}));
			this.myDOSTwitch.myTwitchIRC.SendMsg("HA HA HA HAAAAA!");
			GameManager.GetTheHackerManager().launchTwitchHack(this.pickedBlackHatHacker, this.pickHackType, this.pickHackDiff);
			this.myDOSTwitch.setPollInactive();
		}
		else
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("@" + this.pickedBlackHatHacker + " has failed to pick a skill level!");
			this.myDOSTwitch.setPollInactive();
		}
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;

	private bool bhPickType;

	private bool bhPickDiff;

	private string pickedBlackHatHacker;

	private string pickHackType;

	private string pickHackDiff;

	private bool pollHasRan;
}
