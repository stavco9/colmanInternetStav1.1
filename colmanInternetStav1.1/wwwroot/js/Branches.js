var jsonData = "";

$(document).ready(function () {
    window.addressValid = false;
    $("#AddressValid").html("");
    $("#AlertStoreCreate").html("");
});

function resetCoords() {
    document.getElementById("CoordinateLat").value = "";

    document.getElementById("CoordinateLng").value = "";
}

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

                $("#AddressValid").html("V");
                $("#AddressValid").css({ "color":"green"});

                window.addressValid = true;
            }
            else {
                $("#AddressValid").html("X");
                $("#AddressValid").css({ "color":"red"} );
                
                document.getElementById("CoordinateLat").value = "";

                document.getElementById("CoordinateLng").value = "";
            }
        });
    }
    else {
        $("#AddressValid").html("אנא הזן כתובת");
        $("#AddressValid").css({ "color": "red" });
       
        document.getElementById("CoordinateLat").value = "";

        document.getElementById("CoordinateLng").value = "";

    }
}

function ValidateInput() {
    var inputValid = true;

    var storeName = document.getElementById("Name").value;
    if (storeName.length <= 2 || /[\d]/.test(storeName)) {
        $("#AlertStoreCreate").html("שם החנות אינו תקין");

        inputValid = false;
    }
    else {
        $("#AlertStoreCreate").html("");
    }

    if (document.getElementById("CoordinateLat").value.length < 1 || document.getElementById("CoordinateLng").value.length < 1) {
        if (!window.addressValid) {
            inputValid = false;

            $("#AddressValid").html("יש לבדוק את תקינות הכתובת טרם היצירה");
            $("#AddressValid").css({ "color": "red" });
        }
    }

    return inputValid;
}