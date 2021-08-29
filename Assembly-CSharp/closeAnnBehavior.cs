using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class closeAnnBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		this.myAB.closeMe();
	}

	public annBehavior myAB;
}
