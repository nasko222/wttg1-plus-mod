using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class btnBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
		this.btnIMG = base.GetComponent<Image>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.btnIMG.sprite = this.hoverStateSprite;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.btnIMG.sprite = this.defaultStateSprite;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.btnIMG.sprite = this.clickStateSprite;
		if (this.hasAction)
		{
			this.setAction.DynamicInvoke(new object[0]);
		}
	}

	public Sprite defaultStateSprite;

	public Sprite hoverStateSprite;

	public Sprite clickStateSprite;

	public bool hasAction;

	public Action setAction;

	private Image btnIMG;
}
