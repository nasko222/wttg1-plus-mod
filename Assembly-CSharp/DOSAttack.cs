using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class DOSAttack : MonoBehaviour
{
	public bool getNodeWithKey(string theKey, out DOSAttack.node returnNode)
	{
		returnNode = new DOSAttack.node();
		if (this.nodeLookUp.ContainsKey(theKey))
		{
			returnNode = this.masterNodes[this.nodeLookUp[theKey]];
			return true;
		}
		return false;
	}

	public short getMatrixCount()
	{
		return (this.MATRIX_SIZE + 1) * (this.MATRIX_SIZE + 1);
	}

	public int getNextChainPoints()
	{
		if (this.inHackerMode)
		{
			return this.DOSChains[this.currentDOSChainIndex + 1].pointsRequired;
		}
		return 0;
	}

	public void skipCurrentLevel()
	{
		if (this.inHackerMode)
		{
			this.DOSiSHot = false;
			this.DOSAttackFailed();
		}
	}

	public void prepDOSTwitchAttack(string hackerName, string hackerLevel)
	{
		this.isTwitchAttack = true;
		this.twitchHackerName = hackerName;
		this.twitchHackerLevel = hackerLevel;
		this.prepDOSAttack(false);
	}

	public void prepDOSAttack()
	{
		this.prepDOSAttack(false);
	}

	public void prepDOSAttack(bool inTutMode)
	{
		int num = 0;
		if (this.inHackerMode)
		{
			for (int i = 0; i < this.DOSChains.Count; i++)
			{
				num = i;
				if (i < this.DOSChains.Count - 1 && this.hackerModeManager.getCurrentTMPGamePoints() < this.DOSChains[i + 1].pointsRequired)
				{
					i = this.DOSChains.Count;
				}
			}
			this.currentDOSChainIndex = num;
			this.MATRIX_SIZE = this.DOSChains[num].matrixSize - 1;
			this.ACTION_BLOCK_SIZE = this.DOSChains[num].actionBlockSize;
			this.warmUpTime = this.DOSChains[num].warmUpTime;
			this.hotTime = this.DOSChains[num].hotTime;
			this.DOSTime = (float)(this.DOSChains[num].matrixSize * this.DOSChains[num].matrixSize) * this.hotTime * this.DOSChains[num].gameTimeModifier;
			this.trollNodesActive = this.DOSChains[num].trollNodesActive;
			if (this.currentDOSChainIndex >= this.DOSChains.Count - 1)
			{
				this.hackerModeManager.setChainLevelMaster();
			}
			else
			{
				this.hackerModeManager.updateChainLevel(this.currentDOSChainIndex);
			}
			this.addSkillPoints = true;
			this.nodeClickCount = 0;
			this.maxNodeClickCount = (int)this.DOSChains[num].maxCompleteNodeTurn;
			this.hackerModeManager.addChainCount();
			this.hackerModeManager.updateChainCount();
		}
		else
		{
			for (int j = 0; j < this.DOSLevels.Count; j++)
			{
				num = j;
				if (j < this.DOSLevels.Count - 1 && this.theCloud.getPlayerSkillPoints() < this.DOSLevels[j + 1].skillPointesRequired)
				{
					j = this.DOSLevels.Count;
				}
			}
			if (this.isTwitchAttack)
			{
				if (this.twitchHackerLevel == "1337")
				{
					num = this.DOSLevels.Count - 1;
				}
				else if (this.twitchHackerLevel == "SCRIPT")
				{
					num = this.DOSLevels.Count / 2;
				}
				else
				{
					num = 1;
				}
			}
			this.MATRIX_SIZE = this.DOSLevels[num].matrixSize - 1;
			this.ACTION_BLOCK_SIZE = this.DOSLevels[num].actionBlockSize;
			this.hotTime = this.DOSLevels[num].hotTime;
			this.DOSTime = (float)(this.DOSLevels[num].matrixSize * this.DOSLevels[num].matrixSize) * this.hotTime * this.DOSLevels[num].gameTimeModifier;
			this.trollNodesActive = this.DOSLevels[num].trollNodesActive;
			if (inTutMode)
			{
				this.MATRIX_SIZE = 3;
				this.ACTION_BLOCK_SIZE = 2;
			}
			if (num >= this.DOSLevels.Count - 1 && !this.isTwitchAttack)
			{
				GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_THE_DENIER, true);
			}
		}
		if (Screen.height <= 1000)
		{
			this.nodeWidth = 50;
			this.nodeHeight = 50;
		}
		else if (Screen.height <= 1300)
		{
			this.nodeWidth = 75;
			this.nodeHeight = 75;
		}
		else
		{
			this.nodeWidth = 100;
			this.nodeHeight = 100;
		}
		this.prepPuzzle();
	}

	public void warmDOSAttack()
	{
		int num = 72;
		float num2;
		if (this.inHackerMode)
		{
			num2 = 75f / (float)this.rootNodeWidth;
		}
		else
		{
			num2 = (float)this.nodeWidth / (float)this.rootNodeWidth;
		}
		num = Mathf.RoundToInt((float)num * num2);
		TextGenerationSettings textGenerationSettings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		textGenerationSettings.textAnchor = 1;
		textGenerationSettings.generateOutOfBounds = true;
		textGenerationSettings.generationExtents = new Vector2(50f, 20f);
		textGenerationSettings.pivot = Vector2.zero;
		textGenerationSettings.richText = true;
		textGenerationSettings.font = this.clockFont;
		textGenerationSettings.fontSize = num;
		textGenerationSettings.fontStyle = 0;
		textGenerationSettings.lineSpacing = 1f;
		textGenerationSettings.scaleFactor = 1f;
		textGenerationSettings.verticalOverflow = 1;
		textGenerationSettings.horizontalOverflow = 0;
		this.clockTextHolder = new GameObject("clockTextHolder", new Type[]
		{
			typeof(RectTransform)
		}).GetComponent<RectTransform>();
		if (this.inHackerMode)
		{
			this.clockTextHolder.transform.SetParent(this.hackerModeManager.scoreHeaderObject.transform);
			this.clockTextHolder.sizeDelta = new Vector2(textGenerator.GetPreferredWidth(this.warmUpTime.ToString(), textGenerationSettings), textGenerator.GetPreferredHeight(this.warmUpTime.ToString(), textGenerationSettings));
			this.clockTextHolder.anchorMin = new Vector2(0f, 1f);
			this.clockTextHolder.anchorMax = new Vector2(0f, 1f);
			this.clockTextHolder.pivot = new Vector2(0f, 1f);
			this.clockTextHolder.localPosition = new Vector3((float)Screen.width / 2f - this.clockTextHolder.sizeDelta.x / 2f, 8f, 0f);
			this.clockTextHolder.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			this.clockTextHolder.transform.SetParent(this.DOSHolder.transform);
			this.clockTextHolder.sizeDelta = new Vector2(textGenerator.GetPreferredWidth(this.warmUpTime.ToString(), textGenerationSettings), textGenerator.GetPreferredHeight(this.warmUpTime.ToString(), textGenerationSettings));
			this.clockTextHolder.anchorMin = new Vector2(0f, 1f);
			this.clockTextHolder.anchorMax = new Vector2(0f, 1f);
			this.clockTextHolder.pivot = new Vector2(0f, 1f);
			this.clockTextHolder.localPosition = new Vector3(0f - this.clockTextHolder.sizeDelta.x / 2f, this.nodeHolder.localPosition.y + this.clockTextHolder.sizeDelta.y + (float)this.nodeHeight, 0f);
			this.clockTextHolder.localScale = new Vector3(1f, 1f, 1f);
		}
		this.clockText = new GameObject("clockText", new Type[]
		{
			typeof(Text)
		}).GetComponent<Text>();
		this.clockText.font = this.clockFont;
		this.clockText.text = this.warmUpTime.ToString();
		this.clockText.color = this.clockColor;
		this.clockText.fontSize = num;
		this.clockText.transform.SetParent(this.clockTextHolder.transform);
		this.clockText.rectTransform.sizeDelta = new Vector2(textGenerator.GetPreferredWidth(this.clockText.text, textGenerationSettings), textGenerator.GetPreferredHeight(this.clockText.text, textGenerationSettings));
		this.clockText.transform.localScale = new Vector3(1f, 1f, 1f);
		this.clockText.transform.localPosition = new Vector3(this.clockText.rectTransform.sizeDelta.x / 2f, -(this.clockText.rectTransform.sizeDelta.y / 2f), 0f);
		this.warmClockSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.warmClockSeq, 0f, DOTween.To(() => this.clockText.transform.localScale, delegate(Vector3 x)
		{
			this.clockText.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0f));
		TweenSettingsExtensions.Insert(this.warmClockSeq, 0.1f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.clockText.transform.localScale, delegate(Vector3 x)
		{
			this.clockText.transform.localScale = x;
		}, new Vector3(0.33f, 0.33f, 0.33f), 0.9f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.warmClockSeq, 5);
		TweenExtensions.Play<Sequence>(this.warmClockSeq);
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

	public void fireDOSAttack()
	{
		Object.Destroy(this.clockText.gameObject);
		Object.Destroy(this.clockTextHolder.gameObject);
		float num;
		float num2;
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.CountDownTick2, 0.55f, false);
			num = 75f / (float)this.rootNodeWidth;
			num2 = 75f / (float)this.rootNodeHeight;
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.CountDownTick2, 0.7f, false);
			num = (float)this.nodeWidth / (float)this.rootNodeWidth;
			num2 = (float)this.nodeHeight / (float)this.rootNodeHeight;
		}
		this.finalCountDownFired = false;
		this.setDOSClockObject = Object.Instantiate<GameObject>(this.DOSClockObject);
		this.setDOSClockObject.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
		this.setDOSClockObject.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
		this.setDOSClockObject.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		this.setDOSClockObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.setDOSClockObject.GetComponent<Image>().sprite.rect.width * num, this.setDOSClockObject.GetComponent<Image>().sprite.rect.height * num2);
		this.setDOSClockObject.GetComponent<DOSClock>().DOSClockImage.GetComponent<RectTransform>().sizeDelta = new Vector2(this.setDOSClockObject.GetComponent<DOSClock>().DOSClockImage.sprite.rect.width * num, this.setDOSClockObject.GetComponent<DOSClock>().DOSClockImage.sprite.rect.height * num2);
		if (this.inHackerMode)
		{
			this.setDOSClockObject.transform.SetParent(this.hackerModeManager.scoreHeaderObject.transform);
			this.setDOSClockObject.transform.localPosition = new Vector3((float)Screen.width / 2f - this.setDOSClockObject.GetComponent<RectTransform>().sizeDelta.x / 2f, -1f, 0f);
		}
		else
		{
			this.setDOSClockObject.transform.SetParent(this.DOSHolder.transform);
			this.setDOSClockObject.transform.localPosition = new Vector3(0f - this.setDOSClockObject.GetComponent<RectTransform>().sizeDelta.x / 2f, this.nodeHolder.localPosition.y + this.setDOSClockObject.GetComponent<RectTransform>().sizeDelta.y + 15f, 0f);
		}
		this.setDOSClockObject.transform.localScale = new Vector3(1f, 1f, 1f);
		this.setDOSClockObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		this.DOSAttackClockSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.DOSAttackClockSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.setDOSClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount, delegate(float x)
		{
			this.setDOSClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount = x;
		}, 0f, this.DOSTime), 1));
		TweenExtensions.Play<Sequence>(this.DOSAttackClockSeq);
		this.startNodeObject.stopSubAction();
		this.hotNodeObject = this.startNodeObject;
		this.hotNodeObject.tap();
		this.DOSHOTTimeStamp = Time.time;
		this.DOSGameTimeStamp = Time.time;
		this.DOSiSHot = true;
	}

	public void actionNodeActivated()
	{
		this.actionNodeActivatedCount += 1;
		if (this.actionNodeActivatedCount >= this.ACTION_BLOCK_SIZE)
		{
			if (this.inHackerMode)
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.ExitNodeActive, 0.7f, false);
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.ExitNodeActive, 0.7f, false);
			}
			this.endNodeObject.endNodeNowActive();
			this.allActionNodesActivated = true;
		}
	}

	public void addNodeClickCount()
	{
		if (this.inHackerMode)
		{
			this.nodeClickCount++;
		}
	}

	private void prepPuzzle()
	{
		this.searchingForActionNodes = true;
		this.searchingForEndNode = false;
		this.startEndNodes.Clear();
		this.actionBlockNodes.Clear();
		this.masterNodes.Clear();
		this.nodeLookUp.Clear();
		this.arrowNodes.Clear();
		this.currentTreeNodes.Clear();
		this.currentActionFilledNodeCount = 0;
		this.reCount = 0;
		if (Random.Range(1, 3) == 1)
		{
			this.clockWise = false;
		}
		else
		{
			this.clockWise = true;
		}
		for (int i = 0; i < (int)((this.MATRIX_SIZE + 1) * (this.MATRIX_SIZE + 1)); i++)
		{
			short num = (short)(i % (int)(this.MATRIX_SIZE + 1));
			short num2 = (short)Mathf.FloorToInt((float)(i / (int)(this.MATRIX_SIZE + 1)));
			this.masterNodes.Add(new DOSAttack.node(nodeType.WHITENODE, new DOSAttack.nodePosition(num, num2), this.whiteNodeSprite, this));
			this.nodeLookUp.Add(num + ":" + num2, i);
		}
		this.startEndNodes.Add(new DOSAttack.nodePosition((short)Random.Range(0, (int)this.MATRIX_SIZE), 0));
		this.startEndNodes.Add(new DOSAttack.nodePosition((short)Random.Range(0, (int)this.MATRIX_SIZE), this.MATRIX_SIZE));
		DOSAttack.node node = this.masterNodes[this.nodeLookUp[this.startEndNodes[0].x + ":" + this.startEndNodes[0].y]];
		node.myType = nodeType.STARTNODE;
		node.mySprite = this.startNodeSprite;
		this.masterNodes[this.nodeLookUp[this.startEndNodes[0].x + ":" + this.startEndNodes[0].y]] = node;
		node = this.masterNodes[this.nodeLookUp[this.startEndNodes[1].x + ":" + this.startEndNodes[1].y]];
		node.myType = nodeType.ENDNODE;
		node.mySprite = this.endNodeSprite;
		this.masterNodes[this.nodeLookUp[this.startEndNodes[1].x + ":" + this.startEndNodes[1].y]] = node;
		for (int j = 0; j < (int)this.ACTION_BLOCK_SIZE; j++)
		{
			this.actionBlockNodes.Add(this.generateValidActionBlock());
			node = this.masterNodes[this.nodeLookUp[this.actionBlockNodes[j].x + ":" + this.actionBlockNodes[j].y]];
			node.myType = nodeType.ACTIONNODE;
			node.mySprite = this.actionNodeSprite;
			this.masterNodes[this.nodeLookUp[this.actionBlockNodes[j].x + ":" + this.actionBlockNodes[j].y]] = node;
		}
		this.generateValidArrowBlock(this.masterNodes[this.nodeLookUp[this.startEndNodes[0].x + ":" + this.startEndNodes[0].y]]);
		if (!this.isAStaleMate)
		{
			this.drawPuzzle();
		}
		else
		{
			this.prepPuzzle();
		}
	}

	private DOSAttack.nodePosition generateValidActionBlock()
	{
		bool flag = true;
		DOSAttack.nodePosition nodePosition = new DOSAttack.nodePosition(0, 0);
		while (flag)
		{
			nodePosition = new DOSAttack.nodePosition((short)Random.Range(0, (int)(this.MATRIX_SIZE + 1)), (short)Random.Range(0, (int)(this.MATRIX_SIZE + 1)));
			if (nodePosition != this.startEndNodes[0] && nodePosition != this.startEndNodes[1])
			{
				if (this.actionBlockNodes.Count > 0)
				{
					bool flag2 = true;
					for (int i = 0; i < this.actionBlockNodes.Count; i++)
					{
						if (nodePosition.y == this.actionBlockNodes[i].y)
						{
							flag2 = false;
							i = this.actionBlockNodes.Count;
						}
					}
					if (flag2)
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
		}
		return nodePosition;
	}

	private void generateValidArrowBlock(DOSAttack.node checkNode)
	{
		List<DOSAttack.node> list = new List<DOSAttack.node>();
		List<DOSAttack.node> list2 = new List<DOSAttack.node>();
		DOSAttack.node node = new DOSAttack.node();
		DOSAttack.node checkNode2 = new DOSAttack.node();
		DOSAttack.node node2 = new DOSAttack.node();
		bool flag = false;
		bool flag2 = false;
		checkNode.iWasChecked = true;
		if (this.clockWise)
		{
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(0, -1)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, -1)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, -1)]]);
			}
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(1, 0)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(1, 0)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(1, 0)]]);
			}
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(0, 1)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, 1)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, 1)]]);
			}
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(-1, 0)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(-1, 0)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(-1, 0)]]);
			}
		}
		else
		{
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(0, -1)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, -1)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, -1)]]);
			}
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(-1, 0)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(-1, 0)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(-1, 0)]]);
			}
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(0, 1)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, 1)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(0, 1)]]);
			}
			if (this.nodeLookUp.ContainsKey(checkNode.getNodeKey(1, 0)) && this.isNodeValid(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(1, 0)]]))
			{
				list.Add(this.masterNodes[this.nodeLookUp[checkNode.getNodeKey(1, 0)]]);
			}
		}
		if (list.Count > 0)
		{
			if (this.searchingForActionNodes)
			{
				if (checkNode.myType == nodeType.ACTIONNODE)
				{
					node = checkNode;
					node.myType = nodeType.ACTIONFILLEDNODE;
					this.masterNodes[this.nodeLookUp[checkNode.myPos.x + ":" + checkNode.myPos.y]] = node;
					flag = true;
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].myType == nodeType.ACTIONNODE)
						{
							node = list[i];
							node.myType = nodeType.ACTIONFILLEDNODE;
							node.myParent = checkNode.myPos.x + ":" + checkNode.myPos.y;
							list[i] = node;
							this.masterNodes[this.nodeLookUp[node.myPos.x + ":" + node.myPos.y]] = node;
							flag = true;
							i = list.Count + 1;
						}
						else
						{
							list2.Add(list[i]);
						}
					}
				}
				if (flag)
				{
					this.currentActionFilledNodeCount += 1;
					List<string> list3 = new List<string>();
					list3.Add(node.myPos.x + ":" + node.myPos.y);
					list3.Add(node.myParent);
					if (this.masterNodes[this.nodeLookUp[node.myParent]].myParent != "none")
					{
						bool flag3 = true;
						DOSAttack.node node3 = this.masterNodes[this.nodeLookUp[node.myParent]];
						while (flag3)
						{
							if (node3.myParent != "none")
							{
								list3.Add(node3.myParent);
								node3 = this.masterNodes[this.nodeLookUp[node3.myParent]];
							}
							else
							{
								flag3 = false;
							}
						}
					}
					for (int j = list3.Count - 1; j >= 0; j--)
					{
						if (!this.arrowNodes.ContainsKey(list3[j]))
						{
							this.arrowNodes.Add(list3[j], this.nodeLookUp[list3[j]]);
						}
						if (this.masterNodes[this.nodeLookUp[list3[j]]].myType == nodeType.WHITENODE)
						{
							if (j - 1 >= 0)
							{
								DOSAttack.node node4 = this.masterNodes[this.nodeLookUp[list3[j]]];
								DOSAttack.node node5 = this.masterNodes[this.nodeLookUp[list3[j - 1]]];
								if (node4.myPos.x == node5.myPos.x)
								{
									if (node4.myPos.y > node5.myPos.y)
									{
										node4.myType = nodeType.UPNODE;
										node4.mySprite = this.upNodeSrpite;
									}
									else
									{
										node4.myType = nodeType.DOWNNODE;
										node4.mySprite = this.downNodeSprite;
									}
								}
								else if (node4.myPos.x > node5.myPos.x)
								{
									node4.myType = nodeType.LEFTNODE;
									node4.mySprite = this.leftNodeSprite;
								}
								else
								{
									node4.myType = nodeType.RIGHTNODE;
									node4.mySprite = this.rightNodeSprite;
								}
								this.masterNodes[this.nodeLookUp[list3[j]]] = node4;
							}
							else
							{
								this.searchingForActionNodes = false;
								this.searchingForEndNode = false;
							}
						}
					}
					this.currentTreeNodes.Clear();
					this.reCount = 0;
					list3.Clear();
					for (int k = 0; k < this.masterNodes.Count; k++)
					{
						DOSAttack.node node6 = this.masterNodes[k];
						node6.myParent = "none";
						node6.myParentNode = new DOSAttack.node();
						node6.myBranches.Clear();
						node6.myBranchNodes.Clear();
						node6.mySibNodes.Clear();
						node6.drillUpNodes.Clear();
						node6.iWasChecked = false;
						this.masterNodes[k] = node6;
					}
					if (this.currentActionFilledNodeCount >= this.ACTION_BLOCK_SIZE)
					{
						this.searchingForActionNodes = false;
						this.searchingForEndNode = true;
						string key = string.Empty;
						foreach (KeyValuePair<string, int> keyValuePair in this.arrowNodes)
						{
							key = keyValuePair.Key;
						}
						checkNode2 = this.masterNodes[this.nodeLookUp[key]];
					}
				}
				else
				{
					if (!this.currentTreeNodes.ContainsKey(checkNode.myPos.x + ":" + checkNode.myPos.y))
					{
						this.currentTreeNodes.Add(checkNode.myPos.x + ":" + checkNode.myPos.y, 0);
					}
					for (int l = 0; l < list2.Count; l++)
					{
						if (!this.currentTreeNodes.ContainsKey(list2[l].myPos.x + ":" + list2[l].myPos.y))
						{
							DOSAttack.node node7 = this.masterNodes[this.nodeLookUp[list2[l].myPos.x + ":" + list2[l].myPos.y]];
							node7.myParent = checkNode.myPos.x + ":" + checkNode.myPos.y;
							node7.myParentNode = checkNode;
							this.masterNodes[this.nodeLookUp[list2[l].myPos.x + ":" + list2[l].myPos.y]] = node7;
							checkNode.myBranches.Add(list2[l].myPos.x + ":" + list2[l].myPos.y);
							checkNode.myBranchNodes.Add(node7);
							this.currentTreeNodes.Add(list2[l].myPos.x + ":" + list2[l].myPos.y, 0);
						}
					}
					checkNode.generateSibs();
					this.searchingForActionNodes = checkNode.getNextNodeToCheck(out checkNode2);
				}
			}
			else if (this.searchingForEndNode)
			{
				if (checkNode.myType == nodeType.ENDNODE)
				{
					node2 = checkNode;
					flag2 = true;
				}
				else
				{
					for (int m = 0; m < list.Count; m++)
					{
						if (list[m].myType == nodeType.ENDNODE)
						{
							node2 = list[m];
							node2.myParent = checkNode.myPos.x + ":" + checkNode.myPos.y;
							flag2 = true;
							m = list.Count + 1;
						}
						else
						{
							list2.Add(list[m]);
						}
					}
				}
				if (flag2)
				{
					List<string> list4 = new List<string>();
					list4.Add(node2.myPos.x + ":" + node2.myPos.y);
					list4.Add(node2.myParent);
					if (this.masterNodes[this.nodeLookUp[node2.myParent]].myParent != "none")
					{
						bool flag4 = true;
						DOSAttack.node node8 = this.masterNodes[this.nodeLookUp[node2.myParent]];
						while (flag4)
						{
							if (node8.myParent != "none")
							{
								list4.Add(node8.myParent);
								node8 = this.masterNodes[this.nodeLookUp[node8.myParent]];
							}
							else
							{
								flag4 = false;
							}
						}
					}
					for (int n = list4.Count - 1; n >= 0; n--)
					{
						if (!this.arrowNodes.ContainsKey(list4[n]))
						{
							this.arrowNodes.Add(list4[n], this.nodeLookUp[list4[n]]);
						}
						if (this.masterNodes[this.nodeLookUp[list4[n]]].myType == nodeType.WHITENODE)
						{
							if (n - 1 >= 0)
							{
								DOSAttack.node node9 = this.masterNodes[this.nodeLookUp[list4[n]]];
								DOSAttack.node node10 = this.masterNodes[this.nodeLookUp[list4[n - 1]]];
								if (node9.myPos.x == node10.myPos.x)
								{
									if (node9.myPos.y > node10.myPos.y)
									{
										node9.myType = nodeType.UPNODE;
										node9.mySprite = this.upNodeSrpite;
									}
									else
									{
										node9.myType = nodeType.DOWNNODE;
										node9.mySprite = this.downNodeSprite;
									}
								}
								else if (node9.myPos.x > node10.myPos.x)
								{
									node9.myType = nodeType.LEFTNODE;
									node9.mySprite = this.leftNodeSprite;
								}
								else
								{
									node9.myType = nodeType.RIGHTNODE;
									node9.mySprite = this.rightNodeSprite;
								}
								this.masterNodes[this.nodeLookUp[list4[n]]] = node9;
							}
							else
							{
								this.searchingForActionNodes = false;
								this.searchingForEndNode = false;
							}
						}
					}
					this.currentTreeNodes.Clear();
					this.reCount = 0;
					list4.Clear();
					for (int num = 0; num < this.masterNodes.Count; num++)
					{
						DOSAttack.node node11 = this.masterNodes[num];
						node11.myParent = "none";
						node11.myParentNode = new DOSAttack.node();
						node11.myBranches.Clear();
						node11.myBranchNodes.Clear();
						node11.mySibNodes.Clear();
						node11.drillUpNodes.Clear();
						node11.iWasChecked = false;
						this.masterNodes[num] = node11;
					}
				}
				else
				{
					if (!this.currentTreeNodes.ContainsKey(checkNode.myPos.x + ":" + checkNode.myPos.y))
					{
						this.currentTreeNodes.Add(checkNode.myPos.x + ":" + checkNode.myPos.y, 0);
					}
					for (int num2 = 0; num2 < list2.Count; num2++)
					{
						if (!this.currentTreeNodes.ContainsKey(list2[num2].myPos.x + ":" + list2[num2].myPos.y))
						{
							DOSAttack.node node12 = this.masterNodes[this.nodeLookUp[list2[num2].myPos.x + ":" + list2[num2].myPos.y]];
							node12.myParent = checkNode.myPos.x + ":" + checkNode.myPos.y;
							node12.myParentNode = checkNode;
							this.masterNodes[this.nodeLookUp[list2[num2].myPos.x + ":" + list2[num2].myPos.y]] = node12;
							checkNode.myBranches.Add(list2[num2].myPos.x + ":" + list2[num2].myPos.y);
							checkNode.myBranchNodes.Add(node12);
							this.currentTreeNodes.Add(list2[num2].myPos.x + ":" + list2[num2].myPos.y, 0);
						}
					}
					checkNode.generateSibs();
					this.searchingForEndNode = checkNode.getNextNodeToCheck(out checkNode2);
				}
			}
		}
		else if (!checkNode.getNextNodeToCheck(out checkNode2))
		{
			this.searchingForActionNodes = false;
			this.searchingForEndNode = false;
		}
		if (this.searchingForActionNodes)
		{
			if (!flag)
			{
				this.generateValidArrowBlock(checkNode2);
			}
			else
			{
				string key2 = string.Empty;
				foreach (KeyValuePair<string, int> keyValuePair2 in this.arrowNodes)
				{
					key2 = keyValuePair2.Key;
				}
				this.generateValidArrowBlock(this.masterNodes[this.nodeLookUp[key2]]);
			}
		}
		else if (this.searchingForEndNode)
		{
			if (!flag2)
			{
				this.generateValidArrowBlock(checkNode2);
			}
			else
			{
				this.isAStaleMate = false;
			}
		}
		else
		{
			this.isAStaleMate = true;
		}
	}

	private bool isNodeValid(DOSAttack.node checkNode)
	{
		return checkNode.myType != nodeType.STARTNODE && (this.searchingForEndNode || checkNode.myType != nodeType.ENDNODE) && (this.searchingForEndNode || checkNode.myType != nodeType.ACTIONFILLEDNODE) && checkNode.myType != nodeType.LEFTNODE && checkNode.myType != nodeType.RIGHTNODE && checkNode.myType != nodeType.UPNODE && checkNode.myType != nodeType.DOWNNODE && !this.currentTreeNodes.ContainsKey(checkNode.myPos.x + ":" + checkNode.myPos.y) && (this.searchingForEndNode || !this.arrowNodes.ContainsKey(checkNode.myPos.x + ":" + checkNode.myPos.y));
	}

	private void drawPuzzle()
	{
		List<int> list = new List<int>(this.arrowNodes.Values);
		this.nodeObjects = new Dictionary<string, GameObject>();
		if (this.masterNodes[list[0]].myPos.x != this.masterNodes[list[1]].myPos.x)
		{
			if (this.masterNodes[list[0]].myPos.x > this.masterNodes[list[1]].myPos.x)
			{
				this.masterNodes[list[0]].mySubType = nodeType.LEFTNODE;
			}
			else
			{
				this.masterNodes[list[0]].mySubType = nodeType.RIGHTNODE;
			}
		}
		else if (this.masterNodes[list[0]].myPos.y > this.masterNodes[list[1]].myPos.y)
		{
			this.masterNodes[list[0]].mySubType = nodeType.UPNODE;
		}
		else
		{
			this.masterNodes[list[0]].mySubType = nodeType.DOWNNODE;
		}
		float num = (float)(this.nodeWidth * (int)(this.MATRIX_SIZE + 1));
		float num2 = (float)(this.nodeHeight * (int)(this.MATRIX_SIZE + 1));
		this.nodeHolder = new GameObject("NodeHolder", new Type[]
		{
			typeof(RectTransform)
		}).GetComponent<RectTransform>();
		this.nodeHolder.SetParent(this.DOSHolder.transform);
		this.nodeHolderCG = this.nodeHolder.gameObject.AddComponent<CanvasGroup>();
		this.nodeHolder.sizeDelta = new Vector2(num, num2);
		this.nodeHolder.anchorMin = new Vector2(0f, 1f);
		this.nodeHolder.anchorMax = new Vector2(0f, 1f);
		this.nodeHolder.pivot = new Vector2(0f, 1f);
		this.nodeHolder.localScale = new Vector3(1f, 1f, 1f);
		if (this.inHackerMode)
		{
			this.nodeHolder.localPosition = new Vector3((float)Screen.width / 2f - num / 2f, -((float)Screen.height / 2f - num2 / 2f), 0f);
		}
		else
		{
			this.nodeHolder.localPosition = new Vector3(0f - num / 2f, num2 / 2f, 0f);
		}
		this.nodeBGIMG = new GameObject("nodeBGIMG", new Type[]
		{
			typeof(Image)
		}).GetComponent<Image>();
		this.nodeBGIMG.transform.SetParent(this.nodeHolder.transform);
		this.nodeBGIMG.sprite = this.pixelSprite;
		this.nodeBGIMG.type = 3;
		this.nodeBGIMG.type = 3;
		this.nodeBGIMG.fillMethod = 4;
		this.nodeBGIMG.fillAmount = 0f;
		this.nodeBGIMG.color = this.nodeBorderColor;
		this.nodeBGIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(num, num2);
		this.nodeBGIMG.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
		this.nodeBGIMG.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
		this.nodeBGIMG.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		this.nodeBGIMG.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.nodeBGIMG.transform.localScale = new Vector3(1f, 1f, 1f);
		for (int i = 0; i < this.masterNodes.Count; i++)
		{
			float setX = (float)(i % (int)(this.MATRIX_SIZE + 1) * this.nodeWidth);
			float setY = (float)(Mathf.FloorToInt((float)(i / (int)(this.MATRIX_SIZE + 1))) * -(float)this.nodeHeight);
			GameObject gameObject = Object.Instantiate<GameObject>(this.NodeObject);
			gameObject.transform.SetParent(this.nodeHolder.transform);
			gameObject.GetComponent<NodeObject>().myDoSAttack = this;
			gameObject.GetComponent<NodeObject>().trollNode = this.trollNodesActive;
			gameObject.GetComponent<NodeObject>().buildMe(this.masterNodes[i], setX, setY, (float)this.nodeWidth, (float)this.nodeHeight);
			gameObject.GetComponent<NodeObject>().doSubAction();
			if (this.masterNodes[i].myType == nodeType.STARTNODE)
			{
				this.startNodeObject = gameObject.GetComponent<NodeObject>();
			}
			else if (this.masterNodes[i].myType == nodeType.ENDNODE)
			{
				this.endNodeObject = gameObject.GetComponent<NodeObject>();
			}
			this.nodeObjects.Add(gameObject.GetComponent<NodeObject>().myNodeData.myPos.x.ToString() + ":" + gameObject.GetComponent<NodeObject>().myNodeData.myPos.y.ToString(), gameObject);
		}
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeBGIMG.fillAmount, delegate(float x)
		{
			this.nodeBGIMG.fillAmount = x;
		}, 1f, 0.4f), 1);
		this.masterNodes.Clear();
		this.nodeLookUp.Clear();
		this.startEndNodes.Clear();
		this.actionBlockNodes.Clear();
	}

	private void setNextHotNode()
	{
		string key = string.Empty;
		switch (this.hotNodeObject.myNodeData.myType)
		{
		case nodeType.STARTNODE:
			switch (this.hotNodeObject.myNodeData.mySubType)
			{
			case nodeType.LEFTNODE:
				key = ((float)this.hotNodeObject.myNodeData.myPos.x - 1f).ToString() + ":" + this.hotNodeObject.myNodeData.myPos.y.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			case nodeType.RIGHTNODE:
				key = ((float)this.hotNodeObject.myNodeData.myPos.x + 1f).ToString() + ":" + this.hotNodeObject.myNodeData.myPos.y.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			case nodeType.UPNODE:
			{
				float num = (float)this.hotNodeObject.myNodeData.myPos.y - 1f;
				key = this.hotNodeObject.myNodeData.myPos.x.ToString() + ":" + num.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			}
			case nodeType.DOWNNODE:
			{
				float num = (float)this.hotNodeObject.myNodeData.myPos.y + 1f;
				key = this.hotNodeObject.myNodeData.myPos.x.ToString() + ":" + num.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			}
			}
			break;
		case nodeType.ENDNODE:
			this.DOSiSHot = false;
			if (this.allActionNodesActivated)
			{
				this.DOSAttackFailed();
			}
			else
			{
				this.DOSAttackSucceeded();
			}
			break;
		case nodeType.ACTIONFILLEDNODE:
			switch (this.hotNodeObject.actionNodeDirection)
			{
			case 0:
				this.hotNodeObject = this.startNodeObject;
				if (this.inHackerMode)
				{
					this.addSkillPoints = false;
				}
				break;
			case 1:
			{
				float num = (float)this.hotNodeObject.myNodeData.myPos.y - 1f;
				key = this.hotNodeObject.myNodeData.myPos.x.ToString() + ":" + num.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			}
			case 2:
				key = ((float)this.hotNodeObject.myNodeData.myPos.x + 1f).ToString() + ":" + this.hotNodeObject.myNodeData.myPos.y.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			case 3:
			{
				float num = (float)this.hotNodeObject.myNodeData.myPos.y + 1f;
				key = this.hotNodeObject.myNodeData.myPos.x.ToString() + ":" + num.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			}
			case 4:
				key = ((float)this.hotNodeObject.myNodeData.myPos.x - 1f).ToString() + ":" + this.hotNodeObject.myNodeData.myPos.y.ToString();
				if (this.nodeObjects.ContainsKey(key))
				{
					this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
				}
				else
				{
					if (this.inHackerMode)
					{
						this.addSkillPoints = false;
					}
					this.hotNodeObject = this.startNodeObject;
				}
				break;
			default:
				if (this.inHackerMode)
				{
					this.addSkillPoints = false;
				}
				this.hotNodeObject = this.startNodeObject;
				break;
			}
			break;
		case nodeType.WHITENODE:
			if (this.inHackerMode)
			{
				this.addSkillPoints = false;
			}
			this.hotNodeObject = this.startNodeObject;
			break;
		case nodeType.LEFTNODE:
			key = ((float)this.hotNodeObject.myNodeData.myPos.x - 1f).ToString() + ":" + this.hotNodeObject.myNodeData.myPos.y.ToString();
			if (this.nodeObjects.ContainsKey(key))
			{
				this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
			}
			else
			{
				if (this.inHackerMode)
				{
					this.addSkillPoints = false;
				}
				this.hotNodeObject = this.startNodeObject;
			}
			break;
		case nodeType.RIGHTNODE:
			key = ((float)this.hotNodeObject.myNodeData.myPos.x + 1f).ToString() + ":" + this.hotNodeObject.myNodeData.myPos.y.ToString();
			if (this.nodeObjects.ContainsKey(key))
			{
				this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
			}
			else
			{
				if (this.inHackerMode)
				{
					this.addSkillPoints = false;
				}
				this.hotNodeObject = this.startNodeObject;
			}
			break;
		case nodeType.UPNODE:
		{
			float num = (float)this.hotNodeObject.myNodeData.myPos.y - 1f;
			key = this.hotNodeObject.myNodeData.myPos.x.ToString() + ":" + num.ToString();
			if (this.nodeObjects.ContainsKey(key))
			{
				this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
			}
			else
			{
				if (this.inHackerMode)
				{
					this.addSkillPoints = false;
				}
				this.hotNodeObject = this.startNodeObject;
			}
			break;
		}
		case nodeType.DOWNNODE:
		{
			float num = (float)this.hotNodeObject.myNodeData.myPos.y + 1f;
			key = this.hotNodeObject.myNodeData.myPos.x.ToString() + ":" + num.ToString();
			if (this.nodeObjects.ContainsKey(key))
			{
				this.hotNodeObject = this.nodeObjects[key].GetComponent<NodeObject>();
			}
			else
			{
				if (this.inHackerMode)
				{
					this.addSkillPoints = false;
				}
				this.hotNodeObject = this.startNodeObject;
			}
			break;
		}
		}
	}

	private void ReloadDOSAttack()
	{
		Object.Destroy(this.nodeHolder.gameObject);
		this.prepDOSAttack();
		this.warmDOSAttack();
	}

	private void DOSAttackSucceeded()
	{
		bool flag = true;
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.ClockAlmostUp.name);
		}
		else
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.ClockAlmostUp.name);
		}
		if (this.inHackerMode && this.hackerModeManager.FireAShell())
		{
			flag = false;
		}
		this.nodeObjects.Clear();
		this.hotNodeObject = null;
		this.actionNodeActivatedCount = 0;
		this.allActionNodesActivated = false;
		TweenExtensions.Kill(this.DOSAttackClockSeq, false);
		Object.Destroy(this.setDOSClockObject);
		if (flag)
		{
			this.DOSAttackOverSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.DOSAttackSucOver));
		}
		else
		{
			this.DOSAttackOverSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.ReloadDOSAttack));
		}
		GameManager.GetDOSTwitch().DismissTwitchHacker();
		TweenSettingsExtensions.Insert(this.DOSAttackOverSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeBGIMG.fillAmount, delegate(float x)
		{
			this.nodeBGIMG.fillAmount = x;
		}, 0f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.DOSAttackOverSeq, 0.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHolderCG.alpha, delegate(float x)
		{
			this.nodeHolderCG.alpha = x;
		}, 0f, 0.3f), 3));
		TweenExtensions.Play<Sequence>(this.DOSAttackOverSeq);
	}

	private void DOSAttackSucOver()
	{
		Object.Destroy(this.nodeHolder.gameObject);
		if (this.isTwitchAttack)
		{
			GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("@" + this.twitchHackerName + " IS 1337!", 10f);
			this.isTwitchAttack = false;
			this.twitchHackerLevel = string.Empty;
			this.twitchHackerName = string.Empty;
		}
		if (this.inHackerMode)
		{
			this.hackerModeManager.triggerGameOver();
			this.hackerModeManager.resetDOSAttack();
		}
		else
		{
			this.hackerManager.DOSAttackPassed();
		}
	}

	public void DOSAttackFailed()
	{
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.ClockAlmostUp.name);
		}
		else
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.ClockAlmostUp.name);
		}
		if (this.inHackerMode)
		{
			this.tallyBonusPoints();
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.HMPuzzlePass, 0.55f, false);
		}
		else if ((double)(Time.time - this.DOSGameTimeStamp) <= (double)this.DOSTime * 0.4)
		{
			this.theCloud.addPlayerSkillPoints((int)this.SkillPointsA);
		}
		else
		{
			this.theCloud.addPlayerSkillPoints((int)this.SkillPointsB);
		}
		this.nodeObjects.Clear();
		this.hotNodeObject = null;
		this.actionNodeActivatedCount = 0;
		this.allActionNodesActivated = false;
		TweenExtensions.Kill(this.DOSAttackClockSeq, false);
		Object.Destroy(this.setDOSClockObject);
		if (!this.inHackerMode)
		{
			GameManager.GetDOSTwitch().DismissTwitchHacker();
		}
		this.DOSAttackOverSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.DOSAttackFailOver));
		TweenSettingsExtensions.Insert(this.DOSAttackOverSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeBGIMG.fillAmount, delegate(float x)
		{
			this.nodeBGIMG.fillAmount = x;
		}, 0f, 0.4f), 1));
		TweenSettingsExtensions.Insert(this.DOSAttackOverSeq, 0.4f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.nodeHolderCG.alpha, delegate(float x)
		{
			this.nodeHolderCG.alpha = x;
		}, 0f, 0.3f), 3));
		TweenExtensions.Play<Sequence>(this.DOSAttackOverSeq);
	}

	private void DOSAttackFailOver()
	{
		Object.Destroy(this.nodeHolder.gameObject);
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
			GameManager.TimeSlinger.FireTimer(0.75f, new Action(this.prepDOSAttack));
			GameManager.TimeSlinger.FireTimer(1.3f, new Action(this.warmDOSAttack));
		}
		else
		{
			this.hackerManager.DOSAttackBlocked();
		}
	}

	private void tallyBonusPoints()
	{
		if (this.inHackerMode)
		{
			int completeBonusPoints = this.DOSChains[this.currentDOSChainIndex].completeBonusPoints;
			int timePoints = 0;
			int skillPoints = 0;
			if (Time.time - this.DOSGameTimeStamp <= this.DOSTime * (1f - this.DOSChains[this.currentDOSChainIndex].timeBonusRange))
			{
				timePoints = this.DOSChains[this.currentDOSChainIndex].timeBonusPoints;
			}
			if (this.nodeClickCount > this.maxNodeClickCount)
			{
				this.addSkillPoints = false;
			}
			if (this.addSkillPoints)
			{
				skillPoints = this.DOSChains[this.currentDOSChainIndex].skillBonusPoints;
			}
			this.hackerModeManager.addGamePoints(completeBonusPoints, timePoints, skillPoints, (int)this.DOSChains[this.currentDOSChainIndex].bonusMultiplier);
		}
	}

	private void Start()
	{
		this.startEndNodes = new List<DOSAttack.nodePosition>();
		this.actionBlockNodes = new List<DOSAttack.nodePosition>();
		this.masterNodes = new List<DOSAttack.node>();
		this.nodeLookUp = new Dictionary<string, int>();
		this.arrowNodes = new Dictionary<string, int>();
		this.currentTreeNodes = new Dictionary<string, int>();
	}

	private void Update()
	{
		if (this.warmClockActive)
		{
			if (Time.time - this.clockTimeStamp >= this.warmUpTime)
			{
				this.warmClockActive = false;
				this.fireDOSAttack();
			}
			else if (Time.time - this.clockMicroTimeStamp >= 1f)
			{
				if (this.inHackerMode)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.CountDownTick1, 0.5f, false);
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
		else if (this.DOSiSHot)
		{
			float num = this.hotTime;
			if (CrossPlatformInputManager.GetButton("MiddleClick") || CrossPlatformInputManager.GetButton("RightAlt"))
			{
				if (this.inHackerMode)
				{
					if (this.hackerModeManager.DOSTurboIsHot)
					{
						num = this.boostTime / 2f;
					}
					else
					{
						num = this.boostTime;
					}
				}
				else
				{
					num = this.boostTime;
				}
			}
			if (Time.time - this.DOSGameTimeStamp >= this.DOSTime - this.ClockAlmostUp.length && !this.finalCountDownFired)
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
			if (Time.time - this.DOSGameTimeStamp >= this.DOSTime)
			{
				this.DOSiSHot = false;
				this.DOSAttackSucceeded();
			}
			else if (Time.time - this.DOSHOTTimeStamp >= num)
			{
				this.hotNodeObject.untap();
				this.setNextHotNode();
				if (this.DOSiSHot)
				{
					this.hotNodeObject.tap();
					this.DOSHOTTimeStamp = Time.time;
				}
			}
		}
		if (this.inHackerMode && this.hackerModeGameActive)
		{
			if (this.DOSiSHot)
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

	[Range(2f, 11f)]
	public short MATRIX_SIZE = 2;

	[Range(3f, 8f)]
	public short ACTION_BLOCK_SIZE = 3;

	[Range(1f, 10f)]
	public float warmUpTime = 5f;

	[Range(0.1f, 2.5f)]
	public float hotTime = 1f;

	[Range(0.1f, 0.5f)]
	public float boostTime = 0.1f;

	[Range(5f, 120f)]
	public float DOSTime = 60f;

	public bool trollNodesActive;

	public AudioClip CountDownTick1;

	public AudioClip CountDownTick2;

	public AudioClip NodeHot;

	public AudioClip NodeCold;

	public AudioClip NodeActive;

	public AudioClip ExitNodeActive;

	public AudioClip ActionNodeClick;

	public AudioClip ClockAlmostUp;

	public AudioClip HMPuzzlePass;

	public Font clockFont;

	public Color clockColor;

	public TheCloud theCloud;

	public HackerManager hackerManager;

	public HackerModeManager hackerModeManager;

	public GameObject DOSHolder;

	public GameObject DOSClockObject;

	public Color nodeBorderColor;

	public Sprite pixelSprite;

	public Sprite whiteNodeSprite;

	public Sprite startNodeSprite;

	public Sprite startNodeDownArrowSprite;

	public Sprite endNodeSprite;

	public Sprite actionNodeSprite;

	public Sprite leftNodeSprite;

	public Sprite rightNodeSprite;

	public Sprite upNodeSrpite;

	public Sprite downNodeSprite;

	public Sprite trollLeftNodeSprite;

	public Sprite trollRightNodeSprite;

	public Sprite trollUpNodeSprite;

	public Sprite trollDownNodeSprite;

	public GameObject NodeObject;

	public int rootNodeWidth = 100;

	public int rootNodeHeight = 100;

	public int nodeWidth = 100;

	public int nodeHeight = 100;

	public int reCount;

	public Sprite actionNodeHoverSprite;

	public Sprite actionNodeUpA;

	public Sprite actionNodeRightA;

	public Sprite actionNodeDownA;

	public Sprite actionNodeLeftA;

	public Sprite actionNodeActivatedSprite;

	public Sprite actionNodeActivatedHoverSprite;

	public Sprite endNodeActivatedSprite;

	public short SkillPointsA = 5;

	public short SkillPointsB = 2;

	public List<DOSLevelDefinition> DOSLevels;

	public List<DOSChainDefinition> DOSChains;

	private RectTransform clockTextHolder;

	private Text clockText;

	private float clockTimeStamp;

	private float clockMicroTimeStamp;

	private float clockMicroCount;

	private bool warmClockActive;

	private Sequence warmClockSeq;

	private bool searchingForActionNodes = true;

	private bool searchingForEndNode;

	private bool clockWise;

	private bool isAStaleMate = true;

	private List<DOSAttack.nodePosition> startEndNodes;

	private List<DOSAttack.nodePosition> actionBlockNodes;

	public List<DOSAttack.node> masterNodes;

	private Dictionary<string, int> nodeLookUp;

	private Dictionary<string, int> arrowNodes;

	private Dictionary<string, int> currentTreeNodes;

	private short currentActionFilledNodeCount;

	private NodeObject startNodeObject;

	private NodeObject endNodeObject;

	private Dictionary<string, GameObject> nodeObjects;

	private NodeObject hotNodeObject;

	private RectTransform nodeHolder;

	private Image nodeBGIMG;

	private CanvasGroup nodeHolderCG;

	private GameObject setDOSClockObject;

	private bool DOSiSHot;

	private bool allActionNodesActivated;

	private bool finalCountDownFired;

	private float DOSHOTTimeStamp;

	private float DOSGameTimeStamp;

	private short actionNodeActivatedCount;

	private Sequence DOSAttackOverSeq;

	private Sequence DOSAttackClockSeq;

	private int currentDOSChainIndex;

	private int nodeClickCount;

	private int maxNodeClickCount;

	private bool addSkillPoints = true;

	private bool isTwitchAttack;

	private string twitchHackerName;

	private string twitchHackerLevel;

	public class node
	{
		public node()
		{
		}

		public node(nodeType setType, DOSAttack.nodePosition setPos, Sprite setSprite, DOSAttack setDAP)
		{
			this.myType = setType;
			this.myPos = setPos;
			this.mySprite = setSprite;
			this.myParent = "none";
			this.myParentNode = new DOSAttack.node();
			this.myBranches = new List<string>();
			this.myBranchNodes = new List<DOSAttack.node>();
			this.mySibNodes = new List<DOSAttack.node>();
			this.drillUpNodes = new List<DOSAttack.node>();
			this.iWasChecked = false;
			this.myDAP = setDAP;
		}

		public string getNodeKey(short x = 0, short y = 0)
		{
			return ((int)(this.myPos.x + x)).ToString() + ":" + ((int)(this.myPos.y + y)).ToString();
		}

		public bool getNextNodeToCheck(out DOSAttack.node nodeToCheck)
		{
			nodeToCheck = new DOSAttack.node();
			bool flag = false;
			this.myDAP.reCount++;
			if (this.myDAP.reCount >= (int)this.myDAP.getMatrixCount())
			{
				return false;
			}
			if (!this.iWasChecked)
			{
				nodeToCheck = this;
				return true;
			}
			if (this.mySibNodes.Count > 0)
			{
				for (int i = 0; i < this.mySibNodes.Count; i++)
				{
					if (!this.mySibNodes[i].iWasChecked)
					{
						flag = true;
						nodeToCheck = this.mySibNodes[i];
						i = this.mySibNodes.Count + 1;
					}
				}
				if (flag)
				{
					return true;
				}
				if (this.myParent != "none")
				{
					for (int j = 0; j < this.myParentNode.myBranchNodes.Count; j++)
					{
						if (this.myParentNode.myBranchNodes[j].myBranchNodes.Count > 0 && this.myParentNode.myBranchNodes[j].myBranchNodes[0].getNextNodeToCheck(out nodeToCheck))
						{
							flag = true;
							j = this.mySibNodes.Count + 1;
						}
					}
				}
				else
				{
					for (int k = 0; k < this.mySibNodes.Count; k++)
					{
						if (this.mySibNodes[k].myBranchNodes.Count > 0 && this.mySibNodes[k].myBranchNodes[0].getNextNodeToCheck(out nodeToCheck))
						{
							flag = true;
							k = this.mySibNodes.Count + 1;
						}
					}
				}
				if (flag)
				{
					return true;
				}
				bool flag2 = false;
				DOSAttack.node node = this;
				DOSAttack.node node2 = new DOSAttack.node();
				while (!flag2)
				{
					this.drillUpNodes.Add(node);
					for (int l = 0; l < node.mySibNodes.Count; l++)
					{
						if (!flag)
						{
							if (!node.mySibNodes[l].iWasChecked)
							{
								node2 = node.mySibNodes[l];
								flag = true;
								flag2 = true;
								l = node.mySibNodes.Count + 1;
							}
							for (int m = 0; m < node.mySibNodes[l].myBranchNodes.Count; m++)
							{
								if (!node.mySibNodes[l].myBranchNodes[m].iWasChecked)
								{
									node2 = node.mySibNodes[l].myBranchNodes[m];
									flag = true;
									flag2 = true;
								}
							}
						}
					}
					if (node.myParent == "none")
					{
						flag2 = true;
					}
					else
					{
						node = node.myParentNode;
					}
				}
				if (flag)
				{
					nodeToCheck = node2;
					return true;
				}
				return false;
			}
			else
			{
				if (this.myBranchNodes.Count > 0)
				{
					return this.myBranchNodes[0].getNextNodeToCheck(out nodeToCheck);
				}
				bool flag3 = false;
				DOSAttack.node node3 = this;
				DOSAttack.node node4 = new DOSAttack.node();
				while (!flag3)
				{
					this.drillUpNodes.Add(node3);
					for (int n = 0; n < node3.mySibNodes.Count; n++)
					{
						if (!flag)
						{
							if (!node3.mySibNodes[n].iWasChecked)
							{
								node4 = node3.mySibNodes[n];
								flag = true;
								flag3 = true;
								n = node3.mySibNodes.Count + 1;
							}
							for (int num = 0; num < node3.mySibNodes[n].myBranchNodes.Count; num++)
							{
								if (!node3.mySibNodes[n].myBranchNodes[num].iWasChecked)
								{
									node4 = node3.mySibNodes[n].myBranchNodes[num];
									flag = true;
									flag3 = true;
								}
							}
						}
					}
					if (node3.myParent == "none")
					{
						flag3 = true;
					}
					else
					{
						node3 = node3.myParentNode;
					}
				}
				if (flag)
				{
					nodeToCheck = node4;
					return true;
				}
				return false;
			}
		}

		public void generateSibs()
		{
			if (this.myBranchNodes.Count > 0)
			{
				List<DOSAttack.node> list = new List<DOSAttack.node>();
				for (int i = 0; i < this.myBranchNodes.Count; i++)
				{
					list.Add(this.myBranchNodes[i]);
				}
				for (int j = 0; j < this.myBranchNodes.Count; j++)
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (this.myBranchNodes[j] != list[k])
						{
							this.myBranchNodes[j].mySibNodes.Add(list[k]);
						}
					}
				}
			}
		}

		public nodeType myType;

		public nodeType mySubType;

		public DOSAttack.nodePosition myPos;

		public Sprite mySprite;

		public string myParent;

		public DOSAttack.node myParentNode;

		public List<string> myBranches;

		public List<DOSAttack.node> myBranchNodes;

		public List<DOSAttack.node> mySibNodes;

		public bool iWasChecked;

		public DOSAttack myDAP;

		public List<DOSAttack.node> drillUpNodes;
	}

	public struct nodePosition
	{
		public nodePosition(short x, short y)
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
			DOSAttack.nodePosition nodePosition = (DOSAttack.nodePosition)obj;
			return nodePosition != null && (this.x == nodePosition.x && this.y == nodePosition.y);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool Equals(DOSAttack.nodePosition o)
		{
			return this.x == o.x && this.y == o.y;
		}

		public static bool operator ==(DOSAttack.nodePosition lhs, DOSAttack.nodePosition rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(DOSAttack.nodePosition lhs, DOSAttack.nodePosition rhs)
		{
			return !lhs.Equals(rhs);
		}

		public short x;

		public short y;
	}
}
