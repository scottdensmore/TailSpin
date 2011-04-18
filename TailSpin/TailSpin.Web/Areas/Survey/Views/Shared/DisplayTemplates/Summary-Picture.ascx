<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswersSummary>" %>
<span class="questionText">
    <%:this.Model.QuestionText%>
</span>
<div>
    <span style="font-style: italic">This type of answer does not have a summary yet.</span>
</div>