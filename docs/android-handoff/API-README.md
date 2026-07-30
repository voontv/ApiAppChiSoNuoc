# ReadMeter REST API

## Files

- `swagger.json`: OpenAPI 3 contract containing routes and request schemas.
- `endpoint-list.txt`: endpoints used by the meter-reading application.

## Environment information

Replace these placeholders with the addresses supplied by the backend team:

- Test base URL: `https://<test-host>/<application>/`
- Production base URL: `https://<production-host>/<application>/`
- Swagger UI: `<base-url>/swagger`
- Swagger JSON: `<base-url>/swagger/v1/swagger.json`

## HTTP convention

- Methods: `POST`
- Request header: `Content-Type: application/json`
- Response header: `Accept: application/json`
- Request field names must follow `swagger.json`.
- Responses are JSON directly; SOAP/XML conversion is not required.
- The business result field `ROOT` normally starts with `00- OK` on success.
- Fields such as `PASSWORD_K`, `SO_IMEI`, `MA_BIEN_DOC`, and `TOKEN` are sent in the JSON body where declared by Swagger.
- No Bearer token header is currently declared in this API contract.

## Retrofit

The Android project already uses Retrofit and Gson. Generate request models from the schemas in
`swagger.json`, or define Kotlin data classes with field names matching the JSON contract.

## Important

- Do not place Oracle connection strings in the Android application.
- Test credentials and IMEI values must be supplied separately through a secure channel.
- Write endpoints work only when the backend has `DatabaseSafety:ReadOnly=false`.
