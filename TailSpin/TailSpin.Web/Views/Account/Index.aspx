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

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Models.TenantPageViewData<TailSpin.Web.Survey.Shared.Models.Tenant>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MenuContent" runat="server">
    <ul>
        <li><%:Html.ActionLink("New Survey", "New", "Surveys", new { area = "Survey" }, null)%></li>
        <li><%:Html.ActionLink("My Surveys", "Index", "Surveys", new { area = "Survey" }, null)%></li>
        <li class="current"><a>My Account</a></li>
    </ul>
    <div class="clear">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>My Account</h2>
    <div id="issuerOptionTabs">
        <div class="sectionexplanationcontainer" >
            <span class="titlesection">Account configuration</span>
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
                                        <%:Html.DisplayFor(m => m.ContentModel.Name)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Upload a logo:
                                    </td>
                                    <td>
                                        <% using (Html.BeginForm("UploadLogo", "Account", FormMethod.Post, new {enctype = "multipart/form-data"})) { %>
                                            <input type="file" id="newLogo" name="newLogo" accept="image/jpeg" />
                                            <%: Html.AntiForgeryToken() %>
                                            <input type="submit" name="Upload" value="Upload" />
                                        <% } %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Geolocation of your Windows Azure host:
                                    </td>
                                    <td>
                                        <%:Html.DisplayFor(m => m.ContentModel.HostGeoLocation)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Sign-in URL:
                                    </td>
                                    <td>
                                        <%:Html.DisplayFor(m => m.ContentModel.IssuerUrl)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Thumbprint:
                                    </td>
                                    <td>
                                        <%:Html.DisplayFor(m => m.ContentModel.IssuerThumbPrint)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admin Claim Type:
                                    </td>
                                    <td>
                                        <%:Html.DisplayFor(m => m.ContentModel.ClaimType)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admin Claim Value:
                                    </td>
                                    <td>
                                        <%:Html.DisplayFor(m => m.ContentModel.ClaimValue)%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        SQL Azure Instance (optional):
                                    </td>
                                    <td>
                                        <%:Html.DisplayFor(m => m.ContentModel.SqlAzureConnectionString)%>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </span>
        </div>
    </div>
</asp:Content>

