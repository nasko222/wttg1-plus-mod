using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyQue : MonoBehaviour
{
	public void BeginVote()
	{
		if (this.myDOSTwitch != null)
		{
			GameManager.GetTheUIManager().displayMSGPopUp("Key Cue Alert Poll in Progress");
			this.currentVotes = new Dictionary<string, string>();
			this.myDOSTwitch.myTwitchIRC.SendMsg("WTTG Key Cue Poll!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Should the cue for websites that contain keys be longer, or disabled? Will be active for 5 mins.");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!LONGER");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!DISABLED");
			this.voteIsLive = true;
			GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd));
		}
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "LONGER" || text == "DISABLED"))
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
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Key Cue Alert Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "LONGER")
			{
				num++;
			}
			else if (keyValuePair.Value == "DISABLED")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("LONGER: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("DISABLED: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted.. Story of my life");
		}
		else if (num > num2)
		{
			GameManager.GetTheAnnBehavior().setKeyQueAlert(true);
			GameManager.GetTheUIManager().displayMSGPopUp("The key cue alert will remain longer for the next 5 mins!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("The key cue alert will remain longer for the next 5 mins!");
		}
		else if (num2 > num)
		{
			GameManager.GetTheAnnBehavior().setKeyQueAlert(false);
			GameManager.GetTheUIManager().displayMSGPopUp("The key cue alert will be disabled for the next 5 mins!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("The key cue alert will be disabled for the next 5 mins!");
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.BeginVote));
			this.myDOSTwitch.myTwitchIRC.SendMsg("There is a tie! RE-VOTE!");
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
