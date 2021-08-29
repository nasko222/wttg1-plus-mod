using System;
using System.Collections.Generic;
using UnityEngine;

public class SiteSpeedPoll : MonoBehaviour
{
	public void BeginVote()
	{
		if (this.myDOSTwitch != null)
		{
			GameManager.GetTheUIManager().displayMSGPopUp("Site Load Speed Poll in Progress");
			this.currentVotes = new Dictionary<string, string>();
			this.myDOSTwitch.myTwitchIRC.SendMsg("WTTG Site Load Speed Poll!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Should the websites load in faster or slower for 3 minutes?");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!FASTER");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!SLOWER");
			this.voteIsLive = true;
			GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd));
		}
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "FASTER" || text == "SLOWER"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void PollEnd()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Site Load Speed Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "FASTER")
			{
				num++;
			}
			else if (keyValuePair.Value == "SLOWER")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("FASTER: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("SLOWER: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted.. Story of my life");
		}
		else if (num > num2)
		{
			GameManager.GetTheAnnBehavior().setPageLoadingTime(true);
			GameManager.GetTheUIManager().displayMSGPopUp("Sites will load faster for the next 3 mins!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Sites will now load faster for next 3 mins!");
		}
		else if (num2 > num)
		{
			GameManager.GetTheAnnBehavior().setPageLoadingTime(false);
			GameManager.GetTheUIManager().displayMSGPopUp("Sites will load slower for the next 3 mins!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Sites will now load slower for next 3 mins!");
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
