using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTTPPusher : MonoBehaviour
{
	public void connectionCheck(Action connectionGoodAction, Action connectionBadAction)
	{
		base.StartCoroutine(this.checkForConnection(delegate(bool isConnected)
		{
			if (isConnected)
			{
				connectionGoodAction.DynamicInvoke(new object[0]);
			}
			else
			{
				connectionBadAction.DynamicInvoke(new object[0]);
			}
		}));
	}

	public void submitPostText(string postURL, Dictionary<string, string> postData, Action<bool, WTTGJSONData> returnAction)
	{
		base.StartCoroutine(this.postTextData(postURL, postData, delegate(bool isGood, WTTGJSONData respData)
		{
			if (isGood)
			{
				returnAction(true, respData);
			}
			else
			{
				returnAction(false, respData);
			}
		}));
	}

	private IEnumerator checkForConnection(Action<bool> action)
	{
		WWW www = new WWW("http://www.google.com");
		yield return www;
		if (www.error != null)
		{
			action(false);
		}
		else
		{
			action(true);
		}
		yield break;
	}

	private IEnumerator postTextData(string thePostURL, Dictionary<string, string> thePostData, Action<bool, WTTGJSONData> action)
	{
		WWWForm wForm = new WWWForm();
		bool passJSON = true;
		foreach (KeyValuePair<string, string> keyValuePair in thePostData)
		{
			wForm.AddField(keyValuePair.Key, keyValuePair.Value);
		}
		WWW pRequest = new WWW(thePostURL, wForm);
		yield return pRequest;
		if (pRequest.error != null)
		{
			action(false, new WTTGJSONData
			{
				pass = false,
				data = "0",
				message = pRequest.error
			});
		}
		else
		{
			WTTGJSONData wttgjsondata = new WTTGJSONData();
			try
			{
				wttgjsondata = JsonUtility.FromJson<WTTGJSONData>(pRequest.text);
			}
			catch
			{
				passJSON = false;
			}
			if (passJSON)
			{
				action(true, wttgjsondata);
			}
			else
			{
				wttgjsondata.pass = false;
				wttgjsondata.data = "0";
				wttgjsondata.message = "Internal error has occurred";
				action(false, wttgjsondata);
			}
		}
		yield break;
	}

	private void Awake()
	{
		GameManager.SetHTTPPusher(this);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
