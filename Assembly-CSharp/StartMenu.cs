using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
	private void aniTop()
	{
		this.topSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.topSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.topImage.color, delegate(Color x)
		{
			this.topImage.color = x;
		}, new Color(1f, 1f, 1f, 0.3f), 2f), 3));
		TweenSettingsExtensions.Insert(this.topSeq, 2f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.topImage.color, delegate(Color x)
		{
			this.topImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 2f), 2));
		TweenSettingsExtensions.SetLoops<Sequence>(this.topSeq, -1);
		TweenExtensions.Play<Sequence>(this.topSeq);
	}

	private void aniTitle()
	{
		this.titleSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.titleSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.titleFadeImage.color, delegate(Color x)
		{
			this.titleFadeImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 1.5f), 1));
		TweenSettingsExtensions.Insert(this.titleSeq, 1.5f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.titleFadeImage.color, delegate(Color x)
		{
			this.titleFadeImage.color = x;
		}, new Color(1f, 1f, 1f, 0f), 1.5f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.titleSeq, -1);
		TweenExtensions.Play<Sequence>(this.titleSeq);
	}

	private void aniRec()
	{
		this.recSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.recSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 0.2f), 1f), 1));
		TweenSettingsExtensions.Insert(this.recSeq, 1f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 1f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.recSeq, -1);
		TweenExtensions.Play<Sequence>(this.recSeq);
	}

	private void addViewerCount()
	{
		if (this.viewerCount > 9000)
		{
			this.viewerCountText.text = "Over 9,000";
		}
		else
		{
			this.viewerCountText.text = this.viewerCount.ToString("N0");
			this.viewerCountWindow = Random.Range(1f, 5f);
			this.viewerCountTimeStamp = Time.time;
			this.viewerCountActive = true;
			this.viewerCount += Random.Range(4, 25);
		}
	}

	private void prepLinks()
	{
		this.newGameLink.setAction(new Action(this.newGameHit));
		this.continueLink.setAction(new Action(this.continueHit));
		this.optionsLink.setAction(new Action(this.optionsHit));
		this.quitLink.setAction(new Action(this.quitHit));
		this.yesLink.setAction(new Action(this.yesHit));
		this.noLink.setAction(new Action(this.noHit));
		this.gameModeNormalLink.setAction(new Action(this.gameModeNormalHit));
		this.gameModeCasualLink.setAction(new Action(this.gameModeCasualHit));
		this.gameModeBackLink.setAction(new Action(this.gameModeBackHit));
		this.hackerModeLink.setAction(new Action(this.hackerModeHit));
		if (GameManager.SteamSlinger.triggerCheckForHackerMode())
		{
			this.hackerModeLink.setEnable();
		}
	}

	private void checkSavedData()
	{
		if (this.hasSavedData)
		{
			this.continueLink.setEnable();
		}
	}

	private void showAreYouSure()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.enableAreYouSure));
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.disableGameMode));
		this.menuSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.menuSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.gameMode.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameMode.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f), 1));
		TweenSettingsExtensions.Insert(this.menuSeq, 0.3f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.areYouSure.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.areYouSure.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.menuSeq);
	}

	private void showGameMode()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.enableGameMode));
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.disableMenuHolder));
		this.menuSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.menuSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.menuHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.menuHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f), 1));
		TweenSettingsExtensions.Insert(this.menuSeq, 0.3f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.gameMode.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameMode.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.menuSeq);
	}

	private void goBackToMenuHolder()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.disableAreYouSure));
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.enableMenuHolder));
		this.menuSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.menuSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.areYouSure.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.areYouSure.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f), 1));
		TweenSettingsExtensions.Insert(this.menuSeq, 0.3f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.menuHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.menuHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.menuSeq);
	}

	private void goBackToMenuFromGameMode()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.disableGameMode));
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.enableMenuHolder));
		this.menuSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.menuSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.gameMode.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameMode.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f), 1));
		TweenSettingsExtensions.Insert(this.menuSeq, 0.3f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.menuHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.menuHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f), 1));
		TweenExtensions.Play<Sequence>(this.menuSeq);
	}

	private void enableAreYouSure()
	{
		this.areYouSure.SetActive(true);
	}

	private void disableAreYouSure()
	{
		this.areYouSure.SetActive(false);
	}

	private void enableMenuHolder()
	{
		this.menuHolder.SetActive(true);
	}

	private void disableMenuHolder()
	{
		this.menuHolder.SetActive(false);
	}

	private void enableGameMode()
	{
		this.gameMode.SetActive(true);
	}

	private void disableGameMode()
	{
		this.gameMode.SetActive(false);
	}

	private void newGameHit()
	{
		this.showGameMode();
	}

	private void continueHit()
	{
		if (this.hasSavedData)
		{
			this.myMenuManager.beginGame();
		}
	}

	private void optionsHit()
	{
		this.myMenuManager.showOpts();
	}

	private void quitHit()
	{
		Application.Quit();
	}

	private void yesHit()
	{
		GameManager.FileSlinger.deleteTheFile = true;
		this.myMenuManager.beginGame();
	}

	private void noHit()
	{
		this.goBackToMenuHolder();
	}

	private void hackerModeHit()
	{
		this.myMenuManager.loadHackerMode();
	}

	private void gameModeNormalHit()
	{
		GameManager.GetGameModeManager().setCausalMode(false);
		if (this.hasSavedData)
		{
			this.showAreYouSure();
		}
		else
		{
			this.myMenuManager.beginGame();
		}
	}

	private void gameModeCasualHit()
	{
		GameManager.GetGameModeManager().setCausalMode(true);
		if (this.hasSavedData)
		{
			this.showAreYouSure();
		}
		else
		{
			this.myMenuManager.beginGame();
		}
	}

	private void gameModeBackHit()
	{
		this.goBackToMenuFromGameMode();
	}

	private void Awake()
	{
		this.hasSavedData = GameManager.FileSlinger.loadFile("wttg2.gd");
	}

	private void Start()
	{
		this.aniTop();
		this.aniTitle();
		this.aniRec();
		this.addViewerCount();
		this.prepLinks();
		this.checkSavedData();
	}

	private void Update()
	{
		if (this.viewerCountActive && Time.time - this.viewerCountTimeStamp >= this.viewerCountWindow)
		{
			this.viewerCountActive = false;
			this.addViewerCount();
		}
	}

	public MenuManager myMenuManager;

	public Image topImage;

	public Image titleFadeImage;

	public Image recLiveImage;

	public Text viewerCountText;

	public GameObject menuHolder;

	public GameObject areYouSure;

	public GameObject gameMode;

	public MenuLink newGameLink;

	public MenuLink continueLink;

	public MenuLink optionsLink;

	public MenuLink quitLink;

	public MenuLink yesLink;

	public MenuLink noLink;

	public MenuLink hackerModeLink;

	public MenuLink gameModeNormalLink;

	public MenuLink gameModeCasualLink;

	public MenuLink gameModeBackLink;

	private Sequence topSeq;

	private Sequence titleSeq;

	private Sequence recSeq;

	private Sequence menuSeq;

	private int viewerCount = 4587;

	private float viewerCountTimeStamp;

	private float viewerCountWindow;

	private bool viewerCountActive;

	private bool hasSavedData;
}
