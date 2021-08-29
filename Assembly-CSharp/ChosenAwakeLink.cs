using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChosenAwakeLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void showPiece()
	{
		Color color = base.GetComponent<Image>().color;
		this.aniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.aniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => base.GetComponent<Image>().color, delegate(Color x)
		{
			base.GetComponent<Image>().color = x;
		}, new Color(color.r, color.g, color.b, 1f), 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.aniSeq);
	}

	public void hidePiece()
	{
		base.GetComponent<Image>().color = new Color(base.GetComponent<Image>().color.r, base.GetComponent<Image>().color.g, base.GetComponent<Image>().color.b, 0f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.noLongerActive)
		{
			this.myUIManager.setHoverCursor();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.myUIManager.setDefaultCursor();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.noLongerActive)
		{
			this.myUIManager.setDefaultCursor();
			if (this.iWasTapped)
			{
				this.showPiece();
				this.goodAction.DynamicInvoke(new object[0]);
			}
			else
			{
				this.badAction.DynamicInvoke(new object[0]);
			}
		}
	}

	public UIManager myUIManager;

	public bool iWasTapped;

	public bool noLongerActive;

	public Action goodAction;

	public Action badAction;

	private Sequence aniSeq;
}
