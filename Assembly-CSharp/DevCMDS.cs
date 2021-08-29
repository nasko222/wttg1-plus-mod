using System;
using UnityEngine;

public class DevCMDS : MonoBehaviour
{
	public void runDevCommand(string theCommand)
	{
		int num = theCommand.IndexOf(" ");
		if (num != -1)
		{
			string a = theCommand.Substring(0, theCommand.IndexOf(" ", 0));
			string text = theCommand.Substring(theCommand.IndexOf(" ", 0) + 1);
			if (a == "!MSG")
			{
				this.displayMSG(text);
			}
			else if (a == "!TEXT")
			{
				this.sendTextDoc(text);
			}
			else if (a == "!SOUND")
			{
				this.forceSound(text);
			}
			else if (a == "!LIGHTS")
			{
				this.forceLights();
			}
			else if (a == "!BREATHER")
			{
				this.forceBreather();
			}
			else if (a == "!KIDNAPPER")
			{
				this.forceKidnapper();
			}
			else if (a == "!RESET")
			{
				this.myDOSTwitch.forceReset();
			}
		}
	}

	private void displayMSG(string theMSG)
	{
		GameManager.GetTheUIManager().displayMSGPopUp(theMSG);
	}

	private void sendTextDoc(string theText)
	{
		string txtDocTitle = theText.Substring(0, theText.IndexOf(" ", 0));
		string txtDocText = theText.Substring(theText.IndexOf(" ") + 1);
		GameManager.GetTheCloud().addTxtDoc(txtDocTitle, txtDocText);
	}

	private void forceSound(string theOption)
	{
		if (theOption == "ON")
		{
			this.myMuteBehavior.Force(true);
		}
		else
		{
			this.myMuteBehavior.Force(false);
		}
	}

	private void forceLights()
	{
		GameManager.GetTheSceneManager().ForceLights();
	}

	private void forceBreather()
	{
		GameManager.GetTheBreatherManager().releaseTheBreather();
	}

	private void forceKidnapper()
	{
		GameManager.GetTheTrackerManager().userCanNowBeTracked();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public DOSTwitch myDOSTwitch;

	public muteBehavior myMuteBehavior;
}
