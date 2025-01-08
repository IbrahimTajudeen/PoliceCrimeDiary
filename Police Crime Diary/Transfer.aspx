<%@ Page Language="C#" Title="Transfer" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="Police_Crime_Diary.Transfer" %>

<asp:Content ID="bodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <div class="container p-3 rounded" style="background-color: white;">
        <div class="mb-4">
            <h4 class="border-bottom pb-2 mb-2">Transfer Form</h4>
        </div>
        <div class="row mb-2">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="form-label required" >Suspect Name</label>
                    <asp:DropDownList runat="server" ID="suspects" required="required" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="form-label required" >Transfer From</label>
                    <input class="form-control" id="from" required="required" runat="server" />
                </div>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="form-label required" >Transfer To</label>
                    <input class="form-control" id="to" required="required" runat="server" />
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="form-label required" >Transfer Date</label>
                    <input class="form-control" type="date" required="required" id="date" runat="server" />
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-md-12">
                <div class="form-group">
                    <label class="form-label required" >Transfer Reason</label>
                    <textarea rows="3" class="form-control" id="reason" runat="server"></textarea>
                </div>
            </div>
        </div>
        <div class="text-center">
            <asp:Button Text="Submit" ID="submit" CssClass="btn btn-success my-2" runat="server" OnClick="submit_Click" />
        </div>
    </div>
</asp:Content>