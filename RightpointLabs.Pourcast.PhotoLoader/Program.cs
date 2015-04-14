using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkyBiometry.Client.FC;

namespace RightpointLabs.Pourcast.PhotoLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () => await _Main(args)).Wait();
        }

        static async Task _Main(string[] args)
        {
            InitLogging();
            log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                var ctx = new FCClient(ConfigurationManager.AppSettings["ApiKey"], ConfigurationManager.AppSettings["ApiSecret"]);
                var tagNamespace = ConfigurationManager.AppSettings["TagNamespace"];
                foreach (var file in Directory.GetFiles(ConfigurationManager.AppSettings["SourceDirectory"]))
                {
                    var nameParts = Path.GetFileNameWithoutExtension(file).Split('-');
                    var recognizeAs = nameParts.First();
                    log.DebugFormat("Preparing to recognize {0} as {1}", file, recognizeAs);

                    using (var fs = new FileStream(file, FileMode.Open))
                    {
                        var faces = await ctx.Faces.DetectAsync(new String[0], new[] {fs});
                        var tags = faces.Photos.SelectMany(i => i.Tags).Select(i => i.TagId).ToList();
                        if (tags.Count != 1)
                        {
                            log.WarnFormat("Expected to get 1 face, but got {0} for {1}", tags.Count, recognizeAs);
                            continue;
                        }

                        var userId = string.Format("{0}@{1}", recognizeAs, tagNamespace);
                        var res = await ctx.Tags.SaveAsync(tags, userId);
                        if (res.Status != Status.Success)
                        {
                            log.WarnFormat("Failed to save {0}: {1}{2}", recognizeAs, res.Message, res.ErrorMessage);
                            continue;
                        }
                        log.DebugFormat("Loaded {0}: {1}", recognizeAs, res.Message);

                        log.DebugFormat("Starting training for {0}", userId);
                        var train = await ctx.Faces.TrainAsync(new[] { userId });
                        if (train.Status != Status.Success)
                        {
                            log.WarnFormat("Failed to save {0}: {1}{2}", recognizeAs, train.Message, train.ErrorMessage);
                            continue;
                        }
                        log.DebugFormat("Result: {0}", train.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error("Main", ex);
            }
        }

        private static void InitLogging()
        {
            // initialize log4net
            var file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            if (System.IO.File.Exists(file))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(file));
            }
        }
    }
}
