var map;
var lat_array = [32.163338,
    32.16468698029149, 32.1619890197085];
var lon_array = [34.840315,
    34.84166398029149, 34.83896601970849];


function initialize() {
    var bounds = new google.maps.LatLngBounds();
    var mapOptions = {
        zoom: 10,
        center: new google.maps.LatLng(32.163338, 34.840315)
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);
    var count_total = lat_array.length;
    for (i = 0; i < count_total; i++) {
        (function () {
            var myLatlng = new google.maps.LatLng(lat_array[i], lon_array[i]);
            bounds.extend(myLatlng);
            var image = 'mapmarkers/you-are-here-2.png';
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: 'I am here'
            });
            var contentString = '<div id="content">' +
                '<div id="siteNotice">' +
                '</div>' +
                '<h1 id="firstHeading" class="firstHeading">' + lon_array[i] + '</h1>' +
                '<div id="bodyContent">' +
                '<p><b>ICAO:</b>' + lat_array[i] + '.</p>' +
                '</div>' +
                '</div>';
            var infowindow = new google.maps.InfoWindow
                (
                {
                    content: contentString
                }
                );

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });

            map.fitBounds(bounds);
        })();
    }
}

<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&callback=initialize"></script>
google.maps.event.addDomListener(window, 'load', initialize);