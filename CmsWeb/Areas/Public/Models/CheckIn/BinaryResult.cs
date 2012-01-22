using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CmsWeb.Models
{
    public class BinaryResult : ActionResult
    {
        byte[] bits;
        public BinaryResult(byte[] bits)
        {
            this.bits = bits;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = "application/octet-stream";
            context.HttpContext.Response.BinaryWrite(bits);
        }
    }
}