# PhosconToOpenHAB
Tool to create openHAB 2 config files from a Phoscon Server (Conbee; Deconz).

It will automaticly create:

1. A item file containig items to get the current temprature and acces the target temprature for each room
2. A thing file managing all required things and proxies
3. A sitemap file

## Download
https://github.com/BoBiene/PhosconToOpenHAB/releases

https://hub.docker.com/repository/docker/bobiene/phoscon-to-openhab/

## Usage
```
-a, --addr        Required. The IP-Address oder DNS name of the
                  Phoscon-Mini-Server (e.g 192.168.1.100 or phosconServer)

-k, --apikey      Required. The API Key

-p, --httpPort    (Default: 8080) The httpPort

-o, --output      (Default: ) Target directory to create the openHAB files in.

--help            Display this help screen.

--version         Display version information.
```  
## Example
   
```
PhosconToOpenHAB.exe -a "openhab:8081" -k "MYAPIKEY"
```

or with docker

```
docker run --rm -v <<PATH_TO_CREATE_FILES>>:/app/conf/ bobiene/phoscon-to-openhab -a "<<HOST/IP>>" -k "<<API Key>>"
```
## Requirements for running converter

.NET Core 3.1 or Docker

## Requirements for openHAB

1. Deconz Binding (http://docs.openhab.org/addons/bindings/http1/readme.html)
2. Phillips Hue Binding (http://docs.openhab.org/addons/transformations/jsonpath/readme.html)
3. Recommend: Basic UI
  
