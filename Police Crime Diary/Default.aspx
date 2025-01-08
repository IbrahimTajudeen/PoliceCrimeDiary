<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Police_Crime_Diary._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <div class="container p-3 rounded" style="background-color: white;">
        <div class="mb-3">
            <div>
                <h4 class="border-bottom pb-2 mb-2">Recent Crime</h4>
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
                    <tbody id="recent_crimes" runat="server">
                    </tbody>
                </table>
            </div>
        </div>

        <div class="mb-3">
            <div>
                <h4 class="border-bottom pb-2 mb-2">Notification Alert Crime</h4>
            </div>
            <div runat="server" id="notify">

            </div>
        </div>
        

        <div class="mb-3">
            <div>
                <h4 class="border-bottom pb-2 mb-2">Quick Links</h4>
            </div>
            <div class="row g-4 mb-2" runat="server" id="user_links">
                <div class="col-6">
                    <a href="~/ReportCrime.aspx" class="btn btn-primary" runat="server">Report Crime</a>
                </div>
                <div class="col-6">
                    <a href="~/TodayCrime.aspx" class="btn btn-primary" runat="server">Today Crime</a>
                </div>
                <div class="col-6">
                    <a href="~/CrimeDiary.aspx" class="btn btn-primary" runat="server">Crime Diary</a>
                </div>
                <div class="col-6">
                    <a href="~/BailAction.aspx" class="btn btn-primary" runat="server">Bail Action</a>
                </div>
            </div>

            <div class="row g-4 mb-2" runat="server" id="high_links" style="display: none;">
                <div class="col-6">
                    <a href="~/Charge.aspx" class="btn btn-primary" runat="server">Charge</a>
                </div>
                <div class="col-6">
                    <a href="~/Transfer.aspx" class="btn btn-primary" runat="server">Transfer</a>
                </div>
                <div class="col-6">
                    <a href="~/ReadCrime.aspx" class="btn btn-primary" runat="server">Read Crime</a>
                </div>
                <div class="col-6">
                    <a href="~/ReplyBail.aspx" class="btn btn-primary" runat="server">Reply Bail</a>
                </div>
            </div>
        </div>

        <div class="mb-3">
            <div>
                <h4 class="border-bottom pb-2 mb-2">Current User</h4>
            </div>
            <div class="row g-4 mb-2">
                <div class="col-md-6">
                    <label class="form-label">Name</label>
                    <label class="form-control" runat="server" id="name"></label>
                </div>
                <div class="col-md-6">
                    <label class="form-label">Username</label>
                    <label class="form-control" id="username" runat="server"></label>
                </div>
                <div class="col-md-6">
                    <label class="form-label">User Role</label>
                    <label class="form-control" runat="server" id="userType"></label>
                </div>
                <div class="col-md-6">
                    <label class="form-label">Picture</label>
                    <img runat="server" id="userImg" src="#" style="width: 100px;" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(function () {
            loadRecentCrimes = () => {
                PoliceCrimeDiaryAPIService.RecentCrime((res) => {
                    $recent_body = $('#recent-crimes');
                    if(res.length > 0)
                    {   
                        if ($recent_body.children().length != res.length || $recent_body.children().eq(0).attr('colspan') == 3)
                            $recent_body.children().remove();

                        for (var i = 0; i < res.length; i++) {
                            if (res[i].ID == $recent_body.children().eq(i).data('id') && res[i].Name == $recent_body.children().eq(i).children().eq(0).text()
                                && res[i].CrimeType == $recent_body.children().eq(i).children().eq(1).text() && res[i].Location == $recent_body.children().eq(i).children().eq(2).text()
                                && i <= $recent_body.children().length)
                                continue;

                            else if((res[i].ID != $recent_body.children().eq(i).data('id') || res[i].Name != $recent_body.children().eq(i).children().eq(0).text()
                                || res[i].CrimeType != $recent_body.children().eq(i).children().eq(1).text() || res[i].Location != $recent_body.children().eq(i).children().eq(2).text())
                                && i <= $recent_body.children().length)
                            {
                                $recent_body.children().eq(i).data('id', res[i].ID)
                                $recent_body.children().eq(i).children().eq(0).text(res[i].Name)
                                $recent_body.children().eq(i).children().eq(1).text(res[i].CrimeType)
                                $recent_body.children().eq(i).children().eq(2).text(res[i].Location)
                            }
                            else {
                                $table_row = `<tr data-id="${res[i].ID}"><td>${res[i].Name}</td><td>${res[i].CrimeType}</td><td>${res[i].Location}</td></tr>`
                                $recent_body.append($table_row);
                            }
                        }
                    }
                    else {
                        $recent_body.children().remove();
                        $recent_body.append('<tr><td colspan="3" style="text-align: center; font-weight: bold;">No Recent Crime</td></tr>');
                    }
                },
                (err) => {
                    alert(err._message)
                })
            }

        })
    </script>
</asp:Content>
