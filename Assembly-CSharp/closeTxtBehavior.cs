using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class closeTxtBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		Object.Destroy(this.parentWindow);
	}

	public GameObject parentWindow;
}
