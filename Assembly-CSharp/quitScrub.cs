using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class quitScrub : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.myLink.color = this.hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.myLink.color = this.defaultColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Application.Quit();
	}

	public Color defaultColor;

	public Color hoverColor;

	public Text myLink;
}
