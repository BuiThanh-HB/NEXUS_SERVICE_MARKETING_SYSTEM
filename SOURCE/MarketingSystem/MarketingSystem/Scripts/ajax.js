$(document).ready(function () {
    //tự động chọn option có cùng giá trị
    var typeNews = $("#cbbType").attr("value");
    $("#cbbType option").each(function () {
        if (typeNews == $(this).val()) {
            $(this).attr('selected', 'selected');
        }
    });

    var typeSendNews = $("#cbbTypeSend").attr("value");
    $("#cbbTypeSend option").each(function () {
        if (typeSendNews == $(this).val()) {
            $(this).attr('selected', 'selected');
        }
    });

    //clear text when close modal
    $('.modal').on('hidden.bs.modal', function () {
        $(this).find("input,textarea").val('');
    });

    //change option in Combobox
    $('#status').on("change", function () {
        searchWarrantyCard();
    });

    $('#type').on('change', function () {
        searchPoint();
    });

    $('#itemStatus').on('change', function () {
        SearchItem();
    });

    //auto trim input text
    $('input[type="text"]').change(function () {
        this.value = $.trim(this.value);
    });

    $('#place').on('change', function () {
        LoadPlaceCreateShop();
    });

    //auto format number input
    $('.number').keyup(function () {
        $val = cms_decode_currency_format($(this).val());
        $(this).val(cms_encode_currency_format($val));
    });
}); //end document.ready


const SUCCESS = 1;
const ERROR = 0;
const DUPLICATE_NAME = 2;
const CAN_NOT_DELETE = 2;
const WRONG_PASSWORD = 2;
const NOT_ADMIN = 3;
const EXISTING = 2;
const FAIL_LOGIN = 2;
const ERROR_QUERY = 501;
const URL_ADD_IMG_DEFAULT = "/Uploads/files/add_img.png";


//đăng nhập
function Login() {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    var phone = $("#txtUsernameLogin").val();
    var password = $("#txtPasswordLogin").val();
    if (phone == "" || password == "") {
        swal({
            title: "Vui lòng nhập đầy đủ!",
            text: "",
            icon: "warning"
        })
        return;
    }
    $.ajax({
        url: '/Home/UserLogin',
        data: { account: phone, password: password },
        type: 'POST',
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            if (res.Status == SUCCESS) {
                window.location = "/Home/Index";
            } else {
                swal({
                    title: res.Message,
                    text: "",
                    icon: "error"
                })
            }
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function logout() {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    $.ajax({
        url: '/Home/Logout',
        data: {},
        type: 'POST',
        success: function (response) {
            if (response == SUCCESS) {
                location.reload();
            }
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}


// start thông tin chi tiết 1 người dùng
function GetUserDetail(data) {

    //Lấy các thông tin người dùng
    var id = data.attr("data-id");
    var name = data.attr("data-name");
    var phone = data.attr("data-phone");


    //Đặt các giá trị vào các thẻ input tương ứng
    $('#txt-name-edit').val(name);
    $('#txt-phone-edit').val(phone);
    $('#txt-phone-edit').attr('data-id', id);
    $('#user-detail').modal('show');
}


function isSpace(string) {
    if (/\s/.test(string)) {
        return true;
    }
}

function isNumeric(s) {
    var re = new RegExp("^[0-9,]+$");
    return re.test(s);
}

// start xóa user
function deleteUser(id) {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    swal({
        title: "Bạn chắc chắn xóa chứ?",
        text: "",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: '/User/DeleteUser',
                data: { ID: id },
                type: "POST",
                beforeSend: function () {
                    $('#modalLoad').modal('show');
                },
                success: function (response) {
                    if (response.Status == SUCCESS) {
                        $('#modalLoad').modal('hide');
                        toastr.success("Xóa thành công !");
                        $("#btnSeacrhUser").click();

                    } else {
                        $('#modalLoad').modal('hide');
                        swal({
                            title: res.Message,
                            text: "",
                            icon: "error"
                        });
                    }
                },
                error: function (result) {
                    console.log(result.responseText);
                }
            });
        }
    })
}// end xóa user

function updateUserInfo() {
    var id = $('#txt-phone-edit').attr('data-id');
    var name = $.trim($('#txt-name-edit').val())
    var phone = $.trim($('#txt-phone-edit').val());
    var password = $.trim($('#txt-password-edit').val());

    if (name.length == 0 || phone.length == 0) {
        swal({
            title: "Mời nhập đầy đủ thông tin!",
            text: "",
            icon: "warning"
        })
        return;
    }

    if (!/^0[1-9]{1}[0-9]{8}$/.test(phone)) {
        swal({
            title: "Số điện thoại không đúng định dạng!",
            icon: "warning"
        });
    }

    if (password.length > 0 && password.length < 6) {
        swal({
            title: "Mật khẩu ít nhất phải có 6 ký tự!",
            icon: "warning"
        });
        return;
    }

    $.ajax({
        url: "/User/UpdateUserInfo",
        data: { id: id, userName: name, userPhone: phone, password: password },
        type: 'POST',
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            if (res.Status == SUCCESS) {
                $('#user-detail').modal('hide');
                $('#modalLoad').modal('hide');
                toastr.success("Cập nhật thông tin thành công !");
                $("#btnSeacrhUser").click();
            } else {
                $('#modalLoad').modal('hide');
                swal({
                    title: res.Message,
                    text: "",
                    icon: "error"
                });
            }
        }
    })
}


//Tìm kiếm danh mục
function SearchCate() {
    var searchKey = $.trim($('#txt-search-key').val());
    var fromDate = $('#txt-from-date').val();
    var toDate = $('#txt-to-date').val();
    var type = $('#type-value').val();

    $.ajax({
        url: "/Category/Search",
        data: { page: 1, searchKey: searchKey, fromDate: fromDate, toDate: toDate, type: type },
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        type: "GET",
        success: function (res) {
            $('#modalLoad').modal('hide');
            $('#tbl-cate').html(res);
        }
    })
}

//Thêm danh mục gói cước

function AddCategory() {
    var name = $.trim($('#txt-name').val());
    var type = $("#type-value-add").val();
    if (name.length == 0) {
        swal({
            title: "Tên danh mục không được bỏ trống !",
            icon: "warning"
        })
        return;
    }

    $.ajax({
        url: "/Category/AddCategory",
        data: { name: name, type: type },
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        type: "POST",
        success: function (res) {
            $('#modalLoad').modal('hide');

            if (res.Status == SUCCESS) {
                toastr.success("Thêm mới danh mục thành công !");
                $('#add-category').modal('hide');
                SearchCate();
            } else {
                swal({
                    title: res.Message,
                    text: "",
                    icon: "error"
                });
            }
        }
    })
}

//Show chi tiết danh mục
function GetCategoryDetail(data) {
    //Lấy các thông tin danh mục
    var id = data.attr("data-id");
    var name = data.attr("data-name");
    var type = data.attr("data-type");

    //Đưa các giá trị tương ứng vào modal detail
    $('#txt-name-edit').val(name);
    $('#type-value-edit').val(type);
    $('#id-value').val(id);

    //Show modal
    $('#detail-category').modal('show');

}

//Cập nhật thông tin danh mục
function UpdateCategory() {
    var name = $.trim($('#txt-name-edit').val());
    var id = $('#id-value').val();
    var type = $('#type-value-edit').val();

    if (name.length == 0) {
        swal({
            title: "Tên danh mục không được bỏ trống !",
            icon: "warning"
        })
        return;
    }

    $.ajax({
        url: "/Category/UpdateCategory",
        type: "POST",
        data: { id: id, name: name, type: type },
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            if (res.Status == SUCCESS) {
                toastr.success("Cập nhật danh mục thành công !");
                $('#detail-category').modal('hide');
                SearchCate();
            } else {
                swal({
                    title: res.Message,
                    text: "",
                    icon: "error"
                });
            }
        }
    })
}

function DelCate(id) {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    swal({
        title: "Bạn chắc chắn xóa chứ?",
        text: "",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: '/Category/DelCate',
                data: { ID: id },
                type: "POST",
                beforeSend: function () {
                    $('#modalLoad').modal('show');
                },
                success: function (response) {
                    if (response.Status == SUCCESS) {
                        $('#modalLoad').modal('hide');
                        toastr.success("Xóa thành công !");
                        SearchCate();

                    } else {
                        $('#modalLoad').modal('hide');
                        swal({
                            title: res.Message,
                            text: "",
                            icon: "error"
                        });
                    }
                },
                error: function (result) {
                    console.log(result.responseText);
                }
            });
        }
    })
}

function SearchServicePlan() {
    var searchKey = $.trim($('#txt-search-key').val());
    var fromDate = $('#txt-from-date').val();
    var toDate = $('#txt-to-date').val();
    var status = $('#status-value').val();
    var cateID = $('#cateID-value').val();
    var Status = status == 1 ? true : false;
    if (status < 0)
        Status = null;

    $.ajax({
        url: "/ServicePlan/Search",
        data: { page: 1, searchKey: searchKey, fromDate: fromDate, toDate: toDate, status: Status, cateID: cateID },
        type: "GET",
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            $('#tbl-service-plan').html(res);
        }
    })
}