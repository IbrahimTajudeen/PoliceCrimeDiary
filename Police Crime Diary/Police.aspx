<%@ Page Language="C#" Title="Police List" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Police.aspx.cs" Inherits="Police_Crime_Diary.Police" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <div class="container-fluid rounded p-3" style="background-color: white;">
        <div class="container-fluid text-end border-bottom pb-2">
            <div class="row justify-content-end">
                <div class="col"><a href="~/CreatePolice.aspx" runat="server" class="btn btn-info">Create New Police</a></div>
                <div class="col-6">
                    <input type="search" id="search" class="form-control mx-2" />
                </div>
                <div class="col-2">
                    <div class="btn btn-primary" id="search_btn">Search</div>
                </div>
            </div>
        </div>
        <table class="table table-striped rounded">
            <thead class="thead-dark">
                <tr>
                    <th scope="col" style="min-width: 200px;">Police&nbsp;Name</th>
                    <th scope="col" style="min-width: 150px;">Email</th>
                    <th scope="col" style="min-width: 200px;">Phone</th>
                    <th scope="col" style="min-width: 200px;">Picture</th>
                    <th scope="col" style="min-width: 100px;">Join&nbsp;Date</th>
                    <th scope="col" style="min-width: 120px;">Username</th>
                    <th></th>
                </tr>
            </thead>
            <tbody runat="server" id="police_list">
            
            </tbody>
        </table>
    </div>
    <script>
        $(function () {
            $('#search_btn').on('click', function () {
                $search = $('#search').val().trim();
                window.location.href = "/Police.aspx" + (($search != '') ? "?search=" + $search : '');
            })
        })
    </script>
</asp:Content>