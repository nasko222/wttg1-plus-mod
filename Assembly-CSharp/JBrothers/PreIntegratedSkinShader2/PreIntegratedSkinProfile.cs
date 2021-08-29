using System;
using UnityEngine;

namespace JBrothers.PreIntegratedSkinShader2
{
	public class PreIntegratedSkinProfile : ScriptableObject
	{
		public void NormalizeOriginalWeights()
		{
			this.RecalculateDerived();
			this.gauss6_1.x = this._PSSProfileHigh_weighths1_var1.x;
			this.gauss6_1.y = this._PSSProfileHigh_weighths1_var1.y;
			this.gauss6_1.z = this._PSSProfileHigh_weighths1_var1.z;
			this.gauss6_2.x = this._PSSProfileHigh_weighths2_var2.x;
			this.gauss6_2.y = this._PSSProfileHigh_weighths2_var2.y;
			this.gauss6_2.z = this._PSSProfileHigh_weighths2_var2.z;
			this.gauss6_3.x = this._PSSProfileHigh_weighths3_var3.x;
			this.gauss6_3.y = this._PSSProfileHigh_weighths3_var3.y;
			this.gauss6_3.z = this._PSSProfileHigh_weighths3_var3.z;
			this.gauss6_4.x = this._PSSProfileHigh_weighths4_var4.x;
			this.gauss6_4.y = this._PSSProfileHigh_weighths4_var4.y;
			this.gauss6_4.z = this._PSSProfileHigh_weighths4_var4.z;
			this.gauss6_5.x = this._PSSProfileHigh_weighths5_var5.x;
			this.gauss6_5.y = this._PSSProfileHigh_weighths5_var5.y;
			this.gauss6_5.z = this._PSSProfileHigh_weighths5_var5.z;
			this.gauss6_6.x = this._PSSProfileHigh_weighths6_var6.x;
			this.gauss6_6.y = this._PSSProfileHigh_weighths6_var6.y;
			this.gauss6_6.z = this._PSSProfileHigh_weighths6_var6.z;
			this.needsRenormalize = false;
		}

