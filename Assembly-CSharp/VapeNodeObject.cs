using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VapeNodeObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void buildMe(vapeAttack setVapeAttack, vapeAttack.vapeNode setNodeData = null, float setX = 0f, float setY = 0f, float setWidth = 100f, float setHeight = 100f)
	{
		this.myVapeAttack = setVapeAttack;
		this.myVapeNodeData = setNodeData;
		this.nodeImage.sprite = this.myVapeNodeData.mySprite;
		base.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		base.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
		base.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
		base.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.localPosition = new Vector3(setX, setY, 0f);
		this.nodeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		this.nodeHoverImage.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		this.nodeHoverCanvas.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		this.nodeActiveImage.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		this.nodeActiveCanvas.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		if (setNodeData.myType == vapeNodeType.BLANKNODE)
		{
			this.nodeHoverImage.sprite = this.blankNodeHoverSprite;
		}
		else if (setNodeData.myType == vapeNodeType.BOXNODE)
		{
			this.nodeHoverImage.sprite = this.boxNodeHoverSprite;
			this.nodeActiveImage.sprite = this.boxNodeActiveSprite;
		}
		else if (setNodeData.myType != vapeNodeType.GOODNODE)
		{
			if (setNodeData.myType == vapeNodeType.DEADNODE)
			{
			}
		}
	}

	public void updateMyType(vapeNodeType setType)
	{
		if (setType == vapeNodeType.BLANKNODE)
		{
			this.myVapeNodeData.updateMyInfo(setType, this.blankNodeSprite);
			this.nodeImage.sprite = this.blankNodeSprite;
			this.nodeHoverImage.sprite = this.blankNodeHoverSprite;
		}
		else if (setType == vapeNodeType.BOXNODE)
		{
			this.myVapeNodeData.updateMyInfo(setType, this.boxNodeSprite);
			this.nodeImage.sprite = this.boxNodeSprite;
			this.nodeHoverImage.sprite = this.boxNodeHoverSprite;
			this.nodeActiveImage.sprite = this.boxNodeActiveSprite;
		}
		else if (setType == vapeNodeType.GOODNODE)
		{
			this.myVapeNodeData.updateMyInfo(setType, this.goodNodeSprite);
			this.nodeImage.sprite = this.goodNodeSprite;
		}
		else if (setType == vapeNodeType.DEADNODE)
		{
			this.myVapeNodeData.updateMyInfo(setType, this.deadNodeSprite);
			this.nodeImage.sprite = this.deadNodeSprite;
		}
	}

	public void clearActiveState()
	{
		TweenExtensions.Kill(this.actionSeq, false);
		this.nodeHoverCanvas.alpha = 0f;
		this.nodeActiveCanvas.alpha = 0f;
	}

	public void setAsGoodNode()
	{
		if (this.myVapeAttack.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myVapeAttack.goodNodeActiveClip, 0.6f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myVapeAttack.goodNodeActiveClip, 0.6f, false);
		}
		this.nodeActiveImage.sprite = this.goodNodeActiveSprite;
		this.nodeActiveCanvas.alpha = 1f;
		this.goodSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.goodSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeActiveCanvas.alpha, delegate(float x)
		{
			this.nodeActiveCanvas.alpha = x;
		}, 0f, 0.25f), 1));
		TweenExtensions.Play<Sequence>(this.goodSeq);
		this.updateMyType(vapeNodeType.GOODNODE);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.myVapeAttack.vapeAttackFired)
		{
			this.actionSeq = DOTween.Sequence();
			if (this.myVapeNodeData.myType == vapeNodeType.BOXNODE)
			{
				if (!this.myVapeAttack.curActiveNodeSet)
				{
					if (this.myVapeAttack.inHackerMode)
					{
						GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myVapeAttack.boxNodeHoverClip, 0.75f, false);
					}
					else
					{
						GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myVapeAttack.boxNodeHoverClip, 0.75f, false);
					}
				}
				TweenSettingsExtensions.Insert(this.actionSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHoverCanvas.alpha, delegate(float x)
				{
					this.nodeHoverCanvas.alpha = x;
				}, 1f, 0.25f), 1));
			}
			else if (this.myVapeNodeData.myType == vapeNodeType.BLANKNODE && this.myVapeAttack.curActiveNodeSet)
			{
				if (this.myVapeAttack.inHackerMode)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myVapeAttack.blankNodeHoverClip, 1f, false);
				}
				else
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myVapeAttack.blankNodeHoverClip, 1f, false);
				}
				TweenSettingsExtensions.Insert(this.actionSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHoverCanvas.alpha, delegate(float x)
				{
					this.nodeHoverCanvas.alpha = x;
				}, 1f, 0.15f), 1));
			}
			TweenExtensions.Play<Sequence>(this.actionSeq);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.myVapeAttack.vapeAttackFired)
		{
			TweenExtensions.Kill(this.actionSeq, false);
			this.actionSeq = DOTween.Sequence();
			if (this.myVapeNodeData.myType == vapeNodeType.BOXNODE)
			{
				TweenSettingsExtensions.Insert(this.actionSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHoverCanvas.alpha, delegate(float x)
				{
					this.nodeHoverCanvas.alpha = x;
				}, 0f, 0.25f), 1));
			}
			else if (this.myVapeNodeData.myType == vapeNodeType.BLANKNODE)
			{
				TweenSettingsExtensions.Insert(this.actionSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHoverCanvas.alpha, delegate(float x)
				{
					this.nodeHoverCanvas.alpha = x;
				}, 0f, 0.15f), 1));
			}
			TweenExtensions.Play<Sequence>(this.actionSeq);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.myVapeAttack.vapeAttackFired)
		{
			TweenExtensions.Kill(this.actionSeq, false);
			this.actionSeq = DOTween.Sequence();
			if (this.myVapeNodeData.myType == vapeNodeType.BOXNODE)
			{
				if (!this.myVapeAttack.curActiveNodeSet)
				{
					if (this.myVapeAttack.inHackerMode)
					{
						GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myVapeAttack.boxNodeActiveClip, 0.75f, false);
					}
					else
					{
						GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myVapeAttack.boxNodeActiveClip, 0.75f, false);
					}
					TweenSettingsExtensions.Insert(this.actionSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHoverCanvas.alpha, delegate(float x)
					{
						this.nodeHoverCanvas.alpha = x;
					}, 0f, 0.25f), 1));
					TweenSettingsExtensions.Insert(this.actionSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeActiveCanvas.alpha, delegate(float x)
					{
						this.nodeActiveCanvas.alpha = x;
					}, 1f, 0.25f), 1));
				}
			}
			else if (this.myVapeNodeData.myType == vapeNodeType.BLANKNODE && this.myVapeAttack.curActiveNodeSet)
			{
				TweenSettingsExtensions.Insert(this.actionSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHoverCanvas.alpha, delegate(float x)
				{
					this.nodeHoverCanvas.alpha = x;
				}, 0f, 0.15f), 1));
			}
			TweenExtensions.Play<Sequence>(this.actionSeq);
			this.myVapeAttack.vapeNodeAction(this.myVapeNodeData);
		}
	}

	public Image nodeImage;

	public CanvasGroup nodeHoverCanvas;

	public Image nodeHoverImage;

	public CanvasGroup nodeActiveCanvas;

	public Image nodeActiveImage;

	public Sprite blankNodeSprite;

	public Sprite boxNodeSprite;

	public Sprite goodNodeSprite;

	public Sprite deadNodeSprite;

	public Sprite boxNodeHoverSprite;

	public Sprite boxNodeActiveSprite;

	public Sprite blankNodeHoverSprite;

	public Sprite goodNodeActiveSprite;

	public vapeAttack.vapeNode myVapeNodeData;

	private vapeAttack myVapeAttack;

	private Sequence actionSeq;

	private Sequence goodSeq;
}
