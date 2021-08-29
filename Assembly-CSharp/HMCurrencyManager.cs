using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class HMCurrencyManager : MonoBehaviour
{
	private void prepAssets()
	{
		this.pointsToStackBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.pointsToStacks), true);
		this.compileBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.compileStore));
		this.cCloseBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.hideBinCompiler));
		this.cNOPSledCompileBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.compileNewNopSled), true, "COMPILING");
		this.cShellCompileBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.compileNewShell), true, "COMPILING");
		this.cDOSTurboCompileBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.compileDOSTurbo), true, "COMPILING");
		this.cKERNSkipCompileBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.compileKernSKIP), true, "COMPILING");
		this.cCloudFREEZECompileBTN.GetComponent<HMBTNObject>().setMyAction(new Action(this.compileCloudFREEZE), true, "COMPILING");
		this.myGameData = this.myHMM.getGameData();
	}

	private void prepValues()
	{
		this.cNOPSledCost.text = this.NOPSledCost.ToString();
		this.cShellCost.text = this.ShellCost.ToString();
		this.cDOSTurboCost.text = this.DOSTurboCost.ToString();
		this.cKERNSkipCost.text = this.KernSkipCost.ToString();
		this.cCloudFREEZECost.text = this.cloudFREEZECost.ToString();
		this.cCurrentStacks.text = this.myGameData.currentStacks.ToString();
		this.cCurrentNOPSleds.text = this.myGameData.currentNOPSleds.ToString();
		this.cCurrentShells.text = this.myGameData.currentShells.ToString();
		if (this.myGameData.DOSTurboOnline)
		{
			this.cDOSTurboCompileBTN.gameObject.SetActive(false);
		}
		if (this.myGameData.KernSKIPOnline)
		{
			this.cKERNSkipCompileBTN.gameObject.SetActive(false);
		}
		if (this.myGameData.CloudFREEZEOnline)
		{
			this.cCloudFREEZECompileBTN.gameObject.SetActive(false);
		}
	}

	private void pointsToStacks()
	{
		int num = Mathf.FloorToInt((float)(this.myGameData.currentTotalPoints / this.pointsToStacksValue));
		if (num > 0)
		{
			this.myHMM.lockMenus();
			this.compileBTN.GetComponent<HMBTNObject>().lockMe();
			this.currentStackAddingValue = this.myGameData.currentStacks;
			TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsToStackDefaultIMGCG.alpha, delegate(float x)
			{
				this.pointsToStackDefaultIMGCG.alpha = x;
			}, 0f, 0.35f), 3);
			TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsToStackHoldActionGroup.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.pointsToStackHoldActionGroup.GetComponent<CanvasGroup>().alpha = x;
			}, 1f, 0.35f), 2), 0.35f);
			GameManager.TimeSlinger.FireIntTimer(0.7f, new Action<int>(this.triggerAddNewStacks), num);
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.DeniedSFX, 1f, false);
			GameManager.TimeSlinger.FireTimer(5f, new Action(this.pointsToStackBTN.GetComponent<HMBTNObject>().releaseHold));
			GameManager.TimeSlinger.FireTimer(5f, new Action(this.resetPointsToStackCost));
			this.pointsToStackCost.SetActive(true);
			TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsToStackCost.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.pointsToStackCost.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 2f), 1), 3f);
		}
	}

	private void compileStore()
	{
		this.prepValues();
		this.myHMM.hideMenus();
		GameManager.TimeSlinger.FireTimer(0.9f, new Action(this.presentBinCompiler));
	}

	private void triggerAddNewStacks(int stacksToAdd)
	{
		this.currentStackAddingValue = this.myGameData.currentStacks;
		this.currentStackIndex = stacksToAdd;
		this.tmpCurrentPointsValue = this.myGameData.currentTotalPoints;
		this.myGameData.currentStacks = this.myGameData.currentStacks + stacksToAdd;
		this.myGameData.currentTotalPoints = this.myGameData.currentTotalPoints - stacksToAdd * this.pointsToStacksValue;
		GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
		this.aniAddNewStacks();
	}

	private void aniAddNewStacks()
	{
		if (this.currentStackIndex > 0)
		{
			this.myHMM.fireATextRollerWithSFX(this.currentPointsText.gameObject, this.tmpCurrentPointsValue, this.tmpCurrentPointsValue - this.pointsToStacksValue, this.myHMM.rollerDelayPerUnit, 0.95f, this.TextRollSFX, AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, 0.2f);
			GameManager.TimeSlinger.FireTimer(0.675f, new Action(this.addNewStackValue));
			this.stackAddSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.aniAddNewStacks));
			this.stackAddSeq.timeScale = 1.5f;
			TweenSettingsExtensions.Insert(this.stackAddSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsFullWhite.fillAmount, delegate(float x)
			{
				this.pointsFullWhite.fillAmount = x;
			}, 0f, 0.7f), 1));
			TweenSettingsExtensions.Insert(this.stackAddSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.StackAddIcon.transform.localScale, delegate(Vector3 x)
			{
				this.StackAddIcon.transform.localScale = x;
			}, new Vector3(1.8f, 1.8f, 1.8f), 0.1f), 1));
			TweenSettingsExtensions.Insert(this.stackAddSeq, 0.7f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.StackAddIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.StackAddIcon.GetComponent<CanvasGroup>().alpha = x;
			}, 1f, 0.1f), 1));
			TweenSettingsExtensions.Insert(this.stackAddSeq, 0.8f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.StackAddIcon.transform.localScale, delegate(Vector3 x)
			{
				this.StackAddIcon.transform.localScale = x;
			}, new Vector3(1f, 1f, 1f), 0.55f), 3));
			TweenSettingsExtensions.Insert(this.stackAddSeq, 1.35f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.StackAddIcon.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.StackAddIcon.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 0.55f), 1));
			TweenSettingsExtensions.Insert(this.stackAddSeq, 1.35f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsTrans.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.pointsTrans.GetComponent<CanvasGroup>().alpha = x;
			}, 1f, 0.45f), 1));
			TweenSettingsExtensions.Insert(this.stackAddSeq, 1.9f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsFullWhite.fillAmount, delegate(float x)
			{
				this.pointsFullWhite.fillAmount = x;
			}, 1f, 0f), 1));
			TweenSettingsExtensions.Insert(this.stackAddSeq, 1.91f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsTrans.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.pointsTrans.GetComponent<CanvasGroup>().alpha = x;
			}, 0.3f, 0f), 1));
			TweenExtensions.Play<Sequence>(this.stackAddSeq);
			this.currentStackIndex--;
			this.tmpCurrentPointsValue -= this.pointsToStacksValue;
		}
		else
		{
			TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsToStackHoldActionGroup.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.pointsToStackHoldActionGroup.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 0.35f), 3);
			TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.pointsToStackDefaultIMGCG.alpha, delegate(float x)
			{
				this.pointsToStackDefaultIMGCG.alpha = x;
			}, 1f, 0.35f), 2), 0.35f);
			this.pointsToStackBTN.GetComponent<HMBTNObject>().releaseHold();
			this.currentStackIndex = 0;
			this.tmpCurrentPointsValue = 0;
			this.myHMM.unLockMenus();
			this.compileBTN.GetComponent<HMBTNObject>().unLockMe();
		}
	}

	private void removeStackAni()
	{
		if (this.currentRemoveStackIndex > 0)
		{
			this.currentRemoveStackIndex--;
			this.tmpCurrentStacksValue--;
			this.cCurrentStacks.text = this.tmpCurrentStacksValue.ToString();
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.RemoveStackSFX, 1f, false);
			this.stacksAwayIcon.transform.localScale = new Vector3(1f, 1f, 1f);
			this.stacksAwayIcon.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			this.stackRemoveSeq = TweenSettingsExtensions.OnComplete<Sequence>(DOTween.Sequence(), new TweenCallback(this.removeStackAni));
			TweenSettingsExtensions.Insert(this.stackRemoveSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.stacksAwayIcon.transform.localScale, delegate(Vector3 x)
			{
				this.stacksAwayIcon.transform.localScale = x;
			}, new Vector3(1.5f, 1.5f, 1.5f), 0.4f), 1));
			TweenSettingsExtensions.Insert(this.stackRemoveSeq, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.stacksAwayIcon.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
			{
				this.stacksAwayIcon.gameObject.GetComponent<CanvasGroup>().alpha = x;
			}, 0f, 0.4f), 1));
			TweenExtensions.Play<Sequence>(this.stackRemoveSeq);
		}
		else
		{
			this.tmpCurrentStacksValue = 0;
			this.currentStackIndex = 0;
			this.unLockCompileBTNS();
			this.releaseHoldsOnCompileBTNS();
			this.prepValues();
		}
	}

	private void addNewStackValue()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.NewStackSFX, 0.4f, false);
		this.currentStackAddingValue++;
		this.currentStacksText.text = this.currentStackAddingValue.ToString();
	}

	private void presentBinCompiler()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.BinShowSFX, 0.85f, false);
		TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.binCompilerObject.transform.localPosition, delegate(Vector3 x)
		{
			this.binCompilerObject.transform.localPosition = x;
		}, new Vector3(0f, (float)Screen.height - 137f, 0f), 0.75f), 6), true);
	}

	private void hideBinCompiler()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.BinHideSFX, 0.85f, false);
		TweenSettingsExtensions.SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(() => this.binCompilerObject.transform.localPosition, delegate(Vector3 x)
		{
			this.binCompilerObject.transform.localPosition = x;
		}, new Vector3(0f, (float)(-(float)Screen.height) + 137f, 0f), 0.75f), 5), true);
		GameManager.TimeSlinger.FireTimer(0.65f, new Action(this.myHMM.showMenus));
	}

	private void compileNewNopSled()
	{
		if (this.myGameData.currentStacks >= this.NOPSledCost)
		{
			this.lockCompileBTNS();
			this.tmpCurrentStacksValue = this.myGameData.currentStacks;
			this.currentRemoveStackIndex = this.NOPSledCost;
			this.myGameData.currentStacks = this.myGameData.currentStacks - this.NOPSledCost;
			this.myGameData.currentNOPSleds = this.myGameData.currentNOPSleds + 1;
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
			this.removeStackAni();
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.DeniedSFX, 1f, false);
			GameManager.TimeSlinger.FireTimer(0.1f, new Action(this.cNOPSledCompileBTN.GetComponent<HMBTNObject>().releaseHold));
		}
	}

	private void compileNewShell()
	{
		if (this.myGameData.currentStacks >= this.ShellCost)
		{
			this.lockCompileBTNS();
			this.tmpCurrentStacksValue = this.myGameData.currentStacks;
			this.currentRemoveStackIndex = this.ShellCost;
			this.myGameData.currentStacks = this.myGameData.currentStacks - this.ShellCost;
			this.myGameData.currentShells = this.myGameData.currentShells + 1;
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
			this.removeStackAni();
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.DeniedSFX, 1f, false);
			GameManager.TimeSlinger.FireTimer(0.1f, new Action(this.cShellCompileBTN.GetComponent<HMBTNObject>().releaseHold));
		}
	}

	private void compileDOSTurbo()
	{
		if (this.myGameData.currentStacks >= this.DOSTurboCost)
		{
			this.lockCompileBTNS();
			this.tmpCurrentStacksValue = this.myGameData.currentStacks;
			this.currentRemoveStackIndex = this.DOSTurboCost;
			this.myGameData.currentStacks = this.myGameData.currentStacks - this.DOSTurboCost;
			this.myGameData.DOSTurboOnline = true;
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
			this.removeStackAni();
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.DeniedSFX, 1f, false);
			GameManager.TimeSlinger.FireTimer(0.1f, new Action(this.cDOSTurboCompileBTN.GetComponent<HMBTNObject>().releaseHold));
		}
	}

	private void compileKernSKIP()
	{
		if (this.myGameData.currentStacks >= this.KernSkipCost)
		{
			this.lockCompileBTNS();
			this.tmpCurrentStacksValue = this.myGameData.currentStacks;
			this.currentRemoveStackIndex = this.KernSkipCost;
			this.myGameData.currentStacks = this.myGameData.currentStacks - this.KernSkipCost;
			this.myGameData.KernSKIPOnline = true;
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
			this.removeStackAni();
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.DeniedSFX, 1f, false);
			GameManager.TimeSlinger.FireTimer(0.1f, new Action(this.cKERNSkipCompileBTN.GetComponent<HMBTNObject>().releaseHold));
		}
	}

	private void compileCloudFREEZE()
	{
		if (this.myGameData.currentStacks >= this.cloudFREEZECost)
		{
			this.lockCompileBTNS();
			this.tmpCurrentStacksValue = this.myGameData.currentStacks;
			this.currentRemoveStackIndex = this.cloudFREEZECost;
			this.myGameData.currentStacks = this.myGameData.currentStacks - this.cloudFREEZECost;
			this.myGameData.CloudFREEZEOnline = true;
			GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
			this.removeStackAni();
		}
		else
		{
			GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.DeniedSFX, 1f, false);
			GameManager.TimeSlinger.FireTimer(0.1f, new Action(this.cCloudFREEZECompileBTN.GetComponent<HMBTNObject>().releaseHold));
		}
	}

	private void lockCompileBTNS()
	{
		this.cNOPSledCompileBTN.GetComponent<HMBTNObject>().lockMe();
		this.cShellCompileBTN.GetComponent<HMBTNObject>().lockMe();
		this.cDOSTurboCompileBTN.GetComponent<HMBTNObject>().lockMe();
		this.cKERNSkipCompileBTN.GetComponent<HMBTNObject>().lockMe();
		this.cCloudFREEZECompileBTN.GetComponent<HMBTNObject>().lockMe();
	}

	private void unLockCompileBTNS()
	{
		this.cNOPSledCompileBTN.GetComponent<HMBTNObject>().unLockMe();
		this.cShellCompileBTN.GetComponent<HMBTNObject>().unLockMe();
		this.cDOSTurboCompileBTN.GetComponent<HMBTNObject>().unLockMe();
		this.cKERNSkipCompileBTN.GetComponent<HMBTNObject>().unLockMe();
		this.cCloudFREEZECompileBTN.GetComponent<HMBTNObject>().unLockMe();
	}

	private void releaseHoldsOnCompileBTNS()
	{
		this.cNOPSledCompileBTN.GetComponent<HMBTNObject>().releaseHold();
		this.cShellCompileBTN.GetComponent<HMBTNObject>().releaseHold();
		this.cDOSTurboCompileBTN.GetComponent<HMBTNObject>().releaseHold();
		this.cKERNSkipCompileBTN.GetComponent<HMBTNObject>().releaseHold();
		this.cCloudFREEZECompileBTN.GetComponent<HMBTNObject>().releaseHold();
	}

	private void resetPointsToStackCost()
	{
		this.pointsToStackCost.GetComponent<CanvasGroup>().alpha = 1f;
		this.pointsToStackCost.SetActive(false);
	}

	private void Start()
	{
		this.prepAssets();
		this.prepValues();
	}

	public HackerModeManager myHMM;

	public int pointsToStacksValue;

	public int NOPSledCost;

	public int ShellCost;

	public int DOSTurboCost;

	public int KernSkipCost;

	public int cloudFREEZECost;

	public GameObject pointsToStackBTN;

	public GameObject compileBTN;

	public GameObject pointsToStackCost;

	public Text currentPointsText;

	public Text currentStacksText;

	public Text NOPSledCount;

	public Text ShellCount;

	public Text DOSTurboStatus;

	public Text KSkipLineStatus;

	public Text CLOUDFreezeStatus;

	public GameObject pointsToStackHoldActionGroup;

	public CanvasGroup pointsToStackDefaultIMGCG;

	public Image pointsTrans;

	public Image pointsFullWhite;

	public Image StackAddIcon;

	public GameObject binCompilerObject;

	public Text cCurrentStacks;

	public Text cCurrentNOPSleds;

	public Text cCurrentShells;

	public Text cNOPSledCost;

	public Text cShellCost;

	public Text cDOSTurboCost;

	public Text cKERNSkipCost;

	public Text cCloudFREEZECost;

	public GameObject cNOPSledCompileBTN;

	public GameObject cShellCompileBTN;

	public GameObject cDOSTurboCompileBTN;

	public GameObject cKERNSkipCompileBTN;

	public GameObject cCloudFREEZECompileBTN;

	public GameObject cCloseBTN;

	public Image stacksAwayIcon;

	public AudioClip TextRollSFX;

	public AudioClip NewStackSFX;

	public AudioClip RemoveStackSFX;

	public AudioClip DeniedSFX;

	public AudioClip BinShowSFX;

	public AudioClip BinHideSFX;

	private int currentStackAddingValue;

	private int currentStackIndex;

	private int tmpCurrentPointsValue;

	private int tmpCurrentStacksValue;

	private int currentRemoveStackIndex;

	private HMGameData myGameData;

	private Sequence stackAddSeq;

	private Sequence pointsToStackSeq;

	private Sequence stackRemoveSeq;
}
