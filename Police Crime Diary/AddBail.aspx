<%@ Page Language="C#" Title="Give Bail" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddBail.aspx.cs" Inherits="Police_Crime_Diary.AddBail" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Bail Form <a href="~/BailMenu.aspx" runat="server" class="btn btn-sm btn-info float-end">Bail Menu</a></h4>
        </div>
        <div class="row">
            <div class="col-md-6 my-2">
                <div class="form-group">
                    <label class="form-label required">Suspect Name</label>
                    <asp:DropDownList runat="server" required="required" ID="suspects" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-md-6 my-2">
                <div class="form-group">
                    <label class="form-label required">Officer Incharge</label>
                    <asp:DropDownList runat="server" required="required" ID="officer" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-md-12 my-2">
                <div class="form-group">
                    <label class="form-label required">Bail Amount</label>
                    <input type="number" class="form-control" required="required" runat="server" id="bail_amount" />
                </div>
            </div>
        </div>
        <div class="text-center border-top">
            <asp:Button Text="Submit Bail" ID="add_bail" runat="server" OnClick="add_bail_Click" CssClass="btn btn-success my-3" />
        </div>
    </div>
</asp:Content>