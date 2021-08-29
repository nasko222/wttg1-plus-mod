using System;
using System.Collections.Generic;
using UnityEngine;

public class ModemTimePoll : MonoBehaviour
{
	public void BeginVote()
	{
		if (this.myDOSTwitch != null)
		{
			GameManager.GetTheUIManager().displayMSGPopUp("Modem Reset Time Poll in Progress");
			this.currentVotes = new Dictionary<string, string>();
			this.myDOSTwitch.myTwitchIRC.SendMsg("WTTG Modem Reset Time Poll!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Should the modem reset time be longer or shorter? Will be active for 5 mins.");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!LONGER");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!SHORTER");
			this.voteIsLive = true;
			GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd));
		}
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "LONGER" || text == "SHORTER"))
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
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Modem Reset Time Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "LONGER")
			{
				num++;
			}
			else if (keyValuePair.Value == "SHORTER")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("LONGER: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("SHORTER: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted.. Story of my life");
		}
		else if (num > num2)
		{
			GameManager.GetModemAction().setModemTime(true);
			GameManager.GetTheUIManager().displayMSGPopUp("Modem reset time will be longer for the next 5 mins!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Modem reset time will be longer for the next 5 mins!");
		}
		else if (num2 > num)
		{
			GameManager.GetModemAction().setModemTime(false);
			GameManager.GetTheUIManager().displayMSGPopUp("Modem reset time will be shorter for the next 5 mins!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Modem reset time will be shorter for the next 5 mins!");
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
