using System;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
	public void setCausalMode(bool setValue)
	{
		this.inCasualMode = setValue;
	}

	public bool getCasualMode()
	{
		return this.inCasualMode;
	}

	private void Awake()
	{
		GameManager.SetGameModeManager(this);
	}

	private void Start()
	{
		this.inCasualMode = false;
	}

	private bool inCasualMode;
}
