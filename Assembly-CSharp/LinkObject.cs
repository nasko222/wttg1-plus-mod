using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.deadUnlessTapped && !this.hideTapAction)
		{
			this.myUIManager.setHoverCursor();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.deadUnlessTapped)
		{
			this.myUIManager.setDefaultCursor();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.deadUnlessTapped)
		{
			if (!this.dontSetDefaultCursor)
			{
				this.myUIManager.setDefaultCursor();
			}
			if (this.hasTapAction)
			{
				this.tapAction.DynamicInvoke(new object[0]);
			}
			else if (this.hasIntTapAction)
			{
				this.intTapAction.DynamicInvoke(new object[]
				{
					this.intTapValue
				});
			}
			else if (this.goesToSubPage)
			{
				if (base.gameObject.GetComponentInParent<SubPageHolder>())
				{
					GameManager.GetTheAnnBehavior().forceLoadURL("http://" + base.gameObject.GetComponentInParent<SubPageHolder>().mainSiteDefinition.PageURL + ".ann/" + this.subPageURL, false);
				}
				else
				{
					GameManager.GetTheAnnBehavior().forceLoadURL("http://" + base.gameObject.GetComponentInParent<SiteHolder>().mySiteDeff.PageURL + ".ann/" + this.subPageURL, false);
				}
			}
			else if (this.goesToMainSite)
			{
				GameManager.GetTheAnnBehavior().forceLoadURL("http://" + this.mainSiteDef.PageURL + ".ann", false);
			}
			else
			{
				int num = Random.Range(0, 20);
				if (num == 2)
				{
					GameManager.GetTheHackerManager().launchHack();
				}
				else
				{
					string pageURL = base.gameObject.GetComponentInParent<SiteHolder>().mySiteDeff.PageURL;
					int startIndex = Random.Range(0, pageURL.Length - 6);
					string str = pageURL.Substring(startIndex, 6) + ".html";
					string theURL = "http://" + pageURL + ".ann/" + str;
					GameManager.GetTheAnnBehavior().forceLoadURL(theURL, false);
				}
			}
		}
	}

	public UIManager myUIManager;

	public bool dontSetDefaultCursor;

	public bool deadUnlessTapped;

	public bool hideTapAction;

	public bool hasTapAction;

	public bool hasIntTapAction;

	public bool goesToSubPage;

	public string subPageURL;

	public bool goesToMainSite;

	public SiteDefinition mainSiteDef;

	public int intTapValue;

	public Action tapAction;

	public Action<int> intTapAction;
}
