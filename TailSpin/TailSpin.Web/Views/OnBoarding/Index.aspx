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

<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Models.TenantMasterPageViewData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MenuContent" runat="server">
    <div class="clear">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Welcome to TailSpin!
    </h2>
    <br />
    <%= Html.ActionLink("Enroll your company", "Join") %> in TailSpin!

    <p>To test the application, these tenants that have already been provissioned:</p>
    
    <div id="configured-tenants">
        <ul class="tenants-list">
            <li>
                <div class="configured-tenant-logo">
                    <a href="<%:Url.Action("Index", "Surveys", new { area="Survey", tenant = "adatum" }, null)%>" class="configured-tenants-links">
                            <img src="<%= Url.Content("~/Content/img/adatum-logo.png") %>" alt="Adatum logo" />
                        </a>
                </div>
                <div class="configured-tenant-description">
                    Adatum company has already been configured to <b>authenticate using Adatum's issuer</b>.
                </div>
            </li>
            <li>
                <div class="configured-tenant-logo">
                    <a href="<%:Url.Action("Index", "Surveys", new { area="Survey", tenant = "fabrikam" }, null)%>" class="configured-tenants-links">
                            <img src="<%= Url.Content("~/Content/img/fabrikam-logo.png") %>" alt="Fabrikam logo" />
                        </a>
                </div>
                <div class="configured-tenant-description">
                    Fabrikam company has already been configured to <b>authenticate using Fabrikam's issuer</b>.
                </div>
            </li>
        </ul>
    </div>
</asp:Content>