		public void RecalculateDerived()
		{
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = this.gauss6_1;
			Vector3 vector3 = this.gauss6_2;
			Vector3 vector4 = this.gauss6_3;
			Vector3 vector5 = this.gauss6_4;
			Vector3 vector6 = this.gauss6_5;
			Vector3 vector7 = this.gauss6_6;
			vector += vector2;
			vector += vector3;
			vector += vector4;
			vector += vector5;
			vector += vector6;
			vector += vector7;
			vector2.x /= vector.x;
			vector2.y /= vector.y;
			vector2.z /= vector.z;
			vector3.x /= vector.x;
			vector3.y /= vector.y;
			vector3.z /= vector.z;
			vector4.x /= vector.x;
			vector4.y /= vector.y;
			vector4.z /= vector.z;
			vector5.x /= vector.x;
			vector5.y /= vector.y;
			vector5.z /= vector.z;
			vector6.x /= vector.x;
			vector6.y /= vector.y;
			vector6.z /= vector.z;
			vector7.x /= vector.x;
			vector7.y /= vector.y;
			vector7.z /= vector.z;
			Color color;
			color..ctor(vector2.x, vector2.y, vector2.z);
			float grayscale = color.grayscale;
			Color color2;
			color2..ctor(vector3.x, vector3.y, vector3.z);
			float grayscale2 = color2.grayscale;
			Color color3;
			color3..ctor(vector4.x, vector4.y, vector4.z);
			float grayscale3 = color3.grayscale;
			Color color4;
			color4..ctor(vector5.x, vector5.y, vector5.z);
			float grayscale4 = color4.grayscale;
			Color color5;
			color5..ctor(vector6.x, vector6.y, vector6.z);
			float grayscale5 = color5.grayscale;
			Color color6;
			color6..ctor(vector7.x, vector7.y, vector7.z);
			float grayscale6 = color6.grayscale;
			this._PSSProfileHigh_weighths1_var1 = new Vector4(vector2.x, vector2.y, vector2.z, this.gauss6_1.w);
			this._PSSProfileHigh_weighths2_var2 = new Vector4(vector3.x, vector3.y, vector3.z, this.gauss6_2.w);
			this._PSSProfileHigh_weighths3_var3 = new Vector4(vector4.x, vector4.y, vector4.z, this.gauss6_3.w);
			this._PSSProfileHigh_weighths4_var4 = new Vector4(vector5.x, vector5.y, vector5.z, this.gauss6_4.w);
			this._PSSProfileHigh_weighths5_var5 = new Vector4(vector6.x, vector6.y, vector6.z, this.gauss6_5.w);
			this._PSSProfileHigh_weighths6_var6 = new Vector4(vector7.x, vector7.y, vector7.z, this.gauss6_6.w);
			this._PSSProfileMedium_weighths1_var1 = new Vector4(vector2.x + vector3.x, vector2.y + vector3.y, vector2.z + vector3.z, (this.gauss6_1.w * grayscale + this.gauss6_2.w * grayscale2) / (grayscale + grayscale2));
			this._PSSProfileMedium_weighths2_var2 = new Vector4(vector4.x + vector5.x, vector4.y + vector5.y, vector4.z + vector5.z, (this.gauss6_3.w * grayscale3 + this.gauss6_4.w * grayscale4) / (grayscale3 + grayscale4));
			this._PSSProfileMedium_weighths3_var3 = new Vector4(vector6.x + vector7.x, vector6.y + vector7.y, vector6.z + vector7.z, (this.gauss6_5.w * grayscale5 + this.gauss6_6.w * grayscale6) / (grayscale5 + grayscale6));
			this._PSSProfileLow_weighths1_var1 = new Vector4(vector2.x + vector3.x + vector4.x, vector2.y + vector3.y + vector4.y, vector2.z + vector3.z + vector4.z, (this.gauss6_1.w * grayscale + this.gauss6_2.w * grayscale2 + this.gauss6_3.w * grayscale3) / (grayscale + grayscale2 + grayscale3));
			this._PSSProfileLow_weighths2_var2 = new Vector4(vector5.x + vector6.x + vector7.x, vector5.y + vector6.y + vector7.y, vector5.z + vector6.z + vector7.z, (this.gauss6_4.w * grayscale4 + this.gauss6_5.w * grayscale5 + this.gauss6_6.w * grayscale6) / (grayscale4 + grayscale5 + grayscale6));
			this._PSSProfileHigh_sqrtvar1234.x = Mathf.Sqrt(this._PSSProfileHigh_weighths1_var1.w);
			this._PSSProfileHigh_sqrtvar1234.y = Mathf.Sqrt(this._PSSProfileHigh_weighths2_var2.w);
			this._PSSProfileHigh_sqrtvar1234.z = Mathf.Sqrt(this._PSSProfileHigh_weighths3_var3.w);
			this._PSSProfileHigh_sqrtvar1234.w = Mathf.Sqrt(this._PSSProfileHigh_weighths4_var4.w);
			this._PSSProfileMedium_sqrtvar123.x = Mathf.Sqrt(this._PSSProfileMedium_weighths1_var1.w);
			this._PSSProfileMedium_sqrtvar123.y = Mathf.Sqrt(this._PSSProfileMedium_weighths2_var2.w);
			this._PSSProfileMedium_sqrtvar123.z = Mathf.Sqrt(this._PSSProfileMedium_weighths3_var3.w);
			this._PSSProfileLow_sqrtvar12.x = Mathf.Sqrt(this._PSSProfileLow_weighths1_var1.w);
			this._PSSProfileLow_sqrtvar12.y = Mathf.Sqrt(this._PSSProfileLow_weighths2_var2.w);
			this._PSSProfileHigh_transl123_sqrtvar5.w = Mathf.Sqrt(this._PSSProfileHigh_weighths5_var5.w);
			this._PSSProfileHigh_transl456_sqrtvar6.w = Mathf.Sqrt(this._PSSProfileHigh_weighths6_var6.w);
			float num = -1.442695f;
			this._PSSProfileHigh_transl123_sqrtvar5.x = num / this.gauss6_1.w;
			this._PSSProfileHigh_transl123_sqrtvar5.y = num / this.gauss6_2.w;
			this._PSSProfileHigh_transl123_sqrtvar5.z = num / this.gauss6_3.w;
			this._PSSProfileHigh_transl456_sqrtvar6.x = num / this.gauss6_4.w;
			this._PSSProfileHigh_transl456_sqrtvar6.y = num / this.gauss6_5.w;
			this._PSSProfileHigh_transl456_sqrtvar6.z = num / this.gauss6_6.w;
			this._PSSProfileMedium_transl123.x = num / this._PSSProfileMedium_weighths1_var1.w;
			this._PSSProfileMedium_transl123.y = num / this._PSSProfileMedium_weighths2_var2.w;
			this._PSSProfileMedium_transl123.z = num / this._PSSProfileMedium_weighths3_var3.w;
			Vector3 vector8;
			vector8.x = this.gauss6_1.w * vector2.x + this.gauss6_2.w * vector3.x + this.gauss6_3.w * vector4.x + this.gauss6_4.w * vector5.x + this.gauss6_5.w * vector6.x + this.gauss6_6.w * vector7.x;
			vector8.y = this.gauss6_1.w * vector2.y + this.gauss6_2.w * vector3.y + this.gauss6_3.w * vector4.y + this.gauss6_4.w * vector5.y + this.gauss6_5.w * vector6.y + this.gauss6_6.w * vector7.y;
			vector8.z = this.gauss6_1.w * vector2.z + this.gauss6_2.w * vector3.z + this.gauss6_3.w * vector4.z + this.gauss6_4.w * vector5.z + this.gauss6_5.w * vector6.z + this.gauss6_6.w * vector7.z;
			this._PSSProfileLow_transl.x = num / vector8.x;
			this._PSSProfileLow_transl.y = num / vector8.y;
			this._PSSProfileLow_transl.z = num / vector8.z;
			this.needsRecalcDerived = false;
		}

