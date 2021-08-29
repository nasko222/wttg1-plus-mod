using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class muteBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void Force(bool on)
	{
		if (on)
		{
			this.iAmMuted = false;
			this.MuteIMG.sprite = this.UNMutedSprite;
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.SOFTWARESFX);
		}
		else
		{
			this.iAmMuted = true;
			this.MuteIMG.sprite = this.MutedSprite;
			GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
		}
	}

	private void MuteAction()
	{
		if (this.iAmMuted)
		{
			GameManager.AudioSlinger.UnMuffleGlobalVolume(AudioLayer.SOFTWARESFX);
			this.MuteIMG.sprite = this.UNMutedSprite;
			this.iAmMuted = false;
		}
		else
		{
			GameManager.AudioSlinger.MuffleGlobalVolume(AudioLayer.SOFTWARESFX, 0f);
			this.MuteIMG.sprite = this.MutedSprite;
			this.iAmMuted = true;
		}
	}

	private void Start()
	{
		this.iAmMuted = false;
		this.MuteIMG = base.GetComponent<Image>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.MuteAction();
	}

	public Sprite UNMutedSprite;

	public Sprite MutedSprite;

	public bool iAmMuted;

	private Image MuteIMG;
}
