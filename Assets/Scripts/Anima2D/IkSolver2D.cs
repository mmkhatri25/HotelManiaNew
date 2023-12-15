using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Anima2D
{
	[Serializable]
	public abstract class IkSolver2D
	{
		[Serializable]
		public class SolverPose
		{
			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("bone")]
			private Bone2D m_Bone;

			[SerializeField]
			private Transform m_BoneTransform;

			private Bone2D m_CachedBone;

			public Vector3 solverPosition = Vector3.zero;

			public Quaternion solverRotation = Quaternion.identity;

			public Quaternion defaultLocalRotation = Quaternion.identity;

			public Bone2D bone
			{
				get
				{
					if ((bool)m_Bone)
					{
						bone = m_Bone;
					}
					if ((bool)m_CachedBone && m_BoneTransform != m_CachedBone.transform)
					{
						m_CachedBone = null;
					}
					if (!m_CachedBone && (bool)m_BoneTransform)
					{
						m_CachedBone = m_BoneTransform.GetComponent<Bone2D>();
					}
					return m_CachedBone;
				}
				set
				{
					m_Bone = null;
					m_CachedBone = value;
					m_BoneTransform = null;
					if ((bool)value)
					{
						m_BoneTransform = m_CachedBone.transform;
					}
				}
			}

			public void StoreDefaultPose()
			{
				defaultLocalRotation = bone.transform.localRotation;
			}

			public void RestoreDefaultPose()
			{
				if ((bool)bone)
				{
					bone.transform.localRotation = defaultLocalRotation;
				}
			}
		}

		[SerializeField]
		[HideInInspector]
		private Bone2D m_RootBone;

		[SerializeField]
		private Transform m_RootBoneTransform;

		[SerializeField]
		private List<SolverPose> m_SolverPoses = new List<SolverPose>();

		[SerializeField]
		private float m_Weight = 1f;

		[SerializeField]
		private bool m_RestoreDefaultPose = true;

		private Bone2D m_CachedRootBone;

		public Vector3 targetPosition;

		public Bone2D rootBone
		{
			get
			{
				if ((bool)m_RootBone)
				{
					rootBone = m_RootBone;
				}
				if ((bool)m_CachedRootBone && m_RootBoneTransform != m_CachedRootBone.transform)
				{
					m_CachedRootBone = null;
				}
				if (!m_CachedRootBone && (bool)m_RootBoneTransform)
				{
					m_CachedRootBone = m_RootBoneTransform.GetComponent<Bone2D>();
				}
				return m_CachedRootBone;
			}
			private set
			{
				m_RootBone = null;
				m_CachedRootBone = value;
				m_RootBoneTransform = null;
				if ((bool)value)
				{
					m_RootBoneTransform = value.transform;
				}
			}
		}

		public List<SolverPose> solverPoses => m_SolverPoses;

		public float weight
		{
			get
			{
				return m_Weight;
			}
			set
			{
				m_Weight = Mathf.Clamp01(value);
			}
		}

		public bool restoreDefaultPose
		{
			get
			{
				return m_RestoreDefaultPose;
			}
			set
			{
				m_RestoreDefaultPose = value;
			}
		}

		public void Initialize(Bone2D _rootBone, int numChilds)
		{
			rootBone = _rootBone;
			Bone2D bone2D = rootBone;
			solverPoses.Clear();
			for (int i = 0; i < numChilds; i++)
			{
				if ((bool)bone2D)
				{
					SolverPose solverPose = new SolverPose();
					solverPose.bone = bone2D;
					solverPoses.Add(solverPose);
					bone2D = bone2D.child;
				}
			}
			StoreDefaultPoses();
		}

		public void Update()
		{
			if (weight > 0f)
			{
				if (restoreDefaultPose)
				{
					RestoreDefaultPoses();
				}
				DoSolverUpdate();
				UpdateBones();
			}
		}

		public void StoreDefaultPoses()
		{
			for (int i = 0; i < solverPoses.Count; i++)
			{
				solverPoses[i]?.StoreDefaultPose();
			}
		}

		public void RestoreDefaultPoses()
		{
			for (int i = 0; i < solverPoses.Count; i++)
			{
				solverPoses[i]?.RestoreDefaultPose();
			}
		}

		private void UpdateBones()
		{
			for (int i = 0; i < solverPoses.Count; i++)
			{
				SolverPose solverPose = solverPoses[i];
				if (solverPose != null && (bool)solverPose.bone)
				{
					if (weight == 1f)
					{
						solverPose.bone.transform.localRotation = solverPose.solverRotation;
					}
					else
					{
						solverPose.bone.transform.localRotation = Quaternion.Slerp(solverPose.bone.transform.localRotation, solverPose.solverRotation, weight);
					}
				}
			}
		}

		protected abstract void DoSolverUpdate();
	}
}
