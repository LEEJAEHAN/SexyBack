namespace SexyBackPlayScene
{
    public class StatBonus
    {
        enum Attribute
        {
            DamageXByLevel,
            DamageX,
            AttackSpeed,
        };

        public string targetID;  // both hero and elemental
        Attribute type;
        int value;
        float floatvalue;


    }
}