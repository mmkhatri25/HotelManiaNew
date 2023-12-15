using System;
using UnityEngine;

namespace Anima2D
{
	[Serializable]
	public class IkSolver2DCCD : IkSolver2D
	{
		public int iterations = 10;

		public float damping = 0.8f;

		protected override void DoSolverUpdate()
		{
			if (!base.rootBone)
			{
				return;
			}
			for (int i = 0; i < base.solverPoses.Count; i++)
			{
				SolverPose solverPose = base.solverPoses[i];
				if (solverPose != null && (bool)solverPose.bone)
				{
					solverPose.solverRotation = solverPose.bone.transform.localRotation;
					solverPose.solverPosition = base.rootBone.transform.InverseTransformPoint(solverPose.bone.transform.position);
				}
			}
			Vector3 vector = base.rootBone.transform.InverseTransformPoint(base.solverPoses[base.solverPoses.Count - 1].bone.endPosition);
			Vector3 a = base.rootBone.transform.InverseTransformPoint(targetPosition);
			damping = Mathf.Clamp01(damping);
			float num = 1f - Mathf.Lerp(0f, 0.99f, damping);
			for (int j = 0; j < iterations; j++)
			{
				for (int num2 = base.solverPoses.Count - 1; num2 >= 0; num2--)
				{
					SolverPose solverPose2 = base.solverPoses[num2];
					Vector3 b = a - solverPose2.solverPosition;
					Vector3 a2 = vector - solverPose2.solverPosition;
					b.z = 0f;
					a2.z = 0f;
					Quaternion quaternion = Quaternion.AngleAxis(MathUtils.SignedAngle(a2, b, Vector3.forward) * num, Vector3.forward);
					solverPose2.solverRotation *= quaternion;
					Vector3 solverPosition = solverPose2.solverPosition;
					vector = RotatePositionFrom(vector, solverPosition, quaternion);
					for (int num3 = base.solverPoses.Count - 1; num3 > num2; num3--)
					{
						SolverPose solverPose3 = base.solverPoses[num3];
						solverPose3.solverPosition = RotatePositionFrom(solverPose3.solverPosition, solverPosition, quaternion);
					}
				}
			}
		}

		private Vector3 RotatePositionFrom(Vector3 position, Vector3 pivot, Quaternion rotation)
		{
			Vector3 point = position - pivot;
			point = rotation * point;
			return pivot + point;
		}
	}
}
