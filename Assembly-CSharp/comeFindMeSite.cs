using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class comeFindMeSite : MonoBehaviour
{
	private void prepAssets()
	{
		this.enterBTN.setMyAction(new Action(this.isThePassCodeGood));
	}

	private void isThePassCodeGood()
	{
		if (GameManager.MagicSlinger.md5It(this.answerInput.text) == "7042ad4c85677e521069d8a768dba45c")
		{
			this.theForm.SetActive(false);
			this.triggerShowAnswer();
		}
		else
		{
			this.answerInput.text = string.Empty;
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SFX, this.wrongAnswer, 0.75f, false);
		}
	}

	private void triggerShowAnswer()
	{
		this.showAnswerSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.showAnswerSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.theAnswer.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.theAnswer.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.showAnswerSeq);
	}

	private void OnEnable()
	{
		this.prepAssets();
	}

	public GameObject theAnswer;

	public GameObject theForm;

	public InputField answerInput;

	public SiteBTN enterBTN;

	public AudioClip wrongAnswer;

	private Sequence showAnswerSeq;
}
