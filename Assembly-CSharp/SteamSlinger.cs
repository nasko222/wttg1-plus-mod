using System;
using Steamworks;

public class SteamSlinger
{
	public void triggerSteamAchievement(string achievement, bool store = true)
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.SetAchievement(achievement);
			if (store)
			{
				this.StoreAchievement();
			}
		}
	}

	public bool triggerCheckForHackerMode()
	{
		return SteamManager.Initialized && SteamApps.BIsDlcInstalled((AppId_t)557280u);
	}

	public bool triggerCheckForTheWaitingRoom()
	{
		return SteamManager.Initialized && SteamApps.BIsDlcInstalled((AppId_t)744640u);
	}

	public void InstallTheWaitingRoom()
	{
		if (SteamManager.Initialized)
		{
			SteamApps.InstallDLC((AppId_t)744640u);
		}
	}

	private void StoreAchievement()
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.StoreStats();
		}
	}

	public string ACHIEVEMENT_REDRUM = "WTTG_REDRUM";

	public string ACHIEVEMENT_WHO_AM_I = "WTTG_WHO_AM_I";

	public string ACHIEVEMENT_WHERE_AM_I = "WTTG_WHERE_AM_I";

	public string ACHIEVEMENT_1337 = "WTTG_1337";

	public string ACHIEVEMENT_DEEP_WEB_OUTLAW = "WTTG_DEEP_WEB_OUTLAW";

	public string ACHIEVEMENT_GOOD_GUY_ADAM = "WTTG_GOOD_GUY_ADAM";

	public string ACHIEVEMENT_DEEP_EXPLORER = "WTTG_DEEP_EXPLORER";

	public string ACHIEVEMENT_IMPROVER = "WTTG_IMPROVER";

	public string ACHIEVEMENT_THE_DENIER = "WTTG_THE_DENIER";

	public string ACHIEVEMENT_THE_DEVELOPER = "WTTG_THE_DEVELOPER";

	public string ACHIEVEMENT_THE_CLOUD_CHASER = "WTTG_THE_CLOUD_CHASER";
}
