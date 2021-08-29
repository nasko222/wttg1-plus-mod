using System;
using System.Collections.Generic;
using UnityEngine;

public class SiteTapFile : MonoBehaviour
{
	public void tapTheSite()
	{
		if (!GameManager.GetGameModeManager().getCasualMode())
		{
			this.iWasTapped();
		}
	}

	private void iWasTapped()
	{
		base.GetComponent<SiteHolder>().getHashInfo(out this.setIndex, out this.setHash);
		int index = Random.Range(0, this.linkObjs.Count);
		this.linkObjs[index].deadUnlessTapped = false;
		this.linkObjs[index].hasTapAction = true;
		this.linkObjs[index].tapAction = new Action(this.generateTxtDoc);
	}

	private void generateTxtDoc()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.clickSound, 0.8f, false);
		string txtDocText = this.setIndex.ToString() + " - " + this.setHash;
		GameManager.GetTheCloud().addTxtDoc(this.fileName, txtDocText);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public string fileName = string.Empty;

	public List<LinkObject> linkObjs;

	public AudioClip clickSound;

	private int setIndex;

	private string setHash;
}
