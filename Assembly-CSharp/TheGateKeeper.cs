using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class TheGateKeeper : MonoBehaviour
{
	private void prepQuestions()
	{
		this.questionAnswerHolder.gameObject.SetActive(true);
		this.questionAnswerHolder.alpha = 0f;
		this.wrongPlace.alpha = 0f;
		this.hashHolder.alpha = 0f;
		this.hashText.text = "8 - " + GameManager.GetTheCloud().getRedRoomKey(7);
		this.pickedQuestions = new List<QuestionDefinition>();
		this.questionIndex = 0;
		short num = 0;
		bool flag = true;
		while (flag)
		{
			int index = Random.Range(0, this.questions.Count);
			if (!this.pickedQuestions.Contains(this.questions[index]))
			{
				this.pickedQuestions.Add(this.questions[index]);
				num += 1;
			}
			if (num >= this.questionAmt)
			{
				flag = false;
			}
		}
		for (int i = 0; i < this.answerObjects.Count; i++)
		{
			this.answerObjects[i].theRightAnswer = false;
			this.answerObjects[i].answerText.text = string.Empty;
			this.answerObjects[i].rightAction = new Action(this.getNextQuestion);
			this.answerObjects[i].wrongAction = new Action(this.wrongAnswerHit);
		}
		this.getNextQuestion();
	}

	private void getNextQuestion()
	{
		if (this.questionIndex >= this.questionAmt)
		{
			this.presentTheHash();
		}
		else
		{
			if (this.questionIndex != 0)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.correctAnswerClip, 1f, false);
			}
			this.aniSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.showNextQuestion));
			TweenSettingsExtensions.Insert(this.aniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.questionAnswerHolder.alpha, delegate(float x)
			{
				this.questionAnswerHolder.alpha = x;
			}, 0f, 0.5f), 1));
			TweenExtensions.Play<Sequence>(this.aniSeq);
		}
	}

	private void showNextQuestion()
	{
		int num = Random.Range(0, this.answerObjects.Count);
		List<string> list = new List<string>(this.pickedQuestions[(int)this.questionIndex].wrongAnswers);
		int num2 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			string value = list[i];
			int index = Random.Range(i, list.Count);
			list[i] = list[index];
			list[index] = value;
		}
		for (int j = 0; j < this.answerObjects.Count; j++)
		{
			if (j != num)
			{
				this.answerObjects[j].answerText.text = list[num2];
				this.answerObjects[j].theRightAnswer = false;
				num2++;
			}
		}
		this.questionText.text = this.pickedQuestions[(int)this.questionIndex].theQuestion;
		this.answerObjects[num].answerText.text = this.pickedQuestions[(int)this.questionIndex].theAnswer;
		this.answerObjects[num].theRightAnswer = true;
		this.questionIndex += 1;
		this.aniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.aniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.questionAnswerHolder.alpha, delegate(float x)
		{
			this.questionAnswerHolder.alpha = x;
		}, 1f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.aniSeq);
	}

	private void wrongAnswerHit()
	{
		GameManager.GetTheHackerManager().modemCoolOff = false;
		GameManager.GetTheHackerManager().ipIsMasked = false;
		GameManager.GetTheHackerManager().launchHack();
		this.questionAnswerHolder.gameObject.SetActive(false);
		this.aniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.aniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.questionAnswerHolder.alpha, delegate(float x)
		{
			this.questionAnswerHolder.alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.aniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.wrongPlace.alpha, delegate(float x)
		{
			this.wrongPlace.alpha = x;
		}, 1f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.aniSeq);
	}

	private void presentTheHash()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.SOFTWARESFX, this.hashFoundClip, 1f, false);
		this.questionAnswerHolder.gameObject.SetActive(false);
		this.aniSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.aniSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.questionAnswerHolder.alpha, delegate(float x)
		{
			this.questionAnswerHolder.alpha = x;
		}, 0f, 0.5f), 1));
		TweenSettingsExtensions.Insert(this.aniSeq, 0.5f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.hashHolder.alpha, delegate(float x)
		{
			this.hashHolder.alpha = x;
		}, 1f, 0.5f), 1));
		TweenExtensions.Play<Sequence>(this.aniSeq);
	}

	private void OnEnable()
	{
		this.prepQuestions();
	}

	private void OnDisable()
	{
	}

	public CanvasGroup questionAnswerHolder;

	public CanvasGroup wrongPlace;

	public CanvasGroup hashHolder;

	public AudioClip correctAnswerClip;

	public AudioClip hashFoundClip;

	public Text questionText;

	public Text hashText;

	[Range(1f, 10f)]
	public short questionAmt = 5;

	public List<QuestionDefinition> questions;

	public List<GKAnswerObject> answerObjects;

	private List<QuestionDefinition> pickedQuestions;

	private short questionIndex;

	private Sequence aniSeq;
}
