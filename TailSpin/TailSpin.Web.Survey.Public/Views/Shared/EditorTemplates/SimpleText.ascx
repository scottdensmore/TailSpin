<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswer>" %>
<%:Html.TextAreaFor(m => m.Answer, new {rows = "3", cols = "1", style = "width: 820px;"})%>
