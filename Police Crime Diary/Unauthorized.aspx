<%@ Page Language="C#" Title="Unauthorize" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Unauthorized.aspx.cs" Inherits="Police_Crime_Diary.Unauthorized" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex flex-column justify-content-center align-items-center rounded p-3" style="background-color: white;">
        <h1 class="text-danger text-center border-bottom pb-3 w-100">Unauthorized Request!</h1>
        <h3 class="fw-bold text-center">YOU ARE NOT ALLOWED TO VISIT THIS PAGE.<br />PAGE NAME: <span class="btn btn-secondary" id="the_page" runat="server"></span></h3>
        <h4 class="fw-bold text-center">THIS MEANS YOU DON'T HAVE THE REQUIRE PERMISSION OR AUTHORITY TO VISIT THE PAGE.<br />YOUR CURRENT ROLE IS: <span id="role" runat="server" class="btn btn-secondary"></span></h4>
        <div class="btn btn-danger" id="go_back">Go&nbsp;Back</div>
    </div>
    <script>
        $(() => {
            $('.btn').on('click', () => {
                history.go(-1)
            })
        })
    </script>
</asp:Content>