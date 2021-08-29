using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class closeBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.myMainController.isUsingComputer)
		{
			this.parentWindow.gameObject.SetActive(false);
		}
	}

	public mainController myMainController;

	public Image parentWindow;
}
