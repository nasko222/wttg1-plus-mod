using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class NodeObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void buildMe(DOSAttack.node setNodeData = null, float setX = 0f, float setY = 0f, float setWidth = 100f, float setHeight = 100f)
	{
		this.myNodeData = setNodeData;
		this.nodeImage.sprite = this.myNodeData.mySprite;
		this.mySetW = setWidth;
		this.mySetH = setHeight;
		this.nodeBGImage.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		this.nodeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 4f, setHeight - 4f);
		this.nodeHotImage.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth - 4f, setHeight - 4f);
		base.GetComponent<RectTransform>().sizeDelta = new Vector2(setWidth, setHeight);
		base.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
		base.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
		base.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.localPosition = new Vector3(setX, setY, 0f);
	}

	public void doSubAction()
	{
		if (this.myNodeData.myType == nodeType.STARTNODE)
		{
			switch (this.myNodeData.mySubType)
			{
			case nodeType.LEFTNODE:
				this.nodeImage.sprite = this.myDoSAttack.leftNodeSprite;
				break;
			case nodeType.RIGHTNODE:
				this.nodeImage.sprite = this.myDoSAttack.rightNodeSprite;
				break;
			case nodeType.UPNODE:
				this.nodeImage.sprite = this.myDoSAttack.upNodeSrpite;
				break;
			case nodeType.DOWNNODE:
				this.nodeImage.sprite = this.myDoSAttack.downNodeSprite;
				break;
			}
			float num = this.mySetW / (float)this.myDoSAttack.rootNodeWidth;
			float num2 = this.mySetH / (float)this.myDoSAttack.rootNodeHeight;
			this.startNodeArr = new GameObject("sn1337", new Type[]
			{
				typeof(Image)
			}).GetComponent<Image>();
			this.startNodeArr.sprite = this.myDoSAttack.startNodeDownArrowSprite;
			this.startNodeArr.GetComponent<RectTransform>().SetParent(base.transform);
			this.startNodeArr.GetComponent<RectTransform>().sizeDelta = new Vector2(this.myDoSAttack.startNodeDownArrowSprite.rect.width * num, this.myDoSAttack.startNodeDownArrowSprite.rect.height * num2);
			this.startNodeArr.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
			this.startNodeArr.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
			this.startNodeArr.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
			this.startNodeArr.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			this.startNodeArr.GetComponent<RectTransform>().localPosition = new Vector3(this.mySetW / 2f - this.myDoSAttack.startNodeDownArrowSprite.rect.width * num / 2f, (float)this.myDoSAttack.nodeHeight, 0f);
			this.aniStartNodeArrow();
		}
		else if (this.myNodeData.myType == nodeType.ACTIONFILLEDNODE)
		{
			this.actionArrowIMG = new GameObject("actionARRIMG", new Type[]
			{
				typeof(Image)
			}).GetComponent<Image>();
			this.actionArrowIMG.GetComponent<RectTransform>().SetParent(base.transform);
			this.actionArrowIMG.color = new Color(1f, 1f, 1f, 0f);
			this.actionArrowIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(this.mySetW, this.mySetH);
			this.actionArrowIMG.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
			this.actionArrowIMG.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
			this.actionArrowIMG.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
			this.actionArrowIMG.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			this.actionArrowIMG.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
		}
		else if (this.myNodeData.myType == nodeType.WHITENODE && this.trollNode)
		{
			switch (UnityEngine.Random.Range(1, 5))
			{
			case 1:
				this.nodeImage.sprite = this.myDoSAttack.trollLeftNodeSprite;
				break;
			case 2:
				this.nodeImage.sprite = this.myDoSAttack.trollDownNodeSprite;
				break;
			case 3:
				this.nodeImage.sprite = this.myDoSAttack.trollRightNodeSprite;
				break;
			case 4:
				this.nodeImage.sprite = this.myDoSAttack.trollUpNodeSprite;
				break;
			}
		}
	}

	public void tap()
	{
		this.nodeHotImage.color = new Color(1f, 1f, 1f, 1f);
		if (this.myNodeData.myType == nodeType.ACTIONFILLEDNODE && !this.actionNodeActive)
		{
			if (this.myDoSAttack.inHackerMode)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeActive, 0.5f, false);
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeActive, 0.5f, false);
			}
			this.nodeImage.sprite = this.myDoSAttack.actionNodeActivatedSprite;
			this.actionNodeActive = true;
			this.myDoSAttack.actionNodeActivated();
		}
		if (this.myNodeData.myType == nodeType.WHITENODE)
		{
			if (this.myDoSAttack.inHackerMode)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeCold, 0.5f, false);
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeCold, 0.5f, false);
			}
		}
		else if (this.myNodeData.myType == nodeType.ACTIONFILLEDNODE)
		{
			if (this.actionNodeDirection == 0)
			{
				if (this.myDoSAttack.inHackerMode)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeCold, 0.5f, false);
				}
				else
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeCold, 0.5f, false);
				}
			}
			else if (this.myDoSAttack.inHackerMode)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeHot, 0.5f, false);
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeHot, 0.5f, false);
			}
		}
		else if (this.myDoSAttack.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeHot, 0.5f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myDoSAttack.NodeHot, 0.5f, false);
		}
	}

	public void untap()
	{
		this.nodeHotImage.color = new Color(1f, 1f, 1f, 0f);
	}

	public void stopSubAction()
	{
		if (this.myNodeData.myType == nodeType.STARTNODE)
		{
			this.aniStopNodeArrow();
		}
	}

	public void endNodeNowActive()
	{
		if (this.myNodeData.myType == nodeType.ENDNODE)
		{
			this.nodeImage.sprite = this.myDoSAttack.endNodeActivatedSprite;
		}
	}

	private void aniStartNodeArrow()
	{
		this.myAniSeq = DOTween.Sequence();
		this.myAniSeq.Insert(0f, DOTween.To(() => this.startNodeArr.transform.localPosition, delegate(Vector3 x)
		{
			this.startNodeArr.transform.localPosition = x;
		}, new Vector3(this.startNodeArr.transform.localPosition.x, (float)this.myDoSAttack.nodeHeight / 2f + this.startNodeArr.rectTransform.sizeDelta.y / 2f, 0f), 0.6f).SetEase(Ease.OutSine));
		this.myAniSeq.Insert(0.6f, DOTween.To(() => this.startNodeArr.transform.localPosition, delegate(Vector3 x)
		{
			this.startNodeArr.transform.localPosition = x;
		}, new Vector3(this.startNodeArr.transform.localPosition.x, (float)this.myDoSAttack.nodeHeight, 0f), 0.6f).SetEase(Ease.OutSine));
		this.myAniSeq.SetLoops(-1);
		this.myAniSeq.Play<Sequence>();
	}

	private void aniStopNodeArrow()
	{
		this.myAniSeq.Kill(false);
		UnityEngine.Object.Destroy(this.startNodeArr);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.myNodeData.myType == nodeType.ACTIONFILLEDNODE)
		{
			if (this.actionNodeActive)
			{
				this.nodeImage.sprite = this.myDoSAttack.actionNodeActivatedHoverSprite;
			}
			else
			{
				this.nodeImage.sprite = this.myDoSAttack.actionNodeHoverSprite;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.myNodeData.myType == nodeType.ACTIONFILLEDNODE)
		{
			if (this.actionNodeActive)
			{
				this.nodeImage.sprite = this.myDoSAttack.actionNodeActivatedSprite;
			}
			else
			{
				this.nodeImage.sprite = this.myDoSAttack.actionNodeSprite;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.myNodeData.myType == nodeType.ACTIONFILLEDNODE && CrossPlatformInputManager.GetButtonDown("LeftClick"))
		{
			this.actionNodeDirection += 1;
			if (this.myDoSAttack.inHackerMode)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.myDoSAttack.ActionNodeClick, 0.25f, false);
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.myDoSAttack.ActionNodeClick, 0.15f, false);
			}
			if (this.actionNodeDirection > 4)
			{
				this.myDoSAttack.addNodeClickCount();
				this.actionNodeDirection = 1;
			}
			float num = this.mySetW / (float)this.myDoSAttack.rootNodeWidth;
			float num2 = this.mySetH / (float)this.myDoSAttack.rootNodeHeight;
			switch (this.actionNodeDirection)
			{
			case 1:
				this.actionArrowIMG.sprite = this.myDoSAttack.actionNodeUpA;
				this.actionArrowIMG.color = new Color(1f, 1f, 1f, 1f);
				this.actionArrowIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(this.myDoSAttack.actionNodeUpA.rect.width * num, this.myDoSAttack.actionNodeUpA.rect.height * num2);
				this.actionArrowIMG.GetComponent<RectTransform>().localPosition = new Vector3(this.mySetW / 2f - this.myDoSAttack.actionNodeUpA.rect.width * num / 2f, this.myDoSAttack.actionNodeUpA.rect.height * num / 2f - this.mySetH / 2f, 0f);
				break;
			case 2:
				this.actionArrowIMG.sprite = this.myDoSAttack.actionNodeRightA;
				this.actionArrowIMG.color = new Color(1f, 1f, 1f, 1f);
				this.actionArrowIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(this.myDoSAttack.actionNodeRightA.rect.width * num, this.myDoSAttack.actionNodeRightA.rect.height * num2);
				this.actionArrowIMG.GetComponent<RectTransform>().localPosition = new Vector3(this.mySetW / 2f - this.myDoSAttack.actionNodeRightA.rect.width * num / 2f, this.myDoSAttack.actionNodeRightA.rect.height * num / 2f - this.mySetH / 2f, 0f);
				break;
			case 3:
				this.actionArrowIMG.sprite = this.myDoSAttack.actionNodeDownA;
				this.actionArrowIMG.color = new Color(1f, 1f, 1f, 1f);
				this.actionArrowIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(this.myDoSAttack.actionNodeDownA.rect.width * num, this.myDoSAttack.actionNodeDownA.rect.height * num2);
				this.actionArrowIMG.GetComponent<RectTransform>().localPosition = new Vector3(this.mySetW / 2f - this.myDoSAttack.actionNodeDownA.rect.width * num / 2f, this.myDoSAttack.actionNodeDownA.rect.height * num / 2f - this.mySetH / 2f, 0f);
				break;
			case 4:
				this.actionArrowIMG.sprite = this.myDoSAttack.actionNodeLeftA;
				this.actionArrowIMG.color = new Color(1f, 1f, 1f, 1f);
				this.actionArrowIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(this.myDoSAttack.actionNodeLeftA.rect.width * num, this.myDoSAttack.actionNodeLeftA.rect.height * num2);
				this.actionArrowIMG.GetComponent<RectTransform>().localPosition = new Vector3(this.mySetW / 2f - this.myDoSAttack.actionNodeLeftA.rect.width * num / 2f, this.myDoSAttack.actionNodeLeftA.rect.height * num / 2f - this.mySetH / 2f, 0f);
				break;
			}
		}
	}

	public Image nodeBGImage;

	public Image nodeImage;

	public Image nodeHotImage;

	public DOSAttack myDoSAttack;

	public short actionNodeDirection;

	public bool actionNodeActive;

	public bool trollNode;

	public DOSAttack.node myNodeData;

	private Sequence myAniSeq;

	private Image startNodeArr;

	private Image actionArrowIMG;

	private float mySetW = 100f;

	private float mySetH = 100f;
}
