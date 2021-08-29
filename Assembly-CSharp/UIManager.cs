using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public void displayActionText(string actionText = "Action Text")
	{
		this.ActionText.text = actionText;
		this.ActionGroup.alpha = 1f;
	}

	public void hideActionText()
	{
		this.ActionGroup.alpha = 0f;
		this.ActionText.text = string.Empty;
	}

	public void hideCrossHair()
	{
		this.CrossHair.enabled = false;
	}

	public void showCrossHair()
	{
		this.CrossHair.enabled = true;
	}

	public void showCursorUI()
	{
		this.CursorUI.enabled = true;
	}

	public void hideCursorUI()
	{
		this.CursorUI.enabled = false;
	}

	public void setHoverCursor()
	{
		this.CursorUI.sprite = this.hoverCursor;
	}

	public void setDefaultCursor()
	{
		this.CursorUI.sprite = this.defaultCursor;
	}

	public void flashSaveIcon()
	{
		this.saveIconSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.resetSaveIcon));
		TweenSettingsExtensions.Insert(this.saveIconSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.saveIconHolder.alpha, delegate(float x)
		{
			this.saveIconHolder.alpha = x;
		}, 1f, 0.75f), 3));
		TweenSettingsExtensions.Insert(this.saveIconSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.saveIconHolder.alpha, delegate(float x)
		{
			this.saveIconHolder.alpha = x;
		}, 0.2f, 0.75f), 3));
		TweenSettingsExtensions.SetLoops<Sequence>(this.saveIconSeq, 3);
		TweenExtensions.Play<Sequence>(this.saveIconSeq);
	}

	public void pauseMe()
	{
		this.pauseTextCG.gameObject.SetActive(true);
		this.ScreenFadeHolder.alpha = 1f;
		this.pauseTextCG.alpha = 1f;
	}

	public void unPauseMe()
	{
		this.ScreenFadeHolder.alpha = 0f;
		this.pauseTextCG.alpha = 0f;
		this.pauseTextCG.gameObject.SetActive(false);
	}

	public void fadeScreenInWithTime(float setTime)
	{
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 0f, setTime), 1);
	}

	public void fadeScreenOutWithTime(float setTime)
	{
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 1f, setTime), 1);
	}

	public void showShutDown()
	{
		GameManager.GetTheCloud().myTimeManager.freezeTime = true;
		this.shutDownObject.SetActive(true);
		this.shutDownSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.shutDownSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.shutDownObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.shutDownObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f), 1));
		TweenSettingsExtensions.Insert(this.shutDownSeq, 0.25f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.shutDownWindowObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.shutDownWindowObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f), 3));
		TweenExtensions.Play<Sequence>(this.shutDownSeq);
	}

	public void hideShutDown()
	{
		GameManager.GetTheCloud().myTimeManager.freezeTime = false;
		this.shutDownSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.disableShutDownObject));
		TweenSettingsExtensions.Insert(this.shutDownSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.shutDownWindowObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.shutDownWindowObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f), 3));
		TweenSettingsExtensions.Insert(this.shutDownSeq, 0.25f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.shutDownObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.shutDownObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f), 1));
		TweenExtensions.Play<Sequence>(this.shutDownSeq);
	}

	public void saveAndQuit()
	{
		this.fadeScreenOutWithTime(0.3f);
		GameManager.GetTheCloud().myTimeManager.shutItDown();
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.quitToMainMenu));
	}

	public void triggerGameOver(string reason)
	{
		GameManager.GetTheCloud().myTimeManager.freezeTime = true;
		this.myPauseManager.lockPause = true;
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.OUTSIDE, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTER, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.MAINROOM, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.LEFTROOM, 0f);
		GameManager.AudioSlinger.MuffleAudioHub(AudioHubs.COMPUTERHARDWARE, 0f);
		GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.MUSIC, 0f);
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.gameOverTriggerClip, 1f, false);
		GameManager.FileSlinger.deleteFile("wttg2.gd");
		this.gameOverReason.text = reason;
		this.fadeScreenOutWithTime(0.5f);
		GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.showGameOver));
	}

	public void triggerResetModem(float resetModemTime = 10f)
	{
		this.resetModemHolder.alpha = 1f;
		this.resetModemBar.fillAmount = 0f;
		DOTween.To(() => this.resetModemBar.fillAmount, delegate(float x)
		{
			this.resetModemBar.fillAmount = x;
		}, 1f, resetModemTime);
		GameManager.TimeSlinger.FireTimer(resetModemTime, new Action(this.clearResetModem));
	}

	public void removeRestModem()
	{
		this.resetModemBar.fillAmount = 0f;
		this.resetModemHolder.gameObject.SetActive(false);
	}

	public void displayNoInet()
	{
		this.inetSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.inetSeq, 0f, DOTween.To(() => this.inetConStatHolder.alpha, delegate(float x)
		{
			this.inetConStatHolder.alpha = x;
		}, 1f, 0.5f));
		TweenSettingsExtensions.Insert(this.inetSeq, 4.5f, DOTween.To(() => this.inetConStatHolder.alpha, delegate(float x)
		{
			this.inetConStatHolder.alpha = x;
		}, 0f, 0.5f));
		TweenExtensions.Play<Sequence>(this.inetSeq);
	}

	public void displayConfOffline()
	{
		this.confSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.confSeq, 0f, DOTween.To(() => this.confOfflineHolder.alpha, delegate(float x)
		{
			this.confOfflineHolder.alpha = x;
		}, 1f, 0.5f));
		TweenSettingsExtensions.Insert(this.confSeq, 4.5f, DOTween.To(() => this.confOfflineHolder.alpha, delegate(float x)
		{
			this.confOfflineHolder.alpha = x;
		}, 0f, 0.5f));
		TweenExtensions.Play<Sequence>(this.confSeq);
	}

	public void displayMSGPopUp(string theMSGText)
	{
		bool flag = true;
		this.msgText.text = theMSGText;
		if (this.msgSeq != null && TweenExtensions.IsActive(this.msgSeq))
		{
			flag = false;
		}
		if (flag)
		{
			this.msgSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.clearMsgPopUpText));
			TweenSettingsExtensions.Insert(this.msgSeq, 0f, DOTween.To(() => this.msgPopUpHolder.alpha, delegate(float x)
			{
				this.msgPopUpHolder.alpha = x;
			}, 1f, 0.5f));
			TweenSettingsExtensions.Insert(this.msgSeq, 4.5f, DOTween.To(() => this.msgPopUpHolder.alpha, delegate(float x)
			{
				this.msgPopUpHolder.alpha = x;
			}, 0f, 0.5f));
			TweenExtensions.Play<Sequence>(this.msgSeq);
		}
	}

	public void showBrace()
	{
		this.BraceHolder.SetActive(true);
		this.braceSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.braceSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.LeftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.LeftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.2f), 1));
		TweenSettingsExtensions.Insert(this.braceSeq, 0.2f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.LeftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.LeftClickIMG.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.2f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.braceSeq, -1);
		TweenExtensions.Play<Sequence>(this.braceSeq);
	}

	public void hideBrace()
	{
		this.BraceHolder.SetActive(false);
		TweenExtensions.Kill(this.braceSeq, true);
	}

	public void triggerFlashCreep()
	{
		if (GameManager.GetTheMainController().isUsingComputer)
		{
			this.jumpCreepIMG.fillAmount = 1f;
			GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.MUSIC, this.jumpCreepSFX, 1f, false);
			GameManager.TimeSlinger.FireTimer(0.35f, new Action(this.hideJumpCreepIMG));
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(5f, new Action(this.triggerFlashCreep));
		}
	}

	private void clearMsgPopUpText()
	{
		this.msgText.text = string.Empty;
	}

	private void fadeScreenIn()
	{
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 0f, this.screenFadeTime), 1);
	}

	private void fadeScreenOut()
	{
		TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.ScreenFadeHolder.alpha, delegate(float x)
		{
			this.ScreenFadeHolder.alpha = x;
		}, 1f, this.screenFadeTime), 1);
	}

	private void resetSaveIcon()
	{
		this.saveIconHolder.alpha = 0f;
	}

	private void disableShutDownObject()
	{
		this.shutDownObject.SetActive(false);
	}

	private void quitToMainMenu()
	{
		GameManager.TimeSlinger.clearTimers();
		GameManager.AudioSlinger.ClearAudioHubs();
		SceneManager.LoadScene(0);
	}

	private void showGameOver()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.SFX, this.gameOverClip, 1f, false, 0.3f);
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.quitToMainMenu));
		this.gameOverSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.gameOverSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.gameOverIMG.color, delegate(Color x)
		{
			this.gameOverIMG.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.7f), 3));
		TweenSettingsExtensions.Insert(this.gameOverSeq, 0.7f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.gameOverReason.color, delegate(Color x)
		{
			this.gameOverReason.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.7f), 1));
		TweenExtensions.Play<Sequence>(this.gameOverSeq);
	}

	private void clearResetModem()
	{
		this.resetModemHolder.alpha = 0f;
		this.resetModemBar.fillAmount = 0f;
	}

	private void hideJumpCreepIMG()
	{
		this.jumpCreepIMG.fillAmount = 0f;
	}

	private void showBabyMode()
	{
		this.babyModeIMG.SetActive(true);
	}

	private void Awake()
	{
		GameManager.SetUIManager(this);
	}

	private void Start()
	{
		this.hideCursorUI();
		GameManager.TimeSlinger.FireTimer(this.screenWaitTime, new Action(this.fadeScreenIn));
		if (GameManager.GetGameModeManager().getCasualMode())
		{
			GameManager.TimeSlinger.FireTimer(this.screenWaitTime, new Action(this.showBabyMode));
		}
	}

	[Range(0.5f, 5f)]
	public float screenFadeTime = 1.5f;

	[Range(0.2f, 2f)]
	public float screenWaitTime = 0.5f;

	public muteBehavior myMuteBehavior;

	public PauseManager myPauseManager;

	public Canvas UICanvas;

	public CanvasGroup ActionGroup;

	public Text ActionText;

	public Image CrossHair;

	public Image CursorUI;

	public CanvasGroup ScreenFadeHolder;

	public Sprite defaultCursor;

	public Sprite hoverCursor;

	public CanvasGroup saveIconHolder;

	public CanvasGroup pauseTextCG;

	public CanvasGroup resetModemHolder;

	public Image resetModemBar;

	public GameObject shutDownObject;

	public GameObject shutDownWindowObject;

	public AudioClip gameOverTriggerClip;

	public AudioClip gameOverClip;

	public Image gameOverIMG;

	public Text gameOverReason;

	public CanvasGroup inetConStatHolder;

	public CanvasGroup confOfflineHolder;

	public CanvasGroup msgPopUpHolder;

	public Text msgText;

	public GameObject BraceHolder;

	public GameObject LeftClickIMG;

	public Image jumpCreepIMG;

	public AudioClip jumpCreepSFX;

	public GameObject babyModeIMG;

	private Sequence actionSeq;

	private Sequence saveIconSeq;

	private Sequence shutDownSeq;

	private Sequence gameOverSeq;

	private Sequence inetSeq;

	private Sequence confSeq;

	private Sequence msgSeq;

	private Sequence braceSeq;
}
