using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.StatusServiceFixtures
{
    [TestFixture]
    public class When_Querying_User : ContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            var twitterClient = new Mock<ITwitterClient>();
            var statusProvider = new Mock<IRateLimitStatusProvider>();
            ServiceUnderTest = new StatusService(twitterClient.Object, statusProvider.Object);
            twitterClient.Setup(c => c.GetUser("trasa")).Returns(TwitterXmlResult);
        }
        protected StatusService ServiceUnderTest { get; set; }
        protected UserInfo RetrievedUser { get; set; }

        protected override void When()
        {
            base.When();
            RetrievedUser = ServiceUnderTest.GetUser("trasa");
        }

        protected string TwitterXmlResult
        {
            get
            {
                #region return "<user>...";
                return @"
            <user>
  <id>14184817</id>
  <name>Tony Rasa</name>
  <screen_name>trasa</screen_name>
  <location>Boise, ID</location>
  <description>Software Developer  &amp; Architect, Consultant, wearer of many hats, doing lots of things for lots of people.</description>
  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/51923903/05-03_zod_normal.jpg</profile_image_url>
  <url>http://elegantcode.com</url>
  <protected>false</protected>
  <followers_count>169</followers_count>
  <profile_background_color>1A1B1F</profile_background_color>
  <profile_text_color>666666</profile_text_color>
  <profile_link_color>2FC2EF</profile_link_color>
  <profile_sidebar_fill_color>252429</profile_sidebar_fill_color>
  <profile_sidebar_border_color>181A1E</profile_sidebar_border_color>
  <friends_count>181</friends_count>
  <created_at>Thu Mar 20 15:41:58 +0000 2008</created_at>
  <favourites_count>2</favourites_count>
  <utc_offset>-25200</utc_offset>
  <time_zone>Mountain Time (US &amp; Canada)</time_zone>
  <profile_background_image_url>http://static.twitter.com/images/themes/theme9/bg.gif</profile_background_image_url>
  <profile_background_tile>false</profile_background_tile>
  <statuses_count>1052</statuses_count>
  <notifications>false</notifications>
  <following>false</following>
  <status>
    <created_at>Tue May 26 21:44:35 +0000 2009</created_at>
    <id>1928381214</id>
    <text>IT BLENDS</text>
    <source>&lt;a href='http://www.tweetdeck.com/'&gt;TweetDeck&lt;/a&gt;</source>
    <truncated>false</truncated>
    <in_reply_to_status_id></in_reply_to_status_id>
    <in_reply_to_user_id></in_reply_to_user_id>
    <favorited>false</favorited>
    <in_reply_to_screen_name></in_reply_to_screen_name>
  </status>
</user>";
                #endregion
            }
        }


        [Test]
        public void Retrieved_User_Is_Not_Null()
        {
            Assert.That(RetrievedUser, Is.Not.Null);
        }

        [Test]
        public void User_Info_Parsed_Correctly()
        {
            Assert.That(RetrievedUser.Name, Is.EqualTo("trasa"));
            Assert.That(RetrievedUser.Location, Is.EqualTo("Boise, ID"));
            Assert.That(RetrievedUser.FriendCount, Is.EqualTo(181));
            Assert.That(RetrievedUser.FollowerCount, Is.EqualTo(169));
        }
    }
}
