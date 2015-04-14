<Query Kind="Program">
  <Reference>C:\projects\Pourcast\RightpointLabs.Pourcast.Web\bin\MongoDB.Bson.dll</Reference>
  <Reference>C:\projects\Pourcast\RightpointLabs.Pourcast.Web\bin\MongoDB.Driver.dll</Reference>
  <Namespace>MongoDB</Namespace>
  <Namespace>MongoDB.Driver</Namespace>
</Query>

private byte[] GetDataFromUrl(string dataUrl, out string contentType)
{
  // https://gist.github.com/vbfox/484643
  var match = Regex.Match(dataUrl, @"data:image/(?<type>.+?);base64,(?<data>.+)");
  var type = match.Groups["type"].Value;
  var base64Data = match.Groups["data"].Value;
  var binData = Convert.FromBase64String(base64Data);

  contentType = "image/" + type;
  return binData;
}

void Main()
{
	var connectionString="mongodb://user:pass@server:port/app";
	var database="db";
	var h = new MongoClient(connectionString).GetServer().GetDatabase(database);
	var q = new QueryDocument() { { "TypeName", "PictureTaken" } };
	var qSort = new SortByDocument() { { "OccuredOn", -1 } };
	var targetDir = @"E:\temp\PourcastImages"; // .SetLimit(3)
	var c = h.GetCollection("storedevents").Find(q);
	foreach(var doc in c)
	{
		var de = doc["DomainEvent"];
		var id = (string) doc["_id"];
		var dataUrl = (string) (de.AsBsonDocument.Contains("OriginalDataUrl") ? de["OriginalDataUrl"] : de["DataUrl"]);
		string contentType;
		var data = GetDataFromUrl(dataUrl, out contentType);
		var ext = contentType.Split('/').Last();
		if(ext == "jpeg") { ext = "jpg"; }
		var file = Path.Combine(targetDir, string.Format("{0}.{1}", id, ext));
		if(!File.Exists(file)) {
			File.WriteAllBytes(file, data);
		}
		Console.WriteLine("Wrote {0}", file);
	}
}

// Define other methods and classes here
