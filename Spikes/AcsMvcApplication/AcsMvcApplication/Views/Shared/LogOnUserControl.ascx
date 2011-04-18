<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        Welcome <b><%: Page.User.Identity.Name %></b>!
        [ <%: Html.ActionLink("Log Off", "LogOff", "Account") %> ]
<%
    }
    else {
%>
    <a href="#" id="logon">Log On</a>
    <div id="popup_logon">        
    </div>
    <style type="text/css">
        #popup_logon ul
        {
            list-style: none;
        }
        #popup_logon ul li
        {
            margin: 10px;
            padding: 10px
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#logon").click(function () {
                $("#popup_logon").html("<p>Loading...</p>");
                $("#popup_logon").dialog({ modal: true, draggable: false, resizable: false, title: 'Select your preferred login method' });
                $.ajax({
                    url: '<%: MvcHtmlString.Create(Url.Action("IdentityProviders", "Account", new {serviceNamespace = "tailspin", appId = "http://localhost/AcsMvcApplication/"})) %>',
                    success: function (data) {
                        dialogHtml = '<ul>';
                        for (i = 0; i < data.length; i++) {
                            dialogHtml += '<li>';
                            if (data[i].ImageUrl === '') {
                                dialogHtml += '<a href="' + data[i].LoginUrl + '">' + data[i].Name + '</a>';
                            } else {
                                dialogHtml += '<a href="' + data[i].LoginUrl + '"><img style="border: 0px; width: 100px" src="' + data[i].ImageUrl + '" alt="' + data[i].Name + '" /></a>';
                            }

                            dialogHtml += '</li>';
                        }

                        dialogHtml += '</ul>';

                        $("#popup_logon").html(dialogHtml);
                    }
                })
            });
        });
    </script> 
<%
    }
%>
