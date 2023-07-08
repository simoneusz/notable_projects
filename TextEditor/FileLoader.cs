using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace TextEditor
{
    public interface IFileLoader
    {
        string Load(string fileName);
    }
    public class TextFileLoader : IFileLoader
    {
        public string Load(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }

    public class HtmlFileLoader : IFileLoader
    {
        public string Load(string fileName)
        {
            string html = File.ReadAllText(fileName);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            StringBuilder sb = new StringBuilder();
            String[] tags = { "p", "br", "h1", "h2", "h3", "h4", "h5", "h6", "span", "a" };
            foreach (var node in doc.DocumentNode.DescendantsAndSelf())
            {
                if (tags.Contains(node.Name))
                {
                    sb.AppendLine(node.InnerText);
                }
            }
            return sb.ToString();
        }
    }

    public class BinaryFileLoader : IFileLoader
    {
        public string Load(string fileName)
        {
            using (FileStream stream = File.OpenRead(fileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object obj = formatter.Deserialize(stream);
                return obj.ToString();
            }
        }
    }
    public interface IFileLoaderFactory
    {
        IFileLoader Create(string fileName);
    }

    public class FileLoaderFactory : IFileLoaderFactory
    {
        public IFileLoader Create(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();

            return extension switch
            {
                ".txt" => new TextFileLoader(),
                ".html" => new HtmlFileLoader(),
                ".bin" => new BinaryFileLoader(),
                _ => throw new NotSupportedException($"File extension '{extension}' is not supported."),
            };
        }
    }
}
