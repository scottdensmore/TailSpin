<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswer>" %>
<span class="questionText">
    <%:this.Model.QuestionText%>
</span>
<div>
    <% if (!string.IsNullOrEmpty(this.Model.Answer)) { %>
    <img src="<%=this.Model.Answer%>" alt="Picture answer" style="width: 60px; height:48px;" />
    <% } else { %>
    <span>No picture to display.</span>
    <% } %>
    
</div>
