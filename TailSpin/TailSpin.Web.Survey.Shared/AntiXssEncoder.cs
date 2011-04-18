//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared
{
    using System.IO;
    using System.Web.Util;
    using Microsoft.Security.Application;

    public class AntiXssEncoder : HttpEncoder
    {
        protected override void HtmlAttributeEncode(string value, TextWriter output)
        {
            output.Write(AntiXss.HtmlAttributeEncode(value));
        }

        protected override void HtmlEncode(string value, TextWriter output)
        {
            output.Write(AntiXss.HtmlEncode(value));
        }
    }
}