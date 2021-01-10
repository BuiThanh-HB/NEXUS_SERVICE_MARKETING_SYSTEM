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

function cms_decode_currency_format(obs) {
    if (obs == '')
        return '';
    else
        return parseInt(obs.replace(/,/g, ''));
}
function cms_encode_currency_format(num) {
    if (!isNumeric(num)) {
        return '';
    }
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
}

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
    var status = $('#status-value-search').val();
    var cateID = $('#cateID-value-search').val();
    var Status = status == 1 ? true : false;
    if (status > 1 || status == null)
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

function AddServicePlan() {
    var name = $('#txt-name').val();
    var price = parseInt($('#txt-price').val().replace(/,/g, ''));
    var value = parseInt($('#value').val());
    var status = $('#status-value-add').val();
    var Status = status == 1 ? true : false;
    var img = $('#img-add').attr('src');

    if (name.length == 0 || typeof img === "undefined") {
        swal({
            title: "Vui lòng nhập đầy đủ thông tin!",
            icon: "warning"
        })
        return;
    }

    if (price <= 0 || value <= 0) {
        swal({
            title: "Giá gói cước và giá trị tháng phải lớn hơn 0",
            icon: "warning"
        })
        return;
    }

    $.ajax({
        url: "/ServicePlan/AddServicePlan",
        data: $('#frm-add-service-plan').serialize() + "&Price=" + price + "&Status=" + Status + "&ImageUrl=" + img,
        type: "POST",
        beforeSend: function () {
            $('#modalLoad').modal('show');
            $('#add-service-plan').modal('hide');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            if (res.Status == SUCCESS) {
                toastr.success("Thêm mới gói cước thành công !");
                setTimeout(function () { location.reload(); }, 2000)
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

//Chi tiết gói cước
function GetServicePlanDetail(data) {
    var status = data.attr('data-status');
    var id = data.attr('data-id');
    var name = data.attr('data-name');
    var img = data.attr('data-image');
    var descreiption = data.attr('data-descreiption');
    var cateID = data.attr('data-cateID');
    var value = data.attr('data-value');
    var price = data.attr('data-price');
    var Status = Boolean(status) ? 1 : 0;
    price = cms_encode_currency_format(price);

    //Đưa các giá trị vào các input tương ứng
    $('#txt-name-edit').val(name);
    $('#val-id').val(id);
    $('#txt-price-edit').val(price);
    $('#value-edit').val(value);
    $('#cateID-value-edit').val(cateID);
    $('#status-value-edit').val(Status);
    $('#txt-description-edit').val(descreiption);
    $('#div-img-edit').append('<div style ="margin-top:10px;"><img id="val-img-edit" class=" cursor-pointer" width="100%" height="100%" src="' + img + '" onclick="ChangeImg($(this));" /></div>');

    $('#service-plan-detail').modal('show');
}

//Cập nhật thông tin gói cước
function UpdateServicePlan() {
    var name = $.trim($('#txt-name-edit').val());
    var price = parseInt($('#txt-price-edit').val().replace(/,/g, ''));
    var value = parseInt($('#value-edit').val());
    var status = $('#status-value-edit').val()
    var Status = status == 1 ? true : false;
    var img = $('#val-img-edit').attr('src');

    if (name.length == 0 || typeof img === "undefined") {
        swal({
            title: "Vui lòng nhập đầy đủ thông tin!",
            icon: "warning"
        })
        return;
    }

    if (price <= 0 || value <= 0) {
        swal({
            title: "Giá gói cước và giá trị tháng phải lớn hơn 0",
            icon: "warning"
        })
        return;
    }

    $.ajax({
        url: "/ServicePlan/UpdateServicePlan",
        data: $('#frm-edit-service-plan').serialize() + "&Price=" + price + "&Status=" + Status + "&ImageUrl=" + img,
        type: "POST",
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            $('#service-plan-detail').modal('hide');
            if (res.Status == SUCCESS) {
                toastr.success("Cập nhật thông tin gói cước thành công !");
                setTimeout(function () { location.reload(); }, 2000)
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

function DelServicePlan(id) {
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
                url: '/ServicePlan/DelServicePlan',
                data: { id: id },
                type: "POST",
                beforeSend: function () {
                    $('#modalLoad').modal('show');
                },
                success: function (response) {
                    if (response.Status == SUCCESS) {
                        $('#modalLoad').modal('hide');
                        toastr.success("Xóa thành công !");
                        SearchServicePlan();

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

function SearchNews() {
    var searchKey = $.trim($('#txt-search-key').val());
    var fromDate = $('#txt-from-date').val();
    var toDate = $('#txt-to-date').val();
    var status = $('#status-value-search').val();
    var type = $('#type-value-search').val();
    var Status = status == 1 ? true : false;
    if (status < 1 || status == null)
        Status = null;

    $.ajax({
        url: "/News/Search",
        data: { page: 1, searchKey: searchKey, fromDate: fromDate, toDate: toDate, status: Status, type: type },
        type: "GET",
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            $('#tbl-news').html(res);
        }
    })
}


function CreateNews() {
    var content = $.trim(CKEDITOR.instances['txt-content'].getData());
    var type = $('#type-value').val();
    var title = $.trim($('#txt-title').val());
    var summary = $.trim($('#txt-summary').val());
    var img = $('#img-add').attr('src');
    var status = $('#status-value').val();
    var Status = status == 1 ? true : false;

    if (content.length == 0 || title.length == 0 || summary.length == 0) {
        swal({
            title: "Vui lòng nhập đầy đủ thông tin",
            icon:"warning"
        })
        return;
    }

    if (typeof img === "undefined") {
        swal({
            title: "Vui lòng chọn ảnh đại diện cho bài viết",
            icon: "warning"
        })
        return;
    }

    $.ajax({
        url: "/News/CreateNews",
        data: { type: type, content: content, summary: summary, title: title, img: img, status: Status },
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        type: "POST",
        success: function (res) {
            $('#modalLoad').modal('hide');
            if (res.Status == SUCCESS) {
                toastr.success("Thêm bài viết thành công !");
                setTimeout(function () { window.location = "/News/Index"; }, 2000)
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

//Cập nhật bài viết
function UpdateNews(id) {
    var content = $.trim(CKEDITOR.instances['txt-content'].getData());
    var type = $('#type-value').val();
    var title = $.trim($('#txt-title').val());
    var summary = $.trim($('#txt-summary').val());
    var img = $('#img-add').attr('src');
    var status = $('#status-value').val();
    var Status = status == 1 ? true : false;

    if (content.length == 0 || title.length == 0 || summary.length == 0) {
        swal({
            title: "Vui lòng nhập đầy đủ thông tin",
            icon: "warning"
        })
        return;
    }

    if (typeof img === "undefined") {
        swal({
            title: "Vui lòng chọn ảnh đại diện cho bài viết",
            icon: "warning"
        })
        return;
    }

    $.ajax({
        url: "/News/UpdateNews",
        data: { id: id, type: type, content: content, summary: summary, title: title, img: img, status: Status },
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        type: "POST",
        success: function (res) {
            $('#modalLoad').modal('hide');
            if (res.Status == SUCCESS) {
                toastr.success("Cập nhật bài viết thành công !");
                setTimeout(function () { window.location = "/News/Index"; }, 2000)
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

//Xóa bài viết
function DelNews(id) {
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
                url: '/News/DelNews',
                data: { id: id },
                type: "POST",
                beforeSend: function () {
                    $('#modalLoad').modal('show');
                },
                success: function (response) {
                    if (response.Status == SUCCESS) {
                        $('#modalLoad').modal('hide');
                        toastr.success("Xóa thành công !");
                        SearchNews();

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

function SearchCus() {
    var searchKey = $.trim($('#txt-search-key').val());
    var fromDate = $('#txt-from-date').val();
    var toDate = $('#txt-to-date').val();
    $.ajax({
        url: "/Customer/Search",
        data: { page: 1, searchKey: searchKey, fromDate: fromDate, toDate: toDate},
        type: "GET",
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            $('#tbl-customer').html(res);
        }
    })
}

function SearchOrder() {
    var searchKey = $.trim($('#txt-search-key').val());
    var fromDate = $('#txt-from-date').val();
    var toDate = $('#txt-to-date').val();
    var status = $('#status-value-search').val();

    $.ajax({
        url: "/Order/Search",
        data: { page: 1, searchKey: searchKey, fromDate: fromDate, toDate: toDate, status: status },
        type: "GET",
        beforeSend: function () {
            $('#modalLoad').modal('show');
        },
        success: function (res) {
            $('#modalLoad').modal('hide');
            $('#tbl-order').html(res);
        }
    })
}