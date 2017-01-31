namespace SexyBackPlayScene
{
    internal class Stage // wavemanager
    {
        int GoalFloor;
        private BigInteger exp = new BigInteger();

        public delegate void ExpChange_Event(BigInteger exp);
        public event ExpChange_Event Action_ExpChange;

        public void Init()
        {
        }

        public void Start(StageData stagedata) //  // stagebuilder
        {
            GoalFloor = stagedata.GoalFloor;
            ExpGain(stagedata.InitExp);
            

            // test command
            Singleton<HeroManager>.getInstance().CreateHero();
            Singleton<MonsterManager>.getInstance().CreateFirstMonster();
            Singleton<ElementalManager>.getInstance().SummonNewElemental("fireball");
            Singleton<ElementalManager>.getInstance().SummonNewElemental("snowball");
            Singleton<ElementalManager>.getInstance().SummonNewElemental("magmaball");

        }

        public void Update()
        {

        }

        public void ExpGain(BigInteger e)
        {
            exp += e;
            Action_ExpChange(exp);
        }
        internal bool ExpUse(BigInteger e)
        {
            bool result;

            if (exp - e < 0)
                result = false;
            else
            {
                exp -= e;
                result = true;
            }

            Action_ExpChange(exp);
            return result;
        }


    }
}