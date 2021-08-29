using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void setEnable()
	{
		this.isDisabled = false;
		this.myLink.color = this.defaultColor;
	}

	public void setAction(Action theAction)
	{
		this.myAction = theAction;
	}

	private void Start()
	{
		if (this.isDisabled)
		{
			this.myLink.color = this.disabledColor;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.isDisabled)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuHoverSound, 0.45f, false);
			this.myLink.color = this.hoverColor;
			this.myMenuManager.setHoverCursor();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.isDisabled)
		{
			this.myLink.color = this.defaultColor;
			this.myMenuManager.setDefaultCursor();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.isDisabled)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuClickSound, 0.45f, false);
			this.myLink.color = this.defaultColor;
			this.myMenuManager.setDefaultCursor();
			this.myAction.DynamicInvoke(new object[0]);
		}
	}

	public MenuManager myMenuManager;

	public bool isDisabled;

	public Color defaultColor;

	public Color disabledColor;

	public Color hoverColor;

	public Text myLink;

	private Action myAction;
}
