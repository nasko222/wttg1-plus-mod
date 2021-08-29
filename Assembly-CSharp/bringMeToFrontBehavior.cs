using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class bringMeToFrontBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Transform child = this.parentTrans.GetChild(this.parentTrans.childCount - 1);
		child.SetSiblingIndex(0);
		base.transform.SetSiblingIndex(this.parentTrans.childCount);
	}

	public Transform parentTrans;
}
