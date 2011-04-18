<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswersSummary>" %>
<%@ Import Namespace="TailSpin.Web.Areas.Survey.Models" %>
<span class="questionText">
    <%:this.Model.QuestionText%>
</span>
<div class="answer">
    <% var summary = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, int>>(this.Model.AnswersSummary);
       var total = summary.Total();   
        foreach (var possibleAnswer in this.Model.PossibleAnswers.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries)) { %>
        <div style="padding-top:10px;padding-bottom:10px">
            <span><%: possibleAnswer %>:</span>
            <% if (summary.ContainsKey(possibleAnswer))
               { %>
            <span><%: summary[possibleAnswer]%> responses</span>
            <% var percentage = summary[possibleAnswer].PercentOf(total); %>
            <div class="graph">
               <div class="graphmask" style="width:<%: percentage %>%;"><span><%:percentage%>%</span></div>
            </div>
            <% } else { %>
            <span>0 responses</span>
            <% } %>
        </div>
    <% } %>
</div>