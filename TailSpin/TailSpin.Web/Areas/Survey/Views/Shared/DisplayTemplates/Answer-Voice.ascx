<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswer>" %>
<span class="questionText">
    <%:this.Model.QuestionText%>
</span>
<div>
    <% if (!string.IsNullOrEmpty(this.Model.Answer)) { %>
    <span>Download the <a href="<%=Html.AttributeEncode(this.Model.Answer)%>">answer</a>.</span>
    <% } else { %>
    <span>No answer has been recorded.</span>
    <% } %>
</div>
