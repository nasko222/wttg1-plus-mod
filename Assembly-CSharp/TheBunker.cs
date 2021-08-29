using System;
using UnityEngine;

public class TheBunker : MonoBehaviour
{
	private void iWasTapped()
	{
		int num = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		this.setIndex = num;
		this.setHash = empty;
		this.hotLink.deadUnlessTapped = false;
		this.hotLink.hasTapAction = true;
		this.hotLink.tapAction = new Action(this.generateTxtDoc);
	}

	private void generateTxtDoc()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.clickSound, 0.8f, false);
		string txtDocText = this.setIndex.ToString() + " - " + this.setHash;
		GameManager.GetTheCloud().addTxtDoc("doomsday.txt", txtDocText);
	}

	private void OnEnable()
	{
		if (base.GetComponent<SiteHolder>().wasITapped())
		{
			this.iWasTapped();
		}
	}

	private void OnDisable()
	{
	}

	public LinkObject hotLink;

	public AudioClip clickSound;

	private int setIndex;

	private string setHash;
}
