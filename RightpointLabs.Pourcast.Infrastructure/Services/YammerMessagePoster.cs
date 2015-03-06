using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Domain.Services;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    public class YammerMessagePoster : IMessagePoster
    {
        private readonly string _authCode;
        private readonly int _groupId;

        public YammerMessagePoster(string authCode, int groupId)
        {
            _authCode = authCode;
            _groupId = groupId;
        }

        public int PostNewMessage(string body, int[] users = null, string filename = null, string fileContentType = null, byte[] filedata = null)
        {
            var form = new NameValueCollection();
            form["group_id"] = _groupId.ToString();

            return DoPost(form, body, users, filename, fileContentType, filedata);
        }

        public int PostReply(int repliedTo, string body, int[] users = null, string filename = null, string fileContentType = null, byte[] filedata = null)
        {
            var form = new NameValueCollection();
            form["replied_to_id"] = repliedTo.ToString();

            return DoPost(form, body, users, filename, fileContentType, filedata);
        }

        public async Task<Dictionary<string, MessageUserInfo>>  GetUsers()
        {
            var result = new Dictionary<string, MessageUserInfo>();

            int page = 0;
            while (true)
            {
                var users = await GetUserPage(page);
                foreach (var user in users)
                {
                    result[user.email] = user;
                }
                if (users.Length < 50)
                    break;
                page++;
            }

            return result;
        }

        private async Task<MessageUserInfo[]> GetUserPage(int page)
        {
            var resp = await new HttpClient().GetAsync("https://www.yammer.com/api/v1/users.json?page=" + page);
            var data = await resp.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<JArray>(data);

            return obj.Select(i => new MessageUserInfo {email = (string) i["email"], id = (int) i["id"], name = (string) i["name"]}).ToArray();
        }

        private int DoPost(NameValueCollection form, string body, int[] users = null, string filename = null, string fileContentType = null, byte[] filedata = null)
        {
            var files = new UploadFile[0];
            if (!string.IsNullOrEmpty(filename) && null != filedata)
            {
                var ms = new MemoryStream(filedata);
                files = new[] { new UploadFile(ms, "attachment1", filename, fileContentType) };
            }

            form["body"] = body;
            if (null != users)
            {
                form["cc"] = string.Join(",", users.Select(i => string.Format("[[user:{0}]]", i)));
            }

            var req = (HttpWebRequest)WebRequest.Create("https://www.yammer.com/api/v1/messages.json");
            req.Headers.Add("Authorization", "Bearer " + _authCode);
            HttpWebResponse resp = HttpUploadHelper.Upload(req, files, form);

            using (Stream s = resp.GetResponseStream())
            using (StreamReader sr = new StreamReader(s))
            {
                var data = sr.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<JObject>(data);
                return (int)obj["messages"][0]["id"];
            }
        }

        // code referenced by http://aspnetupload.com/Upload-File-POST-HttpWebRequest-WebClient-RFC-1867.aspx
        public class HttpUploadHelper
        {
            private HttpUploadHelper()
            { }

            public static string Upload(string url, UploadFile[] files, NameValueCollection form)
            {
                HttpWebResponse resp = Upload((HttpWebRequest)WebRequest.Create(url), files, form);

                using (Stream s = resp.GetResponseStream())
                using (StreamReader sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }

            public static HttpWebResponse Upload(HttpWebRequest req, UploadFile[] files, NameValueCollection form)
            {
                List<MimePart> mimeParts = new List<MimePart>();

                try
                {
                    foreach (string key in form.AllKeys)
                    {
                        StringMimePart part = new StringMimePart();

                        part.Headers["Content-Disposition"] = "form-data; name=\"" + key + "\"";
                        part.StringData = form[key];

                        mimeParts.Add(part);
                    }

                    int nameIndex = 0;

                    foreach (UploadFile file in files)
                    {
                        StreamMimePart part = new StreamMimePart();

                        if (string.IsNullOrEmpty(file.FieldName))
                            file.FieldName = "file" + nameIndex++;

                        part.Headers["Content-Disposition"] = "form-data; name=\"" + file.FieldName + "\"; filename=\"" + file.FileName + "\"";
                        part.Headers["Content-Type"] = file.ContentType;

                        part.SetStream(file.Data);

                        mimeParts.Add(part);
                    }

                    string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

                    req.ContentType = "multipart/form-data; boundary=" + boundary;
                    req.Method = "POST";

                    long contentLength = 0;

                    byte[] _footer = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");

                    foreach (MimePart part in mimeParts)
                    {
                        contentLength += part.GenerateHeaderFooterData(boundary);
                    }

                    req.ContentLength = contentLength + _footer.Length;

                    byte[] buffer = new byte[8192];
                    byte[] afterFile = Encoding.UTF8.GetBytes("\r\n");
                    int read;

                    using (Stream s = req.GetRequestStream())
                    {
                        foreach (MimePart part in mimeParts)
                        {
                            s.Write(part.Header, 0, part.Header.Length);

                            while ((read = part.Data.Read(buffer, 0, buffer.Length)) > 0)
                                s.Write(buffer, 0, read);

                            part.Data.Dispose();

                            s.Write(afterFile, 0, afterFile.Length);
                        }

                        s.Write(_footer, 0, _footer.Length);
                    }

                    return (HttpWebResponse)req.GetResponse();
                }
                catch
                {
                    foreach (MimePart part in mimeParts)
                        if (part.Data != null)
                            part.Data.Dispose();

                    throw;
                }
            }
        }
        public abstract class MimePart
        {
            NameValueCollection _headers = new NameValueCollection();
            byte[] _header;

            public NameValueCollection Headers
            {
                get { return _headers; }
            }

            public byte[] Header
            {
                get { return _header; }
            }

            public long GenerateHeaderFooterData(string boundary)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("--");
                sb.Append(boundary);
                sb.AppendLine();
                foreach (string key in _headers.AllKeys)
                {
                    sb.Append(key);
                    sb.Append(": ");
                    sb.AppendLine(_headers[key]);
                }
                sb.AppendLine();

                _header = Encoding.UTF8.GetBytes(sb.ToString());

                return _header.Length + Data.Length + 2;
            }

            public abstract Stream Data { get; }
        }
        public class StreamMimePart : MimePart
        {
            Stream _data;

            public void SetStream(Stream stream)
            {
                _data = stream;
            }

            public override Stream Data
            {
                get
                {
                    return _data;
                }
            }
        }
        public class StringMimePart : MimePart
        {
            Stream _data;

            public string StringData
            {
                set
                {
                    _data = new MemoryStream(Encoding.UTF8.GetBytes(value));
                }
            }

            public override Stream Data
            {
                get
                {
                    return _data;
                }
            }
        }
        public class UploadFile
        {
            Stream _data;
            string _fieldName;
            string _fileName;
            string _contentType;

            public UploadFile(Stream data, string fieldName, string fileName, string contentType)
            {
                _data = data;
                _fieldName = fieldName;
                _fileName = fileName;
                _contentType = contentType;
            }

            public UploadFile(string fileName, string fieldName, string contentType)
                : this(File.OpenRead(fileName), fieldName, Path.GetFileName(fileName), contentType)
            { }

            public UploadFile(string fileName)
                : this(fileName, null, "application/octet-stream")
            { }

            public Stream Data
            {
                get { return _data; }
                set { _data = value; }
            }

            public string FieldName
            {
                get { return _fieldName; }
                set { _fieldName = value; }
            }

            public string FileName
            {
                get { return _fileName; }
                set { _fileName = value; }
            }

            public string ContentType
            {
                get { return _contentType; }
                set { _contentType = value; }
            }
        }

    }
}
