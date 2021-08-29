using System;

[Serializable]
public class TwitchData
{
	public TwitchData()
	{
		this.isActive = false;
		this.twitchOAuth = string.Empty;
		this.twitchUserName = string.Empty;
	}

	public void resetData()
	{
		this.isActive = false;
		this.twitchOAuth = string.Empty;
		this.twitchUserName = string.Empty;
	}

	public bool isActive;

	public string twitchOAuth;

	public string twitchUserName;
}
