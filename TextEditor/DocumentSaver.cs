using System;
using System.Diagnostics;
using System.Text.Json;
using System.IO;


namespace TextEditor {
    public abstract class DocumentSaver
    {
        protected string text;
        public DocumentSaver(string text)
        {
            this.text = text;
        }

        public abstract void Save(string fileName);
    }

    public class TxtDocumentSaver : DocumentSaver
    {
        public TxtDocumentSaver(string text) : base(text)
        {
        }

        public override void Save(string fileName)
        {
            File.WriteAllText(fileName, text);
        }
    }

    public class HtmlDocumentSaver : DocumentSaver
    {
        public HtmlDocumentSaver(string text) : base(text)
        {
        }

        public override void Save(string fileName)
        {
            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            string html = "<!DOCTYPE html>" + Environment.NewLine;
            html += "<html lang = \"ru\"> " + Environment.NewLine;
            html += "\t<head>" + Environment.NewLine;
            html += "\t\t<meta charset=\"UTF - 8\">" + Environment.NewLine;
            html += "\t\t<title>HTML Document</title>" + Environment.NewLine;
            html += "\t</head>" + Environment.NewLine;
            html += "\t<body>" + Environment.NewLine;
            Debug.WriteLine(text);
            int cnt = 0;
            foreach (string line in lines)
            {
                Debug.WriteLine(cnt + ") " + line);
                html += "\t\t<p>" + line + "</p>" + Environment.NewLine;
            }

            html += "\t</body>" + Environment.NewLine;
            html += "</html>" + Environment.NewLine;

            File.WriteAllText(fileName, html);
        }
    }



    public class BinaryDocumentSaver : DocumentSaver
    {
        public BinaryDocumentSaver(string text) : base(text)
        {
        }

        public override void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(text);
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }

}