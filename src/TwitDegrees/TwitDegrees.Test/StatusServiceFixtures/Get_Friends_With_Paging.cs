using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.StatusServiceFixtures
{

    [TestFixture]
    public class Get_Friends_With_Paging
    {
        [SetUp]
        public void SetUp()
        {
            TwitterClient = new Mock<ITwitterClient>();
            var rateLimitProvider = new Mock<IRateLimitStatusProvider>();
            ServiceUnderTest = new StatusService(TwitterClient.Object, rateLimitProvider.Object);
        }
        protected Mock<ITwitterClient> TwitterClient { get; set; }
        protected StatusService ServiceUnderTest { get; set; }


        [Test]
        public void With_Zero_Friends()
        {
            // aww how sad
            TwitterClient.Setup(c => c.GetFriends("trasa", 1)).Returns("<users></users>");
            IEnumerable<UserInfo> retrievedInfo = ServiceUnderTest.GetFriendsOf("trasa");

            Assert.That(retrievedInfo.Count(), Is.EqualTo(0));
        }

        [Test]
        public void With_Under_100_Friends()
        {
            TwitterClient.Setup(c => c.GetFriends("trasa", 1)).Returns(BuildFriends(10));
            IEnumerable<UserInfo> retrievedInfo = ServiceUnderTest.GetFriendsOf("trasa");

            Assert.That(retrievedInfo.Count(), Is.EqualTo(10));
        }

        [Test]
        public void With_Over_100_Friends()
        {
            TwitterClient.Setup(c => c.GetFriends("trasa", 1)).Returns(BuildFriends(100));
            TwitterClient.Setup(c => c.GetFriends("trasa", 2)).Returns(BuildFriends(5));

            IEnumerable<UserInfo> retrievedInfo = ServiceUnderTest.GetFriendsOf("trasa");
            Assert.That(retrievedInfo.Count(), Is.EqualTo(105));
        }

        [Test]
        public void With_Exactly_100_Friends()
        {
            TwitterClient.Setup(c => c.GetFriends("trasa", 1)).Returns(BuildFriends(100));
            TwitterClient.Setup(c => c.GetFriends("trasa", 2)).Returns("<users></users>");
            IEnumerable<UserInfo> retrievedInfo = ServiceUnderTest.GetFriendsOf("trasa");
            Assert.That(retrievedInfo.Count(), Is.EqualTo(100));
        }

        [Test]
        public void With_Over_200_Friends()
        {
            TwitterClient.Setup(c => c.GetFriends("trasa", 1)).Returns(BuildFriends(100));
            TwitterClient.Setup(c => c.GetFriends("trasa", 2)).Returns(BuildFriends(100));
            TwitterClient.Setup(c => c.GetFriends("trasa", 3)).Returns(BuildFriends(99));

            IEnumerable<UserInfo> retrievedInfo = ServiceUnderTest.GetFriendsOf("trasa");
            Assert.That(retrievedInfo.Count(), Is.EqualTo(299));
        }

        [Test]
        public void With_Exactly_200_Friends()
        {
            TwitterClient.Setup(c => c.GetFriends("trasa", 1)).Returns(BuildFriends(100));
            TwitterClient.Setup(c => c.GetFriends("trasa", 2)).Returns(BuildFriends(100));
            TwitterClient.Setup(c => c.GetFriends("trasa", 3)).Returns("<users></users>");
            IEnumerable<UserInfo> retrievedInfo = ServiceUnderTest.GetFriendsOf("trasa");
            Assert.That(retrievedInfo.Count(), Is.EqualTo(200));
        }
            

        private static string BuildFriends(int friendCount)
        {
            var result = new StringBuilder("<users>");
            for (int i =0; i < friendCount; i++)
            {
                #region xml appended
                result.Append(@"<user>
  <id>12958792</id>
  <name>person</name>
  <screen_name>jasonkelly</screen_name>
  <location>Kirkland, Washington</location>
  <description></description>
  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/47161282/JasonKelly_normal.png</profile_image_url>
  <url></url>
  <protected>false</protected>
  <followers_count>25</followers_count>
  <profile_background_color>9ae4e8</profile_background_color>
  <profile_text_color>000000</profile_text_color>
  <profile_link_color>0000ff</profile_link_color>
  <profile_sidebar_fill_color>e0ff92</profile_sidebar_fill_color>
  <profile_sidebar_border_color>87bc44</profile_sidebar_border_color>
  <friends_count>17</friends_count>
  <created_at>Fri Feb 01 20:56:33 +0000 2008</created_at>
  <favourites_count>0</favourites_count>
  <utc_offset>-32400</utc_offset>
  <time_zone>Alaska</time_zone>
  <profile_background_image_url>http://static.twitter.com/images/themes/theme1/bg.gif</profile_background_image_url>
  <profile_background_tile>false</profile_background_tile>
  <statuses_count>23</statuses_count>
  <notifications></notifications>
  <following></following>
  <status>
    <created_at>Wed Apr 01 05:27:35 +0000 2009</created_at>
    <id>1429642547</id>
    <text>Yet Another Enemy at The Spot...  Awesomerockness</text>
    <source>web</source>
    <truncated>false</truncated>
    <in_reply_to_status_id></in_reply_to_status_id>
    <in_reply_to_user_id></in_reply_to_user_id>
    <favorited>false</favorited>
    <in_reply_to_screen_name></in_reply_to_screen_name>
  </status>
</user>
");
                #endregion
            }
            result.Append("</users>");
            return result.ToString();
        }
    }
}
