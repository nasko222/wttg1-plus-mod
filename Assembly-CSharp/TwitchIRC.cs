using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class TwitchIRC : MonoBehaviour
{
	public void StartTwitch(string setOAuth, string setNickName)
	{
		this.oauth = setOAuth;
		this.nickName = setNickName;
		this.channelName = setNickName;
		this.StartIRC();
	}

	private void StartIRC()
	{
		TcpClient tcpClient = new TcpClient();
		tcpClient.Connect(this.server, this.port);
		if (!tcpClient.Connected)
		{
			return;
		}
		this.isConnected = true;
		NetworkStream networkStream = tcpClient.GetStream();
		StreamReader input = new StreamReader(networkStream);
		StreamWriter output = new StreamWriter(networkStream);
		output.WriteLine("PASS " + this.oauth);
		output.WriteLine("NICK " + this.nickName.ToLower());
		output.Flush();
		this.outProc = new Thread(delegate()
		{
			this.IRCOutputProcedure(output);
		});
		this.outProc.Start();
		this.inProc = new Thread(delegate()
		{
			this.IRCInputProcedure(input, networkStream);
		});
		this.inProc.Start();
	}

	private void IRCInputProcedure(TextReader input, NetworkStream networkStream)
	{
		while (!this.stopThreads)
		{
			if (networkStream.DataAvailable)
			{
				this.buffer = input.ReadLine();
				if (this.buffer.Contains("PRIVMSG #"))
				{
					object obj = this.recievedMsgs;
					lock (obj)
					{
						this.recievedMsgs.Add(this.buffer);
					}
				}
				if (this.buffer.StartsWith("PING "))
				{
					this.SendCommand(this.buffer.Replace("PING", "PONG"));
				}
				if (this.buffer.Split(new char[]
				{
					' '
				})[1] == "001")
				{
					this.SendCommand("JOIN #" + this.channelName);
				}
			}
		}
	}

	private void IRCOutputProcedure(TextWriter output)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		while (!this.stopThreads)
		{
			object obj = this.commandQueue;
			lock (obj)
			{
				if (this.commandQueue.Count > 0 && stopwatch.ElapsedMilliseconds > 1750L)
				{
					output.WriteLine(this.commandQueue.Peek());
					output.Flush();
					this.commandQueue.Dequeue();
					stopwatch.Reset();
					stopwatch.Start();
				}
			}
		}
	}

	public void SendCommand(string cmd)
	{
		object obj = this.commandQueue;
		lock (obj)
		{
			this.commandQueue.Enqueue(cmd);
		}
	}

	public void SendMsg(string msg)
	{
		object obj = this.commandQueue;
		lock (obj)
		{
			this.commandQueue.Enqueue("PRIVMSG #" + this.channelName + " :" + msg);
		}
	}

	public void SendMsg(string msg, float delay)
	{
		GameManager.TimeSlinger.FireStringTimer(delay, new Action<string>(this.SendMsg), msg);
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		this.stopThreads = false;
	}

	private void OnDisable()
	{
		this.stopThreads = true;
	}

	private void OnDestroy()
	{
		this.stopThreads = true;
	}

	private void Update()
	{
		object obj = this.recievedMsgs;
		lock (obj)
		{
			if (this.recievedMsgs.Count > 0)
			{
				for (int i = 0; i < this.recievedMsgs.Count; i++)
				{
					this.messageRecievedEvent.Invoke(this.recievedMsgs[i]);
				}
				this.recievedMsgs.Clear();
			}
		}
	}

	public string oauth;

	public string nickName;

	public string channelName;

	public bool isConnected;

	private string server = "irc.chat.twitch.tv";

	private int port = 6667;

	public TwitchIRC.MsgEvent messageRecievedEvent = new TwitchIRC.MsgEvent();

	private string buffer = string.Empty;

	private bool stopThreads;

	private Queue<string> commandQueue = new Queue<string>();

	private List<string> recievedMsgs = new List<string>();

	private Thread inProc;

	private Thread outProc;

	public class MsgEvent : UnityEvent<string>
	{
	}
}
