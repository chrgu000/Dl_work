using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSXML2;

namespace U8API
{
   public class ENUtil
    {
        public static object getStrFromObj(object val)
        {
            if (val == null)
            {
                return "";
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domHead"></param>
        /// <param name="sKey"></param>
        /// <param name="val"></param>
        public static void SetToDomH(MSXML2.IXMLDOMDocument2 domHead, string sKey, Object val)
        {
            if (val != null)
            {
                sKey = sKey.ToLower();
                if (domHead.selectSingleNode("//rs:data/z:row").attributes.getNamedItem(sKey) != null)
                {
                    domHead.selectSingleNode("//rs:data/z:row").attributes.getNamedItem(sKey).nodeValue = val;
                }
                else
                {
                    var attr = domHead.createAttribute(sKey);
                    attr.nodeValue = val;
                    domHead.selectSingleNode("//rs:data/z:row").attributes.setNamedItem(attr);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domBody"></param>
        /// <param name="r"></param>
        /// <param name="sKey"></param>
        /// <param name="val"></param>
        public static void SetToDomB(MSXML2.IXMLDOMDocument2 domBody, int r, string sKey, Object val)
        {
            if (val != null)
            {
                sKey = sKey.ToLower();

                if (domBody.selectNodes("//rs:data/z:row")[r] != null)
                {
                    if (domBody.selectNodes("//rs:data/z:row")[r].attributes.getNamedItem(sKey) != null)
                    {
                        domBody.selectNodes("//rs:data/z:row")[r].attributes.getNamedItem(sKey).nodeValue = val;
                    }
                    else
                    {
                        var attr = domBody.createAttribute(sKey);
                        attr.nodeValue = val;
                        domBody.selectNodes("//rs:data/z:row")[r].attributes.setNamedItem(attr);
                    }
                }
                else
                {

                    IXMLDOMElement ele_body;

                    ele_body = domBody.createElement("z:row");
                    domBody.selectSingleNode("//rs:data").appendChild(ele_body);



                    if (domBody.selectNodes("//rs:data/z:row")[r].attributes.getNamedItem(sKey) != null)
                    {
                        domBody.selectNodes("//rs:data/z:row")[r].attributes.getNamedItem(sKey).nodeValue = val;
                    }
                    else
                    {
                        var attr = domBody.createAttribute(sKey);
                        attr.nodeValue = val;
                        domBody.selectNodes("//rs:data/z:row")[r].attributes.setNamedItem(attr);
                    }
                }
            }
        }
    }
}
