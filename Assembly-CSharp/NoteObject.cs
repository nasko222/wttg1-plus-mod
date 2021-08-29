using System;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour
{
	public void buildMe(float setX = 0f, float setY = 0f, string noteDay = "", string noteTime = "", string noteText = "")
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.noteDate.text = string.Concat(new string[]
		{
			"Day ",
			noteDay,
			" @ ",
			noteTime,
			":"
		});
		this.noteInputField.text = noteText;
		float preferredHeight = LayoutUtility.GetPreferredHeight(this.noteTextField.rectTransform);
		this.noteInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(this.noteInputField.GetComponent<RectTransform>().sizeDelta.x, preferredHeight);
		this.myRT.sizeDelta = new Vector2(this.myRT.sizeDelta.x, preferredHeight);
		base.transform.localPosition = new Vector3(setX, setY, 0f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public Text noteDate;

	public InputField noteInputField;

	public Text noteTextField;

	private RectTransform myRT;
}
