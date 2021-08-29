using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void buildMe(Action<int> setAction, string setRes, int setIndex, float setX, float setY)
	{
		this.myAction = setAction;
		this.myLink.text = setRes;
		this.myResIndex = setIndex;
		this.mySetWidth = this.getStrWidth(setRes);
		this.myLink.GetComponent<RectTransform>().sizeDelta = new Vector2(this.mySetWidth, 36f);
		base.GetComponent<RectTransform>().sizeDelta = new Vector2(this.mySetWidth, 36f);
		base.GetComponent<RectTransform>().localPosition = new Vector3(setX, setY, 0f);
		base.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
	}

	public void setActive()
	{
		this.isActive = true;
		this.myLink.color = this.activeColor;
	}

	public void setInactive()
	{
		this.isActive = false;
		this.myLink.color = this.defaultColor;
	}

	private void Start()
	{
		if (this.isActive)
		{
			this.myLink.color = this.activeColor;
		}
	}

	private float getStrWidth(string theString)
	{
		TextGenerationSettings textGenerationSettings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		textGenerationSettings.textAnchor = 0;
		textGenerationSettings.generateOutOfBounds = true;
		textGenerationSettings.generationExtents = new Vector2(125f, 36f);
		textGenerationSettings.pivot = Vector2.zero;
		textGenerationSettings.richText = false;
		textGenerationSettings.font = this.resFont;
		textGenerationSettings.fontSize = 32;
		textGenerationSettings.fontStyle = 0;
		textGenerationSettings.lineSpacing = 1f;
		textGenerationSettings.scaleFactor = 1f;
		textGenerationSettings.verticalOverflow = 1;
		textGenerationSettings.horizontalOverflow = 0;
		return textGenerator.GetPreferredWidth(theString, textGenerationSettings) + 5f;
	}

	private void Update()
	{
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuHoverSound, 0.45f, false);
		this.myLink.color = this.hoverColor;
		this.myMenuManager.setHoverCursor();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.isActive)
		{
			this.myLink.color = this.activeColor;
		}
		else
		{
			this.myLink.color = this.defaultColor;
		}
		this.myMenuManager.setDefaultCursor();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.MENU, AudioLayer.MENU, this.myMenuManager.menuClickSound, 0.45f, false);
		this.myLink.color = this.defaultColor;
		this.myMenuManager.setDefaultCursor();
		this.myAction(this.myResIndex);
	}

	public MenuManager myMenuManager;

	public Font resFont;

	public bool isActive;

	public Color defaultColor;

	public Color activeColor;

	public Color hoverColor;

	public Text myLink;

	public int myResIndex;

	public float mySetWidth;

	private Action<int> myAction;
}
