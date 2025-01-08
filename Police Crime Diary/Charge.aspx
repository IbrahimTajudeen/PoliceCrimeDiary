<%@ Page Language="C#" Title="Charge" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Charge.aspx.cs" Inherits="Police_Crime_Diary.Charge" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Charge <a href="~/ChargeList.aspx" runat="server" class="btn btn-sm btn-info float-end">Charge List</a></h4>
        </div>
        <div class="row mb-2 g-2">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="required form-label">Suspect Name</label>
                    <asp:DropDownList runat="server" ID="suspects" CssClass="form-control" required="required"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="required form-label">Officer In-Charge</label>
                    <asp:DropDownList runat="server" ID="officers" CssClass="form-control" required="required"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="required form-label">Date of Charge</label>
                    <input class="form-control" type="date" id="date" required="required" runat="server" />
                </div>
            </div>

            <div class="col-md-6">
                <div class="form-group">
                    <label class="required form-label">Evidence Upload</label>
                    <asp:FileUpload ID="files" runat="server" AllowMultiple="true" required="required" CssClass="form-control" />
                </div>
            </div>

            <div class="col-md-6">
                <div class="form-group">
                    <label class="required form-label">Court Name</label>
                    <input runat="server" id="court_name" class="form-control" />
                </div>
            </div>

            <div class="col-md-6">
                <div class="form-group">
                    <label class="required form-label">Court Date</label>
                    <input class="form-control" type="date" id="court_date" required="required" runat="server" />
                </div>
            </div>

            <div class="col-md-12">
                <div class="form-group">
                    <label class="form-label">Crime Description</label>
                    <textarea rows="3" class="form-control" runat="server" placeholder="select a suspect to see crime description" id="description" readonly></textarea>
                </div>
            </div>
            <div class="col-md-12">
                <div class="form-group">
                    <label class="required form-label">Charge Detials</label>
                    <textarea rows="3" class="form-control" id="details" required="required" runat="server" placeholder="enter charge details"></textarea>
                </div>
            </div>

            <div class="col-md-12">
                <div class="form-group">
                    <label class="required form-label">Status</label>
                    <asp:DropDownList runat="server" ID="status" CssClass="form-control">
                        <asp:ListItem Text="Charged" Selected="True" />
                        <asp:ListItem Text="In Court" />
                        <asp:ListItem Text="In Costody" />
                        <asp:ListItem Text="In Prison" />
                        <asp:ListItem Text="In Excuted" />
                    </asp:DropDownList>
                </div>
            </div>

        </div>
        <div class="text-center">
            <asp:Button ID="submit" runat="server" CssClass="btn btn-success my-3" OnClick="submit_Click" Text="Submit Charge" />
        </div>
    </div>
    <script>
        //MainContent_suspects
        $('#MainContent_suspects').on('change', function(){
            //$(this).val()
            if ($(this).val().trim() != '')
            {
                PoliceCrimeDiaryAPIService.GetCrimeDescription($(this).val(),
                    function (res) { $('#MainContent_description').val(res); }, function (err) { alert(err._message) })
            }
        })
    </script>
</asp:Content>