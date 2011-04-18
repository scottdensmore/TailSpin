<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.QuestionAnswer>" %>
<span style="font-style: italic">This question can only be answered from a device running Windows Phone.</span>
<%:Html.HiddenFor(m => m.Answer)%>

