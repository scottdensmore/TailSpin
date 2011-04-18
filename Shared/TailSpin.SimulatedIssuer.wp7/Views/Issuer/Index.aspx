<%--
===============================================================================
 Microsoft patterns & practices
 Windows Phone 7 Developer Guide
===============================================================================
 Copyright © Microsoft Corporation.  All rights reserved.
 This code released under the terms of the 
 Microsoft patterns & practices
===============================================================================
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <%:this.ViewData["ActionExplanation"]%>
    <br /><br />
    <a href="<%=this.ViewData["ReturnUrl"]%>">Go back to TailSpin</a>
</asp:Content>