using System;

[Serializable]
public class OptionData
{
	public OptionData()
	{
		this.vSyncOn = true;
		this.fullScreenMode = true;
		this.qLevel = 0;
		this.contentFilter = true;
	}

	public void resetData()
	{
		this.vSyncOn = true;
		this.fullScreenMode = true;
		this.qLevel = 0;
		this.contentFilter = true;
	}

	public bool vSyncOn;

	public bool fullScreenMode;

	public bool useTheMic;

	public int qLevel;

	public int setScreenWidth;

	public int setScreenHeight;

	public bool contentFilter;
}