		public void ApplyProfile(Material material)
		{
			this.ApplyProfile(material, false);
		}

		public void ApplyProfile(Material material, bool noWarn)
		{
			if (this.needsRecalcDerived)
			{
				this.RecalculateDerived();
			}
			material.SetVector("_PSSProfileHigh_weighths1_var1", this._PSSProfileHigh_weighths1_var1);
			material.SetVector("_PSSProfileHigh_weighths2_var2", this._PSSProfileHigh_weighths2_var2);
			material.SetVector("_PSSProfileHigh_weighths3_var3", this._PSSProfileHigh_weighths3_var3);
			material.SetVector("_PSSProfileHigh_weighths4_var4", this._PSSProfileHigh_weighths4_var4);
			material.SetVector("_PSSProfileHigh_weighths5_var5", this._PSSProfileHigh_weighths5_var5);
			material.SetVector("_PSSProfileHigh_weighths6_var6", this._PSSProfileHigh_weighths6_var6);
			material.SetVector("_PSSProfileHigh_sqrtvar1234", this._PSSProfileHigh_sqrtvar1234);
			material.SetVector("_PSSProfileHigh_transl123_sqrtvar5", this._PSSProfileHigh_transl123_sqrtvar5);
			material.SetVector("_PSSProfileHigh_transl456_sqrtvar6", this._PSSProfileHigh_transl456_sqrtvar6);
			material.SetVector("_PSSProfileMedium_weighths1_var1", this._PSSProfileMedium_weighths1_var1);
			material.SetVector("_PSSProfileMedium_weighths2_var2", this._PSSProfileMedium_weighths2_var2);
			material.SetVector("_PSSProfileMedium_weighths3_var3", this._PSSProfileMedium_weighths3_var3);
			material.SetVector("_PSSProfileMedium_transl123", this._PSSProfileMedium_transl123);
			material.SetVector("_PSSProfileMedium_sqrtvar123", this._PSSProfileMedium_sqrtvar123);
			material.SetVector("_PSSProfileLow_weighths1_var1", this._PSSProfileLow_weighths1_var1);
			material.SetVector("_PSSProfileLow_weighths2_var2", this._PSSProfileLow_weighths2_var2);
			material.SetVector("_PSSProfileLow_sqrtvar12", this._PSSProfileLow_sqrtvar12);
			material.SetVector("_PSSProfileLow_transl", this._PSSProfileLow_transl);
		}

		public Vector4 gauss6_1;

		public Vector4 gauss6_2;

		public Vector4 gauss6_3;

		public Vector4 gauss6_4;

		public Vector4 gauss6_5;

		public Vector4 gauss6_6;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths1_var1;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths2_var2;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths3_var3;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths4_var4;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths5_var5;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths6_var6;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_sqrtvar1234;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_transl123_sqrtvar5;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_transl456_sqrtvar6;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_weighths1_var1;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_weighths2_var2;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_weighths3_var3;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_transl123;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_sqrtvar123;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_weighths1_var1;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_weighths2_var2;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_sqrtvar12;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_transl;

		[HideInInspector]
		public bool needsRenormalize = true;

		[HideInInspector]
		public bool needsRecalcDerived = true;

		[HideInInspector]
		public Texture2D referenceTexture;
	}
}
