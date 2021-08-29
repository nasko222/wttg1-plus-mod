using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class gmCasualHoverScrub : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.casualInfoText.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.casualInfoText.SetActive(false);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.casualInfoText.SetActive(false);
	}

	public GameObject casualInfoText;
}
