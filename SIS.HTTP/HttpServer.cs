using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SIS.HTTP
{
    public class HttpServer : IHttpServer
    {
        private readonly TcpListener tcpListener;

        //TODO: Actions

        public HttpServer(int port) 
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);
            
        }

        public async Task ResetAsync()
        {
            this.Stop();
            await this.StartAsync();
        }

        public async Task StartAsync()
        {
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = await this.tcpListener.AcceptTcpClientAsync();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => ProcessClientAsync(tcpClient));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            using NetworkStream networkStream = tcpClient.GetStream();
            try
            {
                byte[] requestBytes = new byte[100000];
                int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                var requestAsString = Encoding.UTF8.GetString(requestBytes, 0, requestBytes.Length);

                var request = new HttpRequest(requestAsString);
                string content = "<h1>Random Page</h1>";
                if (request.Path == "/")
                {
                    content = "<h1>Home Page</h1>";
                }
                else if (request.Path == "/users/login")
                {
                    content = "<h1>Login Page</h1>";
                }

                byte[] stringContent = Encoding.UTF8.GetBytes(content);
                var response = new HttpResponse(HttpResponseCode.OK, stringContent);
                response.Headers.Add(new Header("Server", "DodoServer/1.0"));
                response.Headers.Add(new Header("Content-Type", "text/html"));
                response.Cookies.Add(new ResponseCookie("SesID", Guid.NewGuid().ToString())
                { MaxAge = 3600 });

                byte[] responseByte = Encoding.UTF8.GetBytes(response.ToString());
                await networkStream.WriteAsync(responseByte, 0, responseByte.Length);
                await networkStream.WriteAsync(response.Body, 0, response.Body.Length);
                Console.WriteLine(request);
                Console.WriteLine(new string('=', 60));
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponse(
                    HttpResponseCode.InternalServerError,
                    Encoding.UTF8.GetBytes(ex.ToString()));
                errorResponse.Headers.Add(new Header("Content-Type", "text/plain"));
                byte[] responseByte = Encoding.UTF8.GetBytes(errorResponse.ToString());
                await networkStream.WriteAsync(responseByte, 0, responseByte.Length);
                await networkStream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
            }
            
            
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }
    }
}
 