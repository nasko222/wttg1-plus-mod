using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TwitchLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	private void Start()
	{
		this.myIMG = base.GetComponent<Image>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuHoverSound, 0.45f, false);
		this.myIMG.sprite = this.myHoverSprite;
		this.myMenuManager.setHoverCursor();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.myIMG.sprite = this.myDefaultSprite;
		this.myMenuManager.setDefaultCursor();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuClickSound, 0.45f, false);
		this.myMenuManager.setDefaultCursor();
		this.myIMG.sprite = this.myDefaultSprite;
		this.myMenuManager.showTwitch();
	}

	public MenuManager myMenuManager;

	public Sprite myDefaultSprite;

	public Sprite myHoverSprite;

	private Image myIMG;
}
