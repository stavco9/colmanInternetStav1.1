var map;
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
    window.jsonData = "";

    var baseUrl = "https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyBJhBoC3z-SpbcLaKO47ok67s8ZBz83n6w&address=";

    var inputAddr = document.getElementById('address').value;

    baseUrl += inputAddr;

    $.getJSON(baseUrl, function (data) {
        window.jsonData = data;
    });

    window.onload = loadScript();
}

function initialize() {
    var bounds = new google.maps.LatLngBounds();

    //var coordinates = getCoorDinateByAddress();

    var mapOptions = {
        zoom: 15,
        center: new google.maps.LatLng(window.jsonData.results[0].geometry.location.lat, window.jsonData.results[0].geometry.location.lng)
        //center: new google.maps.LatLng(32.153646346, 34.435236236)
    };

    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    // The marker
    var marker = new google.maps.Marker({ position: mapOptions.center, map: map });
}

