# EdisonBrick
Sponges and compresses data on Intel Edison 

#Start:
Get the [SimpleWebsocketClient](https://chrome.google.com/webstore/detail/simple-websocket-client/pfdhoblngboilpfeibdedpjgfnlcodoo?hl=en) chrome extension.
If you are running the server on your local machine, direct it to
ws://localhost:80/ws

The server responds to messages, independantly and not nessesarily in order. 
#API
##GetAnnotations
###Request
{
  "Type":"GetAnnotations",
  "Id":0
}
###Responce
{
  "Type":"GetAnnotations",
  "Id":6,
  "Annotations":
    [
      {"DateTimeUTC":"2016-11-29T14:53:47.1549874Z","Name":"Test","Type":"Test","Description":""},
      {"DateTimeUTC":"2016-11-29T14:56:30.0990522Z","Name":"Test","Type":"Test","Description":""}
    ]
}

##GetDataGroups
###Request
{
  "Type":"GetDataGroups",
  "Id":0
}
###Responce
{
  "Type":"AddOrUpdateDatagroup",
  "Id":0,
  "DataGroups":[
      {"StartDateUTC":"2016-11-29T14:57:28.31141Z","Name":"Test", "Description":"Test"}
      ]
}

##AddOrUpdateDatagroup
###Request and reponce
{
  "Type":"AddOrUpdateDatagroup",
  "Id":0,
  "DataGroups":[
      {"StartDateUTC":"2016-11-29T14:57:28.31141Z","Name":"Test", "Description":"Test"}
      ]
}
