using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TheWaitingRoomLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	private void BuildAppPath()
	{
		string[] array = Application.dataPath.Split(new string[]
		{
			"/"
		}, StringSplitOptions.None);
		string str = string.Empty;
		for (int i = 0; i < array.Length - 1; i++)
		{
			str = str + array[i] + "/";
		}
		this.currentAppPath = str;
	}

	private void Start()
	{
		this.BuildAppPath();
		this.hasTheWaitingRoom = GameManager.SteamSlinger.triggerCheckForTheWaitingRoom();
		if (this.hasTheWaitingRoom)
		{
			this.GetText.SetActive(false);
			this.PlayText.SetActive(true);
		}
		else
		{
			this.GetText.SetActive(true);
			this.PlayText.SetActive(false);
		}
	}

	private void Update()
	{
		if (this.isInstalling && Time.time - this.installTimeStamp >= 5f)
		{
			if (File.Exists(this.currentAppPath + "TheWaitingRoom/TWR.exe"))
			{
				this.isInstalling = false;
				this.hasTheWaitingRoom = true;
				this.InstallText.SetActive(false);
				this.PlayText.SetActive(true);
			}
			this.installTimeStamp = Time.time;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.isInstalling)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuHoverSound, 0.45f, false);
			this.myMenuManager.setHoverCursor();
			if (this.hasTheWaitingRoom)
			{
				this.PlayText.GetComponent<Text>().color = this.HighlightColor;
			}
			else
			{
				this.GetText.GetComponent<Text>().color = this.HighlightColor;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.isInstalling)
		{
			this.myMenuManager.setDefaultCursor();
			if (this.hasTheWaitingRoom)
			{
				this.PlayText.GetComponent<Text>().color = this.DefaultColor;
			}
			else
			{
				this.GetText.GetComponent<Text>().color = this.DefaultColor;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.isInstalling)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuClickSound, 0.45f, false);
			this.myMenuManager.setDefaultCursor();
			if (this.hasTheWaitingRoom)
			{
				this.PlayText.GetComponent<Text>().color = this.DefaultColor;
				if (this.currentAppPath.Length > 0)
				{
					Process.Start(this.currentAppPath + "TheWaitingRoom/TWR.exe");
					Application.Quit();
				}
			}
			else
			{
				this.GetText.GetComponent<Text>().color = this.DefaultColor;
				GameManager.SteamSlinger.InstallTheWaitingRoom();
				this.isInstalling = true;
				this.GetText.SetActive(false);
				this.PlayText.SetActive(false);
				this.InstallText.SetActive(true);
				this.installTimeStamp = Time.time;
			}
		}
	}

	public MenuManager myMenuManager;

	public GameObject GetText;

	public GameObject PlayText;

	public GameObject InstallText;

	public Color DefaultColor;

	public Color HighlightColor;

	private bool hasTheWaitingRoom;

	private bool isInstalling;

	private float installTimeStamp;

	private string currentAppPath;
}
