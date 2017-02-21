using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class GridItemIcon
    {
        public string IconName;
        public string SubIconName;
        public string SubIconText;

        public GridItemIcon(string iconname, string subicon)
        {
            IconName = iconname;

            if (subicon == null)
            {
                SubIconName = null;
                SubIconText = null;
                return;
            }
            else if (subicon[0] == '-')
                SubIconName = "IconSmall_02";
            else
                SubIconName = "IconSmall_01";

            SubIconText = subicon.Replace("-", "");
        }

        internal void Draw(GameObject gameObject)
        {
            gameObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            gameObject.GetComponent<UISprite>().spriteName = IconName;

            if (gameObject.transform.childCount == 0)
                return;

            GameObject subiconObject = gameObject.transform.FindChild("SubIcon").gameObject;
            if (SubIconName == null)
                subiconObject.SetActive(false);
            else
            {
                subiconObject.SetActive(true);
                subiconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconSmall", typeof(UIAtlas)) as UIAtlas;
                subiconObject.GetComponent<UISprite>().spriteName = SubIconName;
            }
            GameObject sublabelObject = gameObject.transform.FindChild("SubLabel").gameObject;
            if (SubIconText == null)
                sublabelObject.SetActive(false);
            else
            {
                sublabelObject.SetActive(true);
                sublabelObject.GetComponent<UILabel>().text = SubIconText;
            }
        }
    }

}