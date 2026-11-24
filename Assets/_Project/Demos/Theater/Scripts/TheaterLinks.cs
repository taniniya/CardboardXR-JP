namespace CardboardXR.Demos.Theater
{
    // The pages the control panel offers, and the page the theater opens on. Pure data and checks,
    // so the tests can hold it to account without a device.
    public static class TheaterLinks
    {
        public class Link
        {
            public string Id;
            public string Label;
            public string Url;
        }

        public static readonly Link[] All =
        {
            new Link { Id = "youtube", Label = "YouTube", Url = "https://www.youtube.com/" },
            new Link { Id = "subscriptions", Label = "登録チャンネル", Url = "https://www.youtube.com/feed/subscriptions" },
            new Link { Id = "music", Label = "YouTube Music", Url = "https://music.youtube.com/" },
        };

        // The page the theater opens on: the first link.
        public static string HomeUrl => All[0].Url;

        public static Link Find(string id)
        {
            foreach (var link in All)
                if (link.Id == id)
                    return link;
            return null;
        }

        // Only web pages may be loaded: nothing from the disk and no app intents through the panel.
        public static bool IsWebUrl(string url) =>
            !string.IsNullOrEmpty(url) &&
            (url.StartsWith("https://", System.StringComparison.Ordinal) ||
             url.StartsWith("http://", System.StringComparison.Ordinal));
    }
}
