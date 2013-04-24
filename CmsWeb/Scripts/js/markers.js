$(document).ready(function () {
    $("#map").css({
        height: 500,
        width: 600
    });
    var myLatLng = new google.maps.LatLng(17.74033553, 83.25067267);
    MAP.init('#map', myLatLng, 11);
    MAP.placeMarkers('markers.xml');
});

var MAP = {
    map: null,
    bounds: null
}

MAP.init = function (selector, latLng, zoom) {
    var myOptions = {
        zoom: zoom,
        center: latLng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    this.map = new google.maps.Map($(selector)[0], myOptions);
    this.bounds = new google.maps.LatLngBounds();
}

    MAP.codeAddress = function (addresss) {
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location,
                    title: "my house"
                });
                var infowindow = new google.maps.InfoWindow({
                    content: "<div><a href='www.google.com'>click here</a></div>"
                });
                google.maps.event.addListener(marker, 'click', function() {
                    infowindow.open(map,marker);
                });
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    }

MAP.placeMarkers = function (id) {
    $.post("/SGMap", { id:id }, function (ret) {
        for(var marker in ret.markers) {
            var name = marker.name;
            var address = marker.address;
            var point = new google.maps.LatLng(parseFloat(marker.lat), parseFloat(marker.lng));
            MAP.bounds.extend(point);
            var marker = new google.maps.Marker({
                position: point,
                map: MYMAP.map
            });

            var infoWindow = new google.maps.InfoWindow();
            var html = '<strong>' + name + '</strong.><br />' + address;
            google.maps.event.addListener(marker, 'click', function () {
                infoWindow.setContent(html);
                infoWindow.open(MYMAP.map, marker);
            });
            MAP.map.fitBounds(MYMAP.bounds);
        });
    }, "json");
}