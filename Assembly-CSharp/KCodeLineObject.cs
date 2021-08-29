using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class KCodeLineObject : MonoBehaviour
{
	public void buildMe(int myIndex, string codeNumber, string codeText, float myX, float myY)
	{
		this.myCodeNumber = codeNumber;
		this.myCodeLine = codeText;
		base.GetComponent<RectTransform>().transform.localPosition = new Vector3(myX, myY, 0f);
		base.GetComponent<RectTransform>().transform.localScale = new Vector3(1f, 1f, 1f);
		GameManager.TimeSlinger.FireTimer((float)myIndex * 0.2f, new Action(this.showMe));
	}

	public void IAmActive()
	{
		this.codeHighlight.color = this.codeHighlightColor;
		this.activeColor = this.codeHighlightColor;
		this.amActiveSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.amActiveSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.codeHighlight.color, delegate(Color x)
		{
			this.codeHighlight.color = x;
		}, new Color(this.activeColor.r, this.activeColor.g, this.activeColor.b, 0.1f), 1f), 1));
		TweenSettingsExtensions.Insert(this.amActiveSeq, 1f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.codeHighlight.color, delegate(Color x)
		{
			this.codeHighlight.color = x;
		}, new Color(this.activeColor.r, this.activeColor.g, this.activeColor.b, 1f), 1f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.amActiveSeq, -1);
		TweenExtensions.Play<Sequence>(this.amActiveSeq);
	}

	public void InvalidInput()
	{
		if (this.myKAttack.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myKAttack.InvalidInputSound, 1f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myKAttack.InvalidInputSound, 1f, false);
		}
		this.lineNumber.color = this.boxHighlightColor;
		this.codeLine.color = this.boxHighlightColor;
		this.boxHighlight.color = new Color(this.invalidCodeHighlightColor.r, this.invalidCodeHighlightColor.g, this.invalidCodeHighlightColor.b, 1f);
		this.vSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.vSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.boxHighlight.color, delegate(Color x)
		{
			this.boxHighlight.color = x;
		}, new Color(this.invalidCodeHighlightColor.r, this.invalidCodeHighlightColor.g, this.invalidCodeHighlightColor.b, 0f), 0.25f), 1));
		TweenExtensions.Play<Sequence>(this.vSeq);
	}

	public void ValidInput()
	{
		if (this.myKAttack.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myKAttack.ValidInputSound, 1f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myKAttack.ValidInputSound, 1f, false);
		}
		this.boxHighlight.color = this.validCodeHighlightColor;
		TweenExtensions.Kill(this.amActiveSeq, false);
		this.codeHighlight.color = new Color(this.codeHighlightColor.r, this.codeHighlightColor.g, this.codeHighlightColor.b, 0f);
		this.lineNumber.gameObject.SetActive(false);
		this.checkMark.gameObject.SetActive(true);
		this.codeLine.fontStyle = 2;
		this.codeLine.color = new Color(this.codeLine.color.r, this.codeLine.color.g, this.codeLine.color.b, 0.3f);
		this.showMeSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.showMeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.boxHighlight.color, delegate(Color x)
		{
			this.boxHighlight.color = x;
		}, new Color(this.validCodeHighlightColor.r, this.validCodeHighlightColor.g, this.validCodeHighlightColor.b, 0f), 0.25f), 1));
		TweenExtensions.Play<Sequence>(this.showMeSeq);
	}

	public void hotCheck(string compareString = "")
	{
		if (compareString.Length <= this.myCodeLine.Length)
		{
			if (compareString.Equals(this.myCodeLine.Substring(0, compareString.Length)))
			{
				this.lineNumber.color = this.boxHighlightColor;
				this.codeLine.color = this.boxHighlightColor;
			}
			else
			{
				this.lineNumber.color = this.invalidCodeHighlightColor;
				this.codeLine.color = this.invalidCodeHighlightColor;
			}
		}
		else
		{
			this.lineNumber.color = this.invalidCodeHighlightColor;
			this.codeLine.color = this.invalidCodeHighlightColor;
		}
	}

	private void showMe()
	{
		if (this.myKAttack.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myKAttack.HighlightCodeSound, 0.45f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myKAttack.HighlightCodeSound, 0.3f, false);
		}
		this.boxHighlight.color = this.boxHighlightColor;
		this.lineNumber.text = this.myCodeNumber;
		this.codeLine.text = this.myCodeLine;
		this.showMeSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.showMeSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.boxHighlight.color, delegate(Color x)
		{
			this.boxHighlight.color = x;
		}, new Color(this.boxHighlightColor.r, this.boxHighlightColor.g, this.boxHighlightColor.b, 0f), 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.showMeSeq);
	}

	public KAttack myKAttack;

	public Image checkMark;

	public Image codeHighlight;

	public Image boxHighlight;

	public Text lineNumber;

	public Text codeLine;

	public Color codeHighlightColor;

	public Color boxHighlightColor;

	public Color validCodeHighlightColor;

	public Color invalidCodeHighlightColor;

	private string myCodeNumber;

	private string myCodeLine;

	private Color activeColor;

	private Sequence showMeSeq;

	private Sequence amActiveSeq;

	private Sequence vSeq;
}
