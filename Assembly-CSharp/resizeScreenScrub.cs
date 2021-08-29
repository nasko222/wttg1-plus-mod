using System;
using UnityEngine;

public class resizeScreenScrub : MonoBehaviour
{
	private void resizeMe()
	{
		if (this.resizeWidthToScreen)
		{
			this.myRectTR.sizeDelta = new Vector2((float)Screen.width, this.myRectTR.sizeDelta.y);
		}
		if (this.resizeHeightToScreen)
		{
			this.myRectTR.sizeDelta = new Vector2(this.myRectTR.sizeDelta.x, (float)Screen.height);
		}
		if (this.resizeWidthToScreenPer)
		{
			float num = Mathf.Round(GameManager.MagicSlinger.getScreenWidthPXByPerc(this.screenWidthPer));
			if (this.clampWidth && num >= this.clampWidthPX)
			{
				num = this.clampWidthPX;
			}
			this.myRectTR.sizeDelta = new Vector2(num, this.myRectTR.sizeDelta.y);
		}
		if (this.resizeHeightToScreenPer)
		{
			float num2 = Mathf.Round(GameManager.MagicSlinger.getScreenHeightPXByPerc(this.screenHeightPer));
			if (this.clampHeight && num2 >= this.clampHeightPX)
			{
				num2 = this.clampHeightPX;
			}
			this.myRectTR.sizeDelta = new Vector2(this.myRectTR.sizeDelta.x, num2);
		}
	}

	private void Start()
	{
		this.myRectTR = base.GetComponent<RectTransform>();
		this.resizeMe();
	}

	public bool resizeWidthToScreen;

	public bool resizeHeightToScreen;

	public bool resizeWidthToScreenPer;

	public float screenWidthPer;

	public bool clampWidth;

	public float clampWidthPX;

	public bool resizeHeightToScreenPer;

	public float screenHeightPer;

	public bool clampHeight;

	public float clampHeightPX;

	private RectTransform myRectTR;
}
