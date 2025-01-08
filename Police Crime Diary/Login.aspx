<%@ Page Language="C#" Title="Login" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Police_Crime_Diary.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container pt-3 rounded" style="background-color: white">
        <div>
            <h4>Log in to your account</h4>
        </div>
        <div class="form-group">
            <label class="form-label">Username</label>
            <input id="username" runat="server" type="text" class="form-control" required="required" />
        </div>
        <div class="form-group">
            <label class="form-label">Password</label>
            <input id="password" type="password" runat="server" class="form-control" required="required" />
        </div>
        <div>
            <asp:Button Text="Log in" runat="server" OnClick="Unnamed_Click" CssClass="btn btn-success my-2" />
        </div>
        <div class="row justify-content-center">
            <p class="">New User? <a href="Register.aspx" class="btn btn-info">Register</a></p>
        </div>
    </div>
</asp:Content>