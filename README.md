# Websocket Dbreeze Server
A toy asp.net core Websocket API with embedded DBreeze key-value database. This is purely a demo and not nessesarily "full and proper" way to do things, but it does work.
The point is to demonstrate desining api's with web-socket, and to demonstrate embedded database Dbreeze. 

# Start:
Get the [SimpleWebsocketClient](https://chrome.google.com/webstore/detail/simple-websocket-client/pfdhoblngboilpfeibdedpjgfnlcodoo?hl=en) chrome extension.
If you are running the server on your local machine, direct it to
ws://localhost:80/ws

The server responds to messages, independantly and not nessesarily in order. 
# API
## GetAnnotations
### Request
```javascript
{
  "Type":"GetAnnotations",
  "Id":0
}
```
### Responce	
```javascript
{
  "Type":"GetAnnotations",
  "Id":6,
  "Annotations":
    [
      {"DateTimeUTC":"2016-11-29T14:53:47.1549874Z","Name":"Test","Type":"Test","Description":""},
      {"DateTimeUTC":"2016-11-29T14:56:30.0990522Z","Name":"Test","Type":"Test","Description":""}
    ]
}	
```

## GetDataGroups
### Request
```javascript
{
  "Type":"GetDataGroups",
  "Id":0
}
```
### Responce
```javascript
{
  "Type":"GetDataGroups",
  "Id":0,
  "DataGroups":[
      {"StartDateUTC":"2016-11-29T14:57:28.31141Z","Name":"Test", "Description":"Test"}
      ]
}
```

## AddOrUpdateDatagroup
If  the datagroup does not exist, it will create a new one. if it does exist, it will update / overwrite. 
###Request and reponce
```javascript
{
  "Type":"AddOrUpdateDatagroup",
  "Id":0,
  "DataGroups":[
      {"StartDateUTC":"2016-11-29T14:57:28.31141Z","Name":"Test", "Description":"Test"}
      ]
}
```

## AddOrUpdateAnnotations
### Request and reponce
```javascript
{
  "Type":"AddOrUpdateAnnotations",
  "Id":0,
  "Annotations":[
      ...
      ]
}
```

## DeleteAnnotations
### Request
Really only the time field is mandatory, but it needsto be exactly correct
```javascript
{
  "Type":"DeleteAnnotations",
  "Id":2,
  "Annotations":[
      {"DateTimeUTC":"2016-11-29T14:53:47.1549874Z","Name":"Test","Type":"Test","Description":""}
  ]
}
```

## DeleteDatagroup
Works same as above
