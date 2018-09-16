using System;

namespace func_rocket
{
	public class ControlTask
	{
		static double position;

		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			var segment = new Vector(target.X - rocket.Location.X, target.Y - rocket.Location.Y);
			double checkAngle = Math.Abs(segment.Angle - rocket.Velocity.Angle);
			double checkDirection = Math.Abs(segment.Angle - rocket.Direction);

			if (checkAngle < 0.5 || checkDirection < 0.5)
			{
				position = (segment.Angle - rocket.Velocity.Angle + segment.Angle - rocket.Direction) * 1 / 2;
			}
			else
			{
				position = segment.Angle - rocket.Direction;
			}

			if (position < 0)
				return Turn.Left;

			if (position > 0) 
			{
				return Turn.Right;
			}
			{
				return Turn.None;
			}
		}
	}
}