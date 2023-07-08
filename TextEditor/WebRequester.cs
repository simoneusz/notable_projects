using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace TextEditor
{
    public class WebRequester
    {
        private readonly string url = "https://www.znu.edu.ua/cms/index.php?action=news/view&start=0&site_id=27&lang=ukr";
        public string MakeRequest()
        {
            int countNews = AskCountOfNews();
            string DeleteTags(string text)
            {
                return Regex.Replace(text, "<.*?>", string.Empty);
            }

            WebRequest request = null;
            HttpWebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;

            try
            {
                request = WebRequest.Create(this.url);
                response = (HttpWebResponse)request.GetResponse();

                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();

                HtmlAgilityPack.HtmlDocument doc = new();
                doc.LoadHtml(responseFromServer);
                HtmlNodeCollection titles = doc.DocumentNode.SelectNodes("//div[@class='a-container']//h4/a");
                HtmlNodeCollection texts = doc.DocumentNode.SelectNodes("//div[@class='a-container']//div[@class='text']");

                if (titles == null || texts == null)
                {
                    throw new Exception("Unable to parse news items.");
                }

                string result = "\n";

                for (int i = 0; i < countNews; i++)
                {
                    result += "\n" + titles[i].InnerHtml + "\n\n";
                    result += texts[i].InnerHtml + "\n";
                }

                result = DeleteTags(result);
                result = WebUtility.HtmlDecode(result);

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
            finally
            {
                response?.Close();
                reader?.Close();
                dataStream?.Close();
            }

        }
        public int AskCountOfNews()
        {
            int countNews = 0;
            try
            {
                countNews = Int32.Parse(Microsoft.VisualBasic.Interaction.InputBox("How many news?(<10)", "", "Default Text"));
            }
            catch
            {
                countNews = 1;
            }

            if (countNews > 10 || countNews <= 0)
            {
                countNews = 1;
            }
            return countNews;
        }
    }
}