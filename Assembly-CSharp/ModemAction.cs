using System;
using DG.Tweening;
using UnityEngine;

public class ModemAction : MonoBehaviour
{
	public void runAction()
	{
		if (!GameManager.GetTheSceneManager().isInDoorAction() && GameManager.GetTheSceneManager().areTheLightsOn() && !this.inAction && !this.actionShown)
		{
			this.showAction();
		}
	}

	public void clearAction()
	{
		if (!this.inAction && this.actionShown)
		{
			this.hideAction();
		}
	}

	public void performAction()
	{
		if (!GameManager.GetTheSceneManager().isInDoorAction() && GameManager.GetTheSceneManager().areTheLightsOn() && !this.inAction)
		{
			this.mainController.masterLock = true;
			this.mainController.getMyActionController().lockAction = true;
			this.inAction = true;
			this.hideAction();
			this.resetTheModem();
		}
	}

	public void setModemTime(bool increase)
	{
		if (increase)
		{
			this.resetModemTime = 40f;
		}
		else
		{
			this.resetModemTime = 7f;
		}
		GameManager.TimeSlinger.FireTimer(300f, new Action(this.resetResetModemTime));
	}

	private void resetTheModem()
	{
		this.myUIManager.triggerResetModem(this.resetModemTime);
		this.modemObject.GetComponent<Renderer>().material = this.modemResetMat;
		this.curResetModemEmiColor = this.resetModemStartEmi;
		this.curResetModemAddIt = true;
		this.aniResetModemEmi = true;
		this.curResetModemTimeStamp = Time.time;
		GameManager.TimeSlinger.FireTimer(this.resetModemTime, new Action(this.endResetModem));
		GameManager.GetTheHackerManager().MaskTheIP();
	}

	private void endResetModem()
	{
		this.aniResetModemEmi = false;
		this.modemObject.GetComponent<Renderer>().material = this.modemDefaultMat;
		this.mainController.masterLock = false;
		this.mainController.getMyActionController().lockAction = false;
		this.inAction = false;
	}

	private void showAction()
	{
		this.actionShown = true;
		this.myUIManager.displayActionText("Reset Modem");
	}

	private void hideAction()
	{
		this.myUIManager.hideActionText();
		this.actionShown = false;
	}

	private void resetResetModemTime()
	{
		this.resetModemTime = 20f;
	}

	private void Awake()
	{
		GameManager.SetModemAction(this);
	}

	private void Update()
	{
		if (this.aniResetModemEmi && Time.time - this.curResetModemTimeStamp >= this.curResetModemWaitTime)
		{
			if (this.curResetModemAddIt)
			{
				this.curResetModemEmiColor += this.curResetModemModValue;
				if (this.curResetModemEmiColor >= this.resetModemEndEmi)
				{
					this.curResetModemAddIt = false;
				}
			}
			else
			{
				this.curResetModemEmiColor -= this.curResetModemModValue;
				if (this.curResetModemEmiColor <= this.resetModemStartEmi)
				{
					this.curResetModemAddIt = true;
				}
			}
			this.modemObject.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", new Color(this.curResetModemEmiColor, this.curResetModemEmiColor, this.curResetModemEmiColor, this.curResetModemEmiColor));
			this.curResetModemTimeStamp = Time.time;
		}
	}

	[Range(2f, 60f)]
	public float resetModemTime = 10f;

	[Range(0.2f, 2f)]
	public float resetModemStartEmi = 0.8f;

	[Range(0.5f, 2.5f)]
	public float resetModemEndEmi = 1.2f;

	public UIManager myUIManager;

	public mainController mainController;

	public GameObject modemObject;

	public Material modemDefaultMat;

	public Material modemResetMat;

	private bool actionShown;

	private bool aniResetModemEmi;

	private bool curResetModemAddIt;

	private bool inAction;

	private Sequence rmSeq;

	private float curResetModemEmiColor;

	private float curResetModemWaitTime = 0.1f;

	private float curResetModemModValue = 0.1f;

	private float curResetModemTimeStamp;
}
