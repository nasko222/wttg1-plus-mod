using System;
using UnityEngine;

public class TrackerManager : MonoBehaviour
{
	public void userCanNowBeTracked()
	{
		if (!this.canBeTracked)
		{
			this.canBeTracked = true;
			this.KidnapperVan.transform.localPosition = new Vector3(this.KidnapperVan.transform.localPosition.x, this.KidnapperVan.transform.localPosition.y, 14.63f);
			this.KidnapperVanRB.RenderProbe();
		}
	}

	public bool isTheCoastClear()
	{
		if (this.peepModeActive)
		{
			this.myMicManager.stopPlayerListen();
			this.peepModeActive = false;
			this.canHear = false;
			return false;
		}
		return true;
	}

	public void cancelDoomClock()
	{
		this.doomClockActive = false;
	}

	public void KidnapNow()
	{
		this.inKidnapperAction = true;
		this.canBeTracked = true;
		GameManager.GetTheKidnapper().triggerUsingComptuerJumpIlde();
		this.myMainController.isDoomed = true;
		this.forceTheDoom();
	}

	public void playerNoiseHasHappend()
	{
		if (this.peepModeActive && this.canHear)
		{
			GameManager.TimeSlinger.FireTimer(4.3f, new Action(GameManager.GetThePhoneManager().killPhone));
			this.playerDidTalk();
		}
	}

	public void spoofTracker()
	{
		this.NotifiyIconHolder.SetActive(true);
		GameManager.TimeSlinger.FireTimer(10f, new Action(this.hideNotify));
	}

	public bool isPlayerInKidnapperAction()
	{
		return this.inKidnapperAction;
	}

	private void generateTrackingWindow()
	{
		this.canBeTracked = false;
		float num = (float)UnityEngine.Random.Range(this.trackWindowOpen, this.trackWindowClose) * 60f;
		float num2 = num * ((float)GameManager.GetTheCloud().getRedRoomKeyVistCount() / 10f / 4f);
		float num3 = num - num2;
		this.trackTimeStamp = Time.time;
		this.trackWindowTime = num3;
		this.trackWindowSet = true;
	}

	private void setTrackingWindow(float setTime)
	{
		this.canBeTracked = false;
		this.trackTimeStamp = Time.time;
		this.trackWindowTime = setTime;
		this.trackWindowSet = true;
	}

