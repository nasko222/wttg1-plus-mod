using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class windowBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.myMainController.isUsingComputer)
		{
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.myMainController.isUsingComputer)
		{
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.myMainController.isUsingComputer)
		{
		}
	}

	public mainController myMainController;

	public Image dragBar;
}
