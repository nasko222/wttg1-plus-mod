using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class notesIconBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
		this.clickCount = 0f;
		this.myTimeStamp = Time.time;
	}

	private void Update()
	{
		if (Time.time - this.myTimeStamp >= 1f)
		{
			this.clickCount = 0f;
			this.myTimeStamp = Time.time;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.clickCount >= 1f)
		{
			this.notesWindow.SetActive(true);
			this.clickCount = 0f;
		}
		this.clickCount += 1f;
	}

	public GameObject notesWindow;

	private float clickCount;

	private float myTimeStamp;
}
