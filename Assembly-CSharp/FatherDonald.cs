using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FatherDonald : MonoBehaviour
{
	private void iWasTapped()
	{
		int index = UnityEngine.Random.Range(0, this.keyBTNS.Count);
		this.keyBTNS[index].deadUnlessTapped = false;
		this.keyBTNS[index].hasTapAction = true;
		this.keyBTNS[index].tapAction = new Action(this.showTheHash);
		int num = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		this.hashHolder.text = num.ToString() + " - " + empty;
	}

	private void showTheHash()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.clickSound, 0.8f, false);
		this.hashHolder.gameObject.SetActive(true);
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
		this.hashHolder.gameObject.SetActive(false);
	}

	public Text hashHolder;

	public AudioClip clickSound;

	public List<LinkObject> keyBTNS;
}
