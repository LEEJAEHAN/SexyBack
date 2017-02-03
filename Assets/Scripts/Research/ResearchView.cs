using UnityEngine;

namespace SexyBackPlayScene
{
    public class ResearchView : MonoBehaviour
    {
        public delegate void ResearchSelect_Event(string name);
        public event ResearchSelect_Event Action_ResearchSelect;

        public delegate void ResearchStart_Event(string name);
        public event ResearchStart_Event Action_ResearchStart; // select 될때마다 confirm 버튼이벤트를 ResearchView스크립트로 바인딩한다.




    }
}