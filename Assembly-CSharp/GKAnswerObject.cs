using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GKAnswerObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void resizeMe()
	{
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.myUIManager.setHoverCursor();
		base.GetComponent<Image>().color = this.highlightColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.myUIManager.setDefaultCursor();
		base.GetComponent<Image>().color = this.defaultColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.myUIManager.setDefaultCursor();
		base.GetComponent<Image>().color = this.defaultColor;
		if (this.theRightAnswer)
		{
			this.rightAction.DynamicInvoke(new object[0]);
		}
		else
		{
			this.wrongAction.DynamicInvoke(new object[0]);
		}
	}

	public UIManager myUIManager;

	public Text answerText;

	public bool theRightAnswer;

	public Color defaultColor;

	public Color highlightColor;

	public Action rightAction;

	public Action wrongAction;
}
