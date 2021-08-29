using System;
using UnityEngine;

public class GameManagerHook : MonoBehaviour
{
	private void Awake()
	{
		if (GameManager.Instance.isInited())
		{
			Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			Object.DontDestroyOnLoad(base.gameObject);
			GameManager.Instance.Init();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		GameManager.Instance.Update();
	}
}
