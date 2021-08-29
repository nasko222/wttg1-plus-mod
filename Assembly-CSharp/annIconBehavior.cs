using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class annIconBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
		this.clickCount = 0f;
		this.myTimeStamp = Time.time;
		this.myIB = base.GetComponent<iconBehavior>();
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
			this.myIB.clearActiveState();
			this.annWindow.SetActive(true);
			this.clickCount = 0f;
		}
		this.clickCount += 1f;
	}

	public GameObject annWindow;

	private iconBehavior myIB;

	private float clickCount;

	private float myTimeStamp;
}
