using System;

public class GameManager
{
	public GameManager()
	{
		this.inited = false;
	}

	public static GameManager Instance
	{
		get
		{
			if (GameManager.instance == null)
			{
				GameManager.instance = new GameManager();
			}
			return GameManager.instance;
		}
	}

	public static magicSlinger MagicSlinger
	{
		get
		{
			if (GameManager.magicSlinger == null)
			{
				GameManager.magicSlinger = new magicSlinger();
			}
			return GameManager.magicSlinger;
		}
	}

	public static audioSlinger AudioSlinger
	{
		get
		{
			if (GameManager.audioSlinger == null)
			{
				GameManager.audioSlinger = new audioSlinger();
			}
			return GameManager.audioSlinger;
		}
	}

	public static timeSlinger TimeSlinger
	{
		get
		{
			if (GameManager.timeSlinger == null)
			{
				GameManager.timeSlinger = new timeSlinger();
			}
			return GameManager.timeSlinger;
		}
	}

	public static fileSlinger FileSlinger
	{
		get
		{
			if (GameManager.fileSlinger == null)
			{
				GameManager.fileSlinger = new fileSlinger();
			}
			return GameManager.fileSlinger;
		}
	}

	public static SteamSlinger SteamSlinger
	{
		get
		{
			if (GameManager.steamSlinger == null)
			{
				GameManager.steamSlinger = new SteamSlinger();
			}
			return GameManager.steamSlinger;
		}
	}

	public void Init()
	{
		this.inited = true;
	}

	public bool isInited()
	{
		return this.inited;
	}

	public void Update()
	{
		GameManager.TimeSlinger.Update();
	}

	public static void SetTheCloud(TheCloud setCloud)
	{
		GameManager.MainCloud = setCloud;
	}

	public static void SetTheHackerManager(HackerManager setHM)
	{
		GameManager.HackerManager = setHM;
	}

	public static void SetAnnBehavior(annBehavior setAB)
	{
		GameManager.AnnBehavior = setAB;
	}

	public static void SetKidnapper(KidnapperBehavior setKB)
	{
		GameManager.kidnapperBehavior = setKB;
	}

	public static void SetHTTPPusher(HTTPPusher setHP)
	{
		GameManager.httpPusher = setHP;
	}

	public static void SetUIManager(UIManager setUIMan)
	{
		GameManager.uiManager = setUIMan;
	}

	public static void SetPhoneManager(PhoneManager setPhoneManager)
	{
		GameManager.phoneManager = setPhoneManager;
	}

	public static void SetScenceManager(SceneManagerWTTG setSceneManager)
	{
		GameManager.sceneManager = setSceneManager;
	}

	public static void SetTrackerManager(TrackerManager setTrackerManager)
	{
		GameManager.trackerManager = setTrackerManager;
	}

	public static void SetBreatherManager(BreatherManager setBreatherManager)
	{
		GameManager.breatherManager = setBreatherManager;
	}

	public static void SetMainController(mainController setMainController)
	{
		GameManager.myMainController = setMainController;
	}

	public static void SetDOSTwitch(DOSTwitch setDOSTwitch)
	{
		GameManager.myDOSTwitch = setDOSTwitch;
	}

	public static void SetModemAction(ModemAction setModemAction)
	{
		GameManager.myModemAction = setModemAction;
	}

	public static void SetLightSwitchAction(LightSwitchAction setLightSwitchAction)
	{
		GameManager.myLightSwitchAction = setLightSwitchAction;
	}

	public static void SetGameModeManager(GameModeManager setGameModeManager)
	{
		GameManager.myGameModeManager = setGameModeManager;
	}

	public static TheCloud GetTheCloud()
	{
		return GameManager.MainCloud;
	}

	public static HackerManager GetTheHackerManager()
	{
		return GameManager.HackerManager;
	}

	public static annBehavior GetTheAnnBehavior()
	{
		return GameManager.AnnBehavior;
	}

	public static KidnapperBehavior GetTheKidnapper()
	{
		return GameManager.kidnapperBehavior;
	}

	public static HTTPPusher GetTheHTTPPusher()
	{
		return GameManager.httpPusher;
	}

	public static UIManager GetTheUIManager()
	{
		return GameManager.uiManager;
	}

	public static PhoneManager GetThePhoneManager()
	{
		return GameManager.phoneManager;
	}

	public static SceneManagerWTTG GetTheSceneManager()
	{
		return GameManager.sceneManager;
	}

	public static TrackerManager GetTheTrackerManager()
	{
		return GameManager.trackerManager;
	}

	public static BreatherManager GetTheBreatherManager()
	{
		return GameManager.breatherManager;
	}

	public static mainController GetTheMainController()
	{
		return GameManager.myMainController;
	}

	public static DOSTwitch GetDOSTwitch()
	{
		return GameManager.myDOSTwitch;
	}

	public static ModemAction GetModemAction()
	{
		return GameManager.myModemAction;
	}

	public static LightSwitchAction GetLightSwitchAction()
	{
		return GameManager.myLightSwitchAction;
	}

	public static GameModeManager GetGameModeManager()
	{
		return GameManager.myGameModeManager;
	}

	private static GameManager instance;

	private static magicSlinger magicSlinger;

	private static audioSlinger audioSlinger;

	private static timeSlinger timeSlinger;

	private static fileSlinger fileSlinger;

	private static SteamSlinger steamSlinger;

	private bool inited;

	private static TheCloud MainCloud;

	private static HackerManager HackerManager;

	private static annBehavior AnnBehavior;

	private static KidnapperBehavior kidnapperBehavior;

	private static HTTPPusher httpPusher;

	private static UIManager uiManager;

	private static PhoneManager phoneManager;

	private static SceneManagerWTTG sceneManager;

	private static TrackerManager trackerManager;

	private static BreatherManager breatherManager;

	private static mainController myMainController;

	private static DOSTwitch myDOSTwitch;

	private static ModemAction myModemAction;

	private static LightSwitchAction myLightSwitchAction;

	private static GameModeManager myGameModeManager;
}
