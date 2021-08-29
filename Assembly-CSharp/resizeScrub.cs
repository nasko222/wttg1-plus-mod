using System;
using UnityEngine;

public class resizeScrub : MonoBehaviour
{
	private void setSizeWH()
	{
		if (this.resizeToParent)
		{
			this.sizeWidth = this.resizeParent.GetComponent<RectTransform>().sizeDelta.x;
			this.sizeHeight = this.resizeParent.GetComponent<RectTransform>().sizeDelta.y;
		}
		else
		{
			this.sizeWidth = (float)Screen.width;
			this.sizeHeight = (float)Screen.height;
		}
	}

	private void resizeMe()
	{
		if (this.resizeWidth)
		{
			this.myRectTR.sizeDelta = new Vector3(Mathf.Round(this.sizeWidth * this.resizeWidthPerc), this.myRectTR.sizeDelta.y, 0f);
		}
		if (this.resizeHeight)
		{
			this.myRectTR.sizeDelta = new Vector3(this.myRectTR.sizeDelta.x, Mathf.Round(this.sizeHeight * this.resizeHeightPerc), 0f);
		}
	}

	private void Start()
	{
		this.myRectTR = base.GetComponent<RectTransform>();
		if (!this.waitToRun)
		{
			this.setSizeWH();
			this.resizeMe();
		}
		else
		{
			this.theTimeStamp = Time.time;
		}
	}

	private void Update()
	{
		if (this.waitToRun && Time.time - this.theTimeStamp >= this.waitTime)
		{
			this.waitToRun = false;
			this.setSizeWH();
			this.resizeMe();
		}
	}

	public bool waitToRun;

	[Range(0f, 2f)]
	public float waitTime = 0.5f;

	public bool resizeToParent;

	public GameObject resizeParent;

	public bool resizeWidth;

	public float resizeWidthPerc;

	public bool resizeHeight;

	public float resizeHeightPerc;

	private RectTransform myRectTR;

	private float theTimeStamp;

	private float sizeWidth;

	private float sizeHeight;
}
