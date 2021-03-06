using System;
using UnityEngine;

[Serializable]
public class VapeChainDefinition : Definition
{
	[Range(0.5f, 6f)]
	public float warmUpTime;

	[Range(0.5f, 5f)]
	public float timePerBlock;

	[Range(2f, 12f)]
	public short matrixSize;

	[Range(0f, 1f)]
	public float freeCountPer;

	[Range(2f, 6f)]
	public short groupSize;

	public bool hasDeadNodes;

	[Range(1f, 5f)]
	public short deadNodeSize;

	public int pointsRequired;

	public int completeBonusPoints;

	public int timeBonusPoints;

	public int skillBonusPoints;

	[Range(1f, 6f)]
	public short bonusMultiplier;

	[Range(0f, 1f)]
	public float timeBonusRange;

	public short maxNumOfMoves;
}
