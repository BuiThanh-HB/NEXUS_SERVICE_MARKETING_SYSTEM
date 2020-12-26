
$(document).ready(function () {

    GetSessionLogin();

    FocusTabMenu();

    $('.date').datepicker({
        dateFormat: "dd/mm/yy"
    });

    $(document).on("wheel", "input[type=number]", function (e) {
        $(this).blur();
    });


}); //end document.ready
//
function FocusTabMenu() {

    var url = window.location.pathname;

    switch (url) {
        case "/Home/Index":
            $('#tabHome').addClass('active');
            break;
        case "/Category/Index":
            $('#tabCategory').addClass('active');
            break;
        case "/User/Index":
            $('#tabUsers').addClass('active');
            break;
        default:
            break;
    }
}


//lấy thông tin đối tượng vừa đăng nhập
function GetSessionLogin() {
    $.ajax({
        url: '/Home/GetUserLogin',
        type: 'GET',
        success: function (response) {
            var role = response.Role;
            if (response.Role == -1) {
                swal({
                    title: "Hết phiên đăng nhập!",
                    text: "",
                    icon: "error"
                }).then((error) => {
                    if (error) {
                        window.location = "/Home/Login";
                    }
                });
            }
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });

}
