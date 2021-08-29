using System;
using System.Collections.Generic;
using UnityEngine;

public class SiteHolder : MonoBehaviour
{
	public void tapMe(int setIndex, string setHash)
	{
		this.iWasTapped = true;
		this.keyIndex = setIndex;
		this.keyHash = setHash;
	}

	public bool wasITapped()
	{
		return this.iWasTapped;
	}

	public void getHashInfo(out int theIndex, out string theHash)
	{
		theIndex = this.keyIndex;
		theHash = this.keyHash;
	}

	public bool checkSubPage(string subPageURL, out GameObject returnSubPage)
	{
		returnSubPage = null;
		if (this.haveSubPages)
		{
			bool result = false;
			for (int i = 0; i < this.subPages.Count; i++)
			{
				if (this.subPages[i].GetComponent<SubPageHolder>().mySubPageDefinition.subPageURL == subPageURL)
				{
					returnSubPage = this.subPages[i];
					i = this.subPages.Count;
					result = true;
				}
			}
			return result;
		}
		return false;
	}

	public void rootWasTapped()
	{
		tapType tapType = this.setTapType;
		if (tapType != tapType.Link)
		{
			if (tapType != tapType.Static)
			{
				if (tapType == tapType.File)
				{
					base.GetComponent<SiteTapFile>().tapTheSite();
				}
			}
			else
			{
				base.GetComponent<SiteKeyStatic>().tapTheSite();
			}
		}
		else
		{
			base.GetComponent<SiteTapLink>().tapTheSite();
		}
		if (GameManager.GetGameModeManager().getCasualMode())
		{
			GameManager.GetTheAnnBehavior().aniLoadingPageStop();
		}
		else if (GameManager.GetTheAnnBehavior().isKeyQueAlertActive())
		{
			if (GameManager.GetTheAnnBehavior().getKeyQueStayStatus())
			{
				GameManager.TimeSlinger.FireTimer(1.5f, new Action(GameManager.GetTheAnnBehavior().aniLoadingPageStop));
			}
			else
			{
				GameManager.GetTheAnnBehavior().aniLoadingPageStop();
			}
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(0.75f, new Action(GameManager.GetTheAnnBehavior().aniLoadingPageStop));
		}
	}

	private void setSubPageMainDef()
	{
		if (this.haveSubPages)
		{
			for (int i = 0; i < this.subPages.Count; i++)
			{
				this.subPages[i].GetComponent<SubPageHolder>().mainSiteDefinition = this.mySiteDeff;
			}
		}
	}

	private void OnEnable()
	{
		this.setSubPageMainDef();
		if (this.iWasTapped)
		{
			if (!GameManager.GetGameModeManager().getCasualMode())
			{
				GameManager.GetTheCloud().playerVisitedTappedSite(this.mySiteDeff.PageURL);
				if (base.GetComponent<SiteTapper>() == null)
				{
					if (GameManager.GetTheAnnBehavior().isKeyQueAlertActive())
					{
						if (GameManager.GetTheAnnBehavior().getKeyQueStayStatus())
						{
							GameManager.TimeSlinger.FireTimer(1.5f, new Action(GameManager.GetTheAnnBehavior().aniLoadingPageStop));
						}
						else
						{
							GameManager.GetTheAnnBehavior().aniLoadingPageStop();
						}
					}
					else
					{
						GameManager.TimeSlinger.FireTimer(0.75f, new Action(GameManager.GetTheAnnBehavior().aniLoadingPageStop));
					}
				}
			}
		}
		else
		{
			GameManager.GetTheAnnBehavior().aniLoadingPageStop();
		}
	}

	public SiteDefinition mySiteDeff;

	public bool haveSubPages;

	public List<GameObject> subPages;

	public tapType setTapType;

	private bool iWasTapped;

	private int keyIndex;

	private string keyHash = string.Empty;
}
