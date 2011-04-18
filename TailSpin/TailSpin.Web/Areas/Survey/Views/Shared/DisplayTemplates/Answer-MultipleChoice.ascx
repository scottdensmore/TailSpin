<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswer>" %>
<span class="questionText">
    <%:this.Model.QuestionText%>
</span>
<div class="answer">
    <% foreach (var possibleAnswer in this.Model.PossibleAnswers.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries))
       { %>
    <div class="option">
        <%:Html.RadioButton(possibleAnswer, possibleAnswer, possibleAnswer == this.Model.Answer, new { disabled = "disabled" })%>
        <% if (possibleAnswer == this.Model.Answer) {%>
        <span style="font-weight: bold;"><%:possibleAnswer%></span>
        <% } else {%>
        <span><%:possibleAnswer%></span>
        <%}%>
    </div>
    <% } %>
</div>