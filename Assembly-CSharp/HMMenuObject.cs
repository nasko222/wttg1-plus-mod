using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class HMMenuObject : MonoBehaviour
{
	public void buildMe(float setWidth, float setHeight, string setText)
	{
		base.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight + this.MenuTitleHolder.GetComponent<RectTransform>().sizeDelta.y);
		this.MenuTitleHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, this.MenuTitleHolder.GetComponent<RectTransform>().sizeDelta.y);
		this.MenuTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, this.MenuTitle.GetComponent<RectTransform>().sizeDelta.y);
		this.MenuBaseBG.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, this.MenuBaseBG.GetComponent<RectTransform>().sizeDelta.y);
		this.MenuForegroundBG.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 4f, this.MenuForegroundBG.GetComponent<RectTransform>().sizeDelta.y);
		this.MenuGroupHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		this.MenuBase.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		this.MenuBG.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 4f, setHeight - 4f);
		this.MenuLineNumber.GetComponent<RectTransform>().sizeDelta = new Vector2(this.MenuLineNumber.GetComponent<RectTransform>().sizeDelta.x, setHeight - 4f);
		this.MenuLineHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 4f, setHeight - 4f);
		base.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		this.MenuTitle.text = setText;
	}

	public void showMe(float setDelay = 2f)
	{
		this.aniSeq = DOTween.Sequence();
		this.aniSeq.Insert(setDelay, DOTween.To(() => this.MenuBG.fillAmount, delegate(float x)
		{
			this.MenuBG.fillAmount = x;
		}, 1f, 1f).SetEase(Ease.Linear));
		this.aniSeq.Insert(setDelay, DOTween.To(() => this.MenuBase.fillAmount, delegate(float x)
		{
			this.MenuBase.fillAmount = x;
		}, 1f, 1f).SetEase(Ease.Linear));
		this.aniSeq.Insert(setDelay + 1f, DOTween.To(() => this.MenuLineNumber.fillAmount, delegate(float x)
		{
			this.MenuLineNumber.fillAmount = x;
		}, 1f, 0.5f).SetEase(Ease.OutSine));
		this.aniSeq.Insert(setDelay + 1f, DOTween.To(() => this.MenuTitleHolder.transform.localPosition, delegate(Vector3 x)
		{
			this.MenuTitleHolder.transform.localPosition = x;
		}, new Vector3(0f, 0f, 0f), 0.65f).SetEase(Ease.OutSine));
		this.aniSeq.Insert(setDelay + 1f, DOTween.To(() => this.MenuTitleHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.MenuTitleHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0f).SetEase(Ease.Linear));
		this.aniSeq.Play<Sequence>();
	}

	public void hideMe()
	{
		this.aniSeq = DOTween.Sequence();
		this.aniSeq.Insert(0f, DOTween.To(() => this.MenuLineHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.MenuLineHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.35f).SetEase(Ease.OutSine));
		this.aniSeq.Insert(0f, DOTween.To(() => this.MenuTitleHolder.transform.localPosition, delegate(Vector3 x)
		{
			this.MenuTitleHolder.transform.localPosition = x;
		}, new Vector3(0f, -46f, 0f), 0.35f).SetEase(Ease.Linear));
		this.aniSeq.Insert(0.35f, DOTween.To(() => this.MenuBase.fillAmount, delegate(float x)
		{
			this.MenuBase.fillAmount = x;
		}, 0f, 0.65f).SetEase(Ease.Linear));
		this.aniSeq.Insert(0.35f, DOTween.To(() => this.MenuBG.fillAmount, delegate(float x)
		{
			this.MenuBG.fillAmount = x;
		}, 0f, 0.65f).SetEase(Ease.Linear));
		this.aniSeq.Insert(0.35f, DOTween.To(() => this.MenuLineNumber.fillAmount, delegate(float x)
		{
			this.MenuLineNumber.fillAmount = x;
		}, 0f, 0.65f).SetEase(Ease.OutSine));
		this.aniSeq.Insert(0.35f, DOTween.To(() => this.MenuTitleHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.MenuTitleHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.35f).SetEase(Ease.OutSine));
		this.aniSeq.Play<Sequence>();
	}

	public GameObject MenuObject;

	public GameObject MenuTitleHolder;

	public GameObject MenuBaseBG;

	public GameObject MenuForegroundBG;

	public GameObject MenuGroupHolder;

	public GameObject MenuLineHolder;

	public Text MenuTitle;

	public Image MenuBase;

	public Image MenuBG;

	public Image MenuLineNumber;

	private Sequence aniSeq;
}
