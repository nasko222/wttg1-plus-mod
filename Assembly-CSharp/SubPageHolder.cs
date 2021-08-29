using System;
using UnityEngine;

public class SubPageHolder : MonoBehaviour
{
	public void setKeyIndex(int setIndex, string setKey)
	{
		this.keyIndex = setIndex;
		this.keyHash = setKey;
	}

	public void getKeyIndex(out int getIndex, out string getKey)
	{
		getIndex = this.keyIndex;
		getKey = this.keyHash;
	}

	private void OnEnable()
	{
		if (GameManager.GetGameModeManager().getCasualMode())
		{
			GameManager.GetTheAnnBehavior().aniLoadingPageStop();
		}
		else if (this.iWasTapped)
		{
			GameManager.TimeSlinger.FireTimer(0.75f, new Action(GameManager.GetTheAnnBehavior().aniLoadingPageStop));
		}
		else
		{
			GameManager.GetTheAnnBehavior().aniLoadingPageStop();
		}
	}

	public SubPageDefinition mySubPageDefinition;

	public SiteDefinition mainSiteDefinition;

	public bool iWasTapped;

	private int keyIndex;

	private string keyHash = string.Empty;
}
