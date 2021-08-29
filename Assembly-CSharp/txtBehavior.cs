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
		float num = Mathf.Abs(this.txtField.GetComponent<RectTransform>().transform.localPosition.y) + strHeight + 10f;
		this.txtFieldText.rectTransform.sizeDelta = new Vector2(this.txtFieldText.rectTransform.sizeDelta.x, strHeight);
		this.txtField.GetComponent<RectTransform>().sizeDelta = new Vector2(this.txtField.GetComponent<RectTransform>().sizeDelta.x, strHeight);
		base.transform.localPosition = new Vector3(setX, setY, 0f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		this.myRT.sizeDelta = new Vector2(this.myRT.sizeDelta.x, num);
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
		TextGenerationSettings textGenerationSettings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		textGenerationSettings.textAnchor = 0;
		textGenerationSettings.generateOutOfBounds = true;
		textGenerationSettings.generationExtents = new Vector2(this.txtField.GetComponent<RectTransform>().sizeDelta.x, this.txtField.GetComponent<RectTransform>().sizeDelta.y);
		textGenerationSettings.pivot = Vector2.zero;
		textGenerationSettings.richText = false;
		textGenerationSettings.font = this.txtFont;
		textGenerationSettings.fontSize = 16;
		textGenerationSettings.fontStyle = 0;
		textGenerationSettings.lineSpacing = 1f;
		textGenerationSettings.scaleFactor = 1f;
		textGenerationSettings.verticalOverflow = 1;
		textGenerationSettings.horizontalOverflow = 0;
		return textGenerator.GetPreferredHeight(theString, textGenerationSettings);
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
