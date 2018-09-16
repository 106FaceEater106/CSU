using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics standardPhysics = new Physics();

        public static IEnumerable<Level> CreateLevels()
        {
            yield return IsUp();
            yield return Zero();
            yield return IsHeavy();

            foreach (var parameter in HolesThere())
                yield return parameter;
        }
        
        static Level IsUp() => new Level("Up",
            new Rocket(new Vector(150, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(700, 500),
            (size, v) => new Vector(0, -300 / (300 + size.Height - v.Y)),
            standardPhysics);
        
        static Level Zero() => new Level("Zero",
                new Rocket(new Vector(150, 400), Vector.Zero, -0.5 * Math.PI),
                new Vector(600, 200),
                (size, v) => Vector.Zero, standardPhysics);

        static Level IsHeavy() => new Level("Heavy",
                new Rocket(new Vector(220, 520), Vector.Zero, -0.5 * Math.PI),
                new Vector(700, 500),
                (size, v) => new Vector(0, 0.9),
                standardPhysics);

        static IEnumerable<Level> HolesThere()
        {
            var rocket = new Rocket(new Vector(220, 520), Vector.Zero, -0.5 * Math.PI);
            var finishDot = new Vector(700, 500);
            var black = 0.5 * (finishDot + rocket.Location);
            Gravity blackAttr = (size, v) => {
                var dir = black - v;
                var d = dir.Length;
                return d * dir.Normalize() * 300 / (1+ d * d);
            };
            Gravity whiteAttr = (size, v) => {
                var comm = finishDot - v;
                var c = comm.Length;
                return c * comm.Normalize() * -140 / (1 + c * c);
            };
            Gravity summAttr = (size, v) =>
                (blackAttr(size, v) + whiteAttr(size, v)) * 0.5;
            yield return new Level("WhiteHole", rocket, finishDot, whiteAttr, standardPhysics);
            yield return new Level("BlackHole", rocket, finishDot, blackAttr, standardPhysics);
            yield return new Level("BlackAndWhite", rocket, finishDot, summAttr, standardPhysics);
        }
    }
}