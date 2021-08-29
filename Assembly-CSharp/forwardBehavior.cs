using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class forwardBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void setEnabled()
	{
		if (!this.isEnabled)
		{
			this.btnIMG.color = new Color(this.btnIMG.color.r, this.btnIMG.color.g, this.btnIMG.color.b, 1f);
			this.isEnabled = true;
		}
	}

	public void setDisabled()
	{
		if (this.isEnabled)
		{
			this.btnIMG.color = new Color(this.btnIMG.color.r, this.btnIMG.color.g, this.btnIMG.color.b, 0.5f);
			this.isEnabled = false;
		}
	}

	private void Start()
	{
		this.isEnabled = true;
		this.btnIMG = base.GetComponent<Image>();
		this.setDisabled();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.isEnabled)
		{
			this.btnIMG.sprite = this.hoverStateSprite;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.isEnabled)
		{
			this.btnIMG.sprite = this.defaultStateSprite;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.iAmLocked && this.isEnabled)
		{
			this.myAB.goForward();
			this.btnIMG.sprite = this.defaultStateSprite;
		}
	}

	public annBehavior myAB;

	public Sprite defaultStateSprite;

	public Sprite hoverStateSprite;

	public Sprite clickStateSprite;

	public bool iAmLocked;

	private Image btnIMG;

	private bool isEnabled;
}
