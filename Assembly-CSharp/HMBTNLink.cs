using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HMBTNLink : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void setMyAction(Action setAction)
	{
		this.myCallBackAction = setAction;
	}

	public void setMyBool(bool amITrue)
	{
		this.iAmTrue = amITrue;
		this.updateBoolStatus();
	}

	private void updateBoolStatus()
	{
		if (this.iAmTrue)
		{
			this.LinkText.text = this.TrueText;
			this.LinkText.color = this.TrueColor;
		}
		else
		{
			this.LinkText.text = this.FalseText;
			this.LinkText.color = this.FalseColor;
		}
	}

	private void tirggerBoolStatus()
	{
		if (this.iAmTrue)
		{
			this.iAmTrue = false;
		}
		else
		{
			this.iAmTrue = true;
		}
		this.updateBoolStatus();
	}

	private void resetFire()
	{
		this.iWasFired = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.iWasFired)
		{
			this.iWasFired = true;
			this.tirggerBoolStatus();
			this.myCallBackAction.DynamicInvoke(new object[0]);
			GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.resetFire));
		}
	}

	public bool iAmTrue = true;

	public Text LinkText;

	public string TrueText;

	public string FalseText;

	public Color TrueColor;

	public Color FalseColor;

	private bool iWasFired;

	private Action myCallBackAction;
}
