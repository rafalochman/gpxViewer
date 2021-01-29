var lat;
var lng;

var dis;
var ele;

var elevationLabel;
var distanceLabel;

function setLatLng(serializedLat, serializedLng) {
    lat = serializedLat;
    lng = serializedLng;

    if (serializedLat.length > 1) {
        document.getElementById('detailsDataIndex').style.display = "block";
    }
}

function setChartData(serializedDistances, serializedElevations, distanceName, elevationName) {
    elevationLabel = elevationName;
    distanceLabel = distanceName;

    dis = serializedDistances;
    ele = serializedElevations;

    if (serializedDistances.length > 1) {
        document.getElementById('chartDiv').style.display = "block";
    }
    var x = 0;
    while (x < dis.length) {
        dis[x] = dis[x].toFixed(2);
        x++;
    }
    x = 0;
    while (x < ele.length) {
        ele[x] = ele[x].toFixed(2);
        x++;
    }
}

// google maps
function initMap() {
    var googleMap = new google.maps.Map(document.getElementById('googleMap'),{
        zoom: 5,
        center: { lat: lat[0], lng: lng[0] }
    });

    if (lat.length > 1) {
        var googleCoordinates = [];
        for (let i = 0; i < lat.length; i++) {
            var googleData = new google.maps.LatLng(lat[i], lng[i]);
            googleCoordinates.push(googleData);
        }

        var googlePolyline = new google.maps.Polyline({
            path: googleCoordinates,
            geodesic: true,
            strokeColor: "#FF0000",
            strokeOpacity: 1.0,
            strokeWeight: 2,
        });
        googlePolyline.setMap(googleMap);

        var bounds = new google.maps.LatLngBounds();
        for (var i = 0; i < googleCoordinates.length; i++) {
            bounds.extend(googleCoordinates[i]);
        }
        googleMap.fitBounds(bounds);
    }
}

// bing map
function GetMap() {
    var bingMap = new Microsoft.Maps.Map('#bingMap', {
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 5,
        center: new Microsoft.Maps.Location(lat[0], lng[0])
    });

    if (lat.length > 1) {
        var bingCoordinates = [];
        for (let i = 0; i < lat.length; i++) {
            var bingData = new Microsoft.Maps.Location(lat[i], lng[i]);
            bingCoordinates.push(bingData);
        }

        var bingPolyLine = new Microsoft.Maps.Polyline(bingCoordinates, {
            strokeColor: 'red',
            strokeThickness: 3,
        });
        bingMap.entities.push(bingPolyLine);

        var rect = Microsoft.Maps.LocationRect.fromLocations(bingCoordinates);
        bingMap.setView({ bounds: rect });
    }
}

//open street map
function OpenMap() {
    var openStreetMap = L.map('openStreetMap').setView([lat[0], lng[0]], 5);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: config.OPEN_STREET_MAP_KEY
    }).addTo(openStreetMap);

    if (lat.length > 1) {
        var openStreetMapCoordinates = [];
        for (let i = 0; i < lat.length; i++) {
            var openData = new L.LatLng(lat[i], lng[i]);
            openStreetMapCoordinates.push(openData);
        }

        var polyline = L.polyline(openStreetMapCoordinates, { color: 'red' }).addTo(openStreetMap);
        openStreetMap.fitBounds(polyline.getBounds());
    }
}

//change map from dropdown list
var flag = 0;
function selectedMap(map) {
    var googleMap = document.getElementById("googleMap");
    var bingMap = document.getElementById("bingMap");
    var openStreetMap = document.getElementById("openStreetMap");
    switch (map) {
        case "1":
            bingMap.style.display = "block";
            googleMap.style.display = "none";
            openStreetMap.style.display = "none";
            break;
        case "2":
            bingMap.style.display = "none";
            googleMap.style.display = "block";
            openStreetMap.style.display = "none";
            break;
        case "3":
            bingMap.style.display = "none";
            googleMap.style.display = "none";
            openStreetMap.style.display = "block";
            if (flag == 0) {
                OpenMap();
                flag = 1;
            }
            break;
    }
}

function imputDisplay() {
    document.querySelector("#chooseInput").onchange = function () {
        document.querySelector("#fileNameLabel").textContent = this.files[0].name;
        document.getElementById("tempData").style.display = 'none';
    }
}

window.addEventListener('load', function () {
    imputDisplay();
    staticMap();
    drawChart();
})

function drawChart() {
    if (dis.length > 1) {
        let myChart = document.getElementById('elevationChart').getContext('2d');
        let elevationChart = new Chart(myChart, {
            type: 'line',
            data: {
                labels: dis,
                datasets: [{
                    data: ele,
                    label: elevationLabel + " [m]",
                    borderColor: "#18bc9c",
                    fill: false
                }
                ]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            autoSkip: true,
                            maxTicksLimit: 8
                        }
                    }],
                    xAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: distanceLabel + ' [km]'
                        },
                        ticks: {
                            autoSkip: true,
                            maxTicksLimit: 20
                        }
                    }]
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            }
        });
    }
}

function staticMap() {
    var openStreetMapCoordinates = [];
    for (let i = 0; i < lat.length; i++) {
        var openData = [lng[i], lat[i]];
        openStreetMapCoordinates.push(openData);
    }
    var mapboxClient = mapboxSdk({ accessToken: config.OPEN_STREET_MAP_KEY });

    var request = mapboxClient.static
        .getStaticImage({
            ownerId: 'mapbox',
            styleId: 'streets-v11',
            width: 500,
            height: 300,
            position: 'auto',
            overlays: [
                {
                    path: {
                        strokeColor: 'FF0000',
                        strokeWidth: 3,
                        coordinates: openStreetMapCoordinates,
                    }
                }
            ]
        });
    var staticImageUrl = request.url();
    var url = staticImageUrl.toString();
    console.log(url);
    return url;
}



