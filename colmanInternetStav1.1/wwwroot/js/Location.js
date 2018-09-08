﻿var map;
var lat_array = [32.163338,
    32.16468698029149, 32.1619890197085];
var lon_array = [34.840315,
    34.84166398029149, 34.83896601970849];

// loadScript
function loadScript() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyC13cyh_nyxTW2zg4j0dCgGF0HSz7LxQIA&callback=initialize";
    document.body.appendChild(script);
}

function getCoorDinateByAddress() {
    var jsonData = "";

    var baseUrl = "https://maps.google.com/maps/api/geocode/json?address=";

    var inputAddr = document.getElementById('address').value

    inputAddr = inputAddr.split(' ').join('+');
    inputAddr = inputAddr.split('-').join(",+");

    baseUrl += inputAddr;

    $.ajax({
        dataType: "json",
        data: { get_param: 'value' },
        url: baseUrl,
        success: function (data) {
            jsonData = data;
        }
    });

    //$.getJSON("https://maps.google.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA", function (data) {
    //    jsonData = data;
    //});

    window.onload = loadScript();
}

function initialize() {
    var bounds = new google.maps.LatLngBounds();
    var mapOptions = {
        zoom: 10,
        center: new google.maps.LatLng(32.163338, 34.840315)
    };

    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    // The marker
    var marker = new google.maps.Marker({ position: mapOptions.center, map: map });
}
