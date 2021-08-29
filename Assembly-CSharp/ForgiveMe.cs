using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgiveMe : MonoBehaviour
{
	private void iWasTapped()
	{
		this.iHaveKey = true;
		int num = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		ConfessionDefinition confessionDefinition = (ConfessionDefinition)ScriptableObject.CreateInstance("ConfessionDefinition");
		confessionDefinition.confessionText = "You will regret this. " + num.ToString() + " - " + empty;
		this.confessKey = confessionDefinition;
	}

	private void generateRandomConf()
	{
		bool flag = false;
		if (this.iHaveKey)
		{
			int num = UnityEngine.Random.Range(0, 20);
			if (num == 13)
			{
				flag = true;
			}
		}
		if (flag)
		{
			string confessionText = this.confessKey.confessionText;
			this.confTextHolder.text = confessionText;
		}
		else if (this.haveLiveConfessions)
		{
			int num2 = UnityEngine.Random.Range(0, 10);
			if (num2 < 5)
			{
				if (this.liveConfessions.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, this.liveConfessions.Count);
					string confessionText2 = this.liveConfessions[index].confessionText;
					this.confTextHolder.text = confessionText2;
					this.tmpLiveConfessions.Add(this.liveConfessions[index]);
					this.liveConfessions.RemoveAt(index);
				}
				else
				{
					for (int i = 0; i < this.tmpLiveConfessions.Count; i++)
					{
						this.liveConfessions.Add(this.tmpLiveConfessions[i]);
					}
					this.tmpLiveConfessions.Clear();
					int index2 = UnityEngine.Random.Range(0, this.liveConfessions.Count);
					string confessionText3 = this.liveConfessions[index2].confessionText;
					this.confTextHolder.text = confessionText3;
					this.tmpLiveConfessions.Add(this.liveConfessions[index2]);
					this.liveConfessions.RemoveAt(index2);
				}
			}
			else if (this.theConfessions.Count > 0)
			{
				int index3 = UnityEngine.Random.Range(0, this.theConfessions.Count);
				string confessionText4 = this.theConfessions[index3].confessionText;
				this.confTextHolder.text = confessionText4;
				this.tmpLocalConfessions.Add(this.theConfessions[index3]);
				this.theConfessions.RemoveAt(index3);
			}
			else
			{
				for (int j = 0; j < this.tmpLocalConfessions.Count; j++)
				{
					this.theConfessions.Add(this.tmpLocalConfessions[j]);
				}
				this.tmpLocalConfessions.Clear();
				int index4 = UnityEngine.Random.Range(0, this.theConfessions.Count);
				string confessionText5 = this.theConfessions[index4].confessionText;
				this.confTextHolder.text = confessionText5;
				this.theConfessions.RemoveAt(index4);
			}
		}
		else if (this.theConfessions.Count > 0)
		{
			int index5 = UnityEngine.Random.Range(0, this.theConfessions.Count);
			string confessionText6 = this.theConfessions[index5].confessionText;
			this.confTextHolder.text = confessionText6;
			this.tmpLocalConfessions.Add(this.theConfessions[index5]);
			this.theConfessions.RemoveAt(index5);
		}
		else
		{
			for (int k = 0; k < this.tmpLocalConfessions.Count; k++)
			{
				this.theConfessions.Add(this.tmpLocalConfessions[k]);
			}
			this.tmpLocalConfessions.Clear();
			int index6 = UnityEngine.Random.Range(0, this.theConfessions.Count);
			string confessionText7 = this.theConfessions[index6].confessionText;
			this.confTextHolder.text = confessionText7;
			this.theConfessions.RemoveAt(index6);
		}
	}

	private void checkConnection()
	{
		GameManager.GetTheHTTPPusher().connectionCheck(new Action(this.connectionGood), new Action(this.connectionBad));
	}

	private void connectionGood()
	{
		this.checkConfServerOnline();
	}

	private void connectionBad()
	{
		this.isOnline = false;
	}

	private void checkConfServerOnline()
	{
		string postURL = "http://wttg.reflectstudios.com/stats.php";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("action", "checkConfessServerStats");
		GameManager.GetTheHTTPPusher().submitPostText(postURL, dictionary, new Action<bool, WTTGJSONData>(this.respFromConfServer));
	}

	private void respFromConfServer(bool pass, WTTGJSONData rData)
	{
		if (pass)
		{
			if (rData.data == "1")
			{
				this.getLiveConfessions();
			}
			else
			{
				this.confServerOnline = false;
			}
		}
		else
		{
			this.confServerOnline = false;
		}
		if (!this.confServerOnline)
		{
			GameManager.GetTheUIManager().displayMSGPopUp("No live confessions available");
		}
	}

	private void getLiveConfessions()
	{
		if (this.isOnline && this.confServerOnline)
		{
			string postURL = "http://wttg.reflectstudios.com/stats.php";
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("action", "getPubConfessions");
			GameManager.GetTheHTTPPusher().submitPostText(postURL, dictionary, new Action<bool, WTTGJSONData>(this.respFromLiveConfessions));
		}
	}

	private void respFromLiveConfessions(bool pass, WTTGJSONData rData)
	{
		if (pass)
		{
			this.liveConfessions.Clear();
			string[] array = rData.data.Split(new char[]
			{
				"|"[0]
			});
			if (array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ConfessionDefinition confessionDefinition = (ConfessionDefinition)ScriptableObject.CreateInstance("ConfessionDefinition");
					confessionDefinition.confessionText = array[i];
					this.liveConfessions.Add(confessionDefinition);
				}
				this.haveLiveConfessions = true;
			}
		}
		else
		{
			GameManager.GetTheUIManager().displayMSGPopUp("No live confessions available");
		}
	}

	private void OnEnable()
	{
		this.tmpLocalConfessions = new List<ConfessionDefinition>();
		this.tmpLiveConfessions = new List<ConfessionDefinition>();
		this.newConfBTN.dontSetDefaultCursor = true;
		this.newConfBTN.tapAction = new Action(this.generateRandomConf);
		this.generateRandomConf();
		this.checkConnection();
		if (base.GetComponent<SiteHolder>().wasITapped())
		{
			this.iWasTapped();
		}
	}

	private void OnDisable()
	{
	}

	public Text confTextHolder;

	public LinkObject newConfBTN;

	public List<ConfessionDefinition> theConfessions;

	public List<ConfessionDefinition> liveConfessions;

	private bool isOnline = true;

	private bool confServerOnline = true;

	private bool haveLiveConfessions;

	private bool iHaveKey;

	private ConfessionDefinition confessKey;

	private List<ConfessionDefinition> tmpLocalConfessions;

	private List<ConfessionDefinition> tmpLiveConfessions;
}
