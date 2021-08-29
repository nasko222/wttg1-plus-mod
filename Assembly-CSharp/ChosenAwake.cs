using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class ChosenAwake : MonoBehaviour
{
	private void badPieceHit()
	{
		this.siteScrollRect.verticalNormalizedPosition = 1f;
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.badSound, 1f, false);
		this.prepPuzzle();
	}

	private void prepPuzzle()
	{
		if (this.gIndexs.Count <= 0)
		{
			int item = Random.Range(0, this.firstRowLinks.Count);
			int item2 = Random.Range(0, this.secondRowLinks.Count);
			int item3 = Random.Range(0, this.thridRowLinks.Count);
			int item4 = Random.Range(0, this.fourthRowLinks.Count);
			int item5 = Random.Range(0, this.fithRowLinks.Count);
			int item6 = Random.Range(0, this.sixthRowLinks.Count);
			int item7 = Random.Range(0, this.seventhRowLinks.Count);
			this.gIndexs.Add(item);
			this.gIndexs.Add(item2);
			this.gIndexs.Add(item3);
			this.gIndexs.Add(item4);
			this.gIndexs.Add(item5);
			this.gIndexs.Add(item6);
			this.gIndexs.Add(item7);
		}
		this.wikiLinkText.text = "http://" + GameManager.GetTheCloud().getWikiLink(1) + ".ann";
		this.hideAllPieces();
		this.lockAllPieces();
		this.prepFirstRow();
	}

	private void hideAllPieces()
	{
		for (int i = 0; i < this.firstRowLinks.Count; i++)
		{
			this.firstRowLinks[i].hidePiece();
		}
		for (int j = 0; j < this.secondRowLinks.Count; j++)
		{
			this.secondRowLinks[j].hidePiece();
		}
		for (int k = 0; k < this.thridRowLinks.Count; k++)
		{
			this.thridRowLinks[k].hidePiece();
		}
		for (int l = 0; l < this.fourthRowLinks.Count; l++)
		{
			this.fourthRowLinks[l].hidePiece();
		}
		for (int m = 0; m < this.fithRowLinks.Count; m++)
		{
			this.fithRowLinks[m].hidePiece();
		}
		for (int n = 0; n < this.sixthRowLinks.Count; n++)
		{
			this.sixthRowLinks[n].hidePiece();
		}
		for (int num = 0; num < this.seventhRowLinks.Count; num++)
		{
			this.seventhRowLinks[num].hidePiece();
		}
	}

	private void lockAllPieces()
	{
		for (int i = 0; i < this.firstRowLinks.Count; i++)
		{
			this.firstRowLinks[i].noLongerActive = true;
		}
		for (int j = 0; j < this.secondRowLinks.Count; j++)
		{
			this.secondRowLinks[j].noLongerActive = true;
		}
		for (int k = 0; k < this.thridRowLinks.Count; k++)
		{
			this.thridRowLinks[k].noLongerActive = true;
		}
		for (int l = 0; l < this.fourthRowLinks.Count; l++)
		{
			this.fourthRowLinks[l].noLongerActive = true;
		}
		for (int m = 0; m < this.fithRowLinks.Count; m++)
		{
			this.fithRowLinks[m].noLongerActive = true;
		}
		for (int n = 0; n < this.sixthRowLinks.Count; n++)
		{
			this.sixthRowLinks[n].noLongerActive = true;
		}
		for (int num = 0; num < this.seventhRowLinks.Count; num++)
		{
			this.seventhRowLinks[num].noLongerActive = true;
		}
	}

	private void prepFirstRow()
	{
		this.lockAllPieces();
		for (int i = 0; i < this.firstRowLinks.Count; i++)
		{
			this.firstRowLinks[i].noLongerActive = false;
			this.firstRowLinks[i].badAction = new Action(this.badPieceHit);
			this.firstRowLinks[i].goodAction = new Action(this.prepSecondRow);
			this.firstRowLinks[i].iWasTapped = false;
		}
		this.firstRowLinks[this.gIndexs[0]].iWasTapped = true;
	}

	private void prepSecondRow()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.goodSound, 1f, false);
		this.lockAllPieces();
		for (int i = 0; i < this.secondRowLinks.Count; i++)
		{
			this.secondRowLinks[i].noLongerActive = false;
			this.secondRowLinks[i].badAction = new Action(this.badPieceHit);
			this.secondRowLinks[i].goodAction = new Action(this.prepThirdRow);
			this.secondRowLinks[i].iWasTapped = false;
		}
		this.secondRowLinks[this.gIndexs[1]].iWasTapped = true;
	}

	private void prepThirdRow()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.goodSound, 1f, false);
		this.lockAllPieces();
		for (int i = 0; i < this.thridRowLinks.Count; i++)
		{
			this.thridRowLinks[i].noLongerActive = false;
			this.thridRowLinks[i].badAction = new Action(this.badPieceHit);
			this.thridRowLinks[i].goodAction = new Action(this.prepFourthRow);
			this.thridRowLinks[i].iWasTapped = false;
		}
		this.thridRowLinks[this.gIndexs[2]].iWasTapped = true;
	}

	private void prepFourthRow()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.goodSound, 1f, false);
		this.lockAllPieces();
		for (int i = 0; i < this.fourthRowLinks.Count; i++)
		{
			this.fourthRowLinks[i].noLongerActive = false;
			this.fourthRowLinks[i].badAction = new Action(this.badPieceHit);
			this.fourthRowLinks[i].goodAction = new Action(this.prepFifthRow);
			this.fourthRowLinks[i].iWasTapped = false;
		}
		this.fourthRowLinks[this.gIndexs[3]].iWasTapped = true;
	}

	private void prepFifthRow()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.goodSound, 1f, false);
		this.lockAllPieces();
		for (int i = 0; i < this.fithRowLinks.Count; i++)
		{
			this.fithRowLinks[i].noLongerActive = false;
			this.fithRowLinks[i].badAction = new Action(this.badPieceHit);
			this.fithRowLinks[i].goodAction = new Action(this.prepSixthRow);
			this.fithRowLinks[i].iWasTapped = false;
		}
		this.fithRowLinks[this.gIndexs[4]].iWasTapped = true;
	}

	private void prepSixthRow()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.goodSound, 1f, false);
		this.lockAllPieces();
		for (int i = 0; i < this.sixthRowLinks.Count; i++)
		{
			this.sixthRowLinks[i].noLongerActive = false;
			this.sixthRowLinks[i].badAction = new Action(this.badPieceHit);
			this.sixthRowLinks[i].goodAction = new Action(this.prepSeventhRow);
			this.sixthRowLinks[i].iWasTapped = false;
		}
		this.sixthRowLinks[this.gIndexs[5]].iWasTapped = true;
	}

	private void prepSeventhRow()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.goodSound, 1f, false);
		this.lockAllPieces();
		for (int i = 0; i < this.seventhRowLinks.Count; i++)
		{
			this.seventhRowLinks[i].noLongerActive = false;
			this.seventhRowLinks[i].badAction = new Action(this.badPieceHit);
			this.seventhRowLinks[i].goodAction = new Action(this.showWikiLink);
			this.seventhRowLinks[i].iWasTapped = false;
		}
		this.seventhRowLinks[this.gIndexs[6]].iWasTapped = true;
	}

	private void showWikiLink()
	{
		GameManager.GetTheCloud().addPlayerSkillPoints(20);
		GameManager.GetTheCloud().addPlayerSkillPoints2(20);
		GameManager.GetTheCloud().addPlayerSkillPoints3(20);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.foundSound, 0.75f, false);
		this.aniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.aniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.wikiLinkHolder.alpha, delegate(float x)
		{
			this.wikiLinkHolder.alpha = x;
		}, 1f, 1f), 1));
		TweenExtensions.Play<Sequence>(this.aniSeq);
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		this.gIndexs = new List<int>();
		this.prepPuzzle();
	}

	private void OnDisable()
	{
		this.gIndexs.Clear();
		this.wikiLinkText.text = string.Empty;
		this.wikiLinkHolder.alpha = 0f;
	}

	public ScrollRect siteScrollRect;

	public CanvasGroup wikiLinkHolder;

	public InputField wikiLinkText;

	public AudioClip goodSound;

	public AudioClip badSound;

	public AudioClip foundSound;

	public List<ChosenAwakeLink> firstRowLinks;

	public List<ChosenAwakeLink> secondRowLinks;

	public List<ChosenAwakeLink> thridRowLinks;

	public List<ChosenAwakeLink> fourthRowLinks;

	public List<ChosenAwakeLink> fithRowLinks;

	public List<ChosenAwakeLink> sixthRowLinks;

	public List<ChosenAwakeLink> seventhRowLinks;

	private List<int> gIndexs;

	private Sequence aniSeq;
}
