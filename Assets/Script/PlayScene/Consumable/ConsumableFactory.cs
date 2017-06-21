using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ConsumableFactory
    {
        internal static Consumable CreateConsumable(ConsumableData data, int level)
        {
            //if(data.type == Consumable.Type.ResearchTime)
            //    return new Consumable(data, Mathf.Min(Mathf.Max((level / 10), 1), 4)); // 1~4;
            if (data.type == Consumable.Type.Exp)
                data.icon.IconText = ConsumableManager.CalChestExp(level, data.value).To5String();
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
            foreach (ConsumableData one in list.Values)
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

            foreach (ConsumableData one in list.Values)
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
        private static int GetTotalDensity(int level, Dictionary<string, ConsumableData> list)
        {
            int result = 0;
            foreach (var one in list.Values)
            {
                if (one.AbsRate == 0 && level >= one.dropLevel)
                    result += one.Density;
            }
            return result;
        }

    }
}