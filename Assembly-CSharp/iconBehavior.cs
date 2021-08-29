using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class iconBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void clearActiveState()
	{
		this.iconIMG.sprite = this.defaultState;
	}

	private void Start()
	{
		this.iconIMG = base.GetComponent<Image>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.iconIMG.sprite = this.activeState;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.clearActiveState();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.iconIMG.sprite = this.activeState;
	}

	public Sprite defaultState;

	public Sprite activeState;

	private Image iconIMG;
}
