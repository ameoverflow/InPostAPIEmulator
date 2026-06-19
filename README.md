# Emulator InPost API

Emulator API InPost Mobile, po to żeby móc pracować nad zdalnym otwieraniem paczek bez wydawania miliona zł

Zwraca jakieś tam dane, żeby SwitchPost miał coś do przemielenia i testu

## Lista emulowanych endpointów i przykładowe dane do wysłania

### GET `/v4/parcels/tracked` - Paczki

Brak danych do wysłania

### POST `/v2/collect/validate` - Utworzenie sesji zdalnego otworzenia skrytki

Przykład:
```json
{
  "parcel": {
    "shipmentNumber": "620999566231235677195642",
    "openCode": "123456",
    "recieverPhoneNumber": {
      "prefix": "+48",
      "value": "600100100"
    }
  },
  "geoPoint": {
    "latitude": 50.10209,
    "longitude": 19.95641,
    "accuracy": 13.365
  }
}
```

### POST `/v1/collect/compartment/open` - Zdalne otworzenie paczkomatu

Przykład:
```json
{
  "sessionUuid": "f4ed9aff-edcc-4d23-8db3-cbee35b3b135"
}
```

### POST `/v1/collect/compartment/open` - Zdalne otworzenie paczkomatu

Przykład:
```json
{
  "sessionUuid": "f4ed9aff-edcc-4d23-8db3-cbee35b3b135"
}
```