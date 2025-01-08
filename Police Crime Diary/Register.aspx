<%@ Page Language="C#" Title="Register" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.cs" Inherits="Police_Crime_Diary.Register" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:PlaceHolder ID="display_message" runat="server"></asp:PlaceHolder>
    </div>
    <br />
    <div class="container rounded p-5" id="page-1" style="background-color: white;">
        <div class="mb-4">
            <h4 class="border-bottom pb-2 mb-2">Registration Form</h4>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="name" class="fs-6">Name (in Full): </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <input type="text" id="name" runat="server" required="required" placeholder="enter name (in full)" class="form-control w-100" />
                <p class="validate text-danger fs-6 text-start"></p>
            </div>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="sex" class="fs-6">Sex: </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <select id="sex" runat="server" required="required" class="form-control w-100">
                    <option value="---">---</option>
                    <option value="Male">Male</option>
                    <option value="Female">Female</option>
                </select>
                <p class="validate text-danger fs-6 text-start"></p>
            </div>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="address" class="fs-6">Address: </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <textarea id="address" placeholder="enter address" runat="server" required="required" rows="3" class="form-control w-100"></textarea>
                <p class="validate text-danger fs-6 text-start"></p>
            </div>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="email" class="fs-6">Email: </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <input type="email" id="email" required="required" runat="server" placeholder="enter email address" class="form-control w-100" />
                <p class="validate text-danger fs-6 text-start"></p>
            </div>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="phone" class="fs-6">Phone Number(s): </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <input type="tel" id="phone" runat="server" required="required" placeholder="enter phone number" class="form-control w-100 mb-2" />
                <input type="tel" id="phone2" runat="server" placeholder="enter phone number" class="form-control w-100" />
                <p class="validate text-danger fs-6 text-start"></p>
            </div>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="picture" class="fs-6">Picture: </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <asp:FileUpload ID="picture" required="required" runat="server" accept="image/*" CssClass="form-control w-100 mb-2" />
                <p class="validate text-danger fs-6 text-start"></p>
                <div class="btn btn-primary mb-2" id="upload">Preview Image</div><br />
                <img src="#" alt="Uploade Image" id="image" style="width: 100px; height: 150px;" />
            </div>
        </div>
        
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="phone" class="fs-6">Mode of Identification: </label>
            </div>
            <div class="col-7 p-0 mx-2 text-start">
                <asp:RadioButtonList runat="server" ID="MOID">
                    <asp:ListItem Text="National Passport" Value="National Passport" Selected="True" />
                    <asp:ListItem Text="National ID Card" Value="National ID Card" />
                    <asp:ListItem Text="Driver's License" Value="Driver's License" />
                    <asp:ListItem Text="Any National Bank" Value="Any National Bank" />
                </asp:RadioButtonList>

                <div class="form-group mt-4">
                    <label class="form-label" id="mode_label">National Passport No:</label>
                    <input type="text" id="mode_number" runat="server" class="form-control w-100" placeholder="enter National Passport No." />
                    <p class="validate text-danger fs-6 text-start"></p>
                </div>
                <div class="btn btn-primary mt-2" id="contine">Continue</div>
            </div>
        </div>
    </div>
    
    <div class="container rounded p-3" style="display: none; background-color: white;" id="second">
        <h4 class="text-success text-center lead fw-bold mb-2">One Step More. Please Fill and become a user.</h4>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="phone" class="fs-6">Username: </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <input type="text" id="username" runat="server" required="required" placeholder="enter username"  class="form-control w-100" />
                <p class="validate text-danger fs-6 text-start"></p>
            </div>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="phone" class="fs-6">Pasword: </label>
            </div>
            <div class="col-7 p-0 mx-2 text-end">
                <input type="text" id="password" required="required" runat="server" placeholder="enter password"  class="form-control w-100" />
                <p class="validate text-danger fs-6 text-start"></p>
            </div>
        </div>
        <div class="row p-0 mb-2">
            <div class="col-4 p-0">
                <label for="phone"class="fs-6">Your PCD PIN is: </label>
            </div>

            <div class="col-offset-4 col-7 p-0 mx-2 text-start">
                <label id="PCD" runat="server" class="form-control w-100" />
                <div class="btn btn-primary mt-2" id="back">Previous Page</div> 
                <asp:Button ID="register" runat="server" CssClass="btn btn-success mt-2" OnClick="register_Click" Text="Register" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            let uploaded = false;
            $user = {
                Name: '', Sex: '', Address: '', Email: '',
                Phone: '', Picture: '', Mode: { Type: '', Number: '' },
                Username: '', Password: ''
            }

            $picture = "";
            $('#contine').on('click', function () {
                $valid = true;
                $user.Name = $('#MainContent_name').val().trim()
                $user.Sex = $('#MainContent_sex').val().trim()
                $user.Address = $('#MainContent_address').val().trim()
                $user.Email = $('#MainContent_email').val().trim()
                $user.Phone = $('#MainContent_phone').val() + ($('#MainContent_phone2').val().trim().length > 0 ? '/' + $('#MainContent_phone2').val().trim() : '');
                $user.Mode.Type = $('#MainContent_MOID').filter((v, e) => $(e).children('input').prop('checked') == true).eq(0).children('span').eq(0).text();
                $user.Mode.Number = $('#MainContent_mode_number').val().trim();

                if ($user.Name == '')
                {
                    $('#MainContent_name').next('.validate').text('Invalid name entered')
                    $valid = false
                }
                else $('#MainContent_name').next('.validate').text('')

                if ($user.Sex == '' || ['male', 'female'].indexOf($user.Sex.toLowerCase()) == -1) {
                    $('#MainContent_sex').next('.validate').text('Invalid sex selected')
                    $valid = false
                }
                else $('#MainContent_sex').next('.validate').text('')

                if ($user.Address == '') {
                    $('#MainContent_address').next('.validate').text('Invalid address entered')
                    $valid = false
                }
                else $('#MainContent_address').next('.validate').text('')

                if ($user.Email == '') {
                    $('#MainContent_email').next('.validate').text('Invalid email address entered')
                    $valid = false
                }
                else $('#MainContent_email').next('.validate').text('')
                
                if ($user.Phone == '' || [11, 14, 23, 29, 26].indexOf($user.Phone.length) == -1) {
                    $('#MainContent_phone2').next('.validate').text('Invalid phone number entered')
                    $valid = false
                }
                else $('#MainContent_phone2').next('.validate').text('')

                let fileInput = document.getElementById('MainContent_picture');
                if (fileInput.files.length == 0) {
                    $('#MainContent_picture').next('.validate').text('Invalid picture selected')
                    return;
                }
                $('#MainContent_picture').next('.validate').text('')


                if ($user.Mode.Number == '') {
                    $('#MainContent_mode_number').next('.validate').text('Invalid mode of identification entered')
                    $valid = false
                }
                else $('#MainContent_mode_number').next('.validate').text('')
                
                if ($valid)
                {
                    $('#page-1').hide()
                    $('#second').show();
                    $('#MainContent_PCD').text([Math.floor(Math.random() * 9), Math.floor(Math.random() * 9), Math.floor(Math.random() * 9), Math.floor(Math.random() * 9)].join(""))
                }

            })

            $('#back').on('click', function () {
                $('#page-1').show()
                $('#second').hide();
            })

            $('#MainContent_MOID').on('click', function (e) {
                
                $text = $(e.target).closest('tr').find('label').text() + ' No.'
                $('#mode_label').text($text)
                $('#MainContent_mode_number').attr('placeholder', `enter ${$text}`);
            })

            $('#upload').on('click', function () {
                let fileInput = document.getElementById('MainContent_picture');
                if (fileInput.files.length == 0)
                {
                    $('#MainContent_picture').next('.validate').text('Invalid picture selected')
                    return;
                }
                $('#MainContent_picture').next('.validate').text('')

                let file = fileInput.files[0];
                let reader = new FileReader();

                reader.readAsDataURL(file);
                reader.onloadend = () => {
                    $('#image').prop('src', reader.result);
                    uploaded = true;
                }
            })

            $("#MainContent_register").on('click', function () {
                $user.Username = $('#MainContent_username').val().trim();
                $user.Password = $('#MainContent_password').val().trim();
                $valid = true;
                if ($user.Username == '') {
                    $('#MainContent_username').next('.validate').text('Invalid username entered')
                    $valid = false
                }
                else $('#MainContent_username').next('.validate').text('')

                if ($user.Password == '' || $user.Password.length <= 5) {
                    $('#MainContent_password').next('.validate').text('Invalid password entered. Password must be aleast 6 - 8 characters')
                    $valid = false
                }
                else $('#MainContent_password').next('.validate').text('')
            })
        })

        

    </script>
</asp:Content>