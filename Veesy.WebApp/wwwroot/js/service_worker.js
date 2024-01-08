self.addEventListener('message', function(event) {
    var file = event.data.file;
    var data = new FormData()
    data.append('file', file)
    
    // Esempio: Effettua una chiamata all'API utilizzando fetch()
    fetch('/Portfolio/Media/UploadMedia', {
        method: 'POST',
        body: data,
        contentType: false,
    })
    .then(function(response) {
        // Gestisci la risposta dall'API
    })
    .catch(function(error) {
        // Gestisci l'errore
    });
});

self.addEventListener('fetch', function(event) {
    var requestUrl = new URL(event.request.url);

    // Esempio: Interceptar richieste a un'API specifica
    if (requestUrl.origin === 'https://localhost:7218/' && requestUrl.pathname === '/Portfolio/Media/Test') {
        event.respondWith(handleApiRequest(event.request));
    }
});

function handleApiRequest(request) {
    // Effettua la chiamata all'API utilizzando fetch()
    return fetch(request)
        .then(function(response) {
            // Se la chiamata ha successo, puoi fare qualcosa con la risposta prima di restituirla al client
            // Ad esempio, puoi eseguire una logica di caching o di manipolazione dei dati

            // Ecco un esempio di come puoi modificare la risposta
            var modifiedResponse = response.clone();
            modifiedResponse.headers.set('X-Custom-Header', 'Custom Value');

            // Restituisci la risposta modificata al client
            return modifiedResponse;
        })
        .catch(function(error) {
            // In caso di errore nella chiamata all'API, puoi gestire l'errore qui
            console.error('Error fetching API data:', error);
        });
}