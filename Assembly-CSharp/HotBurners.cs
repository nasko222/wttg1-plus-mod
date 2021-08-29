using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class HotBurners : MonoBehaviour
{
	private void iWasTapped()
	{
		int num = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		Color color = this.hashHolder.color;
		this.hashHolder.text = num.ToString() + " - " + empty;
		this.hashHolder.color = new Color(color.r, color.g, color.b, 0f);
		this.hashHolder.gameObject.SetActive(true);
		this.hotLink.hasTapAction = true;
		this.hotLink.tapAction = new Action(this.showHash);
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
