using System;
using UnityEngine;

public class reposScrub : MonoBehaviour
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

	private float getSizeWidthPXByPerc(float perValue)
	{
		return Mathf.Round(this.sizeWidth * perValue);
	}

	private float getSizeHeightPXByPerc(float perValue)
	{
		return Mathf.Round(this.sizeHeight * perValue);
	}

	private float getXPosWithMod(float deductWidth, float perValue)
	{
		float num3;
		if (this.resizeToParent)
		{
			resizeScreenScrub component = this.resizeParent.GetComponent<resizeScreenScrub>();
			if (component.resizeWidthToScreenPer)
			{
				float num = 1920f * component.screenWidthPer;
				float num2 = num * perValue;
				num2 = Mathf.Round(num2 + deductWidth);
				num2 = Mathf.Round(num - num2);
				return Mathf.Round(this.sizeWidth - num2 - deductWidth);
			}
			num3 = this.sizeWidth / 1920f;
		}
		else
		{
			num3 = this.sizeWidth / 1920f;
		}
		float num4 = this.sizeWidth * perValue;
		float num5 = deductWidth * num3;
		float num6 = deductWidth - num5;
		return Mathf.Round(num4 - num6);
	}

	private float getYPosWithMod(float deductHeight, float perValue)
	{
		float num3;
		if (this.resizeToParent)
		{
			resizeScreenScrub component = this.resizeParent.GetComponent<resizeScreenScrub>();
			if (component.resizeHeightToScreenPer)
			{
				float num = 1080f * component.screenHeightPer;
				float num2 = num * perValue;
				num2 = Mathf.Round(num2 + deductHeight);
				num2 = Mathf.Round(num - num2);
				return Mathf.Round(this.sizeHeight - num2 - deductHeight);
			}
			num3 = this.sizeHeight / 1080f;
		}
		else
		{
			num3 = this.sizeHeight / 1080f;
		}
		float num4 = this.sizeHeight * perValue;
		float num5 = deductHeight * num3;
		float num6 = deductHeight - num5;
		return Mathf.Round(num4 - num6);
	}

	private void rePosMe()
	{
		if (this.rePosX)
		{
			if (this.includeWidth)
			{
				this.myRectTR.localPosition = new Vector3(this.getXPosWithMod(this.myRectTR.sizeDelta.x, this.XPerValue), this.myRectTR.localPosition.y, 0f);
			}
			else
			{
				this.myRectTR.localPosition = new Vector3(this.getSizeWidthPXByPerc(this.XPerValue), this.myRectTR.localPosition.y, 0f);
			}
		}
		if (this.rePosY)
		{
			if (this.includeHeight)
			{
				this.myRectTR.localPosition = new Vector3(this.myRectTR.localPosition.x, -this.getYPosWithMod(this.myRectTR.sizeDelta.y, this.YPerValue), 0f);
			}
			else
			{
				this.myRectTR.localPosition = new Vector3(this.myRectTR.localPosition.x, -this.getSizeHeightPXByPerc(this.YPerValue), 0f);
			}
		}
	}

	private void Start()
	{
		this.myRectTR = base.GetComponent<RectTransform>();
		if (!this.waitToRun)
		{
			this.setSizeWH();
			this.rePosMe();
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
			this.rePosMe();
		}
	}

	public bool waitToRun;

	[Range(0f, 2f)]
	public float waitTime = 0.5f;

	public bool resizeToParent;

	public GameObject resizeParent;

	public bool rePosX;

	public bool includeWidth;

	public float XPerValue;

	public bool rePosY;

	public bool includeHeight;

	public float YPerValue;

	private RectTransform myRectTR;

	private float theTimeStamp;

	private float sizeWidth;

	private float sizeHeight;
}
