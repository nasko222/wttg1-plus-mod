using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class HMUpgradesManager : MonoBehaviour
{
	public void setCoolOff()
	{
		this.coolOffActive = false;
	}

	private void resetCoolOff()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.UpgradeCooledOffSFX, 0.75f, false);
		this.coolOffActive = false;
	}

	private void checkForCurrentGameMode()
	{
		this.currentGameMode = this.myHMM.getCurrentGameMode();
		this.gameModeSet = true;
	}

	private void gameModeUpgrades()
	{
		if (!this.gameModeUpgradeSet)
		{
			hackerModeGameType hackerModeGameType = this.currentGameMode;
			if (hackerModeGameType != hackerModeGameType.DOS)
			{
				if (hackerModeGameType != hackerModeGameType.KERNAL)
				{
					if (hackerModeGameType == hackerModeGameType.CLOUD)
					{
						if (this.myGameData.CloudFREEZEOnline)
						{
							this.cloudFREEZE.SetActive(true);
							this.gameModeUpgradeSet = true;
						}
					}
				}
				else if (this.myGameData.KernSKIPOnline)
				{
					this.KSkip.SetActive(true);
					this.gameModeUpgradeSet = true;
				}
			}
			else if (this.myGameData.DOSTurboOnline)
			{
				this.DOSTurbo.SetActive(true);
				this.gameModeUpgradeSet = true;
			}
		}
		else
		{
			hackerModeGameType hackerModeGameType2 = this.currentGameMode;
			if (hackerModeGameType2 != hackerModeGameType.DOS)
			{
				if (hackerModeGameType2 != hackerModeGameType.KERNAL)
				{
					if (hackerModeGameType2 == hackerModeGameType.CLOUD)
					{
						if (CrossPlatformInputManager.GetButtonDown("Control") && !this.coolOffActive)
						{
							GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.cloudFREEZESFX, 1f, false);
							this.coolOffActive = true;
							this.cloudFREEZE.GetComponent<CanvasGroup>().alpha = 0.1f;
							DOTween.To(() => this.cloudFREEZE.GetComponent<CanvasGroup>().alpha, delegate(float x)
							{
								this.cloudFREEZE.GetComponent<CanvasGroup>().alpha = x;
							}, 1f, this.coolOffTime * 60f).SetEase(Ease.Linear);
							GameManager.TimeSlinger.FireTimer(this.coolOffTime * 60f, new Action(this.resetCoolOff), "HMCoolOffTimer");
							this.myHMM.myVapeAttack.freezeTime();
						}
					}
				}
				else if (CrossPlatformInputManager.GetButtonDown("Control") && !this.coolOffActive)
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.KSkipSFX, 0.75f, false);
					this.coolOffActive = true;
					this.KSkip.GetComponent<CanvasGroup>().alpha = 0.1f;
					DOTween.To(() => this.KSkip.GetComponent<CanvasGroup>().alpha, delegate(float x)
					{
						this.KSkip.GetComponent<CanvasGroup>().alpha = x;
					}, 1f, this.coolOffTime * 60f).SetEase(Ease.Linear);
					GameManager.TimeSlinger.FireTimer(this.coolOffTime * 60f, new Action(this.resetCoolOff), "HMCoolOffTimer");
					this.myHMM.myKAttack.skipCurrentLine();
				}
			}
			else if (CrossPlatformInputManager.GetButtonDown("Control"))
			{
				if (this.DOSTurboActive)
				{
					DOTween.To(() => this.DOSTurbo.GetComponent<CanvasGroup>().alpha, delegate(float x)
					{
						this.DOSTurbo.GetComponent<CanvasGroup>().alpha = x;
					}, 0.1f, 0.7f).SetEase(Ease.Linear);
					this.DOSTurboActive = false;
					this.myHMM.DOSTurboIsHot = false;
				}
				else
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.DOSTurboSFX, 0.5f, false);
					DOTween.To(() => this.DOSTurbo.GetComponent<CanvasGroup>().alpha, delegate(float x)
					{
						this.DOSTurbo.GetComponent<CanvasGroup>().alpha = x;
					}, 1f, 0.7f).SetEase(Ease.Linear);
					this.DOSTurboActive = true;
					this.myHMM.DOSTurboIsHot = true;
				}
			}
		}
	}

	private void triggerNOPPower()
	{
		if (this.myGameData.currentNOPSleds >= 1 && this.myHMM.chainLevelValue.text != "Master" && !this.NOPFired)
		{
			if (CrossPlatformInputManager.GetAxis("RightClick") > this.tmpLastRCAxis)
			{
				GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.NOPPowerDownSFX.name);
				this.nopPowerDown.Kill(false);
				if (!this.nopPowerUp.IsPlaying())
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.NOPPowerUpSFX, 0.15f, false);
					this.nopPowerUp = DOTween.Sequence().OnComplete(new TweenCallback(this.FireNOP));
					this.nopPowerUp.Insert(0f, DOTween.To(() => this.NOPSledPowerUp.transform.localScale, delegate(Vector3 x)
					{
						this.NOPSledPowerUp.transform.localScale = x;
					}, new Vector3(1f, 1f, 1f), 1.5f).SetEase(Ease.OutExpo));
					this.nopPowerUp.Insert(0f, DOTween.To(() => this.NOPSledPowerUp.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
					{
						this.NOPSledPowerUp.gameObject.GetComponent<CanvasGroup>().alpha = x;
					}, 1f, 1.5f).SetEase(Ease.OutExpo));
					this.nopPowerUp.Play<Sequence>();
				}
			}
			else if (CrossPlatformInputManager.GetAxis("RightClick") < 1f && CrossPlatformInputManager.GetAxis("RightClick") != 0f)
			{
				GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.NOPPowerUpSFX.name);
				this.nopPowerUp.Kill(false);
				if (!this.nopPowerDown.IsPlaying())
				{
					GameManager.AudioSlinger.DealSound(AudioHubs.HACKERMODE, AudioLayer.HACKINGSFX, this.NOPPowerDownSFX, 0.15f, false);
					this.nopPowerDown = DOTween.Sequence();
					this.nopPowerDown.Insert(0f, DOTween.To(() => this.NOPSledPowerUp.transform.localScale, delegate(Vector3 x)
					{
						this.NOPSledPowerUp.transform.localScale = x;
					}, new Vector3(0.1f, 0.1f, 0.1f), 0.4f).SetEase(Ease.OutCirc));
					this.nopPowerDown.Insert(0f, DOTween.To(() => this.NOPSledPowerUp.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
					{
						this.NOPSledPowerUp.gameObject.GetComponent<CanvasGroup>().alpha = x;
					}, 0f, 0.4f).SetEase(Ease.OutCirc));
					this.nopPowerDown.Play<Sequence>();
				}
			}
			this.tmpLastRCAxis = CrossPlatformInputManager.GetAxis("RightClick");
		}
	}

	private void FireNOP()
	{
		GameManager.AudioSlinger.RemoveSound(AudioHubs.HACKERMODE, this.NOPPowerUpSFX.name);
		this.NOPFired = true;
		this.myGameData.currentNOPSleds = this.myGameData.currentNOPSleds - 1;
		GameManager.FileSlinger.wildSaveFile<HMGameData>("wttghm.gd", this.myGameData);
		DOTween.To(() => this.NOPSledSled.transform.localPosition, delegate(Vector3 x)
		{
			this.NOPSledSled.transform.localPosition = x;
		}, new Vector3((float)Screen.width + this.NOPSledSled.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0f, 0f), 1f).SetEase(Ease.Linear).SetRelative(true);
		DOTween.To(() => this.NOPSledPowerUp.transform.localScale, delegate(Vector3 x)
		{
			this.NOPSledPowerUp.transform.localScale = x;
		}, new Vector3(2f, 2f, 2f), 0.6f).SetEase(Ease.OutCirc);
		DOTween.To(() => this.NOPSledPowerUp.gameObject.GetComponent<CanvasGroup>().alpha, delegate(float x)
		{
			this.NOPSledPowerUp.gameObject.GetComponent<CanvasGroup>().alpha = x;
		}, 0f, 0.6f).SetEase(Ease.OutCirc);
		this.myHMM.FireANopSled();
		GameManager.TimeSlinger.FireTimer(1.1f, new Action(this.resetNOPSled));
	}

	private void resetNOPSled()
	{
		this.NOPSledPowerUp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		this.NOPFired = false;
		DOTween.To(() => this.NOPSledSled.transform.localPosition, delegate(Vector3 x)
		{
			this.NOPSledSled.transform.localPosition = x;
		}, new Vector3(-((float)Screen.width + this.NOPSledSled.gameObject.GetComponent<RectTransform>().sizeDelta.x), 0f, 0f), 0f).SetEase(Ease.Linear).SetRelative(true);
	}

	private void Start()
	{
		this.myGameData = this.myHMM.getGameData();
		this.nopPowerUp = DOTween.Sequence();
		this.nopPowerDown = DOTween.Sequence();
		this.NOPSledPowerUp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
	}

	private void Update()
	{
		this.curMousePOS = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		this.NOPSledPowerUp.transform.localPosition = new Vector3(this.curMousePOS.x, -this.curMousePOS.y, 0f);
		if (this.myHMM.amIInGameMode())
		{
			this.triggerNOPPower();
			if (this.gameModeSet)
			{
				this.gameModeUpgrades();
			}
			else
			{
				this.checkForCurrentGameMode();
			}
		}
		else
		{
			this.gameModeSet = false;
			this.gameModeUpgradeSet = false;
		}
	}

	[Range(0.5f, 5f)]
	public float coolOffTime = 2f;

	public HackerModeManager myHMM;

	public Image NOPSledPowerUp;

	public Image NOPSledSled;

	public GameObject DOSTurbo;

	public GameObject KSkip;

	public GameObject cloudFREEZE;

	public AudioClip NOPPowerUpSFX;

	public AudioClip NOPPowerDownSFX;

	public AudioClip DOSTurboSFX;

	public AudioClip KSkipSFX;

	public AudioClip UpgradeCooledOffSFX;

	public AudioClip cloudFREEZESFX;

	private bool NOPFired;

	private bool gameModeSet;

	private bool gameModeUpgradeSet;

	private bool DOSTurboActive;

	private bool coolOffActive;

	private HMGameData myGameData;

	private Vector2 curMousePOS;

	private Sequence nopPowerUp;

	private Sequence nopPowerDown;

	private float tmpLastRCAxis;

	private hackerModeGameType currentGameMode;
}
