console.log(geoJsonData);
console.log(selectedMapType);

// Definer kartlagene
var fargekart = L.tileLayer(
  "https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png",
  {
    maxZoom: 18,
    attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>',
  }
);

var gratonekart = L.tileLayer(
  "https://cache.kartverket.no/v1/wmts/1.0.0/topograatone/default/webmercator/{z}/{y}/{x}.png",
  {
    maxZoom: 18,
    attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>',
  }
);

var turkart = L.tileLayer(
  "https://cache.kartverket.no/v1/wmts/1.0.0/toporaster/default/webmercator/{z}/{y}/{x}.png",
  {
    maxZoom: 18,
    attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>',
  }
);

var sjokart = L.tileLayer(
  "https://cache.kartverket.no/v1/wmts/1.0.0/sjokartraster/default/webmercator/{z}/{y}/{x}.png",
  {
    maxZoom: 18,
    attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>',
  }
);

// Start kartet basert på valgt kartlag
var selectedLayer;
switch (selectedMapType) {
  case "gratonekart":
    selectedLayer = gratonekart;
    break;
  case "turkart":
    selectedLayer = turkart;
    break;
  case "sjokart":
    selectedLayer = sjokart;
    break;
  default:
    selectedLayer = fargekart;
}

// Start kartet
var map = L.map("map").setView([58.1467, 7.9956], 15);

// Legg til valgt kartlag
selectedLayer.addTo(map);

// Legg til GeoJSON-data på kartet
if (geoJsonData && geoJsonData.features) {
  var layer = L.geoJSON(geoJsonData, {
    onEachFeature: function (feature, layer) {
      // Hvis egenskapen er et punkt (markør)
      if (feature.geometry.type === "Point") {
        var lat = feature.geometry.coordinates[1];
        var lng = feature.geometry.coordinates[0];
        layer
          .bindPopup("Koordinater: Br. " + lat + ", Lgr. " + lng)
          .openPopup();
      }
      // Hvis det er et polygon
      else if (feature.geometry.type === "Polygon") {
        feature.geometry.coordinates[0].forEach(function (coord) {
          var lat = coord[1];
          var lng = coord[0];
          L.marker([lat, lng])
            .addTo(map)
            .bindPopup("Koordinater: Br. " + lat + ", Lgr. " + lng)
            .openPopup();
        });
      }
      // Hvis det er en polyline (linje)
      else if (feature.geometry.type === "LineString") {
        feature.geometry.coordinates.forEach(function (coord) {
          var lat = coord[1];
          var lng = coord[0];
          L.marker([lat, lng])
            .addTo(map)
            .bindPopup("Koordinater: Br. " + lat + ", Lgr. " + lng)
            .openPopup();
        });
      }
    },
  }).addTo(map);

  // Sett kartvisningen etter lag
  map.fitBounds(layer.getBounds());
} else {
  // Vis en advarselmelding hvis det ikke er GeoJSON-data
  alert("Gyldige GeoJSON-data ble ikke funnet.");
}

// Legg til GeoJSON-data på kartet og oppdater tabellen
if (geoJsonData && geoJsonData.features) {
  var counter = 1;
  var tbody = document.getElementById("coordinatesTableBody"); // Referanse til tabellens kropp

  // Legg til GeoJSON-data på kartet
  var layer = L.geoJSON(geoJsonData, {
    onEachFeature: function (feature, layer) {
      // Hvis egenskapen er et punkt (markør)
      if (feature.geometry.type === "Point") {
        var lat = feature.geometry.coordinates[1];
        var lng = feature.geometry.coordinates[0];

        // Vis koordinater i tabellen
        var row = `<tr><td>${counter}</td><td>${lat}</td><td>${lng}</td></tr>`;
        tbody.innerHTML += row;
        counter++;

        // Vis koordinatinformasjon med pop-up
        layer.bindPopup("Koordinater: Lat " + lat + ", Lng " + lng).openPopup();
      }
      // Hvis det er et polygon
      else if (feature.geometry.type === "Polygon") {
        var coords = feature.geometry.coordinates[0];
        coords.forEach(function (coord) {
          var lat = coord[1];
          var lng = coord[0];

          // Vis koordinater i tabellen
          var row = `<tr><td>${counter}</td><td>${lat}</td><td>${lng}</td></tr>`;
          tbody.innerHTML += row;
          counter++;

          // Legg til pop-up for hvert hjørnepunkt
          var marker = L.marker([lat, lng]).addTo(map);
          marker.bindPopup("Koordinater: Lat " + lat + ", Lng " + lng);
        });
      }
      // Hvis det er en polyline (linje)
      else if (feature.geometry.type === "LineString") {
        feature.geometry.coordinates.forEach(function (coord) {
          var lat = coord[1];
          var lng = coord[0];

          // Vis koordinater i tabellen
          var row = `<tr><td>${counter}</td><td>${lat}</td><td>${lng}</td></tr>`;
          tbody.innerHTML += row;
          counter++;

          // Legg til pop-up for hvert punkt
          var marker = L.marker([lat, lng]).addTo(map);
          marker.bindPopup("Koordinater: Lat " + lat + ", Lng " + lng);
        });
      }
    },
  }).addTo(map);

  // Juster kartvisningen etter lag
  map.fitBounds(layer.getBounds());
} else {
  // Vis en advarselmelding hvis GeoJSON-data ikke er tilgjengelig
  alert("Ingen gyldige GeoJSON-data funnet.");
}
