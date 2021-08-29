using System;
using System.Collections.Generic;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class HackerModeManager : MonoBehaviour
{
	public void performMenuAction(string setActionName)
	{
		this.prepGameType(setActionName);
	}

	public void addChainCount()
	{
		this.currentChainCount++;
	}

	public int getCurrentChainCount()
	{
		return this.currentChainCount;
	}

	public int getCurrentGamePoints()
	{
		return this.currentGamePoints;
	}

	public int getCurrentTMPGamePoints()
	{
		return this.tmpCurrentGamePoints;
	}

	public void updateChainCount()
	{
		this.chainCountValue.text = this.currentChainCount.ToString();
	}

	public void updateChainLevel(int curIndex)
	{
		if (this.currentChainIndex != curIndex)
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.nextChainSFX, 0.7f, false);
			this.chainLevelValue.text = (curIndex + 1).ToString();
			this.currentChainIndex = curIndex;
		}
		else if (curIndex == 0)
		{
			this.chainLevelValue.text = (curIndex + 1).ToString();
			this.currentChainIndex = 0;
		}
	}

	public void setChainLevelMaster()
	{
		if (this.chainLevelValue.text != "Master")
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.masterChainSFX, 1f, false);
			this.chainLevelValue.text = "Master";
		}
	}

	public void addGamePoints(int completePoints, int timePoints, int skillPoints, int bonusMulti)
	{
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		int num2 = completePoints + timePoints + skillPoints;
		this.completePointsValue.text = completePoints.ToString();
		this.timePointsValue.text = timePoints.ToString();
		this.skillPointsValue.text = skillPoints.ToString();
		this.totalPointsValue.text = num2.ToString();
		if (this.presentBonusPointsSeq == null)
		{
			flag2 = false;
		}
		else if (this.presentBonusPointsSeq.IsPlaying())
		{
			flag2 = true;
		}
		if (!flag2)
		{
			if (bonusMulti > 1 && timePoints > 0 && skillPoints > 0)
			{
				flag = true;
				this.multiplierValue.text = bonusMulti.ToString() + "X";
				this.multiplierValueShadow.text = bonusMulti.ToString() + "X";
				num = num2 * bonusMulti;
				GameManager.TimeSlinger.FireIntIntTimer(1.2f, new Action<int, int>(this.setMultiplierText), num2, num);
			}
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.bonusPointSlideSFX, 0.35f, false, 0f);
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.bonusPointSlideSFX, 0.35f, false, 0.3f);
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.bonusPointSlideSFX, 0.35f, false, 0.6f);
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.totalPointSlideSFX, 0.35f, false, 0.9f);
			this.presentBonusPointsSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.resetPresentGamePoints));
			this.presentBonusPointsSeq.Insert(0f, DOTween.To(() => this.completeBonusObject.transform.localPosition, delegate(Vector3 x)
			{
				this.completeBonusObject.transform.localPosition = x;
			}, new Vector3(0f, this.completeBonusObject.transform.localPosition.y, this.completeBonusObject.transform.localPosition.z), 0.4f).SetEase(Ease.OutSine));
			this.presentBonusPointsSeq.Insert(0.3f, DOTween.To(() => this.timeBonusObject.transform.localPosition, delegate(Vector3 x)
			{
				this.timeBonusObject.transform.localPosition = x;
			}, new Vector3(0f, this.timeBonusObject.transform.localPosition.y, this.timeBonusObject.transform.localPosition.z), 0.4f).SetEase(Ease.OutSine));
			this.presentBonusPointsSeq.Insert(0.6f, DOTween.To(() => this.skillBonusObject.transform.localPosition, delegate(Vector3 x)
			{
				this.skillBonusObject.transform.localPosition = x;
			}, new Vector3(0f, this.skillBonusObject.transform.localPosition.y, this.skillBonusObject.transform.localPosition.z), 0.4f).SetEase(Ease.OutSine));
			this.presentBonusPointsSeq.Insert(0.9f, DOTween.To(() => this.totalPointsObject.transform.localPosition, delegate(Vector3 x)
			{
				this.totalPointsObject.transform.localPosition = x;
			}, new Vector3(0f, this.totalPointsObject.transform.localPosition.y, this.totalPointsObject.transform.localPosition.z), 0.4f).SetEase(Ease.OutSine));
			if (flag)
			{
				this.presentBonusPointsSeq.Insert(1.3f, DOTween.To(() => this.multiplierValue.transform.localScale, delegate(Vector3 x)
				{
					this.multiplierValue.transform.localScale = x;
				}, new Vector3(0.1f, 0.1f, 0.1f), 0.75f).SetEase(Ease.InSine));
				this.presentBonusPointsSeq.Insert(1.3f, DOTween.To(() => this.multiplierValueShadow.transform.localScale, delegate(Vector3 x)
				{
					this.multiplierValueShadow.transform.localScale = x;
				}, new Vector3(0.1f, 0.1f, 0.1f), 0.75f).SetEase(Ease.InSine));
				this.presentBonusPointsSeq.Insert(1.3f, DOTween.To(() => this.multiplierHolderObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.multiplierHolderObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.75f).SetEase(Ease.OutSine));
				this.presentBonusPointsSeq.Insert(3.5f, DOTween.To(() => this.completeBonusObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.completeBonusObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
				this.presentBonusPointsSeq.Insert(3.5f, DOTween.To(() => this.timeBonusObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.timeBonusObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
				this.presentBonusPointsSeq.Insert(3.5f, DOTween.To(() => this.skillBonusObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.skillBonusObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
				this.presentBonusPointsSeq.Insert(3.5f, DOTween.To(() => this.totalPointsObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.totalPointsObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
			}
			else
			{
				this.presentBonusPointsSeq.Insert(2.9f, DOTween.To(() => this.completeBonusObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.completeBonusObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
				this.presentBonusPointsSeq.Insert(2.9f, DOTween.To(() => this.timeBonusObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.timeBonusObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
				this.presentBonusPointsSeq.Insert(2.9f, DOTween.To(() => this.skillBonusObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.skillBonusObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
				this.presentBonusPointsSeq.Insert(2.9f, DOTween.To(() => this.totalPointsObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
				{
					this.totalPointsObject.GetComponent<CanvasGroup>().alpha = x;
				}, 0f, 0.4f).SetEase(Ease.Linear));
			}
			this.presentBonusPointsSeq.Play<Sequence>();
		}
		this.addGamePoints(num2 + num);
	}

	public void triggerGameOver()
	{
		this.myHMUM.setCoolOff();
		this.inGameMode = false;
		this.mainCamera.GetComponent<Glitch>().enabled = true;
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.gameOverFlashSFX, 0.85f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.gameOverSayingSFX, 0.85f, false, 1.3f);
		GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.HackerModeMusic.name);
		GameManager.TimeSlinger.FireTimer(2.5f, new Action(this.prepMainMenu));
		GameManager.TimeSlinger.KillTimerWithID("HMCoolOffTimer");
		this.FlashObject.SetActive(true);
		this.FlashObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.GameOverObject.SetActive(true);
		this.scoreHeaderObject.transform.localPosition = new Vector3(0f, 51f, 0f);
		DOTween.To(() => this.upgradesFooterObject.transform.localPosition, delegate(Vector3 x)
		{
			this.upgradesFooterObject.transform.localPosition = x;
		}, new Vector3(0f, -44f, 0f), 0.1f).SetEase(Ease.Linear).SetRelative(true);
		this.gameOverSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.showFinalPoints));
		this.gameOverSeq.Insert(0.2f, DOTween.To(() => this.FlashObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.FlashObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.75f).SetEase(Ease.Linear));
		this.gameOverSeq.Insert(0.5f, DOTween.To(() => this.GameOverObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.GameOverObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.OutSine));
		this.gameOverSeq.Play<Sequence>();
	}

	public void resetDOSAttack()
	{
		this.DOSAttackHolder.SetActive(false);
		this.myDOSAttack.DOSHolder.GetComponent<CanvasGroup>().alpha = 0f;
	}

	public void resetKAttack()
	{
		this.KAttackHolder.SetActive(false);
		this.KAttackHolder.GetComponent<CanvasGroup>().alpha = 0f;
	}

	public void resetVapeAttack()
	{
		this.VapeAttackHolder.SetActive(false);
	}

	public void fireATextRoller(GameObject theTextObject, int fromValue, int toValue, float delayPerUnit, float maxDuration)
	{
		theTextObject.AddComponent<TextRoller>();
		theTextObject.GetComponent<TextRoller>().Fire(fromValue, toValue, delayPerUnit, maxDuration);
	}

	public void fireATextRollerWithSFX(GameObject theTextObject, int fromValue, int toValue, float delayPerUnit, float maxDuration, AudioClip theSFX, AudioHubs setAH, AudioLayer setAL, float setVol)
	{
		theTextObject.AddComponent<TextRoller>();
		theTextObject.GetComponent<TextRoller>().Fire(fromValue, toValue, delayPerUnit, maxDuration, theSFX, setAH, setAL, setVol);
	}

	public HMGameData getGameData()
	{
		return this.myGameData;
	}

	public void hideMenus()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.menuHideSFX2, 1f, false, 0f);
		this.menuHideAniSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.clearMenusSoft));
		for (int i = 0; i < this.currentMenuObjects.Count; i++)
		{
			this.currentMenuObjects[i].GetComponent<HMMenuObject>().hideMe();
		}
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.locTopScoresObject.transform.localPosition, delegate(Vector3 x)
		{
			this.locTopScoresObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.varObject.transform.localPosition, delegate(Vector3 x)
		{
			this.varObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.heapObject.transform.localPosition, delegate(Vector3 x)
		{
			this.heapObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.binObject.transform.localPosition, delegate(Vector3 x)
		{
			this.binObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.clearDataText.transform.localPosition, delegate(Vector3 x)
		{
			this.clearDataText.transform.localPosition = x;
		}, new Vector3(0f, -40f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuHideAniSeq.Play<Sequence>();
	}

	public void showMenus()
	{
		this.prepMenus();
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.menuShowSFX, 1f, false, 0f);
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.rollCurrentStats));
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.aniShowMenuItems));
		this.menuAniSeq = DOTween.Sequence();
		for (int i = 0; i < this.currentMenuObjects.Count; i++)
		{
			this.currentMenuObjects[i].GetComponent<HMMenuObject>().showMe(0f);
		}
		this.menuAniSeq.Insert(2f, DOTween.To(() => this.locTopScoresObject.transform.localPosition, delegate(Vector3 x)
		{
			this.locTopScoresObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(2f, DOTween.To(() => this.varObject.transform.localPosition, delegate(Vector3 x)
		{
			this.varObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(2f, DOTween.To(() => this.heapObject.transform.localPosition, delegate(Vector3 x)
		{
			this.heapObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(2f, DOTween.To(() => this.binObject.transform.localPosition, delegate(Vector3 x)
		{
			this.binObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(2f, DOTween.To(() => this.clearDataText.transform.localPosition, delegate(Vector3 x)
		{
			this.clearDataText.transform.localPosition = x;
		}, new Vector3(0f, 40f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Play<Sequence>();
	}

	public void lockMenus()
	{
		for (int i = 0; i < this.currentMenuItemObjects.Count; i++)
		{
			this.currentMenuItemObjects[i].GetComponent<HMMenuItemObject>().lockMe();
		}
	}

	public void unLockMenus()
	{
		for (int i = 0; i < this.currentMenuItemObjects.Count; i++)
		{
			this.currentMenuItemObjects[i].GetComponent<HMMenuItemObject>().unLockMe();
		}
	}

	public bool amIInGameMode()
	{
		return this.inGameMode && this.gameIsHot;
	}

	public void FireANopSled()
	{
		this.mainCamera.GetComponent<Glitch>().enabled = true;
		this.mainCamera.GetComponent<Glitch>().RandomActivation = false;
		this.mainCamera.GetComponent<AnalogTV>().enabled = true;
		this.mainCamera.GetComponent<PixelMatrix>().enabled = true;
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.clearUpgradeEFXS));
		GameManager.AudioSlinger.FireSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.NOPSledFireSFX, 1f, false);
		hackerModeGameType hackerModeGameType = this.currentGameType;
		if (hackerModeGameType != hackerModeGameType.DOS)
		{
			if (hackerModeGameType != hackerModeGameType.KERNAL)
			{
				if (hackerModeGameType == hackerModeGameType.CLOUD)
				{
					this.tmpCurrentGamePoints = this.myVapeAttack.getNextChainPoints();
					this.myVapeAttack.skipCurrentLevel();
				}
			}
			else
			{
				this.tmpCurrentGamePoints = this.myKAttack.getNextChainPoints();
				this.myKAttack.skipCurrentLevel();
			}
		}
		else
		{
			this.tmpCurrentGamePoints = this.myDOSAttack.getNextChainPoints();
			this.myDOSAttack.skipCurrentLevel();
		}
		this.rollCurrentStats();
	}

	public bool FireAShell()
	{
		if (this.myGameData.currentShells >= 1)
		{
			this.mainCamera.GetComponent<Glitch>().enabled = true;
			this.mainCamera.GetComponent<Glitch>().RandomActivation = false;
			this.mainCamera.GetComponent<AnalogTV>().enabled = true;
			this.mainCamera.GetComponent<PixelMatrix>().enabled = true;
			this.SkullCG.alpha = 1f;
			this.skullLaughSeq = DOTween.Sequence();
			this.skullLaughSeq.Insert(0f, DOTween.To(() => this.skullBottom.transform.localPosition, delegate(Vector3 x)
			{
				this.skullBottom.transform.localPosition = x;
			}, new Vector3(0f, -30f, 0f), 0.15f).SetEase(Ease.Linear).SetRelative(true));
			this.skullLaughSeq.Insert(0.15f, DOTween.To(() => this.skullBottom.transform.localPosition, delegate(Vector3 x)
			{
				this.skullBottom.transform.localPosition = x;
			}, new Vector3(0f, 30f, 0f), 0.15f).SetEase(Ease.Linear).SetRelative(true));
			this.skullLaughSeq.SetLoops(-1);
			this.skullLaughSeq.Play<Sequence>();
			this.presentGameModeSeq = DOTween.Sequence();
			this.presentGameModeSeq.Insert(0f, DOTween.To(() => this.SkullCG.alpha, delegate(float x)
			{
				this.SkullCG.alpha = x;
			}, 0f, 1.8f).SetEase(Ease.Linear));
			this.presentGameModeSeq.Play<Sequence>();
			GameManager.TimeSlinger.FireTimer(2f, new Action(this.clearUpgradeEFXS));
			GameManager.AudioSlinger.FireSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.FireShellSFX, 1f, false);
			this.myGameData.currentShells = this.myGameData.currentShells - 1;
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
			this.rollCurrentStats();
			return true;
		}
		return false;
	}

	public hackerModeGameType getCurrentGameMode()
	{
		return this.currentGameType;
	}

	private void prepAssets()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.None;
		if (this.myGameData.musicIsOn)
		{
			this.MusicONOFFBTN.GetComponent<HMBTNLink>().setMyBool(true);
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.MUSIC);
		}
		else
		{
			this.MusicONOFFBTN.GetComponent<HMBTNLink>().setMyBool(false);
			GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
		}
		if (this.myGameData.sfxIsOn)
		{
			this.SFXONOFFBTN.GetComponent<HMBTNLink>().setMyBool(true);
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.HACKINGSFX);
		}
		else
		{
			this.SFXONOFFBTN.GetComponent<HMBTNLink>().setMyBool(false);
			GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.HACKINGSFX, 0f);
		}
		this.MusicONOFFBTN.GetComponent<HMBTNLink>().setMyAction(new Action(this.toggleMusic));
		this.SFXONOFFBTN.GetComponent<HMBTNLink>().setMyAction(new Action(this.toggleSFX));
	}

	private void toggleMusic()
	{
		if (this.myGameData.musicIsOn)
		{
			this.myGameData.musicIsOn = false;
			GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
		}
		else
		{
			this.myGameData.musicIsOn = true;
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.MUSIC);
		}
		GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
	}

	private void toggleSFX()
	{
		if (this.myGameData.sfxIsOn)
		{
			this.myGameData.sfxIsOn = false;
			GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.HACKINGSFX, 0f);
		}
		else
		{
			this.myGameData.sfxIsOn = true;
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.HACKINGSFX);
		}
		GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
	}

	private void prepGameType(string gameType)
	{
		if (gameType == "DOS")
		{
			this.aniHideMenus();
			this.prepDOSAttack();
		}
		else if (gameType == "KERNAL")
		{
			this.aniHideMenus();
			this.prepKAttack();
		}
		else if (gameType == "CLOUD")
		{
			this.aniHideMenus();
			this.prepCLOUDAttack();
		}
		else if (!(gameType == "TOPDOS"))
		{
			if (!(gameType == "TOPKERNAL"))
			{
				if (!(gameType == "TOPCLOUD"))
				{
					if (gameType == "QMAINMENU")
					{
						this.aniHideMenusNoGameMode();
						GameManager.TimeSlinger.FireTimer(2.5f, new Action(this.loadMainMenu));
					}
					else if (gameType == "QGAME")
					{
						Application.Quit();
					}
				}
			}
		}
	}

	private void prepMenus()
	{
		this.currentMenuObjects = new List<GameObject>();
		float num = 0f;
		for (int i = 0; i < this.hackerMenus.Count; i++)
		{
			HackerModeManager.menuVectors menuVectors = new HackerModeManager.menuVectors((short)this.hackerMenus[i].MenuOptions.Count);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.HMMenuObject);
			gameObject.transform.SetParent(this.MenuHolder.transform);
			gameObject.GetComponent<HMMenuObject>().buildMe(menuVectors.targetWidthFull, menuVectors.targetHeightFull, this.hackerMenus[i].MenuName);
			this.currentMenuObjects.Add(gameObject);
		}
		for (int j = 0; j < this.currentMenuObjects.Count; j++)
		{
			num = num + this.currentMenuObjects[j].GetComponent<RectTransform>().sizeDelta.y + 25f;
		}
		float num2 = -135f;
		for (int k = 0; k < this.currentMenuObjects.Count; k++)
		{
			this.currentMenuObjects[k].transform.localPosition = new Vector3((float)Screen.width / 2f - this.currentMenuObjects[k].GetComponent<RectTransform>().sizeDelta.x / 2f, num2, 0f);
			num2 = num2 - this.currentMenuObjects[k].GetComponent<RectTransform>().sizeDelta.y - 25f;
		}
	}

	private void aniMainMenu()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.menuPromptSFX, 1f, false);
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.menuShowSFX, 1f, false, 2f);
		GameManager.TimeSlinger.FireTimer(2.8f, new Action(this.aniHeader));
		GameManager.TimeSlinger.FireTimer(3f, new Action(this.rollCurrentStats));
		GameManager.TimeSlinger.FireTimer(3f, new Action(this.aniShowMenuItems));
		this.WTTGHeaderObject.SetActive(true);
		this.menuAniSeq = DOTween.Sequence();
		this.menuAniSeq.Insert(0f, DOTween.To(() => this.menuPrompt1.fillAmount, delegate(float x)
		{
			this.menuPrompt1.fillAmount = x;
		}, 1f, 0.2f).SetEase(Ease.Linear));
		this.menuAniSeq.Insert(0.6f, DOTween.To(() => this.menuPrompt2.fillAmount, delegate(float x)
		{
			this.menuPrompt2.fillAmount = x;
		}, 1f, 0.2f).SetEase(Ease.Linear));
		this.menuAniSeq.Insert(1.4f, DOTween.To(() => this.menuPrompt3.fillAmount, delegate(float x)
		{
			this.menuPrompt3.fillAmount = x;
		}, 1f, 0.2f).SetEase(Ease.Linear));
		this.menuAniSeq.Insert(2.8f, DOTween.To(() => this.WTTGHeaderObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.WTTGHeaderObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 1f).SetEase(Ease.OutSine));
		this.menuAniSeq.Insert(2.6f, DOTween.To(() => this.menuPrompt1.fillAmount, delegate(float x)
		{
			this.menuPrompt1.fillAmount = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		this.menuAniSeq.Insert(2.6f, DOTween.To(() => this.menuPrompt2.fillAmount, delegate(float x)
		{
			this.menuPrompt2.fillAmount = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		this.menuAniSeq.Insert(2.6f, DOTween.To(() => this.menuPrompt3.fillAmount, delegate(float x)
		{
			this.menuPrompt3.fillAmount = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		this.menuAniSeq.Insert(2.8f, DOTween.To(() => this.blackBG.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.blackBG.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f).SetEase(Ease.Linear));
		for (int i = 0; i < this.currentMenuObjects.Count; i++)
		{
			this.currentMenuObjects[i].GetComponent<HMMenuObject>().showMe(2f);
		}
		this.menuAniSeq.Insert(3f, DOTween.To(() => this.locTopScoresObject.transform.localPosition, delegate(Vector3 x)
		{
			this.locTopScoresObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(3f, DOTween.To(() => this.varObject.transform.localPosition, delegate(Vector3 x)
		{
			this.varObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(3f, DOTween.To(() => this.heapObject.transform.localPosition, delegate(Vector3 x)
		{
			this.heapObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(3f, DOTween.To(() => this.binObject.transform.localPosition, delegate(Vector3 x)
		{
			this.binObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Insert(3f, DOTween.To(() => this.clearDataText.transform.localPosition, delegate(Vector3 x)
		{
			this.clearDataText.transform.localPosition = x;
		}, new Vector3(0f, 40f, 0f), 1f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuAniSeq.Play<Sequence>();
	}

	private void aniShowMenuItems()
	{
		this.currentMenuItemObjects = new List<GameObject>();
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		HackerModeManager.menuVectors menuVectors = new HackerModeManager.menuVectors(1);
		for (int i = 0; i < this.hackerMenus.Count; i++)
		{
			for (int j = 0; j < this.hackerMenus[i].MenuOptions.Count; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.HMMenuItemObject);
				gameObject.transform.SetParent(this.currentMenuObjects[i].GetComponent<HMMenuObject>().MenuLineHolder.transform);
				gameObject.GetComponent<HMMenuItemObject>().buildMe(this, menuVectors.targetWidthFull, j, this.hackerMenus[i].MenuOptions[j], this.hackerMenus[i].MenuActions[j], 0f, num, num2);
				this.currentMenuItemObjects.Add(gameObject);
				num -= 44f;
				num3 += 0.2f;
			}
			num2 += num3;
			num = 0f;
			num3 = 0f;
		}
	}

	private void aniHideMenus()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.menuHideSFX, 0.9f, false);
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.clearMenus));
		this.menuHideAniSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.presentGameMode));
		this.menuHideAniSeq.Insert(0f, DOTween.To(() => this.WTTGHeaderObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.WTTGHeaderObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.65f).SetEase(Ease.InSine));
		for (int i = 0; i < this.currentMenuObjects.Count; i++)
		{
			this.currentMenuObjects[i].GetComponent<HMMenuObject>().hideMe();
		}
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.locTopScoresObject.transform.localPosition, delegate(Vector3 x)
		{
			this.locTopScoresObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.varObject.transform.localPosition, delegate(Vector3 x)
		{
			this.varObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.heapObject.transform.localPosition, delegate(Vector3 x)
		{
			this.heapObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.binObject.transform.localPosition, delegate(Vector3 x)
		{
			this.binObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.clearDataText.transform.localPosition, delegate(Vector3 x)
		{
			this.clearDataText.transform.localPosition = x;
		}, new Vector3(0f, -40f, 0f), 0.5f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuHideAniSeq.Play<Sequence>();
	}

	private void aniHideMenusNoGameMode()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.HACKINGSFX, this.menuHideSFX2, 0.9f, false);
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.clearMenus));
		this.menuHideAniSeq = DOTween.Sequence();
		this.menuHideAniSeq.Insert(0f, DOTween.To(() => this.WTTGHeaderObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.WTTGHeaderObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.65f).SetEase(Ease.InSine));
		for (int i = 0; i < this.currentMenuObjects.Count; i++)
		{
			this.currentMenuObjects[i].GetComponent<HMMenuObject>().hideMe();
		}
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.locTopScoresObject.transform.localPosition, delegate(Vector3 x)
		{
			this.locTopScoresObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.varObject.transform.localPosition, delegate(Vector3 x)
		{
			this.varObject.transform.localPosition = x;
		}, new Vector3(-184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.heapObject.transform.localPosition, delegate(Vector3 x)
		{
			this.heapObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.binObject.transform.localPosition, delegate(Vector3 x)
		{
			this.binObject.transform.localPosition = x;
		}, new Vector3(184f, 0f, 0f), 0.5f).SetEase(Ease.InSine).SetRelative(true));
		this.menuHideAniSeq.Insert(0.5f, DOTween.To(() => this.clearDataText.transform.localPosition, delegate(Vector3 x)
		{
			this.clearDataText.transform.localPosition = x;
		}, new Vector3(0f, -40f, 0f), 0.5f).SetEase(Ease.OutSine).SetRelative(true));
		this.menuHideAniSeq.Play<Sequence>();
	}

	private void clearMenus()
	{
		this.headerAniSeq.Kill(false);
		this.WTTGGlowImage.GetComponent<CanvasGroup>().alpha = 1f;
		this.WTTGHeaderObject.SetActive(false);
		for (int i = 0; i < this.currentMenuItemObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(this.currentMenuItemObjects[i].gameObject);
		}
		for (int j = 0; j < this.currentMenuObjects.Count; j++)
		{
			UnityEngine.Object.Destroy(this.currentMenuObjects[j].gameObject);
		}
		this.currentMenuItemObjects.Clear();
		this.currentMenuObjects.Clear();
	}

	private void clearMenusSoft()
	{
		for (int i = 0; i < this.currentMenuItemObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(this.currentMenuItemObjects[i].gameObject);
		}
		for (int j = 0; j < this.currentMenuObjects.Count; j++)
		{
			UnityEngine.Object.Destroy(this.currentMenuObjects[j].gameObject);
		}
		this.currentMenuItemObjects.Clear();
		this.currentMenuObjects.Clear();
	}

	private void presentGameMode()
	{
		GameManager.TimeSlinger.FireTimer(1f, new Action(this.resetGameMode));
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.MENU, 0f, 0.5f);
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.HACKERMODE, 1f);
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.MUSIC, this.HackerModeMusic, 0.25f, true);
		this.blackBG.GetComponent<CanvasGroup>().alpha = 1f;
		this.SkullCG.alpha = 1f;
		this.GameModeTextCG.alpha = 1f;
		this.FlashObject.SetActive(true);
		this.FlashObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.skullLaughSeq = DOTween.Sequence();
		this.skullLaughSeq.Insert(0f, DOTween.To(() => this.skullBottom.transform.localPosition, delegate(Vector3 x)
		{
			this.skullBottom.transform.localPosition = x;
		}, new Vector3(0f, -30f, 0f), 0.15f).SetEase(Ease.Linear).SetRelative(true));
		this.skullLaughSeq.Insert(0.15f, DOTween.To(() => this.skullBottom.transform.localPosition, delegate(Vector3 x)
		{
			this.skullBottom.transform.localPosition = x;
		}, new Vector3(0f, 30f, 0f), 0.15f).SetEase(Ease.Linear).SetRelative(true));
		this.skullLaughSeq.SetLoops(-1);
		this.skullLaughSeq.Play<Sequence>();
		this.presentGameModeSeq = DOTween.Sequence();
		this.presentGameModeSeq.Insert(0f, DOTween.To(() => this.FlashObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.FlashObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.4f).SetEase(Ease.Linear));
		this.presentGameModeSeq.Insert(0.2f, DOTween.To(() => this.SkullCG.alpha, delegate(float x)
		{
			this.SkullCG.alpha = x;
		}, 0f, 0.45f).SetEase(Ease.Linear));
		this.presentGameModeSeq.Insert(0.2f, DOTween.To(() => this.GameModeTextCG.alpha, delegate(float x)
		{
			this.GameModeTextCG.alpha = x;
		}, 0f, 0.6f).SetEase(Ease.Linear));
		this.presentGameModeSeq.Insert(0.2f, DOTween.To(() => this.GameModeText.transform.localScale, delegate(Vector3 x)
		{
			this.GameModeText.transform.localScale = x;
		}, new Vector3(0.1f, 0.1f, 0.1f), 0.6f).SetEase(Ease.Linear));
		this.presentGameModeSeq.Insert(0.8f, DOTween.To(() => this.scoreHeaderObject.transform.localPosition, delegate(Vector3 x)
		{
			this.scoreHeaderObject.transform.localPosition = x;
		}, Vector3.zero, 0.8f).SetEase(Ease.OutSine));
		this.presentGameModeSeq.Insert(0.8f, DOTween.To(() => this.upgradesFooterObject.transform.localPosition, delegate(Vector3 x)
		{
			this.upgradesFooterObject.transform.localPosition = x;
		}, new Vector3(0f, 44f, 0f), 0.8f).SetEase(Ease.OutSine).SetRelative(true));
		this.presentGameModeSeq.Play<Sequence>();
	}

	private void resetGameMode()
	{
		this.skullLaughSeq.Kill(true);
		float num = Mathf.Abs(this.skullBottom.transform.localPosition.y) - (float)Screen.height / 2f;
		DOTween.To(() => this.skullBottom.transform.localPosition, delegate(Vector3 x)
		{
			this.skullBottom.transform.localPosition = x;
		}, new Vector3(0f, num - 65f, 0f), 0.1f).SetEase(Ease.Linear).SetRelative(true);
		this.mainCamera.GetComponent<Glitch>().enabled = false;
		this.FlashObject.SetActive(false);
		this.FlashObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.GameModeTextCG.alpha = 0f;
		this.GameModeText.transform.localScale = new Vector3(1f, 1f, 1f);
		this.GameModeText.text = string.Empty;
		this.gameModeBeginTimeStamp = Time.time;
		this.gameModeTimeStamp = Time.time;
		this.inGameMode = true;
	}

	private void prepDOSAttack()
	{
		this.currentGameType = hackerModeGameType.DOS;
		this.myDOSAttack.hackerModeGameActive = true;
		this.GameModeText.text = "DOS_Blocker";
		this.DOSAttackHolder.gameObject.SetActive(true);
		this.myDOSAttack.prepDOSAttack();
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.myDOSAttack.warmDOSAttack));
		GameManager.TimeSlinger.FireTimer(1.9f, new Action(this.presentDOSAttack));
	}

	private void prepKAttack()
	{
		this.currentGameType = hackerModeGameType.KERNAL;
		this.myKAttack.hackerModeGameActive = true;
		this.GameModeText.text = "K3RN3LC0MP1L3R";
		this.KAttackHolder.gameObject.SetActive(true);
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.myKAttack.startKAttack));
		GameManager.TimeSlinger.FireTimer(1.5f, new Action(this.presentKAttack));
	}

	private void prepCLOUDAttack()
	{
		this.currentGameType = hackerModeGameType.CLOUD;
		this.myVapeAttack.hackerModeGameActive = true;
		this.GameModeText.text = "CLOUDGRID";
		this.VapeAttackHolder.gameObject.SetActive(true);
		this.myVapeAttack.prepVapeAttack();
		GameManager.TimeSlinger.FireTimer(2f, new Action(this.myVapeAttack.warmVapeAttack));
		GameManager.TimeSlinger.FireTimer(1.9f, new Action(this.presentDOSAttack));
	}

	private void presentDOSAttack()
	{
		DOTween.To(() => this.DOSNodeHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.DOSNodeHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.4f).SetEase(Ease.OutSine);
	}

	private void presentKAttack()
	{
		DOTween.To(() => this.KAttackHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.KAttackHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.4f).SetEase(Ease.OutSine);
	}

	private void presentCLOUDAttack()
	{
	}

	private void updateGameTime()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)(Time.time - this.gameModeBeginTimeStamp));
		this.timeValue.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	private void addGamePoints(int addAMT = 0)
	{
		this.fireATextRoller(this.pointsValue.gameObject, this.currentGamePoints, this.currentGamePoints + addAMT, this.rollerDelayPerUnit, this.rollerMaxDuration);
		this.currentGamePoints += addAMT;
		this.tmpCurrentGamePoints += addAMT;
		this.myGameData.currentTotalPoints = this.myGameData.currentTotalPoints + addAMT;
		GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
	}

	private void resetPresentGamePoints()
	{
		this.completeBonusObject.transform.localPosition = new Vector3(-245f, this.completeBonusObject.transform.localPosition.y, this.completeBonusObject.transform.localPosition.z);
		this.timeBonusObject.transform.localPosition = new Vector3(-245f, this.timeBonusObject.transform.localPosition.y, this.timeBonusObject.transform.localPosition.z);
		this.skillBonusObject.transform.localPosition = new Vector3(-245f, this.skillBonusObject.transform.localPosition.y, this.skillBonusObject.transform.localPosition.z);
		this.totalPointsObject.transform.localPosition = new Vector3(-245f, this.totalPointsObject.transform.localPosition.y, this.totalPointsObject.transform.localPosition.z);
		this.multiplierHolderObject.transform.localPosition = new Vector3(-245f, this.multiplierHolderObject.transform.localPosition.y, this.multiplierHolderObject.transform.localPosition.z);
		this.multiplierValue.transform.localScale = new Vector3(1f, 1f, 1f);
		this.multiplierValueShadow.transform.localScale = new Vector3(1f, 1f, 1f);
		this.completeBonusObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.timeBonusObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.skillBonusObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.totalPointsObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.completePointsValue.text = string.Empty;
		this.timePointsValue.text = string.Empty;
		this.skillPointsValue.text = string.Empty;
		this.totalPointsValue.text = string.Empty;
	}

	private void showFinalPoints()
	{
		bool flag = false;
		this.FlashObject.SetActive(false);
		this.chainLevelFinalValue.text = this.chainLevelValue.text;
		this.chainCountFinalValue.text = this.chainCountValue.text;
		this.finalScoreValue.text = "0";
		this.finalTimeValue.text = this.timeValue.text;
		int callValue = this.currentGamePoints;
		hackerModeGameType hackerModeGameType = this.currentGameType;
		if (hackerModeGameType != hackerModeGameType.DOS)
		{
			if (hackerModeGameType != hackerModeGameType.KERNAL)
			{
				if (hackerModeGameType == hackerModeGameType.CLOUD)
				{
					if (this.currentGamePoints > this.myGameData.highScoreCloud)
					{
						this.myGameData.highScoreCloud = this.currentGamePoints;
						flag = true;
					}
				}
			}
			else if (this.currentGamePoints > this.myGameData.highScoreKernal)
			{
				this.myGameData.highScoreKernal = this.currentGamePoints;
				flag = true;
			}
		}
		else if (this.currentGamePoints > this.myGameData.highScoreDOS)
		{
			this.myGameData.highScoreDOS = this.currentGamePoints;
			flag = true;
		}
		this.myDOSAttack.hackerModeGameActive = false;
		this.myKAttack.hackerModeGameActive = false;
		this.myVapeAttack.hackerModeGameActive = false;
		this.myHMUM.DOSTurbo.SetActive(false);
		this.myHMUM.KSkip.SetActive(false);
		this.myHMUM.cloudFREEZE.SetActive(false);
		if (flag)
		{
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
			GameManager.TimeSlinger.FireTimer(8f, new Action(this.animateNewHighScore));
		}
		GameManager.TimeSlinger.FireIntTimer(3f, new Action<int>(this.rollFinalPoints), callValue);
		this.gameOverMainMenuBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.mainMenuGameOverHit));
		this.chainLevelFinalObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		this.chainCountFinalObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		this.finalScoreObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		this.currentGamePoints = 0;
		this.tmpCurrentGamePoints = 0;
		this.currentChainCount = 0;
		this.currentChainIndex = 0;
		this.chainLevelValue.text = string.Empty;
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.totalPointSlideSFX, 0.9f, false, 1.5f);
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.totalPointSlideSFX, 0.9f, false, 2.25f);
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.finalPointsImpactSFX, 0.9f, false, 3f);
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.timeShowSFX, 0.6f, false, 8.3f);
		this.finalPointsSeq = DOTween.Sequence();
		this.finalPointsSeq.Insert(1.5f, DOTween.To(() => this.chainLevelFinalObject.transform.localScale, delegate(Vector3 x)
		{
			this.chainLevelFinalObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f).SetEase(Ease.OutSine));
		this.finalPointsSeq.Insert(1.5f, DOTween.To(() => this.chainLevelFinalObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.chainLevelFinalObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.OutSine));
		this.finalPointsSeq.Insert(2.25f, DOTween.To(() => this.chainCountFinalObject.transform.localScale, delegate(Vector3 x)
		{
			this.chainCountFinalObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f).SetEase(Ease.OutSine));
		this.finalPointsSeq.Insert(2.25f, DOTween.To(() => this.chainCountFinalObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.chainCountFinalObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.OutSine));
		this.finalPointsSeq.Insert(3f, DOTween.To(() => this.finalScoreObject.transform.localScale, delegate(Vector3 x)
		{
			this.finalScoreObject.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.75f).SetEase(Ease.OutSine));
		this.finalPointsSeq.Insert(3f, DOTween.To(() => this.finalScoreObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.finalScoreObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.OutSine));
		this.finalPointsSeq.Insert(8.3f, DOTween.To(() => this.finalTimeObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.finalTimeObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.Linear));
		this.finalPointsSeq.Insert(9.3f, DOTween.To(() => this.gameOverMainMenuBTN.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameOverMainMenuBTN.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.75f).SetEase(Ease.OutSine));
		this.finalPointsSeq.Play<Sequence>();
	}

	private void prepMainMenu()
	{
		GameManager.AudioSlinger.UnMuffleAudioHub(AudioHubs.MENU, 5f);
		this.prepAssets();
		this.prepMenus();
	}

	private void mainMenuGameOverHit()
	{
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.aniMainMenu));
		GameManager.TimeSlinger.FireTimer(0.7f, new Action(this.clearGameOver));
		DOTween.To(() => this.GameOverObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.GameOverObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.4f).SetEase(Ease.Linear);
		DOTween.To(() => this.newHighScoreText.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.newHighScoreText.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.5f).SetEase(Ease.Linear);
		DOTween.To(() => this.gameOverMainMenuBTN.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameOverMainMenuBTN.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.4f).SetEase(Ease.Linear);
	}

	private void clearGameOver()
	{
		this.chainLevelFinalObject.GetComponent<CanvasGroup>().alpha = 0f;
		this.chainCountFinalObject.GetComponent<CanvasGroup>().alpha = 0f;
		this.finalScoreObject.GetComponent<CanvasGroup>().alpha = 0f;
		this.finalTimeObject.GetComponent<CanvasGroup>().alpha = 0f;
		this.chainLevelFinalValue.text = "0";
		this.chainLevelValue.text = "0";
		this.chainCountFinalValue.text = "0";
		this.chainCountValue.text = "0";
		this.finalScoreValue.text = "0";
		this.pointsValue.text = "0";
		this.finalTimeValue.text = "00:00:00";
		this.timeValue.text = "00:00:00";
		this.GameOverObject.SetActive(false);
	}

	private void setMultiplierText(int fromValue, int toValue)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.multiImpactSFX, 0.85f, false);
		this.multiplierHolderObject.transform.localPosition = new Vector3(0f, this.multiplierHolderObject.transform.localPosition.y, this.multiplierHolderObject.transform.localPosition.z);
		this.multiplierHolderObject.GetComponent<CanvasGroup>().alpha = 1f;
		this.fireATextRollerWithSFX(this.totalPointsValue.gameObject, fromValue, toValue, this.rollerDelayPerUnit, this.rollerMaxDuration / 2f, this.multiPointRollSFX, AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, 0.2f);
	}

	private void rollFinalPoints(int toValue)
	{
		this.fireATextRollerWithSFX(this.finalScoreValue.gameObject, 0, toValue, this.rollerDelayPerUnit, 5f, this.finalPointsRollSFX, AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, 0.15f);
	}

	private void rollCurrentStats()
	{
		this.fireATextRoller(this.topScoreDOS.gameObject, 0, this.myGameData.highScoreDOS, this.rollerDelayPerUnit, 3f);
		this.fireATextRoller(this.topScoreKernal.gameObject, 0, this.myGameData.highScoreKernal, this.rollerDelayPerUnit, 3f);
		this.fireATextRoller(this.topScoreCloud.gameObject, 0, this.myGameData.highScoreCloud, this.rollerDelayPerUnit, 3f);
		this.fireATextRoller(this.currentTotalPointsValue.gameObject, 0, this.myGameData.currentTotalPoints, this.rollerDelayPerUnit, 3f);
		this.fireATextRoller(this.currentStacksValue.gameObject, 0, this.myGameData.currentStacks, this.rollerDelayPerUnit, 3f);
		this.fireATextRoller(this.currentNOPSledsValue.gameObject, 0, this.myGameData.currentNOPSleds, this.rollerDelayPerUnit, 3f);
		this.fireATextRoller(this.currentShellsValue.gameObject, 0, this.myGameData.currentShells, this.rollerDelayPerUnit, 3f);
		this.upgradesNOPSledValue.text = this.myGameData.currentNOPSleds.ToString();
		this.upgradesShellValue.text = this.myGameData.currentShells.ToString();
		if (this.myGameData.DOSTurboOnline)
		{
			this.currentDOSTurboStatus.text = "ONLINE";
			this.currentDOSTurboStatus.color = this.onlineColor;
		}
		if (this.myGameData.KernSKIPOnline)
		{
			this.currentKernSkipStatus.text = "ONLINE";
			this.currentKernSkipStatus.color = this.onlineColor;
		}
		if (this.myGameData.CloudFREEZEOnline)
		{
			this.currentCloudFreezeStatus.text = "ONLINE";
			this.currentCloudFreezeStatus.color = this.onlineColor;
		}
	}

	private void triggerPause()
	{
		if (this.inGameMode)
		{
			if (this.isPaused)
			{
				Time.timeScale = 1f;
				this.PauseHolder.GetComponent<CanvasGroup>().alpha = 0f;
				this.PauseHolder.gameObject.SetActive(false);
				this.isPaused = false;
				GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.HACKINGSFX);
				GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.MUSIC);
			}
			else
			{
				GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.HACKINGSFX, 0f);
				GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
				this.PauseHolder.gameObject.SetActive(true);
				this.PauseHolder.GetComponent<CanvasGroup>().alpha = 1f;
				this.isPaused = true;
				Time.timeScale = 0f;
			}
		}
		else
		{
			this.isPaused = false;
		}
	}

	private void triggerPause(bool setPause)
	{
		this.isPaused = setPause;
		if (this.inGameMode)
		{
			if (this.isPaused)
			{
				Time.timeScale = 1f;
				this.PauseHolder.GetComponent<CanvasGroup>().alpha = 0f;
				this.PauseHolder.gameObject.SetActive(false);
				this.isPaused = false;
				GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.HACKINGSFX);
				GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.MUSIC);
			}
			else
			{
				GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.HACKINGSFX, 0f);
				GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
				this.PauseHolder.gameObject.SetActive(true);
				this.PauseHolder.GetComponent<CanvasGroup>().alpha = 1f;
				this.isPaused = true;
				Time.timeScale = 0f;
			}
		}
		else
		{
			this.isPaused = false;
		}
	}

	private void animateNewHighScore()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.newHighScoreSFX, 0.95f, false);
		this.newHighScoreText.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		DOTween.To(() => this.newHighScoreText.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.newHighScoreText.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.6f).SetEase(Ease.OutSine);
		DOTween.To(() => this.newHighScoreText.transform.localScale, delegate(Vector3 x)
		{
			this.newHighScoreText.transform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.6f).SetEase(Ease.OutSine);
	}

	private void aniStaticBG()
	{
		this.staticAniSeq = DOTween.Sequence();
		this.staticAniSeq.Insert(0f, DOTween.To(() => this.staticBG1.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG1.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.1f).SetEase(Ease.Linear));
		this.staticAniSeq.Insert(0.1f, DOTween.To(() => this.staticBG2.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG2.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.1f).SetEase(Ease.Linear));
		this.staticAniSeq.Insert(0.2f, DOTween.To(() => this.staticBG3.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG3.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.1f).SetEase(Ease.Linear));
		this.staticAniSeq.Insert(0.3f, DOTween.To(() => this.staticBG4.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG4.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.1f).SetEase(Ease.Linear));
		this.staticAniSeq.Insert(0.4f, DOTween.To(() => this.staticBG4.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG1.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0f).SetEase(Ease.Linear));
		this.staticAniSeq.Insert(0.4f, DOTween.To(() => this.staticBG3.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG2.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0f).SetEase(Ease.Linear));
		this.staticAniSeq.Insert(0.4f, DOTween.To(() => this.staticBG2.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG3.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0f).SetEase(Ease.Linear));
		this.staticAniSeq.Insert(0.4f, DOTween.To(() => this.staticBG1.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.staticBG4.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0f).SetEase(Ease.Linear));
		this.staticAniSeq.SetLoops(-1);
		this.staticAniSeq.Play<Sequence>();
	}

	private void aniHeader()
	{
		this.headerAniSeq = DOTween.Sequence();
		this.headerAniSeq.Insert(0f, DOTween.To(() => this.WTTGGlowImage.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.WTTGGlowImage.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 2f).SetEase(Ease.Linear));
		this.headerAniSeq.Insert(2f, DOTween.To(() => this.WTTGGlowImage.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.WTTGGlowImage.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 2f).SetEase(Ease.Linear));
		this.headerAniSeq.SetLoops(-1);
		this.headerAniSeq.Play<Sequence>();
	}

	private void clearUpgradeEFXS()
	{
		this.mainCamera.GetComponent<Glitch>().RandomActivation = true;
		this.mainCamera.GetComponent<Glitch>().enabled = false;
		this.mainCamera.GetComponent<AnalogTV>().enabled = false;
		this.mainCamera.GetComponent<PixelMatrix>().enabled = false;
		this.skullLaughSeq.Kill(true);
		float num = Mathf.Abs(this.skullBottom.transform.localPosition.y) - (float)Screen.height / 2f;
		DOTween.To(() => this.skullBottom.transform.localPosition, delegate(Vector3 x)
		{
			this.skullBottom.transform.localPosition = x;
		}, new Vector3(0f, num - 65f, 0f), 0.1f).SetEase(Ease.Linear).SetRelative(true);
	}

	private void loadMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	private void Awake()
	{
		if (!GameManager.FileSlinger.wildLoadFile<HMGameData>("wttghm.gd", out this.myGameData))
		{
			this.myGameData = new HMGameData();
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
		}
	}

	private void Start()
	{
		this.prepAssets();
		this.prepMenus();
		this.aniStaticBG();
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.aniMainMenu));
	}

	private void Update()
	{
		if (this.inGameMode && Time.time - this.gameModeTimeStamp >= 1f)
		{
			this.updateGameTime();
			this.gameModeTimeStamp = Time.time;
		}
		if (CrossPlatformInputManager.GetButtonDown("Cancel"))
		{
			this.triggerPause();
		}
		if (!this.inGameMode)
		{
			if (CrossPlatformInputManager.GetAxis("Delete") >= 1f)
			{
				if (this.delIsHot)
				{
					if (Time.time - this.delTimeStamp >= 5f)
					{
						GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.NOPSledFireSFX, 0.85f, false);
						this.myGameData.resetData();
						GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
						this.rollCurrentStats();
						this.delTimeStamp = Time.time;
					}
				}
				else
				{
					this.delIsHot = true;
					this.delTimeStamp = Time.time;
				}
			}
			else
			{
				this.delIsHot = false;
				this.delTimeStamp = 0f;
			}
		}
		this.curMousePOS = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (this.inGameMode && !this.isPaused)
		{
			this.triggerPause(false);
		}
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(this.curMousePOS.x - 4f, this.curMousePOS.y - 4f, 8f, 8f), this.customCursor);
	}

	private const int MENU_SPACE = 25;

	[Range(0f, 1f)]
	public float rollerDelayPerUnit = 0.05f;

	[Range(1f, 10f)]
	public float rollerMaxDuration = 3f;

	public Camera mainCamera;

	public HMUpgradesManager myHMUM;

	public DOSAttack myDOSAttack;

	public KAttack myKAttack;

	public vapeAttack myVapeAttack;

	public GameObject blackBG;

	public GameObject staticBG1;

	public GameObject staticBG2;

	public GameObject staticBG3;

	public GameObject staticBG4;

	public GameObject staticBG5;

	public Text clearDataText;

	public GameObject WTTGHeaderObject;

	public GameObject WTTGGlowImage;

	public GameObject DOSAttackHolder;

	public GameObject DOSNodeHolder;

	public GameObject KAttackHolder;

	public GameObject VapeAttackHolder;

	public GameObject VapeHolder;

	public Texture2D customCursor;

	public Image menuPrompt1;

	public Image menuPrompt2;

	public Image menuPrompt3;

	public GameObject MenuHolder;

	public GameObject HMMenuObject;

	public GameObject HMMenuItemObject;

	public GameObject locTopScoresObject;

	public Text topScoreDOS;

	public Text topScoreKernal;

	public Text topScoreCloud;

	public GameObject varObject;

	public GameObject heapObject;

	public Text currentTotalPointsValue;

	public Text currentStacksValue;

	public GameObject pointsToStacksBTN;

	public GameObject binObject;

	public Text currentNOPSledsValue;

	public Text currentShellsValue;

	public Text currentDOSTurboStatus;

	public Text currentKernSkipStatus;

	public Text currentCloudFreezeStatus;

	public GameObject binCompileBTN;

	public Image skullBottom;

	public CanvasGroup SkullCG;

	public CanvasGroup GameModeTextCG;

	public Text GameModeText;

	public GameObject scoreHeaderObject;

	public Text chainLevelValue;

	public Text chainCountValue;

	public Text pointsValue;

	public Text timeValue;

	public GameObject upgradesFooterObject;

	public Text upgradesNOPSledValue;

	public Text upgradesShellValue;

	public GameObject completeBonusObject;

	public GameObject timeBonusObject;

	public GameObject skillBonusObject;

	public GameObject totalPointsObject;

	public GameObject multiplierHolderObject;

	public Text completePointsValue;

	public Text timePointsValue;

	public Text skillPointsValue;

	public Text totalPointsValue;

	public Text multiplierValue;

	public Text multiplierValueShadow;

	public GameObject GameOverObject;

	public GameObject chainLevelFinalObject;

	public GameObject chainCountFinalObject;

	public GameObject finalScoreObject;

	public GameObject finalTimeObject;

	public GameObject gameOverMainMenuBTN;

	public Text chainLevelFinalValue;

	public Text chainCountFinalValue;

	public Text finalScoreValue;

	public Text finalTimeValue;

	public Text newHighScoreText;

	public GameObject FlashObject;

	public GameObject PauseHolder;

	public Color onlineColor;

	public GameObject MusicONOFFBTN;

	public GameObject SFXONOFFBTN;

	public AudioClip HackerModeMusic;

	public AudioClip menuPromptSFX;

	public AudioClip menuShowSFX;

	public AudioClip menuHideSFX;

	public AudioClip menuHideSFX2;

	public AudioClip bonusPointSlideSFX;

	public AudioClip totalPointSlideSFX;

	public AudioClip gameOverFlashSFX;

	public AudioClip gameOverSayingSFX;

	public AudioClip timeShowSFX;

	public AudioClip nextChainSFX;

	public AudioClip masterChainSFX;

	public AudioClip multiImpactSFX;

	public AudioClip multiPointRollSFX;

	public AudioClip finalPointsImpactSFX;

	public AudioClip finalPointsRollSFX;

	public AudioClip newHighScoreSFX;

	public AudioClip NOPSledFireSFX;

	public AudioClip FireShellSFX;

	public List<HackerModeMenuDefinition> hackerMenus;

	public bool gameIsHot;

	public bool DOSTurboIsHot;

	private int currentChainCount;

	private int currentGamePoints;

	private int tmpCurrentGamePoints;

	private int currentChainIndex;

	private bool inGameMode;

	private bool isPaused;

	private bool delIsHot;

	private float gameModeTimeStamp;

	private float gameModeBeginTimeStamp;

	private float delTimeStamp;

	private Sequence menuAniSeq;

	private Sequence menuHideAniSeq;

	private Sequence presentGameModeSeq;

	private Sequence presentBonusPointsSeq;

	private Sequence gameOverSeq;

	private Sequence finalPointsSeq;

	private Sequence skullLaughSeq;

	private Sequence staticAniSeq;

	private Sequence headerAniSeq;

	private List<GameObject> currentMenuObjects;

	private List<GameObject> currentMenuItemObjects;

	private HMGameData myGameData;

	private Vector2 curMousePOS;

	private hackerModeGameType currentGameType;

	private struct menuVectors
	{
		public menuVectors(short menuLineCount = 0)
		{
			this.targetWidthFull = Math.Max(HackerModeManager.menuVectors.targetWidth, (float)Screen.width * 0.65f);
			this.targetHeightFull = (float)menuLineCount * HackerModeManager.menuVectors.targetMenuLineHeight + HackerModeManager.menuVectors.targetBorderWidth * 2f;
			this.targetWidthSub = HackerModeManager.menuVectors.targetWidth - HackerModeManager.menuVectors.targetBorderWidth * 2f;
			this.targetHeightSub = (float)menuLineCount * HackerModeManager.menuVectors.targetMenuLineHeight;
		}

		public float targetWidthFull;

		public float targetHeightFull;

		public float targetWidthSub;

		public float targetHeightSub;

		private static float targetWidth = 500f;

		private static float targetBorderWidth = 2f;

		private static float targetMenuLineHeight = 44f;
	}
}
