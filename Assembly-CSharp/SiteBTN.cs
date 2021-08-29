using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SiteBTN : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void setMyAction(Action setAction)
	{
		this.myCallBackAction = setAction;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.GetTheUIManager().setHoverCursor();
		DOTween.To(() => this.buttonIMG.color, delegate(Color x)
		{
			this.buttonIMG.color = x;
		}, this.hoverStateBTNColor, 0.3f).SetEase(Ease.Linear);
		if (this.buttonText != null)
		{
			DOTween.To(() => this.buttonText.color, delegate(Color x)
			{
				this.buttonText.color = x;
			}, this.hoverStateTextColor, 0.3f).SetEase(Ease.Linear);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.GetTheUIManager().setDefaultCursor();
		DOTween.To(() => this.buttonIMG.color, delegate(Color x)
		{
			this.buttonIMG.color = x;
		}, this.defaultStateBTNColor, 0.3f).SetEase(Ease.Linear);
		if (this.buttonText != null)
		{
			DOTween.To(() => this.buttonText.color, delegate(Color x)
			{
				this.buttonText.color = x;
			}, this.defaultStateTextColor, 0.3f).SetEase(Ease.Linear);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.GetTheUIManager().setDefaultCursor();
		DOTween.To(() => this.buttonIMG.color, delegate(Color x)
		{
			this.buttonIMG.color = x;
		}, this.defaultStateBTNColor, 0.3f).SetEase(Ease.Linear);
		if (this.buttonText != null)
		{
			DOTween.To(() => this.buttonText.color, delegate(Color x)
			{
				this.buttonText.color = x;
			}, this.defaultStateTextColor, 0.3f).SetEase(Ease.Linear);
		}
		if (this.myCallBackAction != null)
		{
			this.myCallBackAction();
		}
	}

	public Image buttonIMG;

	public Text buttonText;

	public Color defaultStateBTNColor;

	public Color defaultStateTextColor;

	public Color hoverStateBTNColor;

	public Color hoverStateTextColor;

	private Action myCallBackAction;
}
