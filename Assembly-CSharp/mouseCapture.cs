using System;
using UnityEngine;

[Serializable]
public class mouseCapture
{
	public void Init(Transform character, Transform camera)
	{
		this.myCharacterTargetRot = character.localRotation;
		this.myCameraTargetRot = camera.localRotation;
		this.setXSen = this.XSensitivity;
		this.setYSen = this.YSensitivity;
		this.setMinVertX = this.MinVertX;
		this.setMaxVertX = this.MaxVertX;
		this.setMinHorzY = this.MinHorzY;
		this.setMaxHorzY = this.MaxHorzY;
	}

	public void Reset(Transform character, Transform camera)
	{
		this.myCharacterTargetRot = character.localRotation;
		this.myCameraTargetRot = camera.localRotation;
		this.MinHorzY = this.myCharacterTargetRot.eulerAngles.y - this.setMinHorzY;
		this.MaxHorzY = this.myCharacterTargetRot.eulerAngles.y + this.setMaxHorzY;
		this.MinVertX = this.myCameraTargetRot.eulerAngles.x - this.setMinVertX;
		this.MaxVertX = this.myCameraTargetRot.eulerAngles.x + this.setMaxVertX;
	}

	public void LookRotation(Transform character, Transform camera)
	{
		float num = Input.GetAxis("Mouse Y") * this.XSensitivity;
		float num2 = Input.GetAxis("Mouse X") * this.YSensitivity;
		this.myCharacterTargetRot *= Quaternion.Euler(0f, num2, 0f);
		this.myCameraTargetRot *= Quaternion.Euler(-num, 0f, 0f);
		if (this.clampVerticalRotation)
		{
			this.myCameraTargetRot = this.ClampRotationAroundXAxis(this.myCameraTargetRot);
		}
		if (this.clampHorzRotation)
		{
			this.myCharacterTargetRot = this.ClampRotationAroundYAxis(this.myCharacterTargetRot);
		}
		if (this.smooth)
		{
			character.localRotation = Quaternion.Slerp(character.localRotation, this.myCharacterTargetRot, this.smoothTime * Time.deltaTime);
			camera.localRotation = Quaternion.Slerp(camera.localRotation, this.myCameraTargetRot, this.smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = this.myCharacterTargetRot;
			camera.localRotation = this.myCameraTargetRot;
		}
	}

	public void LookVerticalRotation(Transform camera)
	{
		float num = Input.GetAxis("Mouse Y") * this.XSensitivity;
		this.myCameraTargetRot *= Quaternion.Euler(-num, 0f, 0f);
		if (this.clampVerticalRotation)
		{
			this.myCameraTargetRot = this.ClampRotationAroundXAxis(this.myCameraTargetRot);
		}
		if (this.smooth)
		{
			camera.localRotation = Quaternion.Slerp(camera.localRotation, this.myCameraTargetRot, this.smoothTime * Time.deltaTime);
		}
		else
		{
			camera.localRotation = this.myCameraTargetRot;
		}
	}

	public void LookCameraRotation(Transform camera)
	{
		float num = Input.GetAxis("Mouse Y") * this.XSensitivity;
		float num2 = Input.GetAxis("Mouse X") * this.YSensitivity;
		this.myCameraTargetRot *= Quaternion.Euler(-num, num2, 0f);
		this.myCameraTargetRot = Quaternion.Euler(this.myCameraTargetRot.eulerAngles.x, this.myCameraTargetRot.eulerAngles.y, 0f);
		if (this.clampVerticalRotation)
		{
			this.myCameraTargetRot = this.ClampRotationAroundXAxis(this.myCameraTargetRot);
		}
		if (this.clampHorzRotation)
		{
			this.myCameraTargetRot = this.ClampRotationAroundYAxis(this.myCameraTargetRot);
		}
		if (this.smooth)
		{
			camera.localRotation = Quaternion.Slerp(camera.localRotation, this.myCameraTargetRot, this.smoothTime * Time.deltaTime);
		}
		else
		{
			camera.localRotation = this.myCameraTargetRot;
		}
	}

	public void ForceRotation(Transform character, Transform camera, float forceRate)
	{
		float num = 1f * forceRate;
		float num2 = 1f * forceRate;
		this.myCharacterTargetRot *= Quaternion.Euler(0f, num2, 0f);
		this.myCameraTargetRot *= Quaternion.Euler(-num, 0f, 0f);
		character.localRotation = this.myCharacterTargetRot;
		camera.localRotation = this.myCameraTargetRot;
	}

	public void ForceCameraRotationY(Transform camera, float forceRate)
	{
		float num = 1f * forceRate;
		this.myCameraTargetRot *= Quaternion.Euler(0f, num, 0f);
		camera.localRotation = this.myCameraTargetRot;
	}

	public Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float num = 114.59156f * Mathf.Atan(q.x);
		num = Mathf.Clamp(num, this.MinVertX, this.MaxVertX);
		q.x = Mathf.Tan(0.008726646f * num);
		return q;
	}

	public Quaternion ClampRotationAroundXAxis(Quaternion q, float setMinX, float setMaxX)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float num = 114.59156f * Mathf.Atan(q.x);
		num = Mathf.Clamp(num, setMinX, setMaxX);
		q.x = Mathf.Tan(0.008726646f * num);
		return q;
	}

	public Quaternion ClampRotationAroundYAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float num = 114.59156f * Mathf.Atan(q.y);
		num = Mathf.Clamp(num, this.MinHorzY, this.MaxHorzY);
		q.y = Mathf.Tan(0.008726646f * num);
		return q;
	}

	public Quaternion ClampRotationAroundYAxis(Quaternion q, float setMinY, float setMaxY)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float num = 114.59156f * Mathf.Atan(q.y);
		num = Mathf.Clamp(num, setMinY, setMaxY);
		q.y = Mathf.Tan(0.008726646f * num);
		return q;
	}

	public float XSensitivity = 2f;

	public float YSensitivity = 2f;

	public bool clampVerticalRotation = true;

	public bool clampHorzRotation;

	public float MinVertX = -90f;

	public float MaxVertX = 90f;

	public float MinHorzY = -180f;

	public float MaxHorzY = 180f;

	public bool smooth;

	public float smoothTime = 5f;

	public float setXSen;

	public float setYSen;

	public float setMinVertX;

	public float setMaxVertX;

	public float setMinHorzY;

	public float setMaxHorzY;

	public Quaternion myCharacterTargetRot;

	public Quaternion myCameraTargetRot;
}
