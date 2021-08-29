using System;

[Serializable]
public class HMGameData
{
	public HMGameData()
	{
		this.highScoreDOS = (this.highScoreKernal = (this.highScoreCloud = (this.currentTotalPoints = (this.currentStacks = (this.currentNOPSleds = (this.currentShells = 0))))));
		this.DOSTurboOnline = (this.KernSKIPOnline = (this.CloudFREEZEOnline = false));
		this.musicIsOn = (this.sfxIsOn = true);
	}

	public void resetData()
	{
		this.highScoreDOS = (this.highScoreKernal = (this.highScoreCloud = (this.currentTotalPoints = (this.currentStacks = (this.currentNOPSleds = (this.currentShells = 0))))));
		this.DOSTurboOnline = (this.KernSKIPOnline = (this.CloudFREEZEOnline = false));
	}

	public int highScoreDOS;

	public int highScoreKernal;

	public int highScoreCloud;

	public int currentTotalPoints;

	public int currentStacks;

	public int currentNOPSleds;

	public int currentShells;

	public bool DOSTurboOnline;

	public bool KernSKIPOnline;

	public bool CloudFREEZEOnline;

	public bool musicIsOn;

	public bool sfxIsOn;
}
