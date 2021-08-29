using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class magicSlinger
{
	public string md5It(string hash)
	{
		MD5 md = MD5.Create();
		byte[] array = md.ComputeHash(Encoding.Default.GetBytes(hash));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public Vector2 getXYByPerc(float xPer, float yPer)
	{
		return new Vector2((float)Screen.width * xPer, (float)Screen.height * yPer);
	}

	public float getScreenWidthPXByPerc(float per)
	{
		return (float)Screen.width * per;
	}

	public float getScreenHeightPXByPerc(float per)
	{
		return (float)Screen.height * per;
	}

	public float getPercOfSize(float size1, float size2)
	{
		return size1 / size2;
	}

	public bool inRange(float checkValue, float minValue, float maxValue)
	{
		return checkValue >= minValue && checkValue <= maxValue;
	}
}
