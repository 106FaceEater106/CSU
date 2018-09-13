using NUnit.Framework;
using System;
using System.Collections.Generic;
 
 namespace Monobilliards
 {
 	public class Monobilliards : IMonobilliards
 	{
 		public bool IsCheater(IList<int> inspectedBalls)
 		{
 			var realBall = new Stack<int>();
 			int count = 0;
 			
 			if (inspectedBalls.Count == 0)
 			{
 				return false;
 			}
 
 			for (int i = 0; i < inspectedBalls.Count; i++)
 			{
 				realBall.Push(i + 1);
 
 				while (realBall.Count != 0 && realBall.Peek() == inspectedBalls[count])
 				{
 					realBall.Pop();
 					count++;
 					if (count == inspectedBalls.Count) return false;
 				}
 			}
 
 			return true;
 		}
 	}
 }