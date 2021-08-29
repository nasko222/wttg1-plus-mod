using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class dragBehavior : MonoBehaviour, IBeginDragHandler, IDragHandler, IEventSystemHandler
{
	private void SetDragPos(PointerEventData data)
	{
		bool flag = false;
		if (Input.mousePosition.x < GameManager.MagicSlinger.getScreenWidthPXByPerc(0.01f))
		{
			flag = true;
		}
		else if (Input.mousePosition.x > GameManager.MagicSlinger.getScreenWidthPXByPerc(0.98f))
		{
			flag = true;
		}
		else if (Input.mousePosition.y < GameManager.MagicSlinger.getScreenHeightPXByPerc(0.02f))
		{
			flag = true;
		}
		else if (Input.mousePosition.y > GameManager.MagicSlinger.getScreenHeightPXByPerc(0.95f))
		{
			flag = true;
		}
		Vector3 vector;
		if (!flag && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, data.position, data.pressEventCamera, out vector))
		{
			vector -= this.bufferPos;
			this.parentWindow.rectTransform.position = vector;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Vector3 a;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.dragPlane, eventData.position, eventData.pressEventCamera, out a))
		{
			this.bufferPos = a - this.parentWindow.rectTransform.position;
		}
		this.SetDragPos(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.SetDragPos(eventData);
	}

	public mainController myMainController;

	public Image parentWindow;

	public RectTransform dragPlane;

	private Vector3 bufferPos;
}
