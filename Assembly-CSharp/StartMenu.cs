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
		this.topSeq.Insert(0f, DOTween.To(() => this.topImage.color, delegate(Color x)
		{
			this.topImage.color = x;
		}, new Color(1f, 1f, 1f, 0.3f), 2f).SetEase(Ease.OutSine));
		this.topSeq.Insert(2f, DOTween.To(() => this.topImage.color, delegate(Color x)
		{
			this.topImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 2f).SetEase(Ease.InSine));
		this.topSeq.SetLoops(-1);
		this.topSeq.Play<Sequence>();
	}

	private void aniTitle()
	{
		this.titleSeq = DOTween.Sequence();
		this.titleSeq.Insert(0f, DOTween.To(() => this.titleFadeImage.color, delegate(Color x)
		{
			this.titleFadeImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 1.5f).SetEase(Ease.Linear));
		this.titleSeq.Insert(1.5f, DOTween.To(() => this.titleFadeImage.color, delegate(Color x)
		{
			this.titleFadeImage.color = x;
		}, new Color(1f, 1f, 1f, 0f), 1.5f).SetEase(Ease.Linear));
		this.titleSeq.SetLoops(-1);
		this.titleSeq.Play<Sequence>();
	}

	private void aniRec()
	{
		this.recSeq = DOTween.Sequence();
		this.recSeq.Insert(0f, DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 0.2f), 1f).SetEase(Ease.Linear));
		this.recSeq.Insert(1f, DOTween.To(() => this.recLiveImage.color, delegate(Color x)
		{
			this.recLiveImage.color = x;
		}, new Color(1f, 1f, 1f, 1f), 1f).SetEase(Ease.Linear));
		this.recSeq.SetLoops(-1);
		this.recSeq.Play<Sequence>();
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
			this.viewerCountWindow = UnityEngine.Random.Range(1f, 5f);
			this.viewerCountTimeStamp = Time.time;
			this.viewerCountActive = true;
			this.viewerCount += UnityEngine.Random.Range(4, 25);
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
		this.menuSeq.Insert(0f, DOTween.To(() => this.gameMode.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameMode.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Insert(0.3f, DOTween.To(() => this.areYouSure.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.areYouSure.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Play<Sequence>();
	}

	private void showGameMode()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.enableGameMode));
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.disableMenuHolder));
		this.menuSeq = DOTween.Sequence();
		this.menuSeq.Insert(0f, DOTween.To(() => this.menuHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.menuHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Insert(0.3f, DOTween.To(() => this.gameMode.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameMode.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Play<Sequence>();
	}

	private void goBackToMenuHolder()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.disableAreYouSure));
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.enableMenuHolder));
		this.menuSeq = DOTween.Sequence();
		this.menuSeq.Insert(0f, DOTween.To(() => this.areYouSure.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.areYouSure.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Insert(0.3f, DOTween.To(() => this.menuHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.menuHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Play<Sequence>();
	}

	private void goBackToMenuFromGameMode()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.disableGameMode));
		GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.enableMenuHolder));
		this.menuSeq = DOTween.Sequence();
		this.menuSeq.Insert(0f, DOTween.To(() => this.gameMode.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.gameMode.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Insert(0.3f, DOTween.To(() => this.menuHolder.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.menuHolder.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.3f).SetEase(Ease.Linear));
		this.menuSeq.Play<Sequence>();
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
