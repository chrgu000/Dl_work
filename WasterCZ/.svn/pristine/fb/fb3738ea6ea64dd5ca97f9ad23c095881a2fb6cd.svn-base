using MSXML2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8API.Entity
{
    public class U8APIHelper
    {
        public static void FormatDom(ref MSXML2.DOMDocument SourceDom, string editprop)
        {
            //IXMLDOMElement element;
            //IXMLDOMElement ele_head;
            IXMLDOMElement ele_body;
            //IXMLDOMNode nd;
            //MSXML2.DOMDocument tempnd;
            IXMLDOMNodeList ndheadlist;
            IXMLDOMNodeList ndbodylist;
            //DistDom.loadXML("SourceDom.xml");
            String Filedname;
            //'格式部分
            ndheadlist = SourceDom.selectNodes("//s:Schema/s:ElementType/s:AttributeType");
            ndbodylist = SourceDom.selectNodes("//rs:data/z:row");
            if (ndbodylist.length == 0)
            {
                ele_body = SourceDom.createElement("z:row");
                SourceDom.selectSingleNode("//rs:data").appendChild(ele_body);
            }
            ndbodylist = SourceDom.selectNodes("//rs:data/z:row");
            foreach (IXMLDOMElement body in ndbodylist)
            {
                foreach (IXMLDOMElement head in ndheadlist)
                {
                    Filedname = head.attributes.getNamedItem("name").nodeValue + "";
                    if (body.attributes.getNamedItem(Filedname) == null)
                        //  '若没有当前元素，就增加当前元素
                        body.setAttribute(Filedname, "");
                    switch (head.lastChild.attributes.getNamedItem("dt:type").nodeValue.ToString())
                    {
                        case "number":
                        case "float":
                        case "boolean":
                            if (body.attributes.getNamedItem(Filedname).nodeValue.ToString().ToUpper() == "false".ToUpper())
                                body.setAttribute(Filedname, 0);
                            break;
                        default:
                            if (body.attributes.getNamedItem(Filedname).nodeValue.ToString().ToUpper() == "否".ToUpper())
                                body.setAttribute(Filedname, 0);
                            break;
                    }
                }
                if (editprop != "")
                    body.setAttribute("editprop", editprop);
            }
        }
    }
}