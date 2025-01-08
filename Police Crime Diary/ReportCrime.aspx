<%@ Page Language="C#" Title="Report Crime" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportCrime.aspx.cs" Inherits="Police_Crime_Diary.ReportCrime" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <br />
    <div class="container mb-3 pb-3 rounded px-2" style="background-color: white;">
        <h4 class="text-center mb-3 p-3 border-bottom">Enter Suspect's Details</h4>
        
        <div class="row mb-2">
            <div class="col-md-6">
                 <div class="form-group">
                     <label class="form-label required">Suspect Name (in full)</label>
                     <input class="form-control" runat="server" required="required" id="suspect_name" placeholder="Enter suspect name" />
                 </div>
            </div>
            <div class="col-md-6">
                 <div class="form-group">
                     <label class="form-label required">Suspect Gender</label>
                     <select class="form-control w-100" runat="server" required="required" id="suspect_gender">
                         <option value=""></option>
                         <option value="Male">Male</option>
                         <option value="Female">Female</option>
                     </select>
                 </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-md-6">
                 <div class="form-group">
                     <label class="form-label required">Type of Crime</label>
                     <input class="form-control w-100" runat="server" required="required" id="suspect_crime_type" placeholder="Enter type of crime" />
                 </div>
            </div>
            <div class="col-md-6">
                 <div class="form-group">
                     <div class="form-group">
                         <label class="form-label required">Evidence</label>
                         <input class="form-control w-100" runat="server" id="evidence" required="required" placeholder="Enter evidence" />
                     </div>
                 </div>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-md-6">
                 <div class="form-group">
                     <label class="form-label required">Date of Crime</label>
                     <input class="form-control w-100" runat="server" required="required" id="crime_date" type="date"/>
                 </div>
            </div>
            <div class="col-md-6">
                 <div class="form-group">
                     <div class="form-group">
                         <label class="form-label required">Time of Crime</label>
                         <input class="form-control w-100" runat="server" required="required" id="crime_time" type="time" />
                     </div>
                 </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-md-6">
                 <div class="form-group">
                     <label class="form-label required">Location of Crime</label>
                     <input class="form-control" runat="server" id="location" required="required" />
                 </div>
            </div>
            <div class="col-md-6">
                 <div class="form-group">
                     <label class="form-label">Upload Suspect Picture <i>(if available)</i></label>
                     <asp:FileUpload AllowMultiple="false" CssClass="form-control w-100" accept="image/*" ID="suspect_picture" runat="server" />
                 </div>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col">
                 <div class="form-group">
                     <label class="form-label">Upload Any Other Related Documents <i>(if available)</i></label>
                     <asp:FileUpload AllowMultiple="true" CssClass="form-control w-100" ID="related_document" runat="server" />
                 </div>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col">
                 <div class="form-group">
                     <label class="form-label required">Description</label>
                     <textarea rows="4" class="form-control w-100" runat="server" id="description" placeholder="Enter extra detailed description"></textarea>
                 </div>
            </div>
        </div>
        
        <p class="mt-2 lead fs-6 text-danger"><b>NOTE!:</b> <i>Please double check the details you entered before submiting.</i></p>
        <div class="p-2 text-center">
            <asp:Button runat="server" ID="submit" CssClass="btn btn-success mx-auto" Text="Submit Crime" OnClick="submit_Click" />
        </div>
    </div>
    <div class="py-3">Nigeria Police Force : @All Rights Reserved</div>
    <script>
        $(function () {
            $('.close').on('click', function () {
                $(this).closest('div').remove()
            })
        })
    </script>
</asp:Content>