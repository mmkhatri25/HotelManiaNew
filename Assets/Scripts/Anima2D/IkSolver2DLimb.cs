using System;
using UnityEngine;

namespace Anima2D
{
	[Serializable]
	public class IkSolver2DLimb : IkSolver2D
	{
		public bool flip;

		protected override void DoSolverUpdate()
		{
			if ((bool)base.rootBone && base.solverPoses.Count == 2)
			{
				SolverPose solverPose = base.solverPoses[0];
				SolverPose solverPose2 = base.solverPoses[1];
				Vector3 vector = targetPosition - base.rootBone.transform.position;
				vector.z = 0f;
				float magnitude = vector.magnitude;
				float num = 0f;
				float num2 = 0f;
				float sqrMagnitude = vector.sqrMagnitude;
				float num3 = solverPose.bone.length * solverPose.bone.length;
				float num4 = solverPose2.bone.length * solverPose2.bone.length;
				float num5 = (sqrMagnitude + num3 - num4) / (2f * solverPose.bone.length * magnitude);
				float num6 = (sqrMagnitude - num3 - num4) / (2f * solverPose.bone.length * solverPose2.bone.length);
				if (num5 >= -1f && num5 <= 1f && num6 >= -1f && num6 <= 1f)
				{
					num = Mathf.Acos(num5) * 57.29578f;
					num2 = Mathf.Acos(num6) * 57.29578f;
				}
				float num7 = flip ? (-1f) : 1f;
				Vector3 vector2 = Vector3.ProjectOnPlane(targetPosition - base.rootBone.transform.position, base.rootBone.transform.forward);
				if ((bool)base.rootBone.transform.parent)
				{
					vector2 = base.rootBone.transform.parent.InverseTransformDirection(vector2);
				}
				float num8 = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
				solverPose.solverRotation = Quaternion.Euler(0f, 0f, num8 - num7 * num);
				solverPose2.solverRotation = Quaternion.Euler(0f, 0f, num7 * num2);
			}
		}
	}
}
