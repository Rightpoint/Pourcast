using System.IO;

namespace RightpointLabs.Pourcast.Web.Models
{
    public class ComponentTemplateViewModel
    {
        public ComponentTemplateViewModel(FileInfo file)
        {
            this.Name = file.Directory.Name;

            var stream = file.OpenText();
            this.Template = stream.ReadToEnd();
            stream.Close();
        }

        public string Name { get; private set; }

        public string Template { get; private set; }
    }
}