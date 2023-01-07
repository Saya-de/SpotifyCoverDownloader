using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;


namespace SpotifyCoverDL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for(; ; )
            {
                using (WebClient client = new WebClient())
                {
                    string htmlCode = "";
                    Console.WriteLine("Song or Playlist URL: ");
                    string url = Console.ReadLine();
                    client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                    if(url.Contains("open.spotify.com/track")|| url.Contains("open.spotify.com/playlist"))
                    {
                        if (!url.Contains("&nd=1"))
                        {
                            try
                            {
                                htmlCode = client.DownloadString($"{url}&nd=1");
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
                        }
                        else
                        {
                            try
                            {
                                htmlCode = client.DownloadString($"{url}");
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
                        }
                    }
                    else { Console.WriteLine("Not a valid Spotify Track/Playlist URL!"); continue; }
                    
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlCode);
                    var list = doc.DocumentNode.SelectNodes("//meta");
                    foreach (var node in list)
                    {
                        if (node.GetAttributeValue("name", "").Contains("twitter:image"))
                        {
                            using (WebClient dlclient = new WebClient())
                            {
                                client.DownloadFile(node.GetAttributeValue("content", ""), "AlbumCover.png");
                            }
                            Console.WriteLine("Image successfully downloaded!\n\n");
                        }

                    }
                }

            }
           
        }
    }
}
