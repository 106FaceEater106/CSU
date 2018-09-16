using System.Drawing;

namespace func_rocket
{
	public class ForcesTask
	{
		public static RocketForce GetThrustForce(double forceValue)
		{
			return r => new Vector(forceValue, 0).Rotate(r.Direction);
		}

		public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize)
		{
			return r => gravity(spaceSize, r.Location);
		}

		public static RocketForce Sum(params RocketForce[] forces)
		{
			return r =>
			{
				Vector roll = Vector.Zero;
				foreach (var frc in forces)
					roll = frc(r) + roll;
				return roll;
			};
		}
	}
}