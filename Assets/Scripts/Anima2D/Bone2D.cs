using UnityEngine;
using UnityEngine.Serialization;

namespace Anima2D
{
	public class Bone2D : MonoBehaviour
	{
		[SerializeField]
		[FormerlySerializedAs("color")]
		private Color m_Color = Color.white;

		[SerializeField]
		[FormerlySerializedAs("mLength")]
		private float m_Length = 1f;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("mChild")]
		private Bone2D m_Child;

		[SerializeField]
		[HideInInspector]
		private Transform m_ChildTransform;

		[SerializeField]
		private Ik2D m_AttachedIK;

		private Bone2D m_CachedChild;

		private Bone2D mParentBone;

		public Ik2D attachedIK
		{
			get
			{
				return m_AttachedIK;
			}
			set
			{
				m_AttachedIK = value;
			}
		}

		public Color color
		{
			get
			{
				return m_Color;
			}
			set
			{
				m_Color = value;
			}
		}

		public Bone2D child
		{
			get
			{
				if ((bool)m_Child)
				{
					child = m_Child;
				}
				if ((bool)m_CachedChild && m_ChildTransform != m_CachedChild.transform)
				{
					m_CachedChild = null;
				}
				if ((bool)m_ChildTransform && m_ChildTransform.parent != base.transform)
				{
					m_CachedChild = null;
				}
				if (!m_CachedChild && (bool)m_ChildTransform && m_ChildTransform.parent == base.transform)
				{
					m_CachedChild = m_ChildTransform.GetComponent<Bone2D>();
				}
				return m_CachedChild;
			}
			set
			{
				m_Child = null;
				m_CachedChild = value;
				m_ChildTransform = m_CachedChild.transform;
			}
		}

		public Vector3 localEndPosition => Vector3.right * localLength;

		public Vector3 endPosition => base.transform.TransformPoint(localEndPosition);

		public float localLength
		{
			get
			{
				if ((bool)child)
				{
					Vector3 vector = base.transform.InverseTransformPoint(child.transform.position);
					m_Length = Mathf.Clamp(vector.x, 0f, vector.x);
				}
				return m_Length;
			}
			set
			{
				if (!child)
				{
					m_Length = value;
				}
			}
		}

		public float length => base.transform.TransformVector(localEndPosition).magnitude;

		public Bone2D parentBone
		{
			get
			{
				Transform parent = base.transform.parent;
				if (!mParentBone)
				{
					if ((bool)parent)
					{
						mParentBone = parent.GetComponent<Bone2D>();
					}
				}
				else if (parent != mParentBone.transform)
				{
					if ((bool)parent)
					{
						mParentBone = parent.GetComponent<Bone2D>();
					}
					else
					{
						mParentBone = null;
					}
				}
				return mParentBone;
			}
		}

		public Bone2D linkedParentBone
		{
			get
			{
				if ((bool)parentBone && parentBone.child == this)
				{
					return parentBone;
				}
				return null;
			}
		}

		public Bone2D root
		{
			get
			{
				Bone2D bone2D = this;
				while ((bool)bone2D.parentBone)
				{
					bone2D = bone2D.parentBone;
				}
				return bone2D;
			}
		}

		public Bone2D chainRoot
		{
			get
			{
				Bone2D bone2D = this;
				while ((bool)bone2D.parentBone && bone2D.parentBone.child == bone2D)
				{
					bone2D = bone2D.parentBone;
				}
				return bone2D;
			}
		}

		public int chainLength
		{
			get
			{
				Bone2D bone2D = this;
				int num = 1;
				while ((bool)bone2D.parentBone && bone2D.parentBone.child == bone2D)
				{
					num++;
					bone2D = bone2D.parentBone;
				}
				return num;
			}
		}

		public static Bone2D GetChainBoneByIndex(Bone2D chainTip, int index)
		{
			if (!chainTip)
			{
				return null;
			}
			Bone2D bone2D = chainTip;
			int chainLength = bone2D.chainLength;
			for (int i = 0; i < chainLength; i++)
			{
				if (!bone2D)
				{
					break;
				}
				if (i == index)
				{
					return bone2D;
				}
				if ((bool)bone2D.linkedParentBone)
				{
					bone2D = bone2D.parentBone;
					continue;
				}
				return null;
			}
			return null;
		}
	}
}
