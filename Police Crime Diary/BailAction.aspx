<%@ Page Language="C#" Title="Bail Action" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BailAction.aspx.cs" Inherits="Police_Crime_Diary.BailAction" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container p-3 rounded" style="background-color: white;">
        <div>
            <h4 class="border-bottom pb-2 mb-2">Bail Action</h4>
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
        <div class="px-2 action" style="display: none;">
            <div>
                <h4 class="border-bottom pb-2 mb-2">Bail Action Details</h4>
            </div>
            <div class="text-end p-2">
                <img src="#" alt="Suspect Picture" class="border rounded-2" style="height: 100px; width: 100px;" />
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Suspect Name</label>
                <label class="col fw-bold form-control" id="name"></label>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Bail Amount</label>
                <label class="col fw-bold form-control" id="amount"></label>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Offered Amount</label>
                <input class="col form-control" id="offer" placeholder="enter amount to bail" />
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Officer In-Charge</label>
                <label class="col fw-bold form-control" id="incharge"></label>
            </div>
            <div class="row mb-2">
                <label class="col-4 fw-bold">Status</label>
                <label class="col fw-bold form-control" id="status"></label>
            </div>
            <div>
                <p class="border-bottom pb-2 mb-2 fw-bold text-danger">Take Action</p>
            </div>
            <div class="mb-2">
                <div class="btn btn-success" id="_bail_">Bail</div>
                <div class="btn btn-danger" id="close">Close</div>
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

            $('#close').on('click', function () {
                $curr_id = 0;
                clear();
                $('.action').slideUp();
            })

            $('#_bail_').on('click', function () {
                if ($curr_id <= 0) {
                    alert("Could not find selection, try the selecting bail again")
                    return;
                }
                if ($('#offer').val().trim().length == 0)
                {
                    $('#offer').focus();
                    alert("Invalid amount offered")
                    return;
                }

                PoliceCrimeDiaryAPIService.ApplyBail($curr_id, $('#offer').val().trim(),
                    function (res) {
                        if (res != "") {
                            alert(res)
                            $('#status').text(res)
                            $(`tr[data-id="${$curr_id}"`).children('td').eq(5).text(res);
                        }
                        else alert('Failed to Apply for bail, try the selecting bail again or refereshing the page again')

                    },
                    function (err) { alert(err._message) })
            })
        })
    </script>
</asp:Content>