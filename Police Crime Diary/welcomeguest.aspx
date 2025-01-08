<%@ Page Language="C#" Title="Welcome Guest" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="welcomeguest.aspx.cs" Inherits="Police_Crime_Diary.welcomeguest" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container rounded p-3" style="background-color: white;">
        <p style="text-align: justify;">&nbsp;&nbsp;&nbsp;&nbsp;Police crime diary is a place where citizen report crime and their identities are kept secret. This ebsite is own by the Nigeria Police Force NPF.</p>
        <p style="text-align: justify;">&nbsp;&nbsp;&nbsp;&nbsp;I as the Inspector General of Police IGP, assures you that your reported crime must be attended to and justice must prevail. Please to report a crime you must be a registered user. So <a href="~/Register.aspx" runat="server">REGISTER</a> today.</p>
    </div>
</asp:Content>