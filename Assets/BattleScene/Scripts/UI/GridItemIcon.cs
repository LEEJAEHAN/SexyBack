using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public struct GridItemIcon : IDisposable
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
        public void Dispose()
        {
            IconName = null;
            SubIconName = null;
            SubIconText = null;
        }

        internal static void Draw(GridItemIcon icondata, GameObject iconGameObject)
        {
            iconGameObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            iconGameObject.GetComponent<UISprite>().spriteName = icondata.IconName;

            if (iconGameObject.transform.childCount == 0)
                return;

            GameObject subiconObject = iconGameObject.transform.FindChild("SubIcon").gameObject;
            if (icondata.SubIconName == null)
                subiconObject.SetActive(false);
            else
            {
                subiconObject.SetActive(true);
                subiconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconSmall", typeof(UIAtlas)) as UIAtlas;
                subiconObject.GetComponent<UISprite>().spriteName = icondata.SubIconName;
            }
            GameObject sublabelObject = iconGameObject.transform.FindChild("SubLabel").gameObject;
            if (icondata.SubIconText == null)
                sublabelObject.SetActive(false);
            else
            {
                sublabelObject.SetActive(true);
                sublabelObject.GetComponent<UILabel>().text = icondata.SubIconText;
            }
        }
    }

}