window.mapaDetalle =
{
    mapa: null,

    mostrar: function (idElemento, lat, lon, titulo) {
        var contenedor = document.getElementById(idElemento);
        if (!contenedor) {
            return;
        }

        if (this.mapa) {
            this.mapa.remove();
            this.mapa = null;
        }

        this.mapa = L.map(idElemento).setView([lat, lon], 15);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors'
        }).addTo(this.mapa);

        L.marker([lat, lon]).addTo(this.mapa).bindPopup(titulo).openPopup();
    }
};