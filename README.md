# Introduction

A simple REST API with an endpoint that calculates the taxes and performs currency exchange for invoices. The endpoint takes three inputs: Invoice Date, Invoice Pre-Tax Amount in Euro (EUR), and Payment Currency. After completing currency calculations, the endpoint returns four outputs: Pre-Tax Amount, Tax Amount, Grand Total, and Exchange Rate.

Only two currencies are supported for currency conversion: Canadian Dollar (CAD) and US Dollar (USD).

# Requirements
.NET framework version >= 6.0

# Quickstart

In the appsettings.json file replace where it says

```
"INSERT YOUR API KEY HERE"
```

With your Fixer api key

## Visual Studio

Open in Visual Studio and build/debug it through there.

## Testing

All tests are located in the Tests project and are using xUnit

# Example Request

Below is an example request using curl

```
curl -i -H "Accept: application/json" "http://localhost:5066/api/exchange?invoiceDate=2020-08-05&preTaxAmount=123.45&paymentCurrency=USD"
```

And an example response

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Mon, 24 Jan 2022 01:48:18 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"preTaxTotal":146.57,"taxAmount":14.66,"grandTotal":161.23,"exchangeRate":1.187247}
```
