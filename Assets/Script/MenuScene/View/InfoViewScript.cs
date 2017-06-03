using UnityEngine;

namespace SexyBackMenuScene
{
    class InfoViewScript : MonoBehaviour
    {

        private void Awake()
        {
            Singleton<TalentManager>.getInstance().BindInfoView(this);
        }
        private void Start()
        {
            transform.FindChild("Name").GetComponent<UILabel>().text = "내닉네임";
            transform.FindChild("Name/BaseStat").GetComponent<UILabel>().text =
                Singleton<PlayerStatus>.getInstance().GetBaseStat.ToStringBox();
            transform.FindChild("BonusStat/ScrollView/Attribute").GetComponent<UILabel>().text =
                Singleton<EquipmentManager>.getInstance().GetTotalBonus();

            transform.FindChild("Name/Class").GetComponent<UILabel>().text = string.Format("Lv.{0}\n{1}",
                Singleton<TalentManager>.getInstance().TotalLevel, Singleton<TalentManager>.getInstance().CurrentTalent.Name);
            transform.FindChild("ClassStat/ScrollView/Attribute").GetComponent<UILabel>().text =
                Singleton<TalentManager>.getInstance().GetTotalBonus();
        }

        public void RefreshTalent(int level, string job, string bonuses)
        {
            transform.FindChild("Name/Class").GetComponent<UILabel>().text = string.Format("Lv.{0}\n{1}", level, job);
            transform.FindChild("ClassStat/ScrollView/Attribute").GetComponent<UILabel>().text = bonuses;
        }

    }
}
