using System.Xml;

internal static class EquipFactory
{
    public static Equipment CraftEquipment(string tableID)
    {
        Equipment newOne = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[tableID]);
        newOne.isLock = false;
        return newOne;
    }
    public static Equipment LoadEquipment(XmlNode equipNode)
    {
        string tableID = equipNode.Attributes["id"].Value;
        Equipment e = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[tableID]);
        e.grade = int.Parse(equipNode.Attributes["grade"].Value);
        e.exp = int.Parse(equipNode.Attributes["exp"].Value);
        e.evolution = int.Parse(equipNode.Attributes["evolution"].Value);
        e.skillLevel = int.Parse(equipNode.Attributes["skillLevel"].Value);
        e.isLock = bool.Parse(equipNode.Attributes["isLock"].Value);
        return e;
    }

}