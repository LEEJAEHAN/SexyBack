using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public struct GridItemIcon : IDisposable
    {
        public string IconName;
        public string IconText;
        public string SubIconName;

        public GridItemIcon(string iconname, string icontext, string subicon)
        {
            IconName = iconname;
            IconText = icontext;
            SubIconName = subicon;
        }

        public GridItemIcon(string iconName) : this()
        {
            IconName = iconName;
        }

        public void Dispose()
        {
            IconName = null;
            SubIconName = null;
            IconText = null;
        }

        internal static void Draw(GridItemIcon icondata, GameObject canvas)
        {
            canvas.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            canvas.GetComponent<UISprite>().spriteName = icondata.IconName;

            if (canvas.transform.FindChild("SubIcon")!= null)
            {
                GameObject subiconObject = canvas.transform.FindChild("SubIcon").gameObject;
                if (icondata.SubIconName == null)
                    subiconObject.SetActive(false);
                else
                {
                    subiconObject.SetActive(true);
                    subiconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
                    subiconObject.GetComponent<UISprite>().spriteName = icondata.SubIconName;
                }
            }
            if(canvas.transform.FindChild("SubLabel")!= null)
            {
                GameObject sublabelObject = canvas.transform.FindChild("SubLabel").gameObject;
                if (icondata.IconText == null)
                    sublabelObject.SetActive(false);
                else
                {
                    sublabelObject.SetActive(true);
                    sublabelObject.GetComponent<UILabel>().text = icondata.IconText;
                }
            }
        }
    }

}