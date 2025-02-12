using HtmlAgilityPack;
using TitleAggregator.Models;
using System.Text.RegularExpressions;

namespace TitleAggregator.Services
{
    public class WiredScraperService
    {
        private readonly string wiredVideoUrl = "https://www.wired.com/video/";// i have selected the vdeo page for scraping
        private readonly HtmlWeb web = new HtmlWeb();

        public async Task<List<Article>> GetArticlesAsync()
        {
            var articles = new List<Article>();
            int page = 1;
            bool hasMorePages = true;

            while (hasMorePages)
            {
                // Looking forward for video page
                string pageUrl = wiredVideoUrl + (page > 1 ? $"page/{page}/" : "");

                var doc = await Task.Run(() => web.Load(pageUrl));
                var articleNodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'SummaryItemHedLink')]");

                if (articleNodes == null || articleNodes.Count == 0)
                {
                    hasMorePages = false; // No more articles to process
                    break;
                }

                foreach (var node in articleNodes)
                {
                    string title = node.InnerText.Trim();
                    string link = node.GetAttributeValue("href", string.Empty);
                    if (!link.StartsWith("http"))
                    {
                        link = "https://www.wired.com" + link;
                    }

                    var articleDoc = await Task.Run(() => web.Load(link));

                    // capturing "Released on MM/DD/YYYY"
                    var dateRegex = new Regex(@"Released on (\d{2}/\d{2}/\d{4})");
                    var dateNodeText = articleDoc.DocumentNode.InnerText;

                    DateTime publishedDate = DateTime.MinValue;
                    var match = dateRegex.Match(dateNodeText);

                    if (match.Success && DateTime.TryParse(match.Groups[1].Value, out publishedDate))
                    {
                        if (publishedDate >= new DateTime(2022, 1, 1))
                        {
                            articles.Add(new Article
                            {
                                Title = title,
                                Url = link,
                                PublishedDate = publishedDate
                            });
                        }
                    }
                }

                page++; 
            }

            return articles.OrderByDescending(a => a.PublishedDate).ToList();
        }
    }
}

