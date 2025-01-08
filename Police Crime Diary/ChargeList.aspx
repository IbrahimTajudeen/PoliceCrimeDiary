<%@ Page Language="C#" Title="Criminal Charged" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChargeList.aspx.cs" Inherits="Police_Crime_Diary.ChargeList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Charge List <a href="~/Charge.aspx" runat="server" class="btn btn-sm btn-info float-end">Charge Criminal</a></h4>
        </div>
        <div style="width: 100%; max-height: 400px; overflow: auto;">
            <table class="table table-striped rounded mb-2">
                <thead>
                    <tr>
                        <th style="width: 200px;">Criminal Name</th>
                        <th style="width: 180px;">Criminal Picture</th>
                        <th style="width: 180px;">Court Name</th>
                        <th style="width: 150px;">Court Date</th>
                        <th style="width: 150px;">Officer InCharge</th>
                        <th style="width: 150px;">Status</th>
                        <th style="width: 150px;"></th>
                    </tr>
                </thead>
                <tbody runat="server" id="bail_crimes">
                    
                </tbody>
            </table>
        </div>

    </div>
</asp:Content>