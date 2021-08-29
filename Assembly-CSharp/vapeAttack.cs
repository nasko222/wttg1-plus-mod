using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class vapeAttack : MonoBehaviour
{
	public void prepTwitchVapeAttack(string hackerName, string hackerLevel)
	{
		this.isTwitchAttack = true;
		this.twitchHackerName = hackerName;
		this.twitchHackerLevel = hackerLevel;
		this.prepVapeAttack(false);
	}

	public void prepVapeAttack()
	{
		this.prepVapeAttack(false);
	}

	public void prepVapeAttack(bool inTutMode)
	{
		int num = 0;
		if (this.inHackerMode)
		{
			for (int i = 0; i < this.VapeChains.Count; i++)
			{
				num = i;
				if (i < this.VapeChains.Count - 1 && this.hackerModeManager.getCurrentTMPGamePoints() < this.VapeChains[i + 1].pointsRequired)
				{
					i = this.VapeChains.Count;
				}
			}
			this.currentVapeChainIndex = num;
			this.vapeAttackFired = false;
			this.warmUpTime = this.VapeChains[num].warmUpTime;
			this.MATRIX_SIZE = this.VapeChains[num].matrixSize - 1;
			this.FREE_COUNT_PER = this.VapeChains[num].freeCountPer;
			this.GROUP_SIZE = this.VapeChains[num].groupSize;
			this.HAS_DEAD_NODES = this.VapeChains[num].hasDeadNodes;
			this.DEAD_NODE_SIZE = this.VapeChains[num].deadNodeSize;
			int num2 = Mathf.CeilToInt((float)((this.MATRIX_SIZE + 1) * (this.MATRIX_SIZE + 1)) / 2f);
			int num3 = Mathf.FloorToInt((float)num2 * this.FREE_COUNT_PER);
			this.VapeTime = (float)num3 * this.VapeChains[num].timePerBlock;
			if (this.currentVapeChainIndex >= this.VapeChains.Count - 1)
			{
				this.hackerModeManager.setChainLevelMaster();
			}
			else
			{
				this.hackerModeManager.updateChainLevel(this.currentVapeChainIndex);
			}
			this.timeIsFrozen = false;
			this.addSkillPoints = true;
			this.currentMoveCount = 0;
			this.hackerModeManager.addChainCount();
			this.hackerModeManager.updateChainCount();
		}
		else
		{
			for (int j = 0; j < this.VapeLevels.Count; j++)
			{
				num = j;
				if (j < this.VapeLevels.Count - 1 && GameManager.GetTheCloud().getPlayerSkillPoints3() < this.VapeLevels[j + 1].skillPointesRequired)
				{
					j = this.VapeLevels.Count;
				}
			}
			if (this.isTwitchAttack)
			{
				if (this.twitchHackerLevel == "1337")
				{
					num = this.VapeLevels.Count - 1;
				}
				else if (this.twitchHackerLevel == "SCRIPT")
				{
					num = this.VapeLevels.Count / 2;
				}
				else
				{
					num = 1;
				}
			}
			this.vapeAttackFired = false;
			this.MATRIX_SIZE = this.VapeLevels[num].matrixSize - 1;
			this.FREE_COUNT_PER = this.VapeLevels[num].freeCountPer;
			this.GROUP_SIZE = this.VapeLevels[num].groupSize;
			this.HAS_DEAD_NODES = this.VapeLevels[num].hasDeadNodes;
			this.DEAD_NODE_SIZE = this.VapeLevels[num].deadNodeSize;
			int num2 = Mathf.CeilToInt((float)((this.MATRIX_SIZE + 1) * (this.MATRIX_SIZE + 1)) / 2f);
			int num3 = Mathf.FloorToInt((float)num2 * this.FREE_COUNT_PER);
			this.VapeTime = (float)num3 * this.VapeLevels[num].timePerBlock;
			if (inTutMode)
			{
				this.VapeTime = 15f;
			}
			if (num >= this.VapeLevels.Count - 1 && !this.isTwitchAttack)
			{
				GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_THE_CLOUD_CHASER, true);
			}
		}
		if (Screen.height <= 1000)
		{
			this.vapeNodeWidth = 50;
			this.vapeNodeHeight = 50;
		}
		else if (Screen.height <= 1300)
		{
			if (this.MATRIX_SIZE >= 8)
			{
				this.vapeNodeWidth = 75;
				this.vapeNodeHeight = 75;
			}
			else
			{
				this.vapeNodeWidth = 75;
				this.vapeNodeHeight = 75;
			}
		}
		else
		{
			this.vapeNodeWidth = 100;
			this.vapeNodeHeight = 100;
		}
		this.prepPuzzle();
	}

	public void warmVapeAttack()
	{
		this.vapePresentSeq = DOTween.Sequence();
		this.vapePresentSeq.Insert(0f, DOTween.To(() => this.vapeNodeHolderCG.alpha, delegate(float x)
		{
			this.vapeNodeHolderCG.alpha = x;
		}, 1f, 0.5f).SetEase(Ease.OutQuad));
		this.vapePresentSeq.Play<Sequence>();
		int num = 72;
		float num2;
		if (this.inHackerMode)
		{
			num2 = 75f / (float)this.rootVapeNodeWidth;
		}
		else
		{
			num2 = (float)this.vapeNodeWidth / (float)this.rootVapeNodeWidth;
		}
		num = Mathf.RoundToInt((float)num * num2);
		TextGenerationSettings settings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		settings.textAnchor = TextAnchor.UpperCenter;
		settings.generateOutOfBounds = true;
		settings.generationExtents = new Vector2(50f, 20f);
		settings.pivot = Vector2.zero;
		settings.richText = true;
		settings.font = this.clockFont;
		settings.fontSize = num;
		settings.fontStyle = FontStyle.Normal;
		settings.lineSpacing = 1f;
		settings.scaleFactor = 1f;
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		settings.horizontalOverflow = HorizontalWrapMode.Wrap;
		this.clockText = new GameObject("clockText", new Type[]
		{
			typeof(Text)
		}).GetComponent<Text>();
		if (this.inHackerMode)
		{
			this.clockText.transform.SetParent(this.hackerModeManager.scoreHeaderObject.transform);
			this.clockText.transform.localPosition = new Vector3((float)Screen.width / 2f, -(this.clockText.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2f - 30f), 0f);
		}
		else
		{
			this.clockText.transform.SetParent(this.VapeHolder.transform);
			this.clockText.transform.localPosition = new Vector3(0f, this.VapeHolder.GetComponent<RectTransform>().sizeDelta.y / 2f + 75f, 0f);
		}
		this.clockText.font = this.clockFont;
		this.clockText.text = this.warmUpTime.ToString();
		this.clockText.color = this.clockColor;
		this.clockText.fontSize = num;
		this.clockText.rectTransform.sizeDelta = new Vector2(textGenerator.GetPreferredWidth(this.clockText.text, settings), textGenerator.GetPreferredHeight(this.clockText.text, settings));
		this.clockText.transform.localScale = new Vector3(1f, 1f, 1f);
		this.clockText.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		this.warmClockSeq = DOTween.Sequence();
		this.warmClockSeq.Insert(0f, DOTween.To(() => this.clockText.transform.localScale, delegate(Vector3 x)
		{
			this.clockText.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0f));
		this.warmClockSeq.Insert(0.1f, DOTween.To(() => this.clockText.transform.localScale, delegate(Vector3 x)
		{
			this.clockText.transform.localScale = x;
		}, new Vector3(0.33f, 0.33f, 0.33f), 0.9f).SetEase(Ease.Linear));
		this.warmClockSeq.SetLoops(5);
		this.warmClockSeq.Play<Sequence>();
		this.clockTimeStamp = Time.time;
		this.clockMicroTimeStamp = Time.time;
		this.clockMicroCount = this.warmUpTime;
		this.warmClockActive = true;
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.CountDownTick1, 0.55f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.CountDownTick1, 0.7f, false);
		}
	}

	public void fireVapeAttack()
	{
		UnityEngine.Object.Destroy(this.clockText.gameObject);
		float num;
		float num2;
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.CountDownTick2, 0.55f, false);
			num = 75f / (float)this.rootVapeNodeWidth;
			num2 = 75f / (float)this.rootVapeNodeHeight;
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.CountDownTick2, 0.7f, false);
			num = (float)this.vapeNodeWidth / (float)this.rootVapeNodeWidth;
			num2 = (float)this.vapeNodeHeight / (float)this.rootVapeNodeHeight;
		}
		this.finalCountDownFired = false;
		this.curVapeClockObject = UnityEngine.Object.Instantiate<GameObject>(this.VapeClockObject);
		if (this.inHackerMode)
		{
			this.curVapeClockObject.transform.SetParent(this.hackerModeManager.scoreHeaderObject.transform);
			this.curVapeClockObject.transform.localPosition = new Vector3((float)Screen.width / 2f, -(this.curVapeClockObject.GetComponent<RectTransform>().sizeDelta.y / 2f) + 5f, 0f);
		}
		else
		{
			this.curVapeClockObject.transform.SetParent(this.VapeHolder.transform);
			this.curVapeClockObject.transform.localPosition = new Vector3(0f, this.VapeHolder.GetComponent<RectTransform>().sizeDelta.y / 2f + (float)this.vapeNodeHeight, 0f);
		}
		this.curVapeClockObject.transform.localScale = new Vector3(1f, 1f, 1f);
		this.curVapeClockObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		this.curVapeClockObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.curVapeClockObject.GetComponent<Image>().sprite.rect.width * num, this.curVapeClockObject.GetComponent<Image>().sprite.rect.height * num2);
		this.curVapeClockObject.GetComponent<DOSClock>().DOSClockImage.GetComponent<RectTransform>().sizeDelta = new Vector2(this.curVapeClockObject.GetComponent<DOSClock>().DOSClockImage.sprite.rect.width * num, this.curVapeClockObject.GetComponent<DOSClock>().DOSClockImage.sprite.rect.height * num2);
		this.VapeAttackClockSeq = DOTween.Sequence();
		this.VapeAttackClockSeq.Insert(0f, DOTween.To(() => this.curVapeClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount, delegate(float x)
		{
			this.curVapeClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount = x;
		}, 0f, this.VapeTime).SetEase(Ease.Linear));
		this.VapeAttackClockSeq.Play<Sequence>();
		this.VapeGameTimeStamp = Time.time;
		this.vapeIsHot = true;
		this.vapeAttackFired = true;
	}

	public void vapeNodeAction(vapeAttack.vapeNode vnData)
	{
		bool flag = false;
		List<int> list = new List<int>();
		if (this.vapeAttackFired && vnData.myType != vapeNodeType.DEADNODE && vnData.myType != vapeNodeType.GOODNODE)
		{
			if (!this.curActiveNodeSet)
			{
				if (vnData.myType != vapeNodeType.BLANKNODE)
				{
					this.curActiveNodeSet = true;
					this.curActiveNode = vnData.getMyPOS();
				}
			}
			else if (vnData.myType == vapeNodeType.BLANKNODE)
			{
				if (this.vapeNodeObjects.ContainsKey(this.curActiveNode) && this.vapeNodeObjects.ContainsKey(vnData.getMyPOS()))
				{
					this.vapeNodeObjects[this.curActiveNode].GetComponent<VapeNodeObject>().updateMyType(vapeNodeType.BLANKNODE);
					this.vapeNodeObjects[this.curActiveNode].GetComponent<VapeNodeObject>().clearActiveState();
					this.vapeNodeObjects[vnData.getMyPOS()].GetComponent<VapeNodeObject>().updateMyType(vapeNodeType.BOXNODE);
					this.vapeNodeObjects[vnData.getMyPOS()].GetComponent<VapeNodeObject>().clearActiveState();
					flag = true;
				}
				this.curActiveNodeSet = false;
				this.curActiveNode = string.Empty;
			}
			else
			{
				this.vapeNodeObjects[this.curActiveNode].GetComponent<VapeNodeObject>().clearActiveState();
				this.curActiveNodeSet = false;
				this.curActiveNode = string.Empty;
			}
			if (flag)
			{
				if (this.inHackerMode)
				{
					this.currentMoveCount++;
				}
				list = this.getCurActiveBoxNodes();
				if (list.Count > 0)
				{
					bool flag2 = true;
					for (int i = 0; i < list.Count; i++)
					{
						if (this.checkBoxNodeIsGood(this.masterVapeNodes[list[i]]))
						{
							if (this.masterVapeNodes[list[i]].myType != vapeNodeType.GOODNODE)
							{
								this.vapeNodeObjects[this.masterVapeNodes[list[i]].getMyPOS()].GetComponent<VapeNodeObject>().setAsGoodNode();
							}
						}
						else
						{
							this.vapeNodeObjects[this.masterVapeNodes[list[i]].getMyPOS()].GetComponent<VapeNodeObject>().updateMyType(vapeNodeType.BOXNODE);
							flag2 = false;
						}
					}
					if (flag2)
					{
						this.VapeAttackFailed();
					}
				}
			}
		}
	}

	public int getNextChainPoints()
	{
		if (this.inHackerMode)
		{
			return this.VapeChains[this.currentVapeChainIndex + 1].pointsRequired;
		}
		return 0;
	}

	public void skipCurrentLevel()
	{
		if (this.inHackerMode)
		{
			this.VapeAttackFailed();
		}
	}

	public void freezeTime()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.ClockAlmostUp.name);
		GameManager.AudioSlinger.ChangeGlobalPitch(AudioLayer.MUSIC, 0.8f);
		this.finalCountDownFired = true;
		this.VapeAttackClockSeq.Pause<Sequence>();
		this.timeIsFrozen = true;
		GameManager.TimeSlinger.FireTimer(30f, new Action(this.unFreezeTime), "cloudFreezeTimer");
	}

	private void prepPuzzle()
	{
		int num = 0;
		int num2 = 0;
		List<int> list = new List<int>();
		this.masterVapeNodes = new List<vapeAttack.vapeNode>();
		int num3 = Mathf.CeilToInt((float)((this.MATRIX_SIZE + 1) * (this.MATRIX_SIZE + 1)) / 2f);
		int num4 = Mathf.FloorToInt((float)num3 * this.FREE_COUNT_PER);
		for (int i = 0; i < (int)((this.MATRIX_SIZE + 1) * (this.MATRIX_SIZE + 1)); i++)
		{
			short x = (short)(i % (int)(this.MATRIX_SIZE + 1));
			short y = (short)Mathf.FloorToInt((float)(i / (int)(this.MATRIX_SIZE + 1)));
			this.masterVapeNodes.Add(new vapeAttack.vapeNode(vapeNodeType.BLANKNODE, new vapeAttack.vapeNodePosition(x, y), this.blankNodeSprite));
		}
		int max = Mathf.CeilToInt(((float)this.MATRIX_SIZE + 1f) * ((float)this.MATRIX_SIZE + 1f) / Mathf.Ceil((float)num4 / (float)this.GROUP_SIZE));
		int num5 = UnityEngine.Random.Range(1, max);
		for (int j = 0; j < num4; j++)
		{
			int num6 = num + j % (int)this.GROUP_SIZE;
			if (num6 < this.masterVapeNodes.Count)
			{
				this.masterVapeNodes[num6].updateMyInfo(vapeNodeType.BOXNODE, this.boxNodeSprite);
				num2++;
				if (num2 > (int)(this.GROUP_SIZE - 1))
				{
					int num7 = num + (int)this.GROUP_SIZE + num5;
					if (num7 + (int)this.GROUP_SIZE < this.masterVapeNodes.Count)
					{
						num = num7;
					}
					else
					{
						num += (int)this.GROUP_SIZE;
					}
					num2 = 0;
					num5 = UnityEngine.Random.Range(1, max);
				}
			}
		}
		if (this.HAS_DEAD_NODES)
		{
			list = this.getCurBlankBoxNodes();
			for (int k = 0; k < (int)this.DEAD_NODE_SIZE; k++)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				this.masterVapeNodes[list[index]].updateMyInfo(vapeNodeType.DEADNODE, this.deadNodeSprite);
				list.RemoveAt(index);
			}
		}
		this.drawPuzzle();
	}

	private void drawPuzzle()
	{
		this.vapeNodeObjects = new Dictionary<string, GameObject>();
		float num = (float)(this.vapeNodeWidth * (int)(this.MATRIX_SIZE + 1));
		float num2 = (float)(this.vapeNodeHeight * (int)(this.MATRIX_SIZE + 1));
		float x = 0f - num / 2f;
		float y = num2 / 2f;
		this.VapeHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(num, num2);
		this.vapeNodeHolder = new GameObject("VapeNodeHolder", new Type[]
		{
			typeof(RectTransform)
		}).GetComponent<RectTransform>();
		this.vapeNodeHolder.GetComponent<RectTransform>().SetParent(this.VapeHolder.transform);
		this.vapeNodeHolderCG.alpha = 0f;
		this.vapeNodeHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(num, num2);
		this.vapeNodeHolder.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
		this.vapeNodeHolder.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
		this.vapeNodeHolder.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		this.vapeNodeHolder.transform.localScale = new Vector3(1f, 1f, 1f);
		this.vapeNodeHolder.transform.localPosition = new Vector3(x, y, 0f);
		this.nodeBGIMG = new GameObject("nodeBGIMG", new Type[]
		{
			typeof(Image)
		}).GetComponent<Image>();
		this.nodeBGIMG.transform.SetParent(this.vapeNodeHolder.transform);
		this.nodeBGIMG.sprite = this.pixelSprite;
		this.nodeBGIMG.type = Image.Type.Filled;
		this.nodeBGIMG.fillMethod = Image.FillMethod.Radial360;
		this.nodeBGIMG.fillAmount = 1f;
		this.nodeBGIMG.color = this.nodeBorderColor;
		this.nodeBGIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(num, num2);
		this.nodeBGIMG.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
		this.nodeBGIMG.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
		this.nodeBGIMG.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		this.nodeBGIMG.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.nodeBGIMG.transform.localScale = new Vector3(1f, 1f, 1f);
		for (int i = 0; i < this.masterVapeNodes.Count; i++)
		{
			float setX = (float)(i % (int)(this.MATRIX_SIZE + 1) * this.vapeNodeWidth);
			float setY = (float)(Mathf.FloorToInt((float)(i / (int)(this.MATRIX_SIZE + 1))) * -(float)this.vapeNodeHeight);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.VapeNodeObject);
			gameObject.GetComponent<VapeNodeObject>().blankNodeSprite = this.blankNodeSprite;
			gameObject.GetComponent<VapeNodeObject>().boxNodeSprite = this.boxNodeSprite;
			gameObject.GetComponent<VapeNodeObject>().goodNodeSprite = this.goodNodeSprite;
			gameObject.GetComponent<VapeNodeObject>().deadNodeSprite = this.deadNodeSprite;
			gameObject.GetComponent<VapeNodeObject>().boxNodeHoverSprite = this.boxNodeHoverSprite;
			gameObject.GetComponent<VapeNodeObject>().boxNodeActiveSprite = this.boxNodeActiveSprite;
			gameObject.GetComponent<VapeNodeObject>().blankNodeHoverSprite = this.blankNodeHoverSprite;
			gameObject.GetComponent<VapeNodeObject>().goodNodeActiveSprite = this.goodNodeActiveSprite;
			gameObject.transform.SetParent(this.vapeNodeHolder.transform);
			gameObject.GetComponent<VapeNodeObject>().buildMe(this, this.masterVapeNodes[i], setX, setY, (float)this.vapeNodeWidth, (float)this.vapeNodeHeight);
			this.vapeNodeObjects.Add(gameObject.GetComponent<VapeNodeObject>().myVapeNodeData.myPOS.x.ToString() + ":" + gameObject.GetComponent<VapeNodeObject>().myVapeNodeData.myPOS.y.ToString(), gameObject);
		}
		this.curActiveNodeSet = false;
		this.curActiveNode = string.Empty;
	}

	private List<int> getCurBlankBoxNodes()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.masterVapeNodes.Count; i++)
		{
			if (this.masterVapeNodes[i].myType == vapeNodeType.BLANKNODE)
			{
				list.Add(i);
			}
		}
		return list;
	}

	private List<int> getCurActiveBoxNodes()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.masterVapeNodes.Count; i++)
		{
			if (this.masterVapeNodes[i].myType == vapeNodeType.BOXNODE || this.masterVapeNodes[i].myType == vapeNodeType.GOODNODE)
			{
				list.Add(i);
			}
		}
		return list;
	}

	private bool checkBoxNodeIsGood(vapeAttack.vapeNode theVapeNode)
	{
		bool result = true;
		if (theVapeNode.myPOS.x - 1 >= 0 && this.vapeNodeObjects.ContainsKey(((int)(theVapeNode.myPOS.x - 1) + ":" + theVapeNode.myPOS.y).ToString()) && this.vapeNodeObjects[((int)(theVapeNode.myPOS.x - 1) + ":" + theVapeNode.myPOS.y).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.BLANKNODE && this.vapeNodeObjects[((int)(theVapeNode.myPOS.x - 1) + ":" + theVapeNode.myPOS.y).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.DEADNODE)
		{
			result = false;
		}
		if (theVapeNode.myPOS.x + 1 <= this.MATRIX_SIZE && this.vapeNodeObjects.ContainsKey(((int)(theVapeNode.myPOS.x + 1) + ":" + theVapeNode.myPOS.y).ToString()) && this.vapeNodeObjects[((int)(theVapeNode.myPOS.x + 1) + ":" + theVapeNode.myPOS.y).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.BLANKNODE && this.vapeNodeObjects[((int)(theVapeNode.myPOS.x + 1) + ":" + theVapeNode.myPOS.y).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.DEADNODE)
		{
			result = false;
		}
		if (theVapeNode.myPOS.y - 1 >= 0 && this.vapeNodeObjects.ContainsKey((theVapeNode.myPOS.x + ":" + (int)(theVapeNode.myPOS.y - 1)).ToString()) && this.vapeNodeObjects[(theVapeNode.myPOS.x + ":" + (int)(theVapeNode.myPOS.y - 1)).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.BLANKNODE && this.vapeNodeObjects[(theVapeNode.myPOS.x + ":" + (int)(theVapeNode.myPOS.y - 1)).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.DEADNODE)
		{
			result = false;
		}
		if (theVapeNode.myPOS.y + 1 <= this.MATRIX_SIZE && this.vapeNodeObjects.ContainsKey((theVapeNode.myPOS.x + ":" + (int)(theVapeNode.myPOS.y + 1)).ToString()) && this.vapeNodeObjects[(theVapeNode.myPOS.x + ":" + (int)(theVapeNode.myPOS.y + 1)).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.BLANKNODE && this.vapeNodeObjects[(theVapeNode.myPOS.x + ":" + (int)(theVapeNode.myPOS.y + 1)).ToString()].GetComponent<VapeNodeObject>().myVapeNodeData.myType != vapeNodeType.DEADNODE)
		{
			result = false;
		}
		return result;
	}

	private void VapeAttackFailed()
	{
		if (this.inHackerMode)
		{
			GameManager.TimeSlinger.KillTimerWithID("cloudFreezeTimer");
			GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.ClockAlmostUp.name);
			GameManager.AudioSlinger.ChangeGlobalPitch(AudioLayer.MUSIC, 1f);
		}
		else
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.ClockAlmostUp.name);
		}
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.HMPuzzlePass, 0.65f, false);
			this.tallyBonusPoints();
		}
		else if ((double)(Time.time - this.VapeGameTimeStamp) <= (double)this.VapeTime * 0.45)
		{
			GameManager.GetTheCloud().addPlayerSkillPoints3((int)this.SkillPointsA);
		}
		else
		{
			GameManager.GetTheCloud().addPlayerSkillPoints3((int)this.SkillPointsB);
		}
		this.masterVapeNodes.Clear();
		this.vapeNodeObjects.Clear();
		this.curActiveNodeSet = false;
		this.curActiveNode = string.Empty;
		this.vapeAttackFired = false;
		this.vapeIsHot = false;
		this.VapeAttackClockSeq.Kill(false);
		UnityEngine.Object.Destroy(this.curVapeClockObject);
		if (!this.inHackerMode)
		{
			GameManager.GetDOSTwitch().DismissTwitchHacker();
		}
		this.VapeAttackOverSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.VapeAttackFailOver));
		this.VapeAttackOverSeq.Insert(0f, DOTween.To(() => this.vapeNodeHolderCG.alpha, delegate(float x)
		{
			this.vapeNodeHolderCG.alpha = x;
		}, 0f, 0.4f).SetEase(Ease.OutSine));
		this.VapeAttackOverSeq.Play<Sequence>();
	}

	private void VapeAttackSucceeded()
	{
		if (this.inHackerMode)
		{
			GameManager.TimeSlinger.KillTimerWithID("cloudFreezeTimer");
			GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.ClockAlmostUp.name);
			GameManager.AudioSlinger.ChangeGlobalPitch(AudioLayer.MUSIC, 1f);
		}
		else
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.ClockAlmostUp.name);
		}
		if (this.inHackerMode && this.hackerModeManager.FireAShell())
		{
			UnityEngine.Object.Destroy(this.vapeNodeHolder.gameObject);
			this.prepVapeAttack();
			this.warmVapeAttack();
		}
		else if (this.inHackerMode)
		{
			this.VapeAttackOverSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.VapeAttackSucOver));
		}
		this.masterVapeNodes.Clear();
		this.vapeNodeObjects.Clear();
		this.curActiveNodeSet = false;
		this.curActiveNode = string.Empty;
		this.vapeAttackFired = false;
		this.vapeIsHot = false;
		this.VapeAttackClockSeq.Kill(false);
		UnityEngine.Object.Destroy(this.curVapeClockObject);
		GameManager.GetDOSTwitch().DismissTwitchHacker();
		this.VapeAttackOverSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.VapeAttackSucOver));
		this.VapeAttackOverSeq.Insert(0f, DOTween.To(() => this.vapeNodeHolderCG.alpha, delegate(float x)
		{
			this.vapeNodeHolderCG.alpha = x;
		}, 0f, 0.4f).SetEase(Ease.OutSine));
		this.VapeAttackOverSeq.Play<Sequence>();
	}

	private void VapeAttackFailOver()
	{
		UnityEngine.Object.Destroy(this.vapeNodeHolder.gameObject);
		this.VapeHolder.GetComponent<CanvasGroup>().alpha = 0f;
		if (this.isTwitchAttack)
		{
			if (this.twitchHackerLevel == "1337")
			{
				GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("@" + this.twitchHackerName + " HAS FAILED! - TIMEOUT! 5 MINS", 10f);
				GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("/timeout " + this.twitchHackerName + " 300", 10f);
			}
			else if (this.twitchHackerLevel == "SCRIPT")
			{
				GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("@" + this.twitchHackerName + " HAS FAILED! - TIMEOUT! 1 MIN", 10f);
				GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("/timeout " + this.twitchHackerName + " 60", 10f);
			}
			else
			{
				GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("@" + this.twitchHackerName + " HAS FAILED! - TIMEOUT! 10 SECONDS", 10f);
				GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("/timeout " + this.twitchHackerName + " 10", 10f);
			}
			this.isTwitchAttack = false;
			this.twitchHackerLevel = string.Empty;
			this.twitchHackerName = string.Empty;
		}
		if (this.inHackerMode)
		{
			GameManager.TimeSlinger.FireTimer(0.4f, new Action(this.prepVapeAttack));
			GameManager.TimeSlinger.FireTimer(0.8f, new Action(this.warmVapeAttack));
		}
		else
		{
			this.hackerManager.VAttackBlocked();
		}
	}

	private void VapeAttackSucOver()
	{
		UnityEngine.Object.Destroy(this.vapeNodeHolder.gameObject);
		this.VapeHolder.GetComponent<CanvasGroup>().alpha = 0f;
		if (this.isTwitchAttack)
		{
			GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("@" + this.twitchHackerName + " IS 1337!", 10f);
			this.isTwitchAttack = false;
			this.twitchHackerLevel = string.Empty;
			this.twitchHackerName = string.Empty;
		}
		if (this.inHackerMode)
		{
			if (this.hackerModeManager.FireAShell())
			{
				GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.prepVapeAttack));
				GameManager.TimeSlinger.FireTimer(1.2f, new Action(this.warmVapeAttack));
			}
			else
			{
				this.hackerModeManager.triggerGameOver();
				this.hackerModeManager.resetVapeAttack();
			}
		}
		else
		{
			this.hackerManager.VAttackedPassed();
		}
	}

	private void tallyBonusPoints()
	{
		if (this.inHackerMode)
		{
			int completeBonusPoints = this.VapeChains[this.currentVapeChainIndex].completeBonusPoints;
			int timePoints = 0;
			int skillPoints = 0;
			if (Time.time - this.VapeGameTimeStamp <= this.VapeTime * (1f - this.VapeChains[this.currentVapeChainIndex].timeBonusRange))
			{
				timePoints = this.VapeChains[this.currentVapeChainIndex].timeBonusPoints;
			}
			if (this.currentMoveCount > (int)this.VapeChains[this.currentVapeChainIndex].maxNumOfMoves)
			{
				this.addSkillPoints = false;
			}
			if (this.addSkillPoints)
			{
				skillPoints = this.VapeChains[this.currentVapeChainIndex].skillBonusPoints;
			}
			this.hackerModeManager.addGamePoints(completeBonusPoints, timePoints, skillPoints, (int)this.VapeChains[this.currentVapeChainIndex].bonusMultiplier);
		}
	}

	private void unFreezeTime()
	{
		GameManager.AudioSlinger.ChangeGlobalPitch(AudioLayer.MUSIC, 1f);
		this.VapeAttackClockSeq.Play<Sequence>();
		this.timeIsFrozen = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.warmClockActive)
		{
			if (Time.time - this.clockTimeStamp >= this.warmUpTime)
			{
				this.warmClockActive = false;
				this.fireVapeAttack();
			}
			else if (Time.time - this.clockMicroTimeStamp >= 1f)
			{
				if (this.inHackerMode)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.CountDownTick1, 0.7f, false);
				}
				else
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.CountDownTick1, 0.7f, false);
				}
				this.clockMicroCount -= 1f;
				this.clockMicroTimeStamp = Time.time;
				this.clockText.text = this.clockMicroCount.ToString();
			}
		}
		else if (this.vapeIsHot)
		{
			if (this.inHackerMode && this.timeIsFrozen)
			{
				this.VapeGameTimeStamp = Time.time;
			}
			if (Time.time - this.VapeGameTimeStamp >= this.VapeTime - this.ClockAlmostUp.length && !this.finalCountDownFired)
			{
				this.finalCountDownFired = true;
				if (this.inHackerMode)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.ClockAlmostUp, 0.35f, false);
				}
				else
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.ClockAlmostUp, 0.35f, false);
				}
			}
			if (Time.time - this.VapeGameTimeStamp >= this.VapeTime)
			{
				this.vapeIsHot = false;
				this.VapeAttackSucceeded();
			}
		}
		if (this.inHackerMode && this.hackerModeGameActive)
		{
			if (this.vapeIsHot)
			{
				this.hackerModeManager.gameIsHot = true;
			}
			else
			{
				this.hackerModeManager.gameIsHot = false;
			}
		}
	}

	public bool inHackerMode;

	public bool hackerModeGameActive;

	public HackerManager hackerManager;

	public HackerModeManager hackerModeManager;

	[Range(1f, 10f)]
	public float warmUpTime = 5f;

	[Range(5f, 120f)]
	public float VapeTime = 60f;

	[Range(2f, 11f)]
	public short MATRIX_SIZE = 2;

	[Range(0f, 1f)]
	public float FREE_COUNT_PER = 0.5f;

	[Range(2f, 6f)]
	public short GROUP_SIZE = 3;

	public bool HAS_DEAD_NODES;

	[Range(1f, 5f)]
	public short DEAD_NODE_SIZE = 1;

	public int rootVapeNodeWidth = 100;

	public int rootVapeNodeHeight = 100;

	public int vapeNodeWidth = 100;

	public int vapeNodeHeight = 100;

	public short SkillPointsA = 5;

	public short SkillPointsB = 2;

	public AudioClip boxNodeHoverClip;

	public AudioClip boxNodeActiveClip;

	public AudioClip blankNodeHoverClip;

	public AudioClip goodNodeActiveClip;

	public AudioClip CountDownTick1;

	public AudioClip CountDownTick2;

	public AudioClip ClockAlmostUp;

	public AudioClip HMPuzzlePass;

	public Font clockFont;

	public Color clockColor;

	public GameObject VapeHolder;

	public CanvasGroup vapeNodeHolderCG;

	public GameObject VapeNodeObject;

	public GameObject VapeClockObject;

	public Sprite blankNodeSprite;

	public Sprite boxNodeSprite;

	public Sprite goodNodeSprite;

	public Sprite deadNodeSprite;

	public Sprite boxNodeHoverSprite;

	public Sprite boxNodeActiveSprite;

	public Sprite blankNodeHoverSprite;

	public Sprite goodNodeActiveSprite;

	public Color nodeBorderColor;

	public Sprite pixelSprite;

	public bool curActiveNodeSet;

	public bool vapeAttackFired;

	public List<VLevelDefinition> VapeLevels;

	public List<VapeChainDefinition> VapeChains;

	private RectTransform vapeNodeHolder;

	private List<vapeAttack.vapeNode> masterVapeNodes;

	private Dictionary<string, GameObject> vapeNodeObjects;

	private string curActiveNode = string.Empty;

	private Sequence vapePresentSeq;

	private Text clockText;

	private float clockTimeStamp;

	private float clockMicroTimeStamp;

	private float clockMicroCount;

	private float VapeGameTimeStamp;

	private bool warmClockActive;

	private bool finalCountDownFired;

	private bool vapeIsHot;

	private Sequence warmClockSeq;

	private Sequence VapeAttackClockSeq;

	private Sequence VapeAttackOverSeq;

	private GameObject curVapeClockObject;

	private Image nodeBGIMG;

	private bool addSkillPoints;

	private bool timeIsFrozen;

	private int currentVapeChainIndex;

	private int currentMoveCount;

	private bool isTwitchAttack;

	private string twitchHackerName;

	private string twitchHackerLevel;

	public struct vapeNodePosition
	{
		public vapeNodePosition(short x, short y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			vapeAttack.vapeNodePosition vapeNodePosition = (vapeAttack.vapeNodePosition)obj;
			return vapeNodePosition != null && (this.x == vapeNodePosition.x && this.y == vapeNodePosition.y);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool Equals(vapeAttack.vapeNodePosition o)
		{
			return this.x == o.x && this.y == o.y;
		}

		public static bool operator ==(vapeAttack.vapeNodePosition lhs, vapeAttack.vapeNodePosition rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(vapeAttack.vapeNodePosition lhs, vapeAttack.vapeNodePosition rhs)
		{
			return !lhs.Equals(rhs);
		}

		public short x;

		public short y;
	}

	public class vapeNode
	{
		public vapeNode()
		{
		}

		public vapeNode(vapeNodeType setType, vapeAttack.vapeNodePosition setPOS, Sprite setSprite)
		{
			this.myType = setType;
			this.myPOS = setPOS;
			this.mySprite = setSprite;
		}

		public void updateMyInfo(vapeNodeType newType, Sprite newSprite)
		{
			this.myType = newType;
			this.mySprite = newSprite;
		}

		public string getMyPOS()
		{
			return this.myPOS.x.ToString() + ":" + this.myPOS.y.ToString();
		}

		public void myInfo()
		{
			Debug.Log("My Type: " + this.myType);
			Debug.Log(string.Concat(new object[]
			{
				"My POS: ",
				this.myPOS.x,
				":",
				this.myPOS.y
			}));
			Debug.Log("My Sprite:" + this.mySprite.name);
		}

		public vapeNodeType myType;

		public vapeAttack.vapeNodePosition myPOS;

		public Sprite mySprite;
	}
}
