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
using Newtonsoft.Json.Linq;

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

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        async void ParseAndRespond(ArraySegment<byte> recieved)
        {
            string responce = "Error";
            string input = Encoding.UTF8.GetString(recieved.ToArray());
            if(IsValidJson(input))
            {   
                var message = JsonConvert.DeserializeObject<Messages.BaseMessage>(input);

                Messages.BaseMessage.ProsessMessage processMessage;

                if (Messages.BaseMessage.MessageActions.TryGetValue(message.Type, out processMessage))
                {
                    responce = processMessage(input); 
                }
            }
            //Send Responce
            ArraySegment<byte> responceData = new ArraySegment<byte>(Encoding.UTF8.GetBytes(responce));
            await this.socket.SendAsync(responceData, WebSocketMessageType.Text, true, CancellationToken.None);
        }



        async Task RecieveLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            while (this.socket.State == WebSocketState.Open)
            {
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);
                var recieved = new ArraySegment<byte>(buffer, 0, incoming.Count);
                Task.Run(() => ParseAndRespond(recieved)); 
            }
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket);
            await h.RecieveLoop();
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketHandler.Acceptor);
        }
    }
}
