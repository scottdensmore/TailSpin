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

<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Models.TenantPageViewData<TailSpin.Web.Areas.Survey.Models.ExportResponseModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MenuContent" runat="server">
    <ul>
        <li><%:Html.ActionLink("New Survey", "New", "Surveys")%></li>
        <li class="current"><a>My Surveys</a></li>
        <li><%:Html.ActionLink("My Account", "Index", "Account", new {area = string.Empty}, null)%></li>
    </ul>
    <div class="clear">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="pageTitle">
        <h2>
            <%:Html.ActionLink("Summary", "Analyze", "Surveys")%>
            <%:Html.ActionLink("Browse Responses", "BrowseResponses", "Surveys")%>
            <span>Export Responses</span>
        </h2>
    </div>
    <div class="breadcrumbs">
        <%:Html.ActionLink("My Surveys", "Index", "Surveys")%> &gt;
        <%:this.Model.Title%> &gt;
        <%:Html.ActionLink("Analyze", "Analyze", "Surveys")%> &gt;
        Export Reponses
    </div>

    <h2>
        Analyze:
        <%:this.Model.Title%>
    </h2>
    <br />
    <% using (Html.BeginForm()) { %>
    <div id="surveyForm">
        <h3>Export Survey Responses to SQL Azure</h3>
        <div class="sectionexplanationcontainer" >
            <span class="titlesection">To access the exported results connect to:</span>
            <span class="explanationsection">
                <div id="yourIssuerTab" class="issuerOptionTab">
                    <div class="sampleform">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        Database name:
                                    </td>
                                    <td>
                                        <%: Model.ContentModel.Tenant.DatabaseName%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        User Id:
                                    </td>
                                    <td>
                                        <%: Model.ContentModel.Tenant.DatabaseUserName %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Password:
                                    </td>
                                    <td>
                                        <%: Model.ContentModel.Tenant.DatabasePassword %>
                                    </td>
                                </tr>
                                
                            </tbody>
                        </table>
                    </div>
                </div>
            </span>
        </div>        
        <%:Html.AntiForgeryToken() %>
        <p>
        <% if (Model.ContentModel.HasResponses)
           { %>
        
            <input id="create" type="submit" value="Export" class="bigOrangeButton" />
        
        <% } else { %>
            There are no responses to export.
        <% } %>
        </p>
    </div>
    <% } %>
</asp:Content>
