<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswer>" %>
<span class="questionText">
    <%:this.Model.QuestionText%>
</span>
<div class="answer">
    <%:this.Model.Answer%>
</div>
