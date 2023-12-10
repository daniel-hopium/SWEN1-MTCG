using System.Net.Sockets;
using System.Text;

namespace API.HttpServer
{
    /// <summary>This class provides HTTP server event arguments.</summary>
    public class HttpSvrEventArgs: EventArgs
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected members                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>TCP client.</summary>
        protected TcpClient _Client;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="client">TCP client object.</param>
        /// <param name="plainMessage">HTTP plain message.</param>
        public HttpSvrEventArgs(TcpClient client, string plainMessage) 
        {
            _Client = client;
            PlainMessage = plainMessage;
            Payload = string.Empty;
            
            string[] lines = plainMessage.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
            bool inheaders = true;
            List<HttpHeader> headers = new();

            for(int i = 0; i < lines.Length; i++) 
            {
                if(i == 0)
                {
                    string[] inc = lines[0].Split(' ');
                    Method = inc[0].ToUpper(); // TO UPPER NEEDED?
                    Path = inc[1];
                    
                }
                if (i == 2)
                {
                    string[] inc = lines[i].Split(' ');
                    if(inc[0].ToUpper() == "AUTHORIZATION:")
                        Authorization = inc[1] + " " + inc[2];
                }
                else if(inheaders)
                {
                    if(string.IsNullOrWhiteSpace(lines[i]))
                    {
                        inheaders = false;
                    }
                    else { headers.Add(new HttpHeader(lines[i])); }
                }
                else
                {
                    if(!string.IsNullOrWhiteSpace(Payload)) { Payload += "\r\n"; }
                    Payload += lines[i];
                }
            }

            Headers = headers.ToArray();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the plain HTTP message.</summary>
        public string PlainMessage
        {
            get; protected set;
        }


        /// <summary>Gets the HTTP method.</summary>
        public virtual string Method
        {
            get; protected set;
        } = string.Empty;


        /// <summary>Gets the request path.</summary>
        public virtual string Path
        {
            get; protected set;
        } = string.Empty;
        
        /// <summary>Gets the authorization.</summary>
        public virtual string Authorization
        {
            get; protected set;
        } = string.Empty;


        /// <summary>Gets the HTTP hgeaders.</summary>
        public virtual HttpHeader[] Headers
        {
            get; protected set;
        }


        /// <summary>Gets the HTTP payload.</summary>
        public virtual string Payload
        {
            get; protected set;
        }
        


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Returns a reply to the HTTP request.</summary>
        /// <param name="status">Status code.</param>
        /// <param name="payload">Payload.</param>
        public virtual void Reply(int status, string? payload = null)
        {
            string data;

            data = GetHttpResponseStatus(status);
            
            if(string.IsNullOrEmpty(payload)) 
            {
                data += "Content-Length: 0\n";
            }
            data += "Content-Type: text/plain\n\n";

            if(!string.IsNullOrEmpty(payload)) { data += payload; }

            byte[] buf = Encoding.ASCII.GetBytes(data);
            _Client.GetStream().Write(buf, 0, buf.Length);
            _Client.Close();
            _Client.Dispose();
        }
        
        public static string GetHttpResponseStatus(int statusCode)
        {
            string statusLine = statusCode switch
            {
                200 => "HTTP/1.1 200 OK",
                201 => "HTTP/1.1 201 Created",
                204 => "HTTP/1.1 204 No Content",
                400 => "HTTP/1.1 400 Bad Request",
                401 => "HTTP/1.1 401 Unauthorized",
                403 => "HTTP/1.1 403 Forbidden",
                404 => "HTTP/1.1 404 Not Found",
                409 => "HTTP/1.1 409 Conflict",
                500 => "HTTP/1.1 500 Internal Server Error",
                501 => "HTTP/1.1 501 Not Implemented",
                503 => "HTTP/1.1 503 Service Unavailable",
                _ => "HTTP/1.1 418 I'm a Teapot"
            };

            return statusLine + "\n";
        }
        
        public string PathVariable()
        {
            string[] pathSegments = Path.Split("/");
            if (pathSegments.Length < 2 || pathSegments.Length > 4)
            {
                throw new Exception("Invalid Path Variable Format");
            }
            return  pathSegments[2]; 
        }

    }
    
}
