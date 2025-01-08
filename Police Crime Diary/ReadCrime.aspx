<%@ Page Language="C#" Title="Read Crime" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ReadCrime.aspx.cs" Inherits="Police_Crime_Diary.ReadCrime" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <br />
    <table class="table table-striped rounded">
        <thead class="thead-dark">
            <tr>
                <th scope="col" style="min-width: 100px;"></th>
                <th scope="col" style="min-width: 150px;">Reporter&nbsp;Name</th>
                <th scope="col" >Reporter&nbsp;ID</th>
                <th scope="col" style="min-width: 100px;">Suspect</th>
                <th scope="col" style="min-width: 100px;">Susptect's&nbsp;Picture</th>
                <th scope="col" style="min-width: 200px;">Suspect&nbsp;Address</th>
                <th scope="col" style="min-width: 80px;">Gender</th>
                <th scope="col" style="min-width: 100px;">Numbers</th>
                <th scope="col" style="min-width: 100px;">Type&nbsp;of&nbsp;Crime</th>
                <th scope="col" style="min-width: 200px;">Description</th>
                <th scope="col" style="min-width: 100px;">Evidence</th>
                <th scope="col" style="min-width: 100px;">Date</th>
                <th scope="col" style="min-width: 100px;">Time</th>
                <th scope="col" style="min-width: 100px;">Date&nbsp;Reported</th>
            </tr>
        </thead>
        <tbody id="crimes" runat="server">

        </tbody>
    </table>
    <div class="row p-2" style="background-color: white; justify-content: space-between">
        <div class="col-6">
            <p class="fs-6 lead my-auto py-2" runat="server" id="showing">Showing 0 to 0 of 0 entries</p>
        </div>
        <div class="col-6 text-end p-0">
            <div class="btn-group">
                <a href="?page=0" runat="server" id="prev_page" class="btn btn-info">Previous</a>
                <span runat="server" id="page_count" class="btn btn-secondary">0</span>
                <a href="?page=6" runat="server" id="nxt_page" class="btn btn-info">Next</a>
            </div>
        </div>
    </div>
    <div class="container mt-2">
        <h3>Case Taken</h3>
        <h5>From:</h5>
        <div class="row mx-1">
            <div class="col-5">Name:</div>
            <div class="col">Zack Jack</div>
        </div>
        <div class="row mx-1">
            <div class="col-5">My&nbsp;Force&nbsp;Number:</div>
            <div class="col">POLICE 392</div>
        </div>
        <h5>To:</h5>
        <div class="row mx-1 border-bottom pb-2">
            <div class="col-5">Name:</div>
            <div class="col">Some Reporter Username</div>
        </div>
        <div class="btn btn-primary my-3">Investigate</div>
        <br />
    </div>
</asp:Content>