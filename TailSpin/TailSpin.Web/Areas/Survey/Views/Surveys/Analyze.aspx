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

<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Models.TenantPageViewData<TailSpin.Web.Survey.Shared.Models.SurveyAnswersSummary>>" %>

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
            <span>Summary</span>
            <%:Html.ActionLink("Browse Responses", "BrowseResponses", "Surveys")%>
            <%:Html.ActionLink("Export Responses", "ExportResponses", "Surveys")%>
        </h2>
    </div>
    <div class="breadcrumbs">
        <%: Html.ActionLink("My Surveys", "Index", "Surveys")%> &gt;
        <%: this.Model.Title %> &gt;
        Analyze 
    </div>

    <h2>
        Analyze:
        <%: this.Model.Title %>
    </h2>
    <div id="surveyForm">
        <% if (this.Model.ContentModel != null) { %>
        <div class="questionText" style="font-size:16px;padding-top:15px; padding-bottom:10px">
            Total responses: <%: this.Model.ContentModel.TotalAnswers %>
        </div>
        <ol>
            <% for (int i = 0; i < this.Model.ContentModel.QuestionAnswersSummaries.Count; i++ ) { %>
            <li>
                <%: Html.DisplayFor(m => m.ContentModel.QuestionAnswersSummaries[i], "Summary-" + TailSpin.Web.Survey.Public.Utility.QuestionTemplateFactory.Create(Model.ContentModel.QuestionAnswersSummaries[i].QuestionType))%>
            </li>
            <% } %>
        </ol>
        <% } else { %>
        No responses to this survey yet.
        <% } %>
    </div>

    <script src="<%:Url.Content("~/Scripts/jquery-1.4.1.min.js")%>" language="javascript" type="text/javascript"></script>
    <script src="<%:Url.Content("~/Scripts/jquery.rating.js")%>" language="javascript" type="text/javascript"></script>
</asp:Content>
