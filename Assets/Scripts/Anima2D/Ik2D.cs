using UnityEngine;

namespace Anima2D
{
	public abstract class Ik2D : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private Bone2D m_Target;

		[SerializeField]
		private bool m_Record;

		[SerializeField]
		private Transform m_TargetTransform;

		[SerializeField]
		private int m_NumBones;

		[SerializeField]
		private float m_Weight = 1f;

		[SerializeField]
		private bool m_RestoreDefaultPose = true;

		[SerializeField]
		private bool m_OrientChild = true;

		private Bone2D m_CachedTarget;

		public IkSolver2D solver => GetSolver();

		public bool record => m_Record;

		public Bone2D target
		{
			get
			{
				if ((bool)m_Target)
				{
					target = m_Target;
				}
				if ((bool)m_CachedTarget && m_TargetTransform != m_CachedTarget.transform)
				{
					m_CachedTarget = null;
				}
				if (!m_CachedTarget && (bool)m_TargetTransform)
				{
					m_CachedTarget = m_TargetTransform.GetComponent<Bone2D>();
				}
				return m_CachedTarget;
			}
			set
			{
				m_CachedTarget = value;
				m_TargetTransform = value.transform;
				if (!m_Target)
				{
					InitializeSolver();
				}
				m_Target = null;
			}
		}

		public int numBones
		{
			get
			{
				return ValidateNumBones(m_NumBones);
			}
			set
			{
				int num = ValidateNumBones(value);
				if (num != m_NumBones)
				{
					m_NumBones = num;
					InitializeSolver();
				}
			}
		}

		public float weight
		{
			get
			{
				return m_Weight;
			}
			set
			{
				m_Weight = value;
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

		public bool orientChild
		{
			get
			{
				return m_OrientChild;
			}
			set
			{
				m_OrientChild = value;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			if (base.enabled && (bool)target && numBones > 0)
			{
				Gizmos.DrawIcon(base.transform.position, "ikGoal");
			}
			else
			{
				Gizmos.DrawIcon(base.transform.position, "ikGoalDisabled");
			}
		}

		private void OnValidate()
		{
			Validate();
		}

		private void Start()
		{
			OnStart();
		}

		private void Update()
		{
			SetAttachedIK(this);
			OnUpdate();
		}

		private void LateUpdate()
		{
			OnLateUpdate();
			UpdateIK();
		}

		private void SetAttachedIK(Ik2D ik2D)
		{
			for (int i = 0; i < solver.solverPoses.Count; i++)
			{
				IkSolver2D.SolverPose solverPose = solver.solverPoses[i];
				if ((bool)solverPose.bone)
				{
					solverPose.bone.attachedIK = ik2D;
				}
			}
		}

		public void UpdateIK()
		{
			OnIkUpdate();
			solver.Update();
			if (orientChild && (bool)target.child)
			{
				target.child.transform.rotation = base.transform.rotation;
			}
		}

		protected virtual void OnIkUpdate()
		{
			solver.weight = weight;
			solver.targetPosition = base.transform.position;
			solver.restoreDefaultPose = restoreDefaultPose;
		}

		private void InitializeSolver()
		{
			Bone2D chainBoneByIndex = Bone2D.GetChainBoneByIndex(target, numBones - 1);
			SetAttachedIK(null);
			solver.Initialize(chainBoneByIndex, numBones);
		}

		protected virtual int ValidateNumBones(int numBones)
		{
			return numBones;
		}

		protected virtual void Validate()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnLateUpdate()
		{
		}

		protected abstract IkSolver2D GetSolver();
	}
}
