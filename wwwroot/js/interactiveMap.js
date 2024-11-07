// Start kartet
var map = L.map('map').setView([58.1467, 7.9956], 15); // Kristiansand, UiA posisjon

 // Kartverket Fargekart 
 var fargekart = L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png', {
     maxZoom: 18,
     attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>'
 }).addTo(map); // 

 // Kartverket Gråtonekart 
 var gratonekart = L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topograatone/default/webmercator/{z}/{y}/{x}.png', {
     maxZoom: 18,
     attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>'
 });

 // Kartverket Turkart 
 var turkart = L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/toporaster/default/webmercator/{z}/{y}/{x}.png', {
     maxZoom: 18,
     attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>'
 });

// Kartverket Sjøkart (Havkart)
var sjokart = L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/sjokartraster/default/webmercator/{z}/{y}/{x}.png', {
     maxZoom: 18,
     attribution: '&copy; <a href="https://kartverket.no/">Kartverket</a>'
 });

// Legg til Fargekart som startkart
var currentLayer = fargekart;
 var selectedMapType = 'fargekart'; // Varsayılan harita tipi



// Legg til kartalternativer som små firkanter nederst til høyre
var thumbnailsContainer = document.createElement('div');
 thumbnailsContainer.classList.add('map-thumbnails');
 document.getElementById('map').appendChild(thumbnailsContainer);

// Legg til kartminiaturer
var maps = [
    { id: 'fargekart', layer: fargekart, imageUrl: '/images/4.png', title: 'Fargekart' },
    { id: 'gratonekart', layer: gratonekart, imageUrl: '/images/3.png', title: 'Gråtonekart' },
    { id: 'turkart', layer: turkart, imageUrl: '/images/2.png', title: 'Turkart' },
    { id: 'sjokart', layer: sjokart, imageUrl: '/images/1.png', title: 'Sjøkart' }
];



maps.forEach(function(mapInfo, index) {
var thumbnailDiv = document.createElement('div');
thumbnailDiv.classList.add('map-thumbnail');
if (index === 0) {
 thumbnailDiv.classList.add('selected');
}

var img = document.createElement('img');
img.src = mapInfo.imageUrl;
img.alt = mapInfo.title;
thumbnailDiv.appendChild(img);

// Ny div for å legge til en tittel
var titleDiv = document.createElement('div');
titleDiv.classList.add('map-title');
titleDiv.textContent = mapInfo.title; 
thumbnailDiv.appendChild(titleDiv);

thumbnailsContainer.appendChild(thumbnailDiv);

// Endre kartet når det klikkes
thumbnailDiv.addEventListener('click', function() {
 if (currentLayer !== mapInfo.layer) {
     map.removeLayer(currentLayer);
     map.addLayer(mapInfo.layer);
     currentLayer = mapInfo.layer;

// Oppdater verdien for valgt karttype
selectedMapType = mapInfo.id;
     document.getElementById('selectedMapType').value = selectedMapType;

// Oppdater valgt klasse
document.querySelectorAll('.map-thumbnail').forEach(function(thumb) {
         thumb.classList.remove('selected');
     });
     thumbnailDiv.classList.add('selected');
 }
});
});



// Tegningsfunksjonalitet
var drawnItems = new L.FeatureGroup();
 map.addLayer(drawnItems);

 var drawControl = new L.Control.Draw({
     edit: {
         featureGroup: drawnItems,
         edit: true,
         remove: true
     },
     draw: {
         polygon: true,
         polyline: true,
         rectangle: true,
         circle: false,
         circlemarker: false,
         marker: true
     }
 });
 map.addControl(drawControl);

// Funksjon som oppdaterer adresseinndatafeltet
function updateGeoJson() {
     var allLayers = [];
     drawnItems.eachLayer(function(layer) {
         var geoJson = layer.toGeoJSON();
         allLayers.push(geoJson);
     });

     var featureCollection = {
         "type": "FeatureCollection",
         "features": allLayers
     };

     document.getElementById('geoJsonInput').value = JSON.stringify(featureCollection);
 }

 // Adres input alanını güncelleyen fonksiyon
 function updateAddressInput(lat, lng) {
     var url = `https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}&accept-language=no`;

     fetch(url)
         .then(response => response.json())
         .then(data => {
             var address = data.display_name ? data.display_name : "Address not found";
             document.getElementById('searchInput').value = address;
             document.getElementById('addressInput').value = address;
         })
         .catch(error => {
             console.error('Error fetching address:', error);
             document.getElementById('searchInput').value = "Error fetching address";
         });
 }

