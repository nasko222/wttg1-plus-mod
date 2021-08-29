using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class txtIconBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void buildMe(float setX = 0f, float setY = 0f, string setTxtTitle = "", string setTxtText = "")
	{
		this.txtTile = setTxtTitle;
		this.txtText = setTxtText;
		this.txtIconTitle.text = setTxtTitle;
		this.clickCount = 0f;
		this.myTimeStamp = Time.time;
		base.transform.localPosition = new Vector3(setX, setY, 0f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
	}

	private void buildTxtDoc()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.txtObject);
		gameObject.transform.SetParent(this.windowHolderRT);
		gameObject.GetComponent<txtBehavior>().mainController = this.mainController;
		gameObject.GetComponent<txtBehavior>().windowHolderRT = this.windowHolderRT;
		gameObject.GetComponent<txtBehavior>().dragPlane = this.dragPlane;
		gameObject.GetComponent<txtBehavior>().buildMe((float)Screen.width, (float)Screen.height, this.txtTile, this.txtText);
		Vector2 myWH = gameObject.GetComponent<txtBehavior>().getMyWH();
		float setX = UnityEngine.Random.Range(20f, (float)Screen.width - myWH.x - 20f);
		float setY = -UnityEngine.Random.Range(60f, (float)Screen.height - myWH.y - 20f);
		gameObject.GetComponent<txtBehavior>().rePOSme(setX, setY);
	}

	private void Start()
	{
		this.clickCount = 0f;
		this.myTimeStamp = Time.time;
	}

	private void Update()
	{
		if (Time.time - this.myTimeStamp >= 1f)
		{
			this.clickCount = 0f;
			this.myTimeStamp = Time.time;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.clickCount >= 1f)
		{
			this.buildTxtDoc();
			this.clickCount = 0f;
		}
		this.clickCount += 1f;
	}

	public GameObject txtObject;

	public Text txtIconTitle;

	public mainController mainController;

	public RectTransform windowHolderRT;

	public RectTransform dragPlane;

	private float clickCount;

	private float myTimeStamp;

	private string txtTile = string.Empty;

	private string txtText = string.Empty;
}
