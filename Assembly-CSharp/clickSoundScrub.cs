using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class clickSoundScrub : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.clickNoise, 1f, false);
	}

	public AudioClip clickNoise;
}
