using System;
using UnityEngine;
using UnityEngine.UI;

public class txtBehavior : MonoBehaviour
{
	public void buildMe(float setX = 0f, float setY = 0f, string setTxtTitle = "", string setTxt = "")
	{
		base.GetComponent<bringMeToFrontBehavior>().parentTrans = this.windowHolderRT;
		this.myDragBeh.myMainController = this.mainController;
		this.myDragBeh.dragPlane = this.dragPlane;
		this.myCloseBeh.myMainController = this.mainController;
		this.myRT = base.GetComponent<RectTransform>();
		float strHeight = this.getStrHeight(setTxt);
		float y = Mathf.Abs(this.txtField.GetComponent<RectTransform>().transform.localPosition.y) + strHeight + 10f;
		this.txtFieldText.rectTransform.sizeDelta = new Vector2(this.txtFieldText.rectTransform.sizeDelta.x, strHeight);
		this.txtField.GetComponent<RectTransform>().sizeDelta = new Vector2(this.txtField.GetComponent<RectTransform>().sizeDelta.x, strHeight);
		base.transform.localPosition = new Vector3(setX, setY, 0f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		this.myRT.sizeDelta = new Vector2(this.myRT.sizeDelta.x, y);
		this.txtTitle.text = setTxtTitle;
		this.txtField.text = setTxt;
	}

	public Vector2 getMyWH()
	{
		return this.myRT.sizeDelta;
	}

	public void rePOSme(float setX = 0f, float setY = 0f)
	{
		base.transform.localPosition = new Vector3(setX, setY, 0f);
	}

	private float getStrHeight(string theString)
	{
		TextGenerationSettings settings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		settings.textAnchor = TextAnchor.UpperLeft;
		settings.generateOutOfBounds = true;
		settings.generationExtents = new Vector2(this.txtField.GetComponent<RectTransform>().sizeDelta.x, this.txtField.GetComponent<RectTransform>().sizeDelta.y);
		settings.pivot = Vector2.zero;
		settings.richText = false;
		settings.font = this.txtFont;
		settings.fontSize = 16;
		settings.fontStyle = FontStyle.Normal;
		settings.lineSpacing = 1f;
		settings.scaleFactor = 1f;
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		settings.horizontalOverflow = HorizontalWrapMode.Wrap;
		return textGenerator.GetPreferredHeight(theString, settings);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public Text txtTitle;

	public InputField txtField;

	public Text txtFieldText;

	public Font txtFont;

	public dragBehavior myDragBeh;

	public closeBehavior myCloseBeh;

	public mainController mainController;

	public RectTransform windowHolderRT;

	public RectTransform dragPlane;

	private RectTransform myRT;
}
