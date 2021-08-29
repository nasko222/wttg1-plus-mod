using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyPoll : MonoBehaviour
{
	public void BeginVote()
	{
		if (this.myDOSTwitch != null)
		{
			GameManager.GetTheUIManager().displayMSGPopUp("Escalation Poll In Progress");
			this.currentVotes = new Dictionary<string, string>();
			this.myDOSTwitch.myTwitchIRC.SendMsg("WTTG Escalation Poll!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Shall the player get a random free key? Or release The Kidnapper Or release The Breather?");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!KEY");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!KIDNAPPER");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!BREATHER");
			this.voteIsLive = true;
			GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd));
		}
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "KEY" || text == "KIDNAPPER" || text == "BREATHER"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void PollEnd()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Escalation Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "KEY")
			{
				num++;
			}
			else if (keyValuePair.Value == "KIDNAPPER")
			{
				num2++;
			}
			else if (keyValuePair.Value == "BREATHER")
			{
				num3++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("KEY: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("KIDNAPPER: " + num2.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("BREATHER: " + num3.ToString());
		if (num == 0 && num2 == 0 && num3 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted.. Story of my life");
		}
		else if (num > num2 && num > num3)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("A random key was placed on players desktop.");
			string randomKey = GameManager.GetTheCloud().getRandomKey();
			GameManager.GetTheCloud().addTxtDoc("key.txt", randomKey);
		}
		else if (num2 > num && num2 > num3)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Kidnapper has been set free!");
			GameManager.GetTheTrackerManager().userCanNowBeTracked();
		}
		else if (num3 > num && num3 > num2)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Breather has been set free!");
			GameManager.GetTheBreatherManager().releaseTheBreather();
		}
		else
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("There is a tie! RE-VOTE!");
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.BeginVote));
			flag = true;
		}
		if (!flag)
		{
			this.myDOSTwitch.setPollInactive();
		}
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;
}
