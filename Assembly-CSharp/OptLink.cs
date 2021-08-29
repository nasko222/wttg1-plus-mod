using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void setActive()
	{
		this.isActive = true;
		this.myLink.color = this.activeColor;
	}

	public void setInactive()
	{
		this.isActive = false;
		this.myLink.color = this.defaultColor;
	}

	public void setAction(Action theAction)
	{
		this.myAction = theAction;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuHoverSound, 0.45f, false);
		this.myLink.color = this.hoverColor;
		this.myMenuManager.setHoverCursor();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.isActive)
		{
			this.myLink.color = this.activeColor;
		}
		else
		{
			this.myLink.color = this.defaultColor;
		}
		this.myMenuManager.setDefaultCursor();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuClickSound, 0.45f, false);
		this.myLink.color = this.defaultColor;
		this.myMenuManager.setDefaultCursor();
		this.myAction.DynamicInvoke(new object[0]);
	}

	public MenuManager myMenuManager;

	public bool isActive;

	public Color defaultColor;

	public Color activeColor;

	public Color hoverColor;

	public Text myLink;

	private Action myAction;
}
