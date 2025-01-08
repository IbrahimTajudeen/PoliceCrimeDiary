<%@ Page Language="C#" Title="Crime Diary" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CrimeDiary.aspx.cs" Inherits="Police_Crime_Diary.CrimeDiary" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container rounded p-3" style="background-color: white;">
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
        <table class="table table-striped rounded">
            <thead class="thead-dark">
                <tr>
                    <th scope="col" style="min-width: 100px;">Suspect</th>
                    <th scope="col" style="min-width: 80px;">Type&nbsp;of&nbsp;Crime</th>
                    <th scope="col" style="min-width: 120px;">Location</th>
                    <th scope="col" style="min-width: 200px;">Evidence</th>
                    <th scope="col" style="min-width: 120px;">Description</th>
                    <th scope="col" style="min-width: 100px;">Date</th>
                    <th scope="col" style="min-width: 100px;">Time</th>
                    <th scope="col" style="min-width: 100px;">Date&nbsp;Reported</th>
                </tr>
            </thead>
            <tbody runat="server" id="crime_list">
            
            </tbody>
        </table>
    </div>
    <script>
        $(function () {
            $('#search_btn').on('click', function () {
                $search = $('#search').val().trim();
                window.location.href = "/CrimeDiary.aspx" + (($search != '') ? "?search=" + $search : '');
            })
        })
    </script>
</asp:Content>