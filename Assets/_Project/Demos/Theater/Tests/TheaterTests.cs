using CardboardXR.Demos.Theater;
using NUnit.Framework;
using UnityEngine;

namespace CardboardXR.Demos.Theater.Tests
{
    public class TheaterTests
    {
        [Test]
        public void UvFromLocalPutsTheMiddleInTheMiddle()
        {
            var size = new Vector2(3.2f, 1.8f);
            Assert.AreEqual(new Vector2(0.5f, 0.5f), TheaterMath.UvFromLocal(Vector2.zero, size));
            Assert.AreEqual(new Vector2(0f, 0f), TheaterMath.UvFromLocal(new Vector2(-1.6f, -0.9f), size));
            Assert.AreEqual(new Vector2(1f, 1f), TheaterMath.UvFromLocal(new Vector2(1.6f, 0.9f), size));
        }

        [Test]
        public void WebPointSwapsUpAndDown()
        {
            // Texture coordinates start at the bottom-left; the page's start at the top-left.
            Assert.AreEqual(new Vector2(0f, 1f), TheaterMath.ToWebPoint(new Vector2(0f, 0f)));
            Assert.AreEqual(new Vector2(1f, 0f), TheaterMath.ToWebPoint(new Vector2(1f, 1f)));
            Assert.AreEqual(new Vector2(0.5f, 0.5f), TheaterMath.ToWebPoint(new Vector2(0.5f, 0.5f)));
        }

        [Test]
        public void WebPointClampsPointsOffTheScreen()
        {
            Assert.AreEqual(new Vector2(0f, 0f), TheaterMath.ToWebPoint(new Vector2(-0.5f, 2f)));
            Assert.AreEqual(new Vector2(1f, 1f), TheaterMath.ToWebPoint(new Vector2(2f, -0.5f)));
        }

        [Test]
        public void LinksAreUniqueWebPages()
        {
            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (var link in TheaterLinks.All)
            {
                Assert.IsTrue(ids.Add(link.Id), $"Duplicate link id: {link.Id}");
                Assert.IsTrue(TheaterLinks.IsWebUrl(link.Url), $"Not a web page: {link.Url}");
                Assert.AreSame(link, TheaterLinks.Find(link.Id));
            }
            Assert.IsNull(TheaterLinks.Find("no-such-link"));
        }

        [Test]
        public void HomeIsTheFirstLink()
        {
            Assert.AreEqual(TheaterLinks.All[0].Url, TheaterLinks.HomeUrl);
            StringAssert.Contains("youtube.com", TheaterLinks.HomeUrl);
        }

        [TestCase("https://www.youtube.com/", true)]
        [TestCase("http://example.com/", true)]
        [TestCase("file:///sdcard/movie.mp4", false)]
        [TestCase("intent://scan#Intent", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void OnlyWebUrlsLoad(string url, bool expected)
        {
            Assert.AreEqual(expected, TheaterLinks.IsWebUrl(url));
        }
    }
}
