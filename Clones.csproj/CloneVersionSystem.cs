using System;
using System.Collections.Generic;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private List<Dupe> clones = new List<Dupe>();

        public string Execute(string call)
        {
            var orders = call.Split();
            var counter = int.Parse(orders[1]) - 1;
            if (clones.Count < counter + 1)
                clones.Add(new Dupe { AdvancesTrack = new ScheduleStack(), BackTrack = new ScheduleStack() });
            return ExecuteCommand(orders, counter);
        }

        private string ExecuteCommand(string[] orders, int counter)
        {
            string report = null;
            switch (orders[0])
            {
                case "check":
                    report = clones[counter].Check();
                    break;
                case "clone":
                    clones.Add(clones[counter].MakeCopy());
                    break;
                case "rollback":
                    clones[counter].RollBack();
                    break;
                case "learn":
                    clones[counter].Learn(int.Parse(orders[2]));
                    break;
                case "relearn":
                    clones[counter].Relearn();
                    break;
            }
            return report;
        }
    }
    
    public class ScheduleStack
    {
        private bool IsBlank() => stackList.Count == 0;

        List<int> stackList = new List<int>();
        
        public int Pop()
        {
            if (IsBlank()) throw new InvalidOperationException();
            var result = stackList[stackList.Count - 1];
            stackList.RemoveAt(stackList.Count - 1);
            return result;
        }
        
        public void Push(int counter)
        {
            stackList.Add(counter);
        }

        public string Peek()
        {
            return IsBlank() ? null : stackList[stackList.Count - 1].ToString();
        }

        public ScheduleStack Copy()
        {
            return new ScheduleStack {stackList = new List<int>(stackList)};
        }
    }

    public class Dupe
    {
        public ScheduleStack BackTrack;
        public ScheduleStack AdvancesTrack;
        private bool duplicate;

        public string Check()
        {
            return AdvancesTrack.Peek() == null
                ? "basic"
                : AdvancesTrack.Peek();
        }
        
        public Dupe MakeCopy()
        {
            var cln = new Dupe
            {
                AdvancesTrack = AdvancesTrack,
                BackTrack = BackTrack,
                duplicate = true
            };
            duplicate = true;
            return cln;
        }

        public void RollBack()
        {
            if (duplicate)
            {
                AdvancesTrack = AdvancesTrack.Copy();
                BackTrack = BackTrack.Copy();
                duplicate = false;
            }
            BackTrack.Push(AdvancesTrack.Pop());
        }

        public void Learn(int track)
        {
            if (duplicate)
            {
                AdvancesTrack = AdvancesTrack.Copy();
                duplicate = false;
            }
            BackTrack = new ScheduleStack();
            AdvancesTrack.Push(track);
        }
        
        public void Relearn()
        {
            if (duplicate)
            {
                AdvancesTrack = AdvancesTrack.Copy();
                BackTrack = BackTrack.Copy();
                duplicate = false;
            }
            AdvancesTrack.Push(BackTrack.Pop());
        }
    }
}