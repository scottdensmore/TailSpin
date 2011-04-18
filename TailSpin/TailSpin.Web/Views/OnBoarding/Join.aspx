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

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Models.TenantMasterPageViewData>" %>


<asp:Content ID="JoinHead" ContentPlaceHolderID="Head" runat="server">
    
</asp:Content>

<asp:Content ID="JoinMenuContent" ContentPlaceHolderID="MenuContent" runat="server">
     <div class="clear">
    </div>
</asp:Content>

<asp:Content ID="JoinMainContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Join</h2>
    In a real federated application, a new customer would be able to become a tenant
    of Tailspin by completing a form similar to the one below.
    <div id="issuerOptionTabs">
        <div class="sectionexplanationcontainer" >
            <span class="titlesection">Issuer configuration</span>
            <span class="explanationsection">
                <div id="yourIssuerTab" class="issuerOptionTab">
                    <div class="sampleform">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        Organization name:
                                    </td>
                                    <td>
                                        <%= Html.TextBox("OrganizationName", Samples.Web.ClaimsUtillities.Adatum.OrganizationName, new { size = "55" })%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Geolocation of your Windows Azure host:
                                    </td>
                                    <td>
                                        <%= Html.DropDownList("Geolocation", new List<SelectListItem>
                                                            {
                                                                new SelectListItem {Text = "Anywhere Asia", Value = "Anywhere Asia"},
                                                                new SelectListItem {Text = "Anywhere Europe", Value = "Anywhere Europe"},
                                                                new SelectListItem {Text = "Anywhere US", Value = "Anywhere US"},
                                                                new SelectListItem {Text = "East Asia", Value = "East Asia"},
                                                                new SelectListItem {Text = "North Central US", Value = "North Central US"},
                                                                new SelectListItem {Text = "North Europe", Value = "North Europe"},
                                                                new SelectListItem {Text = "South Central US", Value = "South Central US"},
                                                                new SelectListItem {Text = "Southeast Asia", Value = "Southeast Asia"},
                                                                new SelectListItem {Text = "West Europe", Value = "West Europe"},
                                                            })%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Sign-in URL:
                                    </td>
                                    <td>
                                        <%: Html.TextBox("IPStsAddress", "https://localhost/Adatum.SimulatedIssuer.wp7/", new { size = "55" })%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Thumbprint:
                                    </td>
                                    <td>
                                        <%: Html.TextBox("Thumbprint", "f260042d59e14817984c6183fbc6bfc71baf5462", new { size = "55" })%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admin Claim Type:
                                    </td>
                                    <td>
                                        <%: Html.TextBox("ClaimType", "http://schemas.xmlsoap.org/claims/group", new { size = "55" })%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admin Claim Value:
                                    </td>
                                    <td>
                                        <%: Html.TextBox("ClaimValue", "Marketing Managers", new { size = "55" })%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        SQL Azure Instance (optional):
                                    </td>
                                    <td>
                                        <span><%: Html.RadioButton("Size1", "1GB", false)%>1 GB</span>
                                        <span><%: Html.RadioButton("Size10", "10GB", false)%>10 GB</span>
                                        <span><%: Html.RadioButton("Size50", "50GB", false)%>50 GB</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div style="text-align: right; margin-top: 10px;">
                        <input type="button" onclick="alert('This page is just a mockup, new tenants cannot be added to the Tailspin sample application.');"
                            value="Become a Tenant" />
                    </div>
                </div>
            </span>
        </div>
    </div>
</asp:Content>