// Funksjon som legger til/oppdaterer koordinater i tabellen
function updateCoordinatesTable(layer) {
     var tbody = document.getElementById('coordinatesTableBody');
     tbody.innerHTML = '';

     if (layer instanceof L.Marker) {
         var latLng = layer.getLatLng();
         var row = `<tr><td>1</td><td>${latLng.lat}</td><td>${latLng.lng}</td></tr>`;
         tbody.innerHTML = row;
     } else if (layer instanceof L.Polygon || layer instanceof L.Polyline || layer instanceof L.Rectangle) {
         var latlngs = layer.getLatLngs();
         var counter = 1;

         latlngs.forEach(function(latlngArray) {
             if (Array.isArray(latlngArray)) {
                 latlngArray.forEach(function(latlng) {
                     var row = `<tr><td>${counter}</td><td>${latlng.lat}</td><td>${latlng.lng}</td></tr>`;
                     tbody.innerHTML += row;
                     counter++;
                 });
             } else {
                 var row = `<tr><td>${counter}</td><td>${latlngArray.lat}</td><td>${latlngArray.lng}</td></tr>`;
                 tbody.innerHTML += row;
                 counter++;
             }
         });
     }
 }

// Legg til et nytt lag når tegningen er fullført
map.on('draw:created', function(event) {
     var layer = event.layer;
     if (drawnItems.getLayers().length > 0) {
         drawnItems.clearLayers();
     }
     drawnItems.addLayer(layer);
     updateGeoJson();
     updateCoordinatesTable(layer);

     if (layer instanceof L.Marker) {
         var latLng = layer.getLatLng();
         updateAddressInput(latLng.lat, latLng.lng);
     }
 });

 map.on('draw:edited', function () {
     var layers = drawnItems.getLayers();
     if (layers.length > 0) {
         updateGeoJson();
         updateCoordinatesTable(layers[0]);
     }
 });

 map.on('draw:deleted', function () {
     updateGeoJson();
     document.getElementById('coordinatesTableBody').innerHTML = '';
 });
    
// Oppdater skjult inputfelt når adressen er angitt manuelt
document.getElementById('searchInput').addEventListener('input', function() {
document.getElementById('addressInput').value = this.value;
});


// Funksjon for å søke etter adresse
function searchAddress() {
     var address = document.getElementById('searchInput').value;
     if (address) {
         var url = `https://nominatim.openstreetmap.org/search?format=json&limit=1&q=${encodeURIComponent(address)}`;

         fetch(url)
             .then(response => response.json())
             .then(data => {
                 if (data.length > 0) {
                     var lat = data[0].lat;
                     var lon = data[0].lon;

                     drawnItems.clearLayers();
                     var marker = L.marker([lat, lon]).addTo(drawnItems);
                     map.setView([lat, lon], 15);

                     updateGeoJson();
                     updateCoordinatesTable(marker);
                 } else {
                     alert('Address not found.');
                 }
             })
             .catch(error => {
                 console.error('Error fetching address:', error);
                 alert('Error fetching address.');
             });
     } else {
         alert('Please enter an address.');
     }
 }

// Vis adresseforslag
document.getElementById('searchInput').addEventListener('input', function() {
     var query = this.value;
     if (query.length > 0) {
         var url = `https://nominatim.openstreetmap.org/search?format=json&addressdetails=1&limit=5&q=${encodeURIComponent(query)}`;

         fetch(url)
             .then(response => response.json())
             .then(data => {
                 var suggestions = document.getElementById('suggestions');
                 suggestions.innerHTML = '';
                 suggestions.style.display = 'block';

                 data.slice(0, 3).forEach(function(item) {
                     if (item.display_name.includes('Norveç')) {
                         var street = item.address.road || item.address.pedestrian || item.address.highway || '';
                         var house_number = item.address.house_number || '';
                         var city = item.address.city || item.address.village || '';
                         var county = item.address.county || '';
                         var country = "Norge";

                         var option = document.createElement('div');
                         option.classList.add('suggestion-item');
                         option.innerHTML = `
                             <div class="suggestion-title">${street} ${house_number}</div>
                             <div class="suggestion-address">${city}, ${county}, ${country}</div>
                         `;
                         option.addEventListener('click', function() {
                             document.getElementById('searchInput').value = `${street} ${house_number}, ${city}, ${county}, ${country}`;
                             suggestions.innerHTML = '';
                             suggestions.style.display = 'none';

                             var lat = item.lat;
                             var lon = item.lon;
                             drawnItems.clearLayers();
                             var marker = L.marker([lat, lon]).addTo(drawnItems);
                             map.setView([lat, lon], 15);
                             updateGeoJson();
                             updateCoordinatesTable(marker);
                         });
                         suggestions.appendChild(option);
                     }
                 });
             })
             .catch(error => console.error('Error fetching suggestions:', error));
     }
 });

// Klikk på søkeknappen for å utføre søket
document.getElementById('searchAddressBtn').addEventListener('click', searchAddress);
 document.getElementById('searchInput').addEventListener('keypress', function(event) {
     if (event.key === 'Enter') {
         event.preventDefault();
         searchAddress();
     }
 });