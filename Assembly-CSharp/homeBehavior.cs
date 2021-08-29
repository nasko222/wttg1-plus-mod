using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class homeBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
		this.timeStamp = Time.time;
		this.waitTime = 1f;
		this.checkTime = true;
	}

	private void Update()
	{
		if (this.checkTime && Time.time - this.timeStamp >= this.waitTime)
		{
			this.wikiURL = "http://" + this.myCloud.getWikiLink(0) + ".ann";
			this.checkTime = false;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.IAmLocked && !this.checkTime && this.myMainController.isUsingComputer)
		{
			this.myAB.homeBtnWasHit();
			this.myAB.forceLoadURL(this.wikiURL, true);
		}
	}

	public mainController myMainController;

	public TheCloud myCloud;

	public annBehavior myAB;

	public bool IAmLocked;

	private string wikiURL;

	private float timeStamp;

	private float waitTime;

	private bool checkTime;
}
