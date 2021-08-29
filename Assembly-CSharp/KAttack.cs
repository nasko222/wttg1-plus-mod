using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KAttack : MonoBehaviour
{
	public void PrepTwitchAttack(string hackerName, string hackerLevel)
	{
		this.isTwitchAttack = true;
		this.twitchHackerName = hackerName;
		this.twitchHackerLevel = hackerLevel;
	}

	public void startKAttack()
	{
		this.startKAttack(false);
	}

	public void startKAttack(bool inTutMode)
	{
		this.inTutorialMode = inTutMode;
		this.prepLevel();
	}

	public void skipCurrentLevel()
	{
		if (this.inHackerMode)
		{
			this.kISHot = false;
			this.KAttackFailed();
		}
	}

	public int getNextChainPoints()
	{
		if (this.inHackerMode)
		{
			return this.KChains[this.currentKChainIndex + 1].pointsRequired;
		}
		return 0;
	}

	public void skipCurrentLine()
	{
		if (this.inHackerMode)
		{
			this.KCodeLineInput.text = this.currentCodeObjects[(int)this.currentCodeStackIndex].GetComponent<KCodeLineObject>().codeLine.text;
			this.verifyCodeInput(this.KCodeLineInput.text);
		}
	}

	private void prepLevel()
	{
		int num = 0;
		if (this.inHackerMode)
		{
			for (int i = 0; i < this.KChains.Count; i++)
			{
				num = i;
				if (i < this.KChains.Count - 1 && this.hackerModeManager.getCurrentTMPGamePoints() < this.KChains[i + 1].pointsRequired)
				{
					i = this.KChains.Count;
				}
			}
			this.currentKChainIndex = num;
			this.warmUpTime = this.KChains[num].warmUpTime;
			this.KTime = this.KChains[num].KTime;
			this.KBoostTime = this.KChains[num].KBoostTime;
			this.codeLineCount = this.KChains[num].KNumOfLines;
			if (this.currentKChainIndex >= this.KChains.Count - 1)
			{
				this.hackerModeManager.setChainLevelMaster();
			}
			else
			{
				this.hackerModeManager.updateChainLevel(this.currentKChainIndex);
			}
			this.addSkillPoints = true;
			this.hackerModeManager.addChainCount();
			this.hackerModeManager.updateChainCount();
			this.currentBackSpaceCount = 0;
			this.addSkillPoints = true;
		}
		else
		{
			for (int j = 0; j < this.KLevels.Count; j++)
			{
				num = j;
				if (j < this.KLevels.Count - 1 && this.theCloud.getPlayerSkillPoints2() < this.KLevels[j + 1].skillPointesRequired)
				{
					j = this.KLevels.Count;
				}
			}
			if (this.isTwitchAttack)
			{
				if (this.twitchHackerLevel == "1337")
				{
					num = this.KLevels.Count - 1;
				}
				else if (this.twitchHackerLevel == "SCRIPT")
				{
					num = this.KLevels.Count / 2;
				}
				else
				{
					num = 1;
				}
			}
			this.KTime = this.KLevels[num].KTime;
			this.KBoostTime = this.KLevels[num].KBoostTime;
			this.codeLineCount = this.KLevels[num].KNumOfLines;
			if (num >= this.KLevels.Count - 1 && !this.isTwitchAttack)
			{
				GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_THE_DEVELOPER, true);
			}
		}
		this.prepKAttack();
	}

	private void prepKAttack()
	{
		bool flag = false;
		short num = 0;
		this.currentCodeStack = new List<KCodeLineDefinition>();
		if (this.inTutorialMode)
		{
			this.currentCodeStack.Add(this.tutorialLines[0]);
			this.currentCodeStack.Add(this.tutorialLines[1]);
		}
		else
		{
			while (!flag)
			{
				int index = UnityEngine.Random.Range(0, this.codeLines.Count);
				KCodeLineDefinition item = this.codeLines[index];
				if (!this.currentCodeStack.Contains(item))
				{
					this.currentCodeStack.Add(item);
					num += 1;
					if (num >= this.codeLineCount)
					{
						flag = true;
					}
				}
			}
		}
		float targetWidth = this.TargetWidth;
		float num2 = (float)this.codeLineCount * this.CodeLineHeight + this.TargetBorderWidth * 2f;
		float x2 = this.TargetWidth - this.TargetBorderWidth * 2f;
		float y = (float)this.codeLineCount * this.CodeLineHeight;
		float num3;
		if (this.inHackerMode)
		{
			num3 = 0f;
		}
		else
		{
			num3 = num2 / 2f + this.KTermTitle.GetComponent<RectTransform>().sizeDelta.y / 2f + 3f;
		}
		float y2 = num3 + 50f;
		float num4;
		float y3;
		if (this.inHackerMode)
		{
			num4 = -((float)Screen.height / 2f) - num2 / 2f - this.KTermInput.GetComponent<RectTransform>().sizeDelta.y / 2f - 3f;
			y3 = num4 + 50f;
		}
		else
		{
			num4 = -(num2 / 2f + this.KTermInput.GetComponent<RectTransform>().sizeDelta.y / 2f + 3f);
			y3 = num4 - 50f;
		}
		this.KGroup.SetActive(true);
		this.KGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, num2);
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
		submitEvent.AddListener(new UnityAction<string>(this.verifyCodeInput));
		this.KCodeLineInput.onEndEdit = submitEvent;
		this.KCodeLineInput.enabled = false;
		if (this.inHackerMode)
		{
			this.KTermInput.GetComponent<RectTransform>().transform.localPosition = new Vector3((float)Screen.width / 2f, y3, 0f);
		}
		else
		{
			this.KTermTitle.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, y2, 0f);
			this.KTermInput.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, y3, 0f);
		}
		this.KBase.GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, num2);
		this.KCodeBG.GetComponent<RectTransform>().sizeDelta = new Vector2(x2, y);
		this.KCodeNumberLine.GetComponent<RectTransform>().sizeDelta = new Vector2(this.KCodeNumberLine.GetComponent<RectTransform>().sizeDelta.x, y);
		this.KCodeLineHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(this.KCodeLineHolder.GetComponent<RectTransform>().sizeDelta.x, y);
		this.KCodeLineHolder.GetComponent<CanvasGroup>().alpha = 1f;
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.PowerMeUp, 1f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.PowerMeUp, 1f, false);
		}
		this.KAniSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.engageKAttack));
		this.KAniSeq.Insert(0f, DOTween.To(() => this.KBase.fillAmount, delegate(float x)
		{
			this.KBase.fillAmount = x;
		}, 1f, 1f).SetEase(Ease.Linear));
		this.KAniSeq.Insert(1f, DOTween.To(() => this.KCodeNumberLine.fillAmount, delegate(float x)
		{
			this.KCodeNumberLine.fillAmount = x;
		}, 1f, 0.5f).SetEase(Ease.OutSine));
		if (!this.inHackerMode)
		{
			this.KAniSeq.Insert(1f, DOTween.To(() => this.KTermTitle.GetComponent<RectTransform>().transform.localPosition, delegate(Vector3 x)
			{
				this.KTermTitle.GetComponent<RectTransform>().transform.localPosition = x;
			}, new Vector3(0f, num3, 0f), 0.5f).SetEase(Ease.OutSine));
			this.KAniSeq.Insert(1f, DOTween.To(() => this.KTermTitle.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.KTermTitle.GetComponent<CanvasGroup>().alpha = x;
			}, 1f, 0.5f).SetEase(Ease.OutSine));
			this.KAniSeq.Insert(1f, DOTween.To(() => this.KTermInput.GetComponent<RectTransform>().transform.localPosition, delegate(Vector3 x)
			{
				this.KTermInput.GetComponent<RectTransform>().transform.localPosition = x;
			}, new Vector3(0f, num4, 0f), 0.5f).SetEase(Ease.OutSine));
		}
		else
		{
			this.KAniSeq.Insert(1f, DOTween.To(() => this.KTermInput.GetComponent<RectTransform>().transform.localPosition, delegate(Vector3 x)
			{
				this.KTermInput.GetComponent<RectTransform>().transform.localPosition = x;
			}, new Vector3(0f, -50f, 0f), 0.5f).SetEase(Ease.OutSine).SetRelative(true));
		}
		this.KAniSeq.Insert(1f, DOTween.To(() => this.KTermInput.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KTermInput.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.5f).SetEase(Ease.OutSine));
		this.KAniSeq.Play<Sequence>();
	}

	private void engageKAttack()
	{
		float num = 0f;
		this.currentCodeObjects = new List<GameObject>();
		for (int i = 0; i < this.currentCodeStack.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.KCLObject);
			gameObject.transform.SetParent(this.KCodeLineHolder.transform);
			gameObject.GetComponent<KCodeLineObject>().myKAttack = this;
			gameObject.GetComponent<KCodeLineObject>().buildMe(i, (i + 1).ToString(), this.currentCodeStack[i].theCodeLine, 0f, num);
			num -= 44f;
			this.currentCodeObjects.Add(gameObject);
		}
		this.currentCodeStackIndex = 0;
		this.fireWarmClock();
	}

	private void fireKAttack()
	{
		UnityEngine.Object.Destroy(this.clockText.gameObject);
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.CountDownTick2, 0.7f, false);
		this.KClockObject = UnityEngine.Object.Instantiate<GameObject>(this.DOSClockObject);
		if (this.inHackerMode)
		{
			this.KClockObject.transform.SetParent(this.hackerModeManager.scoreHeaderObject.transform);
			this.KClockObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.KClockObject.GetComponent<Image>().sprite.rect.width * 0.75f, this.KClockObject.GetComponent<Image>().sprite.rect.height * 0.75f);
			this.KClockObject.GetComponent<DOSClock>().DOSClockImage.GetComponent<RectTransform>().sizeDelta = new Vector2(this.KClockObject.GetComponent<DOSClock>().DOSClockImage.sprite.rect.width * 0.75f, this.KClockObject.GetComponent<DOSClock>().DOSClockImage.sprite.rect.height * 0.75f);
			this.KClockObject.transform.localScale = new Vector3(1f, 1f, 1f);
			this.KClockObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
			this.KClockObject.transform.localPosition = new Vector3((float)Screen.width / 2f, -(this.KClockObject.GetComponent<RectTransform>().sizeDelta.y / 2f) - 1f, 0f);
		}
		else
		{
			this.KClockObject.transform.SetParent(this.KGroup.transform);
			this.KClockObject.transform.localScale = new Vector3(1f, 1f, 1f);
			this.KClockObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
			this.KClockObject.transform.localPosition = new Vector3(0f, this.KGroup.GetComponent<RectTransform>().sizeDelta.y / 2f + 75f, 0f);
			this.KClockObject.GetComponent<Image>().SetNativeSize();
			this.KClockObject.GetComponent<DOSClock>().DOSClockImage.SetNativeSize();
		}
		this.KAttackClockSeq = DOTween.Sequence();
		this.KAttackClockSeq.Insert(0f, DOTween.To(() => this.KClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount, delegate(float x)
		{
			this.KClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount = x;
		}, 0f, this.KTime).SetEase(Ease.Linear));
		this.KAttackClockSeq.Play<Sequence>();
		this.KCodeLineInput.enabled = true;
		this.KCodeLineInput.ActivateInputField();
		this.currentCodeObjects[(int)this.currentCodeStackIndex].GetComponent<KCodeLineObject>().IAmActive();
		this.finalCountDownFired = false;
		this.KGameTimeStamp = Time.time;
		this.HMKTimeStamp = Time.time;
		this.kISHot = true;
	}

	private void fireWarmClock()
	{
		TextGenerationSettings settings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		settings.textAnchor = TextAnchor.UpperCenter;
		settings.generateOutOfBounds = true;
		settings.generationExtents = new Vector2(50f, 20f);
		settings.pivot = Vector2.zero;
		settings.richText = true;
		settings.font = this.clockFont;
		settings.fontSize = 56;
		settings.fontStyle = FontStyle.Normal;
		settings.lineSpacing = 1f;
		settings.scaleFactor = 1f;
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		settings.horizontalOverflow = HorizontalWrapMode.Wrap;
		this.clockText = new GameObject("clockText", new Type[]
		{
			typeof(Text)
		}).GetComponent<Text>();
		this.clockText.font = this.clockFont;
		this.clockText.text = this.warmUpTime.ToString();
		this.clockText.color = this.clockColor;
		this.clockText.fontSize = 56;
		if (this.inHackerMode)
		{
			this.clockText.transform.SetParent(this.hackerModeManager.scoreHeaderObject.transform);
			this.clockText.transform.localPosition = new Vector3((float)Screen.width / 2f, -(this.clockText.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2f - 30f), 0f);
		}
		else
		{
			this.clockText.transform.SetParent(this.KGroup.transform);
			this.clockText.transform.localPosition = new Vector3(0f, this.KGroup.GetComponent<RectTransform>().sizeDelta.y / 2f + 75f, 0f);
		}
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
		this.warmClockSeq.SetLoops(Mathf.RoundToInt(this.warmUpTime));
		this.warmClockSeq.Play<Sequence>();
		this.clockTimeStamp = Time.time;
		this.clockMicroTimeStamp = Time.time;
		this.clockMicroCount = this.warmUpTime;
		this.warmClockActive = true;
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.CountDownTick1, 0.7f, false);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.HACKINGSFX, this.CountDownTick1, 0.7f, false);
		}
	}

	private void verifyCodeInput(string theCode)
	{
		if (theCode != string.Empty)
		{
			this.KCodeLineInput.text = string.Empty;
			this.KCodeLineInput.ActivateInputField();
			if (theCode == this.currentCodeStack[(int)this.currentCodeStackIndex].theCodeLine)
			{
				this.currentCodeObjects[(int)this.currentCodeStackIndex].GetComponent<KCodeLineObject>().ValidInput();
				this.currentCodeStackIndex += 1;
				if ((int)this.currentCodeStackIndex >= this.currentCodeStack.Count)
				{
					this.KAttackFailed();
				}
				else
				{
					this.boostKTime();
					this.currentCodeObjects[(int)this.currentCodeStackIndex].GetComponent<KCodeLineObject>().IAmActive();
				}
			}
			else
			{
				this.addSkillPoints = false;
				this.currentCodeObjects[(int)this.currentCodeStackIndex].GetComponent<KCodeLineObject>().InvalidInput();
			}
		}
		else
		{
			this.KCodeLineInput.ActivateInputField();
		}
	}

	private void boostKTime()
	{
		if (this.KBoostTime > 0f)
		{
			this.KGameTimeStamp += this.KBoostTime;
			if (this.finalCountDownFired)
			{
				if (this.inHackerMode)
				{
					GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.CountDownTick1.name);
				}
				else
				{
					GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.CountDownTick1.name);
				}
				this.finalCountDownFired = false;
			}
			this.KAttackClockSeq.Kill(false);
			this.KAttackClockSeq = DOTween.Sequence();
			float num = Time.time - this.KGameTimeStamp;
			float num2 = this.KTime - num;
			float fillAmount;
			if (num2 > this.KTime)
			{
				fillAmount = 1f;
			}
			else
			{
				fillAmount = num2 / this.KTime;
			}
			this.KClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount = fillAmount;
			this.KAttackClockSeq.Insert(0f, DOTween.To(() => this.KClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount, delegate(float x)
			{
				this.KClockObject.GetComponent<DOSClock>().DOSClockImage.fillAmount = x;
			}, 0f, num2).SetEase(Ease.Linear));
			this.KAttackClockSeq.Play<Sequence>();
		}
	}

	private void KAttackPassed()
	{
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.ClockAlmostUp.name);
		}
		else
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.ClockAlmostUp.name);
		}
		if (this.isTwitchAttack)
		{
			GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("@" + this.twitchHackerName + " IS 1337!", 10f);
			this.isTwitchAttack = false;
			this.twitchHackerLevel = string.Empty;
			this.twitchHackerName = string.Empty;
		}
		this.KCodeLineInput.enabled = false;
		this.kISHot = false;
		this.KAttackClockSeq.Kill(false);
		UnityEngine.Object.Destroy(this.KClockObject);
		this.DidPlayerPass = false;
		this.TriggerByeAni();
	}

	private void KAttackFailed()
	{
		if (this.inHackerMode)
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.ClockAlmostUp.name);
		}
		else
		{
			GameManager.AudioSlinger.RemoveSound(AudioHubs.COMPUTER, this.ClockAlmostUp.name);
		}
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
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.HMPuzzlePass, 0.65f, false);
			this.tallyBonusPoints();
		}
		else if ((double)(Time.time - this.KGameTimeStamp) <= (double)this.KTime * 0.2)
		{
			this.theCloud.addPlayerSkillPoints2(this.SkillPointsA);
		}
		else if ((double)(Time.time - this.KGameTimeStamp) <= (double)this.KTime * 0.65)
		{
			this.theCloud.addPlayerSkillPoints2(this.SkillPointsB);
		}
		else
		{
			this.theCloud.addPlayerSkillPoints2(this.SkillPointsC);
		}
		this.KCodeLineInput.enabled = false;
		this.kISHot = false;
		this.KAttackClockSeq.Kill(false);
		UnityEngine.Object.Destroy(this.KClockObject);
		this.DidPlayerPass = true;
		this.TriggerByeAni();
	}

	private void TriggerByeAni()
	{
		float num = (float)this.codeLineCount * this.CodeLineHeight + this.TargetBorderWidth * 2f;
		float num2;
		float y;
		if (this.inHackerMode)
		{
			num2 = 0f;
			float num3 = -((float)Screen.height / 2f) - num / 2f - this.KTermInput.GetComponent<RectTransform>().sizeDelta.y / 2f - 3f;
			y = num3 + 50f;
		}
		else
		{
			num2 = num / 2f + this.KTermTitle.GetComponent<RectTransform>().sizeDelta.y / 2f + 3f;
			float num3 = -(num / 2f + this.KTermInput.GetComponent<RectTransform>().sizeDelta.y / 2f + 3f);
			y = num3 - 50f;
		}
		float y2 = num2 + 50f;
		if (!this.inHackerMode)
		{
			GameManager.GetDOSTwitch().DismissTwitchHacker();
		}
		this.ByeSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.ByeAniDone));
		this.ByeSeq.Insert(0f, DOTween.To(() => this.KCodeLineHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KCodeLineHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f).SetEase(Ease.OutSine));
		this.ByeSeq.Insert(0f, DOTween.To(() => this.KBase.fillAmount, delegate(float x)
		{
			this.KBase.fillAmount = x;
		}, 0f, 0.5f).SetEase(Ease.OutSine));
		this.ByeSeq.Insert(0f, DOTween.To(() => this.KCodeNumberLine.fillAmount, delegate(float x)
		{
			this.KCodeNumberLine.fillAmount = x;
		}, 0f, 0.5f).SetEase(Ease.OutSine));
		if (this.inHackerMode)
		{
			this.ByeSeq.Insert(0f, DOTween.To(() => this.KTermInput.GetComponent<RectTransform>().transform.localPosition, delegate(Vector3 x)
			{
				this.KTermInput.GetComponent<RectTransform>().transform.localPosition = x;
			}, new Vector3((float)Screen.width / 2f, y, 0f), 0.5f).SetEase(Ease.OutSine));
		}
		else
		{
			this.ByeSeq.Insert(0f, DOTween.To(() => this.KTermTitle.GetComponent<RectTransform>().transform.localPosition, delegate(Vector3 x)
			{
				this.KTermTitle.GetComponent<RectTransform>().transform.localPosition = x;
			}, new Vector3(0f, y2, 0f), 0.5f).SetEase(Ease.OutSine));
			this.ByeSeq.Insert(0f, DOTween.To(() => this.KTermTitle.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.KTermTitle.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 0.5f).SetEase(Ease.OutSine));
			this.ByeSeq.Insert(0f, DOTween.To(() => this.KTermInput.GetComponent<RectTransform>().transform.localPosition, delegate(Vector3 x)
			{
				this.KTermInput.GetComponent<RectTransform>().transform.localPosition = x;
			}, new Vector3(0f, y, 0f), 0.5f).SetEase(Ease.OutSine));
		}
		this.ByeSeq.Insert(0f, DOTween.To(() => this.KTermInput.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KTermInput.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f).SetEase(Ease.OutSine));
	}

	private void ByeAniDone()
	{
		for (int i = 0; i < this.currentCodeObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(this.currentCodeObjects[i]);
		}
		this.currentCodeObjects.Clear();
		this.currentCodeStack.Clear();
		this.currentCodeStackIndex = 0;
		this.KGroup.SetActive(false);
		if (this.inHackerMode)
		{
			if (this.DidPlayerPass)
			{
				GameManager.TimeSlinger.FireTimer(0.4f, new Action(this.startKAttack));
			}
			else if (this.hackerModeManager.FireAShell())
			{
				GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.startKAttack));
			}
			else
			{
				this.hackerModeManager.triggerGameOver();
				this.hackerModeManager.resetKAttack();
			}
		}
		else if (this.DidPlayerPass)
		{
			this.hackerManager.KAttackBlocked();
		}
		else
		{
			this.hackerManager.KAttackPassed();
		}
	}

	private void tallyBonusPoints()
	{
		if (this.inHackerMode)
		{
			int completeBonusPoints = this.KChains[this.currentKChainIndex].completeBonusPoints;
			int timePoints = 0;
			int skillPoints = 0;
			if (Time.time - this.HMKTimeStamp <= this.KTime * (1f - this.KChains[this.currentKChainIndex].timeBonusRange))
			{
				timePoints = this.KChains[this.currentKChainIndex].timeBonusPoints;
			}
			if (this.currentBackSpaceCount > (int)this.KChains[this.currentKChainIndex].maxNumberOfBackSpaces)
			{
				this.addSkillPoints = false;
			}
			if (this.addSkillPoints)
			{
				skillPoints = this.KChains[this.currentKChainIndex].skillBonusPoints;
			}
			this.hackerModeManager.addGamePoints(completeBonusPoints, timePoints, skillPoints, (int)this.KChains[this.currentKChainIndex].bonusMultiplier);
		}
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
				this.fireKAttack();
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
		else if (this.kISHot)
		{
			if (Time.time - this.KGameTimeStamp >= this.KTime - this.ClockAlmostUp.length && !this.finalCountDownFired)
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
			if (Time.time - this.KGameTimeStamp >= this.KTime)
			{
				this.kISHot = false;
				this.KAttackPassed();
			}
		}
		if (this.KCodeLineInput.enabled)
		{
			if (!this.KCodeLineInput.isFocused)
			{
				this.KCodeLineInput.ActivateInputField();
			}
			else if (this.kISHot)
			{
				if (this.inHackerMode && (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)))
				{
					this.currentBackSpaceCount++;
				}
				this.currentCodeObjects[(int)this.currentCodeStackIndex].GetComponent<KCodeLineObject>().hotCheck(this.KCodeLineInput.text);
			}
		}
		if (this.inHackerMode && this.hackerModeGameActive)
		{
			if (this.kISHot)
			{
				this.hackerModeManager.gameIsHot = true;
			}
			else
			{
				this.hackerModeManager.gameIsHot = false;
			}
		}
	}

	[Range(1f, 10f)]
	public float warmUpTime = 3f;

	public bool inHackerMode;

	public bool hackerModeGameActive;

	public bool inTutorialMode;

	public TheCloud theCloud;

	public HackerManager hackerManager;

	public HackerModeManager hackerModeManager;

	public GameObject KCLObject;

	public GameObject DOSClockObject;

	public GameObject KGroup;

	public GameObject KTermTitle;

	public GameObject KTermInput;

	public GameObject KCodeLineHolder;

	public Image KBase;

	public Image KCodeBG;

	public Image KCodeNumberLine;

	public InputField KCodeLineInput;

	public float TargetWidth = 1076f;

	public float TargetHeight = 300f;

	public float TargetBorderWidth = 2f;

	public float CodeLineHeight = 44f;

	public Font clockFont;

	public Color clockColor;

	public AudioClip PowerMeUp;

	public AudioClip CountDownTick1;

	public AudioClip CountDownTick2;

	public AudioClip ClockAlmostUp;

	public AudioClip ValidInputSound;

	public AudioClip InvalidInputSound;

	public AudioClip HighlightCodeSound;

	public AudioClip HMPuzzlePass;

	public int SkillPointsA = 10;

	public int SkillPointsB = 5;

	public int SkillPointsC = 2;

	public List<KLevelDefinition> KLevels;

	public List<KChainDefinition> KChains;

	public List<KCodeLineDefinition> codeLines;

	public List<KCodeLineDefinition> tutorialLines;

	private float KTime = 30f;

	private float KBoostTime = 5f;

	private short codeLineCount = 3;

	private Sequence KAniSeq;

	private List<KCodeLineDefinition> currentCodeStack;

	private List<GameObject> currentCodeObjects;

	private short currentCodeStackIndex;

	private Text clockText;

	private float clockTimeStamp;

	private float clockMicroTimeStamp;

	private float clockMicroCount;

	private float KGameTimeStamp;

	private bool warmClockActive;

	private bool kISHot;

	private bool finalCountDownFired;

	private bool DidPlayerPass;

	private GameObject KClockObject;

	private Sequence warmClockSeq;

	private Sequence KAttackClockSeq;

	private Sequence ByeSeq;

	private int currentKChainIndex;

	private int currentBackSpaceCount;

	private bool addSkillPoints;

	private float HMKTimeStamp;

	private bool isTwitchAttack;

	private string twitchHackerName;

	private string twitchHackerLevel;
}
