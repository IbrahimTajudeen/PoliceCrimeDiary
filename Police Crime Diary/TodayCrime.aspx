<%@ Page Language="C#" Title="Today Crime" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TodayCrime.aspx.cs" Inherits="Police_Crime_Diary.TodayCrime" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Today Crime</h4>
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
                        <th  style="width: 150px;">Crime Type</th>
                        <th  style="width: 150px;">Location</th>
                    </tr>
                </thead>
                <tbody runat="server" id="today_crimes">
                    
                </tbody>
            </table>
        </div>
    </div>
    <script>
        $(function () {
            $('#search_btn').on('click', function () {
                $search = $('#search').val().trim();
                window.location.href = "/TodayCrime.aspx" + (($search != '') ? "?search=" + $search : '');
            })
        })
    </script>
</asp:Content>