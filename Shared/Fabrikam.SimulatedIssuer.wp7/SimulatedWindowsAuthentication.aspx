<%--
===============================================================================
 Microsoft patterns & practices
 Windows Phone 7 Developer Guide
===============================================================================
 Copyright © Microsoft Corporation.  All rights reserved.
 This code released under the terms of the 
 Microsoft patterns & practices
===============================================================================
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SimulatedWindowsAuthentication.aspx.cs" Inherits="Adatum.SimulatedIssuer.SimulatedWindowsAuthentication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />

<asp:Content ID="Content2" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <div id="login">
        Fabrikam issuer is logging you in using Windows Integrated Authentication. Please select a User to continue:
        <div id="UserOptions">
            <asp:RadioButtonList ID="UserList" runat="server">
                <asp:ListItem Text="Fabrikam\mary (Groups: 'Domain Users' & 'Marketing Managers')" Value="Fabrikam\mary" Selected="True" />
            </asp:RadioButtonList>
        </div>
        <asp:Button ID="ContinueButton" runat="server" class="tooltip" Text="Continue with login..." OnClick="ContinueButtonClick"  />
    </div>
</asp:Content>
