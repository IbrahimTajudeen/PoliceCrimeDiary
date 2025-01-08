<%@ Page Language="C#" Title="Bail Menu" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BailMenu.aspx.cs" Inherits="Police_Crime_Diary.BailMenu" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Bail Menu <a href="~/AddBail.aspx" runat="server" class="btn btn-sm btn-info float-end">Give Bail</a></h4>
        </div>
        <div class="container-fluid text-end border-bottom pb-2">
            <div class="row justify-content-end">
                <div class="col"></div>
                <div class="col-6">
                    <input type="search" id="search" class="form-control mx-2" />
                </div>
                <div class="col-2">
                    <div class="btn btn-primary" id="search_btn">Search</div>
                </div>
            </div>
        </div>
        <div style="width: 100%; max-height: 400px; overflow: auto;">
            <table class="table table-striped rounded mb-2">
                <thead>
                    <tr>
                        <th style="width: 200px;">Suspect Name</th>
                        <th style="width: 180px;">Suspect Picture</th>
                        <th style="width: 150px;">Bail Amount</th>
                        <th style="width: 150px;">Amount Offered</th>
                        <th style="width: 150px;">Officer Incharge</th>
                        <th style="width: 150px;">Status</th>
                        <th style="width: 150px;"></th>
                    </tr>
                </thead>
                <tbody runat="server" id="bail_crimes">
                    
                </tbody>
            </table>
        </div>
    </div>
    <script>
        $(function () {
            $('#search_btn').on('click', function () {
                $search = $('#search').val().trim();
                window.location.href = "/BailMenu.aspx" + (($search != '') ? "?search=" + $search : '');
            })
        })
    </script>
</asp:Content>