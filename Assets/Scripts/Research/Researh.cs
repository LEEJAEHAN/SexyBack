using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Research
    {
        private ResearchData table;

        GameObject avatar;
        public ResearchView view;

        public Research(ResearchData data)
        {
            table = data;
            view = avatar.GetComponent<ResearchView>();
            InitAvatar();

        }

        private void InitAvatar()
        {
            //TODO : 프리펩만들기.
        }
    }
}