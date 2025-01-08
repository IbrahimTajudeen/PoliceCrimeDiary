<%@ Page Language="C#" Title="Reply Bail" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplyBail.aspx.cs" Inherits="Police_Crime_Diary.ReplyBail" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Reply Bail</h4>
        </div>
        <div style="width: 100%; max-height: 400px; overflow: auto;">
            <table class="table table-striped rounded mb-2">
                <thead>
                    <tr>
                        <th></th>
                        <th style="width: 200px;">Suspect Name</th>
                        <th  style="width: 150px;">Bail Amount</th>
                        <th  style="width: 150px;">Offered Amount</th>
                        <th  style="width: 200px;">Officer In-Charge</th>
                        <th  style="width: 100px;">Status</th>
                    </tr>
                </thead>
                <tbody runat="server" id="bail_crimes">
                    
                </tbody>
            </table>
        </div>
        <hr />
        <div class="action px-2" style="display: none;">
            <div>
                <h4 class="border-bottom pb-2 mb-2">Bail Action Details</h4>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Suspect Name</label>
                <label class="col fw-bold" id="name"></label>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Bail Amount</label>
                <label class="col fw-bold" id="amount"></label>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Offered Amount</label>
                <label class="col fw-bold" id="offer"></label>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Officer In-Charge</label>
                <label class="col fw-bold" id="incharge"></label>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Status</label>
                <label class="col fw-bold" id="status"></label>
            </div>
            <div>
                <p class="border-bottom pb-2 mb-2 fw-bold text-danger">Take Action on Bail</p>
            </div>
            <div class="mb-2">
                <div class="btn btn-info" id="ignore">Ignore</div>
                <div class="btn btn-secondary-outline" id="close">Close</div>
                <div class="btn btn-success" id="approve">Approve</div>
                <div class="btn btn-danger" id="reject">Rejected</div>
            </div>
        </div>
    </div>
    <script>
        $(function () {
            $curr_id = 0;
            $('.view-btn').on('click', function (e) {
                $curr_id = parseInt($(this).closest('tr').data('id'));
                $('#name').text($(this).closest('tr').children().eq(1).text())
                $('#amount').text($(this).closest('tr').children().eq(2).text())
                $('#offer').text($(this).closest('tr').children().eq(3).text())
                $('#incharge').text($(this).closest('tr').children().eq(4).text())
                $('#status').text($(this).closest('tr').children().eq(5).text())

                $('.action').slideDown();
            })

            function clear() {
                $('#name').text('')
                $('#amount').text('')
                $('#offer').text('')
                $('#incharge').text('')
                $('#status').text('')
            }

            $('#ignore, #close').on('click', function () {
                $curr_id = 0;
                clear();
                $('.action').slideUp();
            })

            reply = function (action, id) {
                if(id <= 0)
                {
                    alert("Could not find selection, try the selecting bail again")
                    return;
                }
                PoliceCrimeDiaryAPIService.ReplyBail(action, id, $('#name').text().trim(), parseInt($('#amount').text().trim()), parseInt($('#offer').text().trim()),
                    function (res) {
                        if(res != "")
                        {
                            $('#status').text(res)
                            $(`tr[data-id="${$curr_id}"`).children('td').eq(5).text(res);
                        }
                        else alert('Action failed to update, try the selecting bail again')
                    }, function (err) { alert(err._message) })
            }

            $('#approve').on('click', function () {
                reply('Approved', $curr_id)
            })

            $('#reject').on('click', function () {
                reply('Rejected', $curr_id)
            })
        })
    </script>
</asp:Content>