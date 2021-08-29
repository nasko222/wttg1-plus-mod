using System;
using UnityEngine;

public class SiteTapper : MonoBehaviour
{
	private void iWasTapped()
	{
		int setIndex = 0;
		string empty = string.Empty;
		if (this.tapSubPages)
		{
			int num;
			if (GameManager.FileSlinger.saveData.siteDataHex.ContainsKey(base.GetComponent<SiteHolder>().mySiteDeff.PageURL))
			{
				num = GameManager.FileSlinger.saveData.siteDataHex[base.GetComponent<SiteHolder>().mySiteDeff.PageURL];
				if (num == -1)
				{
					base.GetComponent<SiteHolder>().rootWasTapped();
				}
				else
				{
					base.GetComponent<SiteHolder>().subPages[num].GetComponent<SubPageHolder>().iWasTapped = true;
					GameManager.GetTheAnnBehavior().aniLoadingPageStop();
				}
			}
			else
			{
				if (this.canTapRoot)
				{
					num = UnityEngine.Random.Range(-1, base.GetComponent<SiteHolder>().subPages.Count);
				}
				else
				{
					num = UnityEngine.Random.Range(0, base.GetComponent<SiteHolder>().subPages.Count);
				}
				if (num == -1)
				{
					base.GetComponent<SiteHolder>().rootWasTapped();
				}
				else
				{
					base.GetComponent<SiteHolder>().subPages[num].GetComponent<SubPageHolder>().iWasTapped = true;
					GameManager.GetTheAnnBehavior().aniLoadingPageStop();
				}
				GameManager.FileSlinger.saveData.siteDataHex.Add(base.GetComponent<SiteHolder>().mySiteDeff.PageURL, num);
				GameManager.FileSlinger.saveFile("wttg2.gd");
			}
			if (num != -1)
			{
				base.GetComponent<SiteHolder>().getHashInfo(out setIndex, out empty);
				base.GetComponent<SiteHolder>().subPages[num].GetComponent<SubPageHolder>().setKeyIndex(setIndex, empty);
			}
		}
		else
		{
			base.GetComponent<SiteHolder>().rootWasTapped();
		}
	}

	private void OnEnable()
	{
		if (base.GetComponent<SiteHolder>().wasITapped())
		{
			this.iWasTapped();
		}
	}

	public bool tapSubPages;

	public bool canTapRoot;
}
