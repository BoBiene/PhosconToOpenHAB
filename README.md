# PhosconToOpenHAB
Tool to create openHAB 2 config files from a Phoscon Server (Conbee; Deconz).

It will automaticly create:

1. A item file containig items to get the current temprature and acces the target temprature for each room
2. A thing file managing all required things and proxies
3. A sitemap file

## Download
https://github.com/BoBiene/PhosconToOpenHAB/releases

## Usage
```
PhosconToOpenHAB 1.0.0.0
Copyright ©  2019

  -a, --addr      Required. The IP-Address oder DNS name of the
                  Phoscon-Mini-Server (e.g 192.168.1.100 or phosconServer)

  -k, --apikey    Required. The API Key

  -o, --output    (Default: ) Target directory to create the openHAB files in.

  --help          Display this help screen.
```  
## Example
   
```
PhosconToOpenHAB.exe -a "openhab:8081" -k "MYAPIKEY"
```
```
Type is not supported, ignoring Daylight
Created config files at C:\Users\bo_bi\source\repos\BoBiene\PhosconToOpenHAB\PhosconToOpenHAB\bin\Debug\conf
```
  
  ## Requirements
  
  1. Deconz Binding (http://docs.openhab.org/addons/bindings/http1/readme.html)
  2. Phillips Hue Binding (http://docs.openhab.org/addons/transformations/jsonpath/readme.html)
  3. Recommend: Basic UI
  