<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Survey.Public.Models.TenantPageViewData<TailSpin.Web.Survey.Shared.Models.SurveyAnswer>>" %>
<%@ Import Namespace="TailSpin.Web.Survey.Public.Utility" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="surveyTitle">
        <h1>
            <%:this.Model.ContentModel.Title%>
        </h1>
    </div>
    <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>
    <div id="surveyForm">
        <ol>
            <% for (int i = 0; i < this.Model.ContentModel.QuestionAnswers.Count; i++ ) { %>
            <li>
                <div class="questionText">
                    <%:this.Model.ContentModel.QuestionAnswers[i].QuestionText%>
                    <%:Html.ValidationMessageFor(m => m.ContentModel.QuestionAnswers[i].Answer)%>
                </div>
                <%: Html.EditorFor(m => m.ContentModel.QuestionAnswers[i], QuestionTemplateFactory.Create(Model.ContentModel.QuestionAnswers[i].QuestionType)) %>
            </li>
            <% } %>
        </ol>
        <input id="finish" type="submit" value="Finish" class="bigOrangeButton" />
    </div>
    <% } %>

    <script src="<%:Url.Content("~/Scripts/jquery-1.4.1.min.js")%>" language="javascript" type="text/javascript"></script>
    <script src="<%:Url.Content("~/Scripts/jquery.rating.js")%>" language="javascript" type="text/javascript"></script>
</asp:Content>
