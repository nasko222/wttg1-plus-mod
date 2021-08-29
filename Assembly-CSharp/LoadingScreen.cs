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
		this.prepSeq = DOTween.Sequence().OnComplete(new TweenCallback(this.startLoading));
		this.prepSeq.Insert(0f, DOTween.To(() => this.redRoomDef.color, delegate(Color x)
		{
			this.redRoomDef.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.75f).SetEase(Ease.Linear));
		this.prepSeq.Insert(0f, DOTween.To(() => this.loadingIcon.color, delegate(Color x)
		{
			this.loadingIcon.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.5f).SetEase(Ease.Linear));
		this.prepSeq.Insert(0f, DOTween.To(() => this.loadingIcon.rectTransform.localScale, delegate(Vector3 x)
		{
			this.loadingIcon.rectTransform.localScale = x;
		}, new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutSine));
		this.prepSeq.Play<Sequence>();
	}

	private void startLoading()
	{
		GameManager.TimeSlinger.FireTimer(2.5f, new Action(this.loadGame));
		this.loadingSeq = DOTween.Sequence();
		this.loadingSeq.Insert(0f, DOTween.To(() => this.loadingIcon.color, delegate(Color x)
		{
			this.loadingIcon.color = x;
		}, new Color(1f, 1f, 1f, 0.05f), 0.75f).SetEase(Ease.Linear));
		this.loadingSeq.Insert(0.75f, DOTween.To(() => this.loadingIcon.color, delegate(Color x)
		{
			this.loadingIcon.color = x;
		}, new Color(1f, 1f, 1f, 1f), 0.75f).SetEase(Ease.Linear));
		this.loadingSeq.SetLoops(-1);
		this.loadingSeq.Play<Sequence>();
	}

	private void loadGame()
	{
		base.StartCoroutine(this.loadGameAdd());
	}

	private IEnumerator loadGameAdd()
	{
		AsyncOperation result = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		while (!result.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		UnityEngine.Object.Destroy(this.MenuWorld);
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
