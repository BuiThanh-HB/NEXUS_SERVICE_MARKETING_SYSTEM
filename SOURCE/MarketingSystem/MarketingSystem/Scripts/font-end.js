$("#btn-register").click(function () {
    let phone = $("#phone").val().trim();
    let email = $("#email").val().trim();
    let name = $("#name").val().trim();
    let password = $("#password").val().trim();
    let confirm_password = $("#confirm_password").val().trim();
    let address = $("#address").val().trim();
    let province_id = $("#province-register").val();
    let district_id = $("#district-register").val();
    let village_id = $("#village-register").val();
    if (phone.length <= 0 || email.length <= 0 || password.length <= 0 || confirm_password.length <= 0 || address.length <= 0 || name.length<=0) {
        swal({
            title: "Mời điền đầy đủ thông tin!",
            text: "",
            icon: "warning"
        })
        return;
    }
    if (!/(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/.test(phone)) {
        swal({
            title: "Số điện thoại không hợp lệ!",
            text: "",
            icon: "warning"
        })
        return;
    }
    if (!/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(email)) {
        swal({
            title: "Email không hợp lệ!",
            text: "",
            icon: "warning"
        })
        return;
    }
    if (password != confirm_password) {
        swal({
            title: "Mật khẩu không trùng khớp!",
            text: "",
            icon: "warning"
        })
        return;
    }
    let data = {
        name,
        phone,
        email,
        password,
        address,
        province_id,
        district_id,
        village_id
    }
    $.ajax({
        url: "Register",
        type: "POST",
        data: {
            input: data
        }, success: function (res) {
            if (res.Code == 1) {
                swal({
                    title: "Thêm khách hàng thành công!",
                    text: "",
                    icon: "success"
                }).then(() => {
                    location.href = "Login";

                })
            }
        }
    })
})


function ChangProvince($target) {
    let province_id = $target.val();
    $.ajax({
        url: "GetListDistrict",
        type: "GET",
        data: {
            province_id: province_id
        }, success: function (res) {
            $("#district-register").empty();
            $.each(res, function () {
                $("#district-register").append('<option value="'+this.id+'">'+this.value+'</option>')
            })
            ChangeDistrict($("#district-register"))
        }
    })
}

function ChangeDistrict($target) {
    let district_id = $target.val();
    $.ajax({
        url: "GetListVillage",
        type: "GET",
        data: {
            district_id: district_id
        }, success: function (res) {
            $("#village-register").empty();
            $.each(res, function () {
                $("#village-register").append('<option value="' + this.id + '">' + this.value + '</option>')
            })
        }
    })
}

function Login() {
    let userName = $("#user-name").val().trim();
    let password = $("#password").val().trim();
    if (userName.length <= 0 || password.length <= 0) {
        swal({
            title: "Mời điền đầy đủ thông tin!",
            text: "",
            icon: "warning"
        })
        return;
    }
    $.ajax({
        url: "Login",
        type: "POST",
        data: {
            userName: userName,
            password: password
        },
        beforeSend: function () {
            //$("#modalLoad").modal("show");
        }
        , success: function (res) {
            if (res.Code == 1) {
                location.href = "Index";
            }
            else {
                swal({
                    title: res.Message,
                    text: "",
                    icon: "error"
                });
            }
        }
    })
}