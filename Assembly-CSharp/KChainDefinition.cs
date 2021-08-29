using System;
using UnityEngine;

[Serializable]
public class KChainDefinition : Definition
{
	[Range(0.5f, 6f)]
	public float warmUpTime;

	public float KTime;

	public short KNumOfLines;

	public float KBoostTime;

	public int pointsRequired;

	public int completeBonusPoints;

	public int timeBonusPoints;

	public int skillBonusPoints;

	[Range(1f, 6f)]
	public short bonusMultiplier;

	[Range(0f, 1f)]
	public float timeBonusRange;

	public short maxNumberOfBackSpaces;
}
