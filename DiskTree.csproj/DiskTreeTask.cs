using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree
{
    public class DiskTreeTask
    {
        public static List<string> Solve(List<string> date)
        {
            var root = new Core("");
            foreach (var name in date)
            {
                var way = name.Split('\\');
                var unit = root;
                foreach (var item in way)
                    unit = unit.GetVector(item);
            }

            return root.IsOutcoming(-1, new List<string>());
        }
        
        public class Core
        {
            public string Parition;
            public Dictionary<string, Core> Units = new Dictionary<string, Core>();

            public Core(string parition)
            {
                Parition = parition;
            }

            public Core GetVector(string subCore)
            {
                return Units.TryGetValue(subCore, out Core unit) 
                    ? unit : Units[subCore] = new Core(subCore);
            }

            public List<string> IsOutcoming(int a, List<string> inventory)
            {
                if (a >= 0)
                    inventory.Add(new string(' ', a) + Parition);
                a++;

                foreach (var heir in Units.Values.OrderBy(core => core.Parition, 
                                                           StringComparer.Ordinal))
                    inventory = heir.IsOutcoming(a, inventory);
                return inventory;
            }
        }
    }
}