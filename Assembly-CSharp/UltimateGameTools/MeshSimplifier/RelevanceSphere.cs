using System;
using UnityEngine;

namespace UltimateGameTools.MeshSimplifier
{
	[Serializable]
	public class RelevanceSphere
	{
		public RelevanceSphere()
		{
			this.m_v3Scale = Vector3.one;
		}

		public void SetDefault(Transform target, float fRelevance)
		{
			this.m_bExpanded = true;
			this.m_v3Position = target.position + Vector3.up;
			this.m_v3Rotation = target.rotation.eulerAngles;
			this.m_v3Scale = Vector3.one;
			this.m_fRelevance = fRelevance;
		}

		public bool m_bExpanded;

		public Vector3 m_v3Position;

		public Vector3 m_v3Rotation;

		public Vector3 m_v3Scale;

		public float m_fRelevance;
	}
}
