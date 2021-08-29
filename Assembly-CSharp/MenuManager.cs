using System;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public void setDefaultCursor()
	{
		Cursor.SetCursor(this.defaultCursor, Vector2.zero, CursorMode.Auto);
	}

	public void setHoverCursor()
	{
		Cursor.SetCursor(this.hoverCursor, Vector2.zero, CursorMode.Auto);
	}

	public void beginGame()
	{
		this.glitchFX.enabled = false;
		this.gameSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.startLoad));
		this.gameSeq.Insert(0f, DOTween.To(() => this.startMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.startMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		this.gameSeq.Play<Sequence>();
	}

	public void showOpts()
	{
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.disableStartMenu));
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.enableOptMenu));
		this.glitchFX.enabled = false;
		this.optSeq = DOTween.Sequence();
		this.optSeq.Insert(0f, DOTween.To(() => this.startMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.startMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		this.optSeq.Insert(0.25f, DOTween.To(() => this.optionsMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.optionsMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		this.optSeq.Play<Sequence>();
	}

	public void hideOpts()
	{
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.disableOptMenu));
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.enableStartMenu));
		this.glitchFX.enabled = true;
		this.optSeq = DOTween.Sequence();
		this.optSeq.Insert(0f, DOTween.To(() => this.optionsMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.optionsMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		this.optSeq.Insert(0.25f, DOTween.To(() => this.startMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.startMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		this.optSeq.Play<Sequence>();
	}

	public void loadHackerMode()
	{
		SceneManager.LoadScene(4);
	}

	public void showTwitch()
	{
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.disableStartMenu));
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.enableTwitchMenu));
		this.glitchFX.enabled = false;
		this.twitchSeq = DOTween.Sequence();
		this.twitchSeq.Insert(0f, DOTween.To(() => this.startMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.startMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		this.twitchSeq.Insert(0.25f, DOTween.To(() => this.twitchMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.twitchMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		this.twitchSeq.Play<Sequence>();
	}

	public void hideTwitch()
	{
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.disableTwitchMenu));
		GameManager.TimeSlinger.FireTimer(0.25f, new Action(this.enableStartMenu));
		this.glitchFX.enabled = true;
		this.twitchSeq = DOTween.Sequence();
		this.twitchSeq.Insert(0f, DOTween.To(() => this.twitchMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.twitchMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear));
		this.twitchSeq.Insert(0.25f, DOTween.To(() => this.startMenu.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.startMenu.GetComponent<CanvasGroup>().alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear));
		this.twitchSeq.Play<Sequence>();
	}

	private void enableOptMenu()
	{
		this.optionsMenu.SetActive(true);
	}

	private void disableOptMenu()
	{
		this.optionsMenu.SetActive(false);
	}

	private void enableStartMenu()
	{
		this.startMenu.SetActive(true);
	}

	private void disableStartMenu()
	{
		this.startMenu.SetActive(false);
	}

	private void enableTwitchMenu()
	{
		this.twitchMenu.SetActive(true);
	}

	private void disableTwitchMenu()
	{
		this.twitchMenu.SetActive(false);
	}

	private void startLoad()
	{
		this.startMenu.SetActive(false);
		this.loadingMenu.SetActive(true);
	}

	private void Start()
	{
		this.glitchFX = this.mainCamera.gameObject.GetComponent<Glitch>();
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		this.setDefaultCursor();
	}

	public Camera mainCamera;

	public Texture2D defaultCursor;

	public Texture2D hoverCursor;

	public GameObject startMenu;

	public GameObject loadingMenu;

	public GameObject optionsMenu;

	public GameObject twitchMenu;

	public AudioClip menuHoverSound;

	public AudioClip menuClickSound;

	private Sequence gameSeq;

	private Sequence optSeq;

	private Sequence twitchSeq;

	private Glitch glitchFX;
}
