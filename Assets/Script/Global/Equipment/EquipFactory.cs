
internal static class EquipFactory
{


    public static Equipment CraftEquipment(string tableID)
    {
        //id를 풀을 만드는작업.

        Equipment newOne = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[tableID]);
        newOne.Lock = false;

        return newOne;
                
    }
}