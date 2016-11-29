using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace EdisonBrick
{


    public class SocketHandler
    {
        public const int BufferSize = 4096;


        WebSocket socket;

        SocketHandler(WebSocket socket)
        {
            this.socket = socket;
        }



        async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            while (this.socket.State == WebSocketState.Open)
            {
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);

                string responce = "Error";

                var recieved = new ArraySegment<byte>(buffer, 0, incoming.Count);
                {
                   

                    var message = JsonConvert.DeserializeObject<Messages.BaseMessage>(Encoding.UTF8.GetString(recieved.ToArray()));
                    if(message != null)
                    {
                        switch(message.Type)
                        {
                            case Messages.BaseMessage.GetDataGroup:
                                responce = JsonConvert.SerializeObject(DbAccess.DataGroupsList);
                                break;
                            case Messages.BaseMessage.GetAnnotations:
                                responce = JsonConvert.SerializeObject(DbAccess.AnnotationsList);
                                break;
                        }
                    }
                }
                ArraySegment<byte> responceData = new ArraySegment<byte>(Encoding.UTF8.GetBytes(responce)); 
                await this.socket.SendAsync(responceData, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket);
            await h.EchoLoop();
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketHandler.Acceptor);
        }
    }
}
