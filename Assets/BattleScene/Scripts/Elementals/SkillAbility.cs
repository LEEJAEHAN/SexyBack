namespace SexyBackPlayScene
{
    internal class SkillAbility
    {
        // 맞았을때 적용할것들

        internal string Type; //  "instancedmg", "buff", "dotdmg", , ;
        internal int DamageXH = 300;
        internal readonly int DamageXHPerLV = 20;
        internal int Duration = 30;
        internal string buffID = "동상";

    }
}