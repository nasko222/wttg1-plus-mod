using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class HailSatan : MonoBehaviour
{
	private void iWasTapped()
	{
		int setIndex = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out setIndex, out empty);
		int num;
		if (GameManager.FileSlinger.saveData.siteDataHex.ContainsKey(base.GetComponent<SiteHolder>().mySiteDeff.PageURL))
		{
			num = GameManager.FileSlinger.saveData.siteDataHex[base.GetComponent<SiteHolder>().mySiteDeff.PageURL];
			base.GetComponent<SiteHolder>().subPages[num].GetComponent<SubPageHolder>().iWasTapped = true;
		}
		else
		{
			num = UnityEngine.Random.Range(0, base.GetComponent<SiteHolder>().subPages.Count);
			base.GetComponent<SiteHolder>().subPages[num].GetComponent<SubPageHolder>().iWasTapped = true;
			GameManager.FileSlinger.saveData.siteDataHex.Add(base.GetComponent<SiteHolder>().mySiteDeff.PageURL, num);
			GameManager.FileSlinger.saveFile("wttg2.gd");
		}
		base.GetComponent<SiteHolder>().subPages[num].GetComponent<SubPageHolder>().setKeyIndex(setIndex, empty);
	}

	private void showHash()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.clickSound, 0.8f, false);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.hashHolder.color, delegate(Color x)
		{
			this.hashHolder.color = x;
		}, new Color(this.hashHolder.color.r, this.hashHolder.color.g, this.hashHolder.color.b, 1f), 0.75f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
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

	public LinkObject hotLink;

	public Text hashHolder;

	public AudioClip clickSound;
}
