<%@ Page Language="C#" AutoEventWireup="true" Title="Inbox" MasterPageFile="~/Site.Master" CodeBehind="Inbox.aspx.cs" Inherits="Police_Crime_Diary.Inbox" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Inbox Messages</h4>
        </div>
        <div id="notify" runat="server">

        </div>
    </div>
</asp:Content>