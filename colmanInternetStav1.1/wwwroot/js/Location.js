$(document).ready(function () {
    window.onload = loadScript();
});

// loadScript
function loadScript() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyC13cyh_nyxTW2zg4j0dCgGF0HSz7LxQIA&callback=initialize";
    document.body.appendChild(script);
}

function initialize() {
    var bounds = new google.maps.LatLngBounds();

    var mapOptions = {
        zoom: 15,
        center: new google.maps.LatLng(document.getElementById("CoordinateLat").value, document.getElementById("CoordinateLng").value)
    };

    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    // The marker
    var marker = new google.maps.Marker({ position: mapOptions.center, map: map });
}

