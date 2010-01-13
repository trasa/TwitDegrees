using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.StatusServiceFixtures
{
    [TestFixture]
    public abstract class Get_Friends_For_User : ContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            var twitterClient = new Mock<ITwitterClient>();
            var rateLimitProvider = new Mock<IRateLimitStatusProvider>();
            ServiceUnderTest = new StatusService(twitterClient.Object, rateLimitProvider.Object);
            twitterClient.Setup(c => c.GetFriends("trasa", 1)).Returns(TwitterXmlResult);
        }
        protected StatusService ServiceUnderTest { get; set; }
        protected IEnumerable<UserInfo> RetrievedInfo { get; set; }

        protected override void When()
        {
            base.When();
            RetrievedInfo = ServiceUnderTest.GetFriendsOf("trasa");
        }

        protected abstract string TwitterXmlResult { get; }
    }


    public class When_Only_One_Friend : Get_Friends_For_User
    {
        protected override string TwitterXmlResult
        {
            get
            {
                #region return "<users><user>....";
                return @"<users>
<user>
  <id>9663592</id>
  <name>Joe Ocampo</name>
  <screen_name>agilejoe</screen_name>
  <location>San Antonio, Texas</location>
  <description>really, com on, really?</description>
  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/63469763/JoeO_normal.jpg</profile_image_url>
  <url>http://agilejoe.lostechies.com</url>
  <protected>false</protected>
  <followers_count>429</followers_count>
  <profile_background_color>1A1B1F</profile_background_color>
  <profile_text_color>666666</profile_text_color>
  <profile_link_color>2FC2EF</profile_link_color>
  <profile_sidebar_fill_color>252429</profile_sidebar_fill_color>
  <profile_sidebar_border_color>181A1E</profile_sidebar_border_color>
  <friends_count>175</friends_count>
  <created_at>Wed Oct 24 19:28:03 +0000 2007</created_at>
  <favourites_count>1</favourites_count>
  <utc_offset>-21600</utc_offset>
  <time_zone>Central Time (US &amp; Canada)</time_zone>
  <profile_background_image_url>http://static.twitter.com/images/themes/theme9/bg.gif</profile_background_image_url>
  <profile_background_tile>false</profile_background_tile>
  <statuses_count>1946</statuses_count>
  <notifications></notifications>
  <following></following>
  <status>
    <created_at>Tue May 26 06:19:51 +0000 2009</created_at>
    <id>1921037538</id>
    <text>My dog goes nuts in thunderstorms!  He is like a child.</text>
    <source>&lt;a href='http://www.atebits.com/'&gt;Tweetie&lt;/a&gt;</source>
    <truncated>false</truncated>
    <in_reply_to_status_id></in_reply_to_status_id>
    <in_reply_to_user_id></in_reply_to_user_id>
    <favorited>false</favorited>
    <in_reply_to_screen_name></in_reply_to_screen_name>
  </status>
</user>
</users>
";

                #endregion
            }
        }

        [Test]
        public void Retrieved_Info_Contains_Friend_Name()
        {
            Assert.That(RetrievedInfo.Single().Name, Is.EqualTo("agilejoe"));
        }

        [Test]
        public void Retrieved_Info_Contains_Friend_Count()
        {
            Assert.That(RetrievedInfo.Single().FriendCount, Is.EqualTo(175));
        }

        [Test]
        public void Retrieved_Info_Is_Not_Null()
        {
            Assert.That(RetrievedInfo, Is.Not.Null);
        }
    }

    public class When_Many_Friends : Get_Friends_For_User
    {
        protected override string TwitterXmlResult
        {
            get
            {
                #region return "<users><user>....";
                return @"
<users>
<user>
  <id>428333</id>
  <name>CNN Breaking News</name>
  <screen_name>cnnbrk</screen_name>
  <location>Everywhere</location>
  <description>CNN.com is among the world's leaders in online news and information delivery.</description>
  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/67263363/icon.cnnbrk_normal.png</profile_image_url>
  <url>http://cnn.com/</url>
  <protected>false</protected>
  <followers_count>1590492</followers_count>
  <profile_background_color>323232</profile_background_color>
  <profile_text_color>000000</profile_text_color>
  <profile_link_color>004287</profile_link_color>
  <profile_sidebar_fill_color>EEEEEE</profile_sidebar_fill_color>
  <profile_sidebar_border_color>DADADA</profile_sidebar_border_color>
  <friends_count>9</friends_count>
  <created_at>Tue Jan 02 01:48:14 +0000 2007</created_at>
  <favourites_count>1</favourites_count>
  <utc_offset>-18000</utc_offset>
  <time_zone>Eastern Time (US &amp; Canada)</time_zone>
  <profile_background_image_url>http://s3.amazonaws.com/twitter_production/profile_background_images/3586568/background.png</profile_background_image_url>
  <profile_background_tile>false</profile_background_tile>
  <statuses_count>762</statuses_count>
  <notifications></notifications>
  <following></following>
  <status>
    <created_at>Wed May 27 20:39:00 +0000 2009</created_at>
    <id>1939464599</id>
    <text>Barcelona win European Cup for third time following 2-0 win over Manchester United in Champions League Final.</text>
    <source>&lt;a href=&quot;http://cnn.com&quot;&gt;CNN&lt;/a&gt;</source>
    <truncated>false</truncated>
    <in_reply_to_status_id></in_reply_to_status_id>
    <in_reply_to_user_id></in_reply_to_user_id>
    <favorited>false</favorited>
    <in_reply_to_screen_name></in_reply_to_screen_name>
  </status>
</user>
<user>
  <id>9663592</id>
  <name>Joe Ocampo</name>
  <screen_name>agilejoe</screen_name>
  <location>San Antonio, Texas</location>
  <description>really, com on, really?</description>
  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/63469763/JoeO_normal.jpg</profile_image_url>
  <url>http://agilejoe.lostechies.com</url>
  <protected>false</protected>
  <followers_count>429</followers_count>
  <profile_background_color>1A1B1F</profile_background_color>
  <profile_text_color>666666</profile_text_color>
  <profile_link_color>2FC2EF</profile_link_color>
  <profile_sidebar_fill_color>252429</profile_sidebar_fill_color>
  <profile_sidebar_border_color>181A1E</profile_sidebar_border_color>
  <friends_count>175</friends_count>
  <created_at>Wed Oct 24 19:28:03 +0000 2007</created_at>
  <favourites_count>1</favourites_count>
  <utc_offset>-21600</utc_offset>
  <time_zone>Central Time (US &amp; Canada)</time_zone>
  <profile_background_image_url>http://static.twitter.com/images/themes/theme9/bg.gif</profile_background_image_url>
  <profile_background_tile>false</profile_background_tile>
  <statuses_count>1946</statuses_count>
  <notifications></notifications>
  <following></following>
  <status>
    <created_at>Tue May 26 06:19:51 +0000 2009</created_at>
    <id>1921037538</id>
    <text>My dog goes nuts in thunderstorms!  He is like a child.</text>
    <source>&lt;a href='http://www.atebits.com/'&gt;Tweetie&lt;/a&gt;</source>
    <truncated>false</truncated>
    <in_reply_to_status_id></in_reply_to_status_id>
    <in_reply_to_user_id></in_reply_to_user_id>
    <favorited>false</favorited>
    <in_reply_to_screen_name></in_reply_to_screen_name>
  </status>
</user>
</users>";
                #endregion
            }
        }

        [Test]
        public void Found_Two_Friends()
        {
            Assert.That(RetrievedInfo.Count(), Is.EqualTo(2));
        }
    }
}
