using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShutDownIconBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		this.myUIManger.showShutDown();
	}

	public UIManager myUIManger;
}