	private void notifyUserBeingTracked()
	{
		this.inKidnapperAction = true;
		GameManager.GetTheCloud().myTimeManager.freezeTime = true;
		if (GameManager.GetTheSceneManager().isInDoorAction())
		{
			GameManager.FileSlinger.deleteFile("wttg2.gd");
			GameManager.GetTheMainController().doorKidnap = true;
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(this.notifyTime, new Action(this.launchReacitonWindow));
			int num = UnityEngine.Random.Range(0, 11);
			if (num <= 4)
			{
				this.NotifiyIconHolder.SetActive(true);
			}
			else
			{
				GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.notifySound, 1f, false);
				GameManager.AudioSlinger.DealSound(AudioHubs.PLAYER, AudioLayer.BACKGROUND, this.notifySound2, 1f, false, 1f);
				GameManager.AudioSlinger.DealSound(AudioHubs.MAINROOM, AudioLayer.BACKGROUND, this.notifySound3, 1f, false, 2.8f);
			}
		}
	}

	private void hideNotify()
	{
		this.NotifiyIconHolder.SetActive(false);
	}

	private void launchReacitonWindow()
	{
		this.NotifiyIconHolder.SetActive(false);
		GameManager.TimeSlinger.FireTimer(this.reactionTime, new Action(this.reactionWindowClosed));
	}

	private void reactionWindowClosed()
	{
		if (this.myScenceManager.areTheLightsOn())
		{
			if (this.myMainController.isUsingComputer)
			{
				GameManager.GetTheKidnapper().triggerUsingComptuerJumpIlde();
				this.myMainController.isDoomed = true;
				this.setDoomClock();
			}
			else if (this.myMainController.isMovingAround)
			{
				GameManager.TimeSlinger.FireTimer(10f, new Action(this.reactionWindowClosed));
			}
			else if (GameManager.GetTheSceneManager().isInDoorAction())
			{
				GameManager.FileSlinger.deleteFile("wttg2.gd");
				GameManager.GetTheMainController().doorKidnap = true;
			}
			else
			{
				GameManager.FileSlinger.deleteFile("wttg2.gd");
				GameManager.GetTheKidnapper().triggerClimbJump();
				GameManager.GetTheUIManager().removeRestModem();
				this.myMainController.triggerClimbJumpMovement();
			}
		}
		else
		{
			this.triggerPeepMode();
		}
	}

	private void triggerPeepMode()
	{
		this.peepWindowTime = UnityEngine.Random.Range(this.peepModeWindowOpen, this.peepModeWindowClose);
		this.peepTimeStamp = Time.time;
		this.peepModeActive = true;
		GameManager.GetTheKidnapper().triggerRoamMode(this.peepWindowTime);
		GameManager.TimeSlinger.FireTimer(4.5f, new Action(this.myMicManager.triggerPlayerListen));
		GameManager.TimeSlinger.FireTimer(4.5f, new Action(this.setCanHear));
	}

	private void stopPeepMode()
	{
		this.inKidnapperAction = false;
		this.myMicManager.stopPlayerListen();
		GameManager.GetTheKidnapper().endRoamMode();
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.kidNapperCooledOff));
	}

	private void noMicFound()
	{
	}

	private void playerDidTalk()
	{
		if (this.inKidnapperAction)
		{
			GameManager.FileSlinger.deleteFile("wttg2.gd");
			GameManager.GetTheKidnapper().triggerGotoClimbPoint();
			GameManager.TimeSlinger.FireTimer(0.3f, new Action(this.myMainController.triggerClimbJumpMovement));
		}
	}

	private void setDoomClock()
	{
		GameManager.FileSlinger.deleteFile("wttg2.gd");
		this.doomClockWindowTime = UnityEngine.Random.Range(this.doomClockOpen, this.doomClockClosed);
		this.doomClockTimeStamp = Time.time;
		this.doomClockActive = true;
	}

	private void forceTheDoom()
	{
		GameManager.GetTheKidnapper().triggerHeyKid();
		this.myMainController.myComputerController.lockOutRightClick = true;
		GameManager.TimeSlinger.FireTimer(1.8f, new Action(this.myMainController.switchToMainView));
	}

	private void kidNapperCooledOff()
	{
		this.canBeTracked = true;
	}

	private void setCanHear()
	{
		this.canHear = true;
	}

	private void Awake()
	{
		GameManager.SetTrackerManager(this);
	}

	private void Start()
	{
		this.myMicManager.playerHasNoMic += this.noMicFound;
		this.myMicManager.playerDidTalk += this.playerDidTalk;
	}

	private void Update()
	{
		if (!GameManager.GetGameModeManager().getCasualMode())
		{
			if (this.canBeTracked)
			{
				this.generateTrackingWindow();
			}
			if (this.trackWindowSet && Time.time - this.trackTimeStamp >= this.trackWindowTime && !GameManager.GetTheCloud().myTimeManager.freezeTime)
			{
				if (GameManager.GetTheBreatherManager().isPlayerInBreatherAction())
				{
					this.generateTrackingWindow();
				}
				else if (GameManager.GetTheMainController().isMovingAround)
				{
					this.setTrackingWindow(15f);
				}
				else
				{
					this.trackWindowSet = false;
					this.notifyUserBeingTracked();
				}
			}
			if (this.peepModeActive && Time.time - this.peepTimeStamp >= this.peepWindowTime)
			{
				this.peepModeActive = false;
				this.canHear = false;
				this.stopPeepMode();
			}
			if (this.doomClockActive && Time.time - this.doomClockTimeStamp >= this.doomClockWindowTime)
			{
				this.doomClockActive = false;
				this.forceTheDoom();
			}
		}
	}

	public mainController myMainController;

	public SceneManagerWTTG myScenceManager;

	public MicManager myMicManager;

	public AudioClip notifySound;

	public AudioClip notifySound2;

	public AudioClip notifySound3;

	public GameObject KidnapperVan;

	public ReflectionProbe KidnapperVanRB;

	[Range(1f, 10f)]
	public int trackWindowOpen = 2;

	[Range(5f, 20f)]
	public int trackWindowClose = 11;

	[Range(0f, 20f)]
	public float notifyTime = 10f;

	[Range(10f, 60f)]
	public float reactionTime = 10f;

	[Range(10f, 30f)]
	public float peepModeWindowOpen = 30f;

	[Range(30f, 120f)]
	public float peepModeWindowClose = 60f;

	[Range(10f, 30f)]
	public float doomClockOpen = 15f;

	[Range(30f, 90f)]
	public float doomClockClosed = 45f;

	public GameObject NotifiyIconHolder;

	private bool canBeTracked;

	private bool trackWindowSet;

	private bool peepModeActive;

	private bool doomClockActive;

	private bool canHear;

	private bool inKidnapperAction;

	private float trackTimeStamp;

	private float trackWindowTime;

	private float peepTimeStamp;

	private float peepWindowTime;

	private float doomClockTimeStamp;

	private float doomClockWindowTime;
}
