using System;
using System.Collections.Generic;
using UnityEngine;

public class SpookPoll : MonoBehaviour
{
	public void BeginVote()
	{
		if (this.myDOSTwitch != null)
		{
			GameManager.GetTheUIManager().displayMSGPopUp("Spooky Poll in Progress");
			this.currentVotes = new Dictionary<string, string>();
			this.myDOSTwitch.myTwitchIRC.SendMsg("WTTG Random Spooky Poll!");
			this.myDOSTwitch.myTwitchIRC.SendMsg("Should a random spooky thing happen?");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!YES");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!NO");
			this.voteIsLive = true;
			GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd));
		}
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "YES" || text == "NO"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void triggerSpookyThing()
	{
		int num = 0;
		for (int i = 0; i < 20; i++)
		{
			num = Random.Range(0, 11);
		}
		if (num < 3)
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(30f, 210f), new Action(GameManager.GetTheSceneManager().triggerLightJump));
		}
		else if (num < 7 && num >= 3)
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(30f, 210f), new Action(GameManager.GetTheBreatherManager().triggerBreatherWindowJump));
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(30f, 210f), new Action(GameManager.GetTheUIManager().triggerFlashCreep));
		}
	}

	private void PollEnd()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Spooky Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "YES")
			{
				num++;
			}
			else if (keyValuePair.Value == "NO")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("YES: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("NO: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted.. Story of my life");
		}
		else if (num > num2)
		{
			this.triggerSpookyThing();
			this.myDOSTwitch.myTwitchIRC.SendMsg("A random spooky thing will happen!");
		}
		else if (num2 > num)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No spooky thing will happen!");
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
