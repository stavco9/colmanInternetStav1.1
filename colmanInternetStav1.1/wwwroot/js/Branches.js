var jsonData = "";

window.addressValid = false;

function getCoorDinateByAddress() {

    window.addressValid = false;

    var baseUrl = "https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyBJhBoC3z-SpbcLaKO47ok67s8ZBz83n6w&address=";

    var inputAddr = document.getElementById('Address').value;

    if (inputAddr.length > 0) {
        baseUrl += inputAddr;

        $.getJSON(baseUrl, function (data) {
            jsonData = data;

            if (jsonData.status == "OK") {
                document.getElementById("CoordinateLat").value = jsonData.results[0].geometry.location.lat;

                document.getElementById("CoordinateLng").value = jsonData.results[0].geometry.location.lng;

                document.getElementById("AddressValid").value = "V";

                window.addressValid = true;
            }
            else {
                document.getElementById("AddressValid").value = "X";
            }
        });
    }
    else {
        document.getElementById("AddressValid").value = "אנא הזן כתובת";
    }
}

$(document).ready(function () {
    $('#CreateBranch').validate({
        rules: {
            required: true,
            minlength: 3
        },
        errorPlacement: function () {
            document.getElementById("AlertStoreCreate").value = "שם החנות אינו תקין";
        },
        success: function () {
            document.getElementById("AlertStoreCreate").value = "";
        }
    });

});

function ValidateInput() {
    var inputValid = true;

    var storeName = document.getElementById("Name").value;
    if (storeName.length <= 2 || /[\d]/.test(storeName)) {
        document.getElementById("AlertStoreCreate").value = "שם החנות אינו תקין";

        inputValid = false;
    }
    else {
        document.getElementById("AlertStoreCreate").value = "";
    }

    if (!window.addressValid) {
        inputValid = false;

        document.getElementById("AddressValid").value = "יש להזין כתובת";
    }

    return inputValid;
}