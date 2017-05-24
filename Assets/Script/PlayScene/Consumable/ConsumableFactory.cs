using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ConsumableFactory
    {
        internal static Consumable CreateConsumable(ConsumableData data, int level)
        {
            if(data.type == Consumable.Type.ResearchTime)
                return new Consumable(data, Mathf.Min((level / 10), 4));
 
            return new Consumable(data);
        }
        internal static Consumable PickRandomConsumable(int level)
        {
            Consumable AbsResult = PickByAbs(level);
            if (AbsResult == null)
                return PickByRelative(level);
            return AbsResult;
        }
        private static Consumable PickByAbs(int level)
        {
            var list = Singleton<TableLoader>.getInstance().consumable;
            int rand = UnityEngine.Random.Range(0, 100);
            foreach (ConsumableData one in list)
            {
                if (one.AbsRate > 0 && level >= one.dropLevel)
                {
                    rand -= one.AbsRate;
                    if (rand < 0)
                        return CreateConsumable(one, level);
                }
            }
            return null;
        }
        private static Consumable PickByRelative(int level)
        {
            var list = Singleton<TableLoader>.getInstance().consumable;
            int sumDensity = GetTotalDensity(level, list);
            int rand = UnityEngine.Random.Range(0, sumDensity);

            foreach (ConsumableData one in list)
            {
                if (one.AbsRate == 0 && level >= one.dropLevel)
                {
                    rand -= one.Density;
                    if (rand < 0)
                        return CreateConsumable(one, level);
                }
            }
            return null;
        }
        private static int GetTotalDensity(int level, List<ConsumableData> list)
        {
            int result = 0;
            foreach (var one in list)
            {
                if (one.AbsRate == 0 && level >= one.dropLevel)
                    result += one.Density;
            }
            return result;
        }

    }
}