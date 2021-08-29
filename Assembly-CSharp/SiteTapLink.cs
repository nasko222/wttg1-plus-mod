using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiteTapLink : MonoBehaviour
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
		int num = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		int index = UnityEngine.Random.Range(0, this.linkObjs.Count);
		this.hashHolder.text = num.ToString() + " - " + empty;
		this.linkObjs[index].deadUnlessTapped = false;
		this.linkObjs[index].hasTapAction = true;
		this.linkObjs[index].tapAction = new Action(this.showHash);
	}

	private void showHash()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.clickSound, 0.8f, false);
		this.hashHolder.gameObject.SetActive(true);
	}

	private void OnEnable()
	{
		this.hashHolder.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		this.hashHolder.gameObject.SetActive(false);
	}

	public Text hashHolder;

	public List<LinkObject> linkObjs;

	public AudioClip clickSound;
}
