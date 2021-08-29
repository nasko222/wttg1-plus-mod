using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class KeyboardSFXScrub : MonoBehaviour
{
	private void processKeyboardSFX()
	{
		int num = UnityEngine.Random.Range(1, this.KeySFXs.Length);
		AudioClip audioClip = this.KeySFXs[num];
		GameManager.AudioSlinger.DealSound(this.aHub, this.aLayer, this.KeySFXs[num], 1f, false);
		this.KeySFXs[num] = this.KeySFXs[0];
		this.KeySFXs[0] = audioClip;
	}

	private void processSpaceReturnSFX()
	{
		GameManager.AudioSlinger.DealSound(this.aHub, this.aLayer, this.SpaceReturnSFX, 1f, false);
	}

	private void Start()
	{
		this.myTextInput = base.GetComponent<InputField>();
	}

	private void Update()
	{
		if (this.myTextInput.isFocused)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.processSpaceReturnSFX();
			}
			else if (Input.GetKeyDown(KeyCode.Return))
			{
				this.processSpaceReturnSFX();
			}
			else if (Input.anyKeyDown && !CrossPlatformInputManager.GetButtonDown("LeftClick") && !CrossPlatformInputManager.GetButtonDown("MiddleClick") && !CrossPlatformInputManager.GetButtonDown("RightClick"))
			{
				this.processKeyboardSFX();
			}
		}
	}

	public AudioHubs aHub;

	public AudioLayer aLayer = AudioLayer.COMPUTERSFX;

	public AudioClip[] KeySFXs;

	public AudioClip SpaceReturnSFX;

	private InputField myTextInput;
}
