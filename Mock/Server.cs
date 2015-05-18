using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mock {
    public class Server : IDisposable {

        bool _life = true;

        public Server(int port) {
            var listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://*:{0}/", port));
            listener.Start();

            Task.Run(() => {
                while (_life) {
                    var context = listener.GetContext();
                    var buf = WebApi(context.Request.RawUrl);
                    var res = context.Response;
                    res.OutputStream.Write(buf, 0, buf.Length);
                    res.Close();
                }
            });
        }

        public void Dispose() {
            _life = false;
        }

        byte[] WebApi(string url) {
            var tmp = url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            var path = tmp[0];
            var param = tmp[1];

            tmp = path.Split(new []{'/'},StringSplitOptions.RemoveEmptyEntries);
            var command = tmp[0];
            var option = tmp[1];
            var value = tmp[2];


            // JSONにしたいDictionaryデータ
            var map = new Dictionary<string, string>();
            map["url"] = url;
            map["command"] = command;
            map["option"] = option;
            map["value"] = value;
            map["param"] = param;

            // Dictionary<string, string>に変換するシリアライザー
            var serializer = new DataContractJsonSerializer(map.GetType());
            var stream = new MemoryStream();
            serializer.WriteObject(stream, map);
            stream.Position = 0;
            var reader = new StreamReader(stream);
            var x = reader.ReadToEnd();

            return Encoding.ASCII.GetBytes(x);
        }
    }
}

