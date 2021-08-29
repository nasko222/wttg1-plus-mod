using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	private void prepLoading()
	{
		this.prepSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.startLoading));
		TweenSettingsExtensions.Insert(this.prepSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.redRoomDef.color, delegate(Color x)
		{
			this.redRoomDef.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.75f), 1));
		TweenSettingsExtensions.Insert(this.prepSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.loadingIcon.color, delegate(Color x)
		{
			this.loadingIcon.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.5f), 1));
		TweenSettingsExtensions.Insert(this.prepSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.loadingIcon.rectTransform.localScale, delegate(Vector3 x)
		{
			this.loadingIcon.rectTransform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.5f), 3));
		TweenExtensions.Play<Sequence>(this.prepSeq);
	}

	private void startLoading()
	{
		GameManager.TimeSlinger.FireTimer(2.5f, new Action(this.loadGame));
		this.loadingSeq = DOTween.Sequence();
		TweenSettingsExtensions.Insert(this.loadingSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.loadingIcon.color, delegate(Color x)
		{
			this.loadingIcon.color = x;
		}, new Color(1f, 1f, 1f, 0.05f), 0.75f), 1));
		TweenSettingsExtensions.Insert(this.loadingSeq, 0.75f, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(() => this.loadingIcon.color, delegate(Color x)
		{
			this.loadingIcon.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.75f), 1));
		TweenSettingsExtensions.SetLoops<Sequence>(this.loadingSeq, -1);
		TweenExtensions.Play<Sequence>(this.loadingSeq);
	}

	private void loadGame()
	{
		base.StartCoroutine(this.loadGameAdd());
	}

	private IEnumerator loadGameAdd()
	{
		AsyncOperation result = SceneManager.LoadSceneAsync(1, 1);
		while (!result.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		Object.Destroy(this.MenuWorld);
		yield break;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		this.prepLoading();
	}

	public GameObject eventSystemObject;

	public GameObject MenuWorld;

	public Image redRoomDef;

	public Image loadingIcon;

	private Sequence prepSeq;

	private Sequence loadingSeq;
}
