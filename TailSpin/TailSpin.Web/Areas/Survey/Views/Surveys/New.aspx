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

<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
         Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Models.TenantPageViewData<TailSpin.Web.Survey.Shared.Models.Survey>>" %>
<%@ Import Namespace="TailSpin.Web.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MenuContent" runat="server">
    <ul>
        <li class="current"><a>New Survey</a></li>
        <li><%:Html.ActionLink("My Surveys", "Index", "Surveys")%></li>
        <li><%:Html.ActionLink("My Account", "Index", "Account", new {area = string.Empty}, null)%></li>
    </ul>
    <div class="clear">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs">
        New Survey
    </div>

    <h2>Create a new survey</h2>
    <br />
    <% using (Html.BeginForm()) { %>
    <%: Html.Serialize("hiddenSurvey", Model.ContentModel)%>
    <%: Html.Hidden("referrer", "newSurvey") %>
    <%: Html.ValidationSummary(true) %>
    <%: Html.AntiForgeryToken() %>
    <dl>
        <dt><%: Html.LabelFor(model => model.ContentModel.Title) %></dt>
        <dd>
            <%: Html.TextBoxFor(model => model.ContentModel.Title, new { size = "40" })%>
            <%: Html.ValidationMessageFor(model => model.ContentModel.Title)%>
        </dd>
    </dl>

    <br />
    <table>
        <tr>
            <th align="left">
                Questions
                <%: Html.ValidationMessageFor(model => model.ContentModel.Questions)%>
            </th>
        </tr>
        <tr>
            <td>
                <a href="#" onclick="javascript: submitToNewQuestion()">Add Question</a>
            </td>
        </tr>
    
        <% foreach (var question in this.Model.ContentModel.Questions) { %>
    
            <tr>
                <td>
                    <%:question.Text%>
                </td>
            </tr>
    
        <% } %>

        </table>
    
        <br />
        <br />
        <p>
            <input id="create" type="submit" value="Create" class="bigOrangeButton" />
        </p>
    <% } %>
    <script type="text/javascript">
        function submitToNewQuestion() {
            document.forms[0].action = '<%=Url.Action("NewQuestion", "Surveys")%>'
            document.forms[0].submit();
        }
    </script>

</asp:Content>

