using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class Claffis : MonoBehaviour
{
	private void prepPuzzle()
	{
		this.puzzleIsActive = false;
		for (int i = 0; i < this.noteLinks.Count; i++)
		{
			this.noteLinks[i].hasIntTapAction = true;
			this.noteLinks[i].intTapValue = i;
			this.noteLinks[i].intTapAction = new Action<int>(this.noteLinkHit);
		}
		this.startPuzzleLink.hasTapAction = true;
		this.startPuzzleLink.tapAction = new Action(this.beginPuzzle);
		this.curPuzzleIndex = -1;
		this.curNotes = new List<int>();
		this.wikiLinkText.text = "http://" + GameManager.GetTheCloud().getWikiLink(2) + ".ann";
	}

	private void noteLinkHit(int hitIndex)
	{
		if (this.puzzleIsActive)
		{
			if (this.puzzleReadyForInput)
			{
				if (hitIndex == this.curNotes[this.curPuzzleIndex])
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.noteSFXs[hitIndex], 0.7f, false);
					this.highlightNote(hitIndex);
					this.curPuzzleIndex++;
					if (this.curPuzzleIndex >= this.curNotes.Count)
					{
						this.puzzleSolved();
					}
				}
				else
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.puzzleFailSFX, 0.85f, false);
					this.puzzleIsActive = false;
					this.puzzleReadyForInput = false;
					this.firstTry = false;
					this.curPuzzleIndex = -1;
					this.curNotes.Clear();
				}
			}
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.noteSFXs[hitIndex], 0.7f, false);
			this.highlightNote(hitIndex);
		}
	}

	private void beginPuzzle()
	{
		if (!this.puzzleIsActive)
		{
			this.puzzleReadyForInput = false;
			int i = 1;
			float num = 0.5f;
			this.curNotes.Clear();
			for (int j = 0; j < this.noteLinks.Count; j++)
			{
				this.noteLinks[j].deadUnlessTapped = true;
			}
			while (i <= this.keyNoteLength)
			{
				int item = Random.Range(0, this.noteSFXs.Count);
				if (!this.curNotes.Contains(item))
				{
					this.curNotes.Add(item);
					i++;
				}
			}
			this.puzzleIsActive = true;
			this.curPuzzleIndex = 0;
			for (int k = 0; k < this.curNotes.Count; k++)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.noteSFXs[this.curNotes[k]], 0.7f, false, num);
				GameManager.TimeSlinger.FireIntTimer(num, new Action<int>(this.highlightNote), this.curNotes[k]);
				num += 2.5f;
			}
			GameManager.TimeSlinger.FireTimer(num, new Action(this.setReadyForInput));
		}
	}

	private void setReadyForInput()
	{
		for (int i = 0; i < this.noteLinks.Count; i++)
		{
			this.noteLinks[i].deadUnlessTapped = false;
		}
		this.puzzleReadyForInput = true;
	}

	private void highlightNote(int setIndex)
	{
		TweenExtensions.Kill(this.noteSeq, true);
		this.noteHighlights[setIndex].alpha = 1f;
		this.noteSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.noteSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.noteHighlights[setIndex].alpha, delegate(float x)
		{
			this.noteHighlights[setIndex].alpha = x;
		}, 0f, 1f), 1));
		TweenExtensions.Play<Sequence>(this.noteSeq);
	}

	private void puzzleSolved()
	{
		for (int i = 0; i < this.noteLinks.Count; i++)
		{
			this.noteLinks[i].hasIntTapAction = false;
			this.noteLinks[i].deadUnlessTapped = true;
		}
		this.startPuzzleLink.hasTapAction = false;
		this.startPuzzleLink.deadUnlessTapped = true;
		GameManager.GetTheCloud().addPlayerSkillPoints(10);
		GameManager.GetTheCloud().addPlayerSkillPoints2(10);
		GameManager.GetTheCloud().addPlayerSkillPoints3(10);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.puzzlePassSFX, 1f, false, 1f);
		this.aniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.aniSeq, 1f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.wikiLinkHolder.alpha, delegate(float x)
		{
			this.wikiLinkHolder.alpha = x;
		}, 1f, 1f), 1));
		TweenExtensions.Play<Sequence>(this.aniSeq);
		if (this.firstTry)
		{
			GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_IMPROVER, true);
		}
	}

	private void OnEnable()
	{
		this.prepPuzzle();
		this.firstTry = true;
	}

	[Range(1f, 12f)]
	public int keyNoteLength = 8;

	public LinkObject startPuzzleLink;

	public List<LinkObject> noteLinks;

	public List<AudioClip> noteSFXs;

	public List<CanvasGroup> noteHighlights;

	public AudioClip puzzlePassSFX;

	public AudioClip puzzleFailSFX;

	public CanvasGroup wikiLinkHolder;

	public InputField wikiLinkText;

	private bool puzzleIsActive;

	private bool puzzleReadyForInput;

	private bool firstTry;

	private List<int> curNotes;

	private int curPuzzleIndex = -1;

	private Sequence noteSeq;

	private Sequence aniSeq;
}
