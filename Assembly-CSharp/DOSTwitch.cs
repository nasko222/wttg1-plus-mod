using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(TwitchIRC))]
[RequireComponent(typeof(HackPoll))]
[RequireComponent(typeof(KeyPoll))]
[RequireComponent(typeof(SiteSpeedPoll))]
[RequireComponent(typeof(KeyQue))]
[RequireComponent(typeof(ModemTimePoll))]
[RequireComponent(typeof(SpookPoll))]
[RequireComponent(typeof(DevCMDS))]
public class DOSTwitch : MonoBehaviour
{
	public void PresentTwitchHacker(string hackerName)
	{
		this.TwitchHackerDisplayName.text = hackerName.ToUpper();
		GameManager.TimeSlinger.FireTimer(12f, new Action(this.animateDisplayTwitchHacker));
	}

	public void DismissTwitchHacker()
	{
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.TwitchHackerDisplayGroup.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TwitchHackerDisplayGroup.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.35f), 2);
	}

	public void setPollInactive()
	{
		this.poolActive = false;
	}

	public void forceReset()
	{
		GameManager.TimeSlinger.KillTimerWithID("WTTGTwitchHPRR");
		GameManager.TimeSlinger.KillTimerWithID("WTTGTwitchSPRR");
		GameManager.TimeSlinger.KillTimerWithID("WTTGTwitchKQPRR");
		GameManager.TimeSlinger.KillTimerWithID("WTTGTwitchMTPRR");
		GameManager.TimeSlinger.KillTimerWithID("WTTGTwitchSPRR");
		this.generateHackerPollWindow();
		this.generateSpeedPollWindow();
		this.generateKeyQuePollWindow();
		this.generateModemTimePollWindow();
		this.generateSpookPoolWindow();
		this.poolActive = false;
	}

	private void prepDOSTwitch()
	{
		this.myTwitchIRC = base.GetComponent<TwitchIRC>();
		TwitchData twitchData = new TwitchData();
		if (GameManager.FileSlinger.wildLoadFile<TwitchData>("twitchData.gd", out twitchData) && twitchData.isActive)
		{
			this.myTwitchIRC.StartTwitch(twitchData.twitchOAuth, twitchData.twitchUserName);
			GameManager.TimeSlinger.FireTimer(5f, new Action(this.warmDOSTwitch));
		}
	}

	private void warmDOSTwitch()
	{
		this.myHackPoll = base.GetComponent<HackPoll>();
		this.myKeyPoll = base.GetComponent<KeyPoll>();
		this.mySiteSpeedPoll = base.GetComponent<SiteSpeedPoll>();
		this.myKeyQue = base.GetComponent<KeyQue>();
		this.myModemTimePoll = base.GetComponent<ModemTimePoll>();
		this.mySpookPoll = base.GetComponent<SpookPoll>();
		this.myDevCMDS = base.GetComponent<DevCMDS>();
		this.myHackPoll.myDOSTwitch = this;
		this.myKeyPoll.myDOSTwitch = this;
		this.mySiteSpeedPoll.myDOSTwitch = this;
		this.myKeyQue.myDOSTwitch = this;
		this.myModemTimePoll.myDOSTwitch = this;
		this.mySpookPoll.myDOSTwitch = this;
		this.myDevCMDS.myDOSTwitch = this;
		if (this.myTwitchIRC.isConnected)
		{
			this.amConnected = true;
			this.displayTwitchConnected();
			this.prepPolls();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(30f, new Action(this.checkCon));
		}
	}

	private void checkCon()
	{
		if (this.myTwitchIRC.isConnected)
		{
			this.amConnected = true;
			this.displayTwitchConnected();
		}
		else
		{
			this.conCount += 1;
			if (this.conCount < 5)
			{
				GameManager.TimeSlinger.FireTimer(30f, new Action(this.checkCon));
			}
			else
			{
				GameManager.GetTheUIManager().displayMSGPopUp("Could not connect to Twitch. FeelsBadMan");
			}
		}
	}

	private void chatMessageRecv(string theMSG)
	{
		string[] array = theMSG.Split(new string[]
		{
			"PRIVMSG"
		}, StringSplitOptions.None);
		string[] array2 = array[0].Split(new string[]
		{
			"!"
		}, StringSplitOptions.None);
		string[] array3 = array[1].Split(new string[]
		{
			":"
		}, StringSplitOptions.None);
		string text = array2[0].Replace(":", string.Empty);
		string text2 = array3[1];
		string theCommand = text2;
		text2 = text2.ToUpper();
		if (this.poolActive)
		{
			this.currentPollAction(text, text2);
		}
		if (text == "reflectstudios")
		{
			this.myDevCMDS.runDevCommand(theCommand);
		}
	}

	private void displayTwitchConnected()
	{
		this.myTwitchIRC.messageRecievedEvent.AddListener(new UnityAction<string>(this.chatMessageRecv));
		GameManager.GetTheUIManager().displayMSGPopUp("Twitch Integration Now Live! FeelsGoodMan");
		this.myTwitchIRC.SendMsg("Welcome to the Game - Twitch Integration Is Now Live!");
	}

	private void prepPolls()
	{
		this.generateHackerPollWindow();
		this.generateKeyPollWindow();
		this.generateSpeedPollWindow();
		this.generateKeyQuePollWindow();
		this.generateModemTimePollWindow();
		this.generateSpookPoolWindow();
	}

	private void generateHackerPollWindow()
	{
		this.hackerPollTimeWindow = Random.Range(this.hackerPollMinWindow, this.hackerPollMaxWindow);
		this.hackerPollTimeStamp = Time.time;
		this.hackerPollWindowActive = true;
	}

	private void generateKeyPollWindow()
	{
		this.keyPollTimeWindow = Random.Range(this.keyPollMinWindow, this.keyPollMaxWindow);
		this.keyPollTimeStamp = Time.time;
		this.keyPollWindowActive = true;
	}

	private void generateSpeedPollWindow()
	{
		this.speedPollTimeWindow = Random.Range(this.speedPollMinWindow, this.speedPollMaxWindow);
		this.speedPollTimeStamp = Time.time;
		this.speedPollWindowActive = true;
	}

	private void generateKeyQuePollWindow()
	{
		this.keyQuePollTimeWindow = Random.Range(this.keyQueMinWindow, this.keyQueMaxWindow);
		this.keyQuePollTimeStamp = Time.time;
		this.keyQuePollWindowActive = true;
	}

	private void generateModemTimePollWindow()
	{
		this.modemTimePollWindow = Random.Range(this.modemTimePollMinWindow, this.modemTimePollMaxWindow);
		this.modemTimePollTimeStamp = Time.time;
		this.modemTimePollWindowActive = true;
	}

	private void generateSpookPoolWindow()
	{
		this.spookPollWindow = Random.Range(this.spookPollMinWindow, this.spookPollMaxWindow);
		this.spookPollTimeStamp = Time.time;
		this.spookPollWindowActive = true;
	}

	private void triggerHackPoll()
	{
		if (!this.poolActive)
		{
			this.currentPollAction = new Action<string, string>(this.myHackPoll.CastVote);
			this.poolActive = true;
			this.myHackPoll.BeginVote();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(45f, 128f), new Action(this.triggerHackPoll), "WTTGTwitchHPRR");
		}
	}

	private void triggerKeyPoll()
	{
		if (!this.poolActive)
		{
			this.currentPollAction = new Action<string, string>(this.myKeyPoll.CastVote);
			this.poolActive = true;
			this.myKeyPoll.BeginVote();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(12f, 350f), new Action(this.triggerKeyPoll));
		}
	}

	private void triggerSpeedPoll()
	{
		if (!this.poolActive)
		{
			this.currentPollAction = new Action<string, string>(this.mySiteSpeedPoll.CastVote);
			this.poolActive = true;
			this.mySiteSpeedPoll.BeginVote();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(45f, 300f), new Action(this.triggerSpeedPoll), "WTTGTwitchSPRR");
		}
	}

	private void triggerKeyQuePoll()
	{
		if (!this.poolActive)
		{
			this.currentPollAction = new Action<string, string>(this.myKeyQue.CastVote);
			this.poolActive = true;
			this.myKeyQue.BeginVote();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(50f, 500f), new Action(this.triggerKeyQuePoll), "WTTGTwitchKQPRR");
		}
	}

	private void triggerModemTimePoll()
	{
		if (!this.poolActive)
		{
			this.currentPollAction = new Action<string, string>(this.myModemTimePoll.CastVote);
			this.poolActive = true;
			this.myModemTimePoll.BeginVote();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(35f, 75f), new Action(this.triggerModemTimePoll), "WTTGTwitchMTPRR");
		}
	}

	private void triggerSpookPool()
	{
		if (!this.poolActive)
		{
			this.currentPollAction = new Action<string, string>(this.mySpookPoll.CastVote);
			this.poolActive = true;
			this.mySpookPoll.BeginVote();
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(Random.Range(18f, 120f), new Action(this.triggerSpookPool), "WTTGTwitchSPRR");
		}
	}

	private void animateDisplayTwitchHacker()
	{
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.TwitchHackerDisplayGroup.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.TwitchHackerDisplayGroup.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 3);
	}

	private void Awake()
	{
		GameManager.SetDOSTwitch(this);
	}

	private void Start()
	{
		this.conCount = 0;
		if (!GameManager.GetGameModeManager().getCasualMode())
		{
			GameManager.TimeSlinger.FireTimer(5f, new Action(this.prepDOSTwitch));
		}
	}

	private void Update()
	{
		if (this.hackerPollWindowActive && Time.time - this.hackerPollTimeStamp >= this.hackerPollTimeWindow)
		{
			this.hackerPollWindowActive = false;
			this.triggerHackPoll();
			this.generateHackerPollWindow();
		}
		if (this.keyPollWindowActive && Time.time - this.keyPollTimeStamp >= this.keyPollTimeWindow)
		{
			this.keyPollWindowActive = false;
			this.triggerKeyPoll();
		}
		if (this.speedPollWindowActive && Time.time - this.speedPollTimeStamp >= this.speedPollTimeWindow)
		{
			this.speedPollWindowActive = false;
			this.triggerSpeedPoll();
			this.generateSpeedPollWindow();
		}
		if (this.keyQuePollWindowActive && Time.time - this.keyQuePollTimeStamp >= this.keyQuePollTimeWindow)
		{
			this.keyQuePollWindowActive = false;
			this.triggerKeyQuePoll();
			this.generateKeyQuePollWindow();
		}
		if (this.modemTimePollWindowActive && Time.time - this.modemTimePollTimeStamp >= this.modemTimePollWindow)
		{
			this.modemTimePollWindowActive = false;
			this.triggerModemTimePoll();
			this.generateModemTimePollWindow();
		}
		if (this.spookPollWindowActive && Time.time - this.spookPollTimeStamp >= this.spookPollWindow)
		{
			this.spookPollWindowActive = false;
			this.triggerSpookPool();
			this.generateSpookPoolWindow();
		}
	}

	public GameObject TwitchHackerDisplayGroup;

	public Text TwitchHackerDisplayName;

	public TwitchIRC myTwitchIRC;

	public float hackerPollMinWindow = 120f;

	public float hackerPollMaxWindow = 900f;

	public float keyPollMinWindow = 120f;

	public float keyPollMaxWindow = 300f;

	public float speedPollMinWindow = 300f;

	public float speedPollMaxWindow = 600f;

	public float keyQueMinWindow = 240f;

	public float keyQueMaxWindow = 720f;

	public float modemTimePollMinWindow = 380f;

	public float modemTimePollMaxWindow = 720f;

	public float spookPollMinWindow = 360f;

	public float spookPollMaxWindow = 780f;

	private HackPoll myHackPoll;

	private KeyPoll myKeyPoll;

	private SiteSpeedPoll mySiteSpeedPoll;

	private KeyQue myKeyQue;

	private ModemTimePoll myModemTimePoll;

	private SpookPoll mySpookPoll;

	private DevCMDS myDevCMDS;

	private bool amConnected;

	private bool poolActive;

	private short conCount;

	private bool hackerPollWindowActive;

	private bool keyPollWindowActive;

	private bool speedPollWindowActive;

	private bool keyQuePollWindowActive;

	private bool modemTimePollWindowActive;

	private bool spookPollWindowActive;

	private float hackerPollTimeWindow;

	private float keyPollTimeWindow;

	private float speedPollTimeWindow;

	private float keyQuePollTimeWindow;

	private float modemTimePollWindow;

	private float spookPollWindow;

	private float hackerPollTimeStamp;

	private float keyPollTimeStamp;

	private float speedPollTimeStamp;

	private float keyQuePollTimeStamp;

	private float modemTimePollTimeStamp;

	private float spookPollTimeStamp;

	private Action<string, string> currentPollAction;
}
