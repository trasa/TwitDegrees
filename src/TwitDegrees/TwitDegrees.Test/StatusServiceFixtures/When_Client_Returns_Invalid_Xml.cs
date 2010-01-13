using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Blackfin.Core.NUnit;
using log4net;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.StatusServiceFixtures
{
    [TestFixture]
    public class When_Client_Returns_Invalid_Xml
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Test]
        public void Retry_Gives_Correct_Xml()
        {
            var twitterClient = new GetFriendsSucceedsSecondTimeTwitterClient();
            var rateLimitProvider = new Mock<IRateLimitStatusProvider>();
            var serviceUnderTest = new StatusService(twitterClient, rateLimitProvider.Object) { MaxCommunicationTries = 3 };

            var result = serviceUnderTest.GetFriendsOf("test");
            Assert.IsTrue(twitterClient.GetFriendsCalled);
            Assert.That(result.Single().Name, Is.EqualTo("test"));
        }

        [Test]
        public void All_Retries_Fail()
        {
            var twitterClient = new Mock<ITwitterClient>();
            var rateLimitProvider = new Mock<IRateLimitStatusProvider>();
            var serviceUnderTest = new StatusService(twitterClient.Object, rateLimitProvider.Object) { MaxCommunicationTries = 2 };

            #region xml setup
            twitterClient.Setup(c => c.GetFriends("test", 1)).Returns(@"<?xml version='1.0' encoding='UTF-8'?><users type='array'><user>  <id>33171615</id>  <name>meng he</name>  <screen_name>sophiemeng</screen_name>  <location></location>  <description></description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/146323587/D0103_0939_02_normal.jpg</profile_image_url>  <url></url>  <protected>false</protected>  <followers_count>35</followers_count>  <profile_background_color>9ae4e8</profile_background_color>  <profile_text_color>000000</profile_text_color>  <profile_link_color>0000ff</profile_link_color>  <profile_sidebar_fill_color>e0ff92</profile_sidebar_fill_color>  <profile_sidebar_border_color>87bc44</profile_sidebar_border_color>  <friends_count>57</friends_count>  <created_at>Sun Apr 19 10:02:41 +0000 2009</created_at>  <favourites_count>1</favourites_count>  <utc_offset></utc_offset>  <time_zone></time_zone>  
<profile_background_image_url>http://static.twitter.com/images/themes/theme1/bg.gif</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>7</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    <created_at>Sun May 17 05:29:08 +0000 2009</created_at>    <id>1823622220</id>    <text>I like spring</text>    <source>web</source>    <truncated>false</truncated>    <in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id></in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name></in_reply_to_screen_name>  </status></user><user>  <id>17016988</id>  <name>webovative</name>  <screen_name>webovative</screen_name>  <location></location>  <description>Providing bleeding edge online marketing services to our clients since 1995</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/106650239/twitter_normal.jpg</profile_image_url>  <url></url>  <protected>false</protected>  
<followers_count>1054</followers_count>  <profile_background_color>0099B9</profile_background_color>  <profile_text_color>3C3940</profile_text_color>  <profile_link_color>0099B9</profile_link_color>  <profile_sidebar_fill_color>95E8EC</profile_sidebar_fill_color>  <profile_sidebar_border_color>5ED4DC</profile_sidebar_border_color>  <friends_count>1109</friends_count>  <created_at>Tue Oct 28 06:52:56 +0000 2008</created_at>  <favourites_count>1</favourites_count>  <utc_offset>-28800</utc_offset>  <time_zone>Pacific Time (US &amp; Canada)</time_zone>  <profile_background_image_url>http://static.twitter.com/images/themes/theme4/bg.gif</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>7</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    <created_at>Sun Apr 19 22:00:16 +0000 2009</created_at>    <id>1560863556</id>    <text>@Debbas Lucky dog, pics or it didnt happen ;-)</text>    
<source>&lt;a href=&quot;http://desktop.seesmic.com/&quot;&gt;Seesmic Desktop&lt;/a&gt;</source>    <truncated>false</truncated>    <in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id>15195171</in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name>Debbas</in_reply_to_screen_name>  </status></user><user>  <id>11101852</id>  <name>Sebastian Johnsson</name>  <screen_name>SebastianJ</screen_name>  <location>Sydney, Australia</location>  <description>Swedish entrepreneur interested in startups, web development, blogging, internet marketing, seo and personal development.</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/40386632/sebastian_normal.jpg</profile_image_url>  <url>http://www.sebastianjohnsson.com</url>  <protected>false</protected>  <followers_count>20233</followers_count>  <profile_background_color>9AE4E8</profile_background_color>  <profile_text_color>333333</profile_text_color>  
<profile_link_color>0084B4</profile_link_color>  <profile_sidebar_fill_color>DDFFCC</profile_sidebar_fill_color>  <profile_sidebar_border_color>BDDCAD</profile_sidebar_border_color>  <friends_count>17892</friends_count>  <created_at>Wed Dec 12 20:27:12 +0000 2007</created_at>  <favourites_count>3</favourites_count>  <utc_offset>3600</utc_offset>  <time_zone>Stockholm</time_zone>  <profile_background_image_url>http://static.twitter.com/images/themes/theme1/bg.gif</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>2813</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    <created_at>Wed Jun 03 06:02:18 +0000 2009</created_at>    <id>2013584111</id>    <text>New blog post: Too early...  http://bit.ly/1yDRZm</text>    <source>&lt;a href='http://alexking.org/projects/wordpress'&gt;Twitter Tools&lt;/a&gt;</source>    <truncated>false</truncated>    
<in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id></in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name></in_reply_to_screen_name>  </status></user><user>  <id>33157975</id>  <name>MarketingQuickies3</name>  <screen_name>GetMQ3</screen_name>  <location>UK</location>  <description>Do you want to lift your web sales?</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/146248067/CASH_normal.jpg</profile_image_url>  <url>http://www.getmarketingquickies3.com</url>  <protected>false</protected>  <followers_count>557</followers_count>  <profile_background_color>9AE4E8</profile_background_color>  <profile_text_color>333333</profile_text_color>  <profile_link_color>0084B4</profile_link_color>  <profile_sidebar_fill_color>DDFFCC</profile_sidebar_fill_color>  <profile_sidebar_border_color>BDDCAD</profile_sidebar_border_color>  <friends_count>947</friends_count>  <created_at>Sun Apr 19 07:44:59 +0000 2009</created_at> 
 <favourites_count>0</favourites_count>  <utc_offset>0</utc_offset>  <time_zone>Dublin</time_zone>  <profile_background_image_url>http://s3.amazonaws.com/twitter_production/profile_background_images/9117268/GETMQ3.jpg</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>2</statuses_count>  <notifications></notifications>  <following></following>  <status>    <created_at>Sun Apr 19 08:33:57 +0000 2009</created_at>    <id>1557020320</id>    <text>Sign up for the affiliate program. 50% recurring payouts...</text>    <source>web</source>    <truncated>false</truncated>    <in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id></in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name></in_reply_to_screen_name>  </status></user><user>  <id>18794485</id>  <name>Pinpointe</name>  <screen_name>pinpointe</screen_name>  <location>Silicon Valley USA</location>  
<description>On-Demand B2B Email Marketing Service (Like ConstantContact on steroids)</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/71023613/pinpointe-twitter_normal.gif</profile_image_url>  <url>http://www.pinpointe.com/blog</url>  <protected>false</protected>  <followers_count>1965</followers_count>  <profile_background_color>1A1B1F</profile_background_color>  <profile_text_color>000000</profile_text_color>  <profile_link_color>2FC2EF</profile_link_color>  <profile_sidebar_fill_color>ffffff</profile_sidebar_fill_color>  <profile_sidebar_border_color>181A1E</profile_sidebar_border_color>  <friends_count>363</friends_count>  <created_at>Fri Jan 09 06:26:00 +0000 2009</created_at>  <favourites_count>5</favourites_count>  <utc_offset>-32400</utc_offset>  <time_zone>Alaska</time_zone>  <profile_background_image_url>http://s3.amazonaws.com/twitter_production/profile_background_images/5396491/twitter-bg4.gif</profile_background_image_url>  
<profile_background_tile>false</profile_background_tile>  <statuses_count>76</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    
<created
");
            #endregion
            // exception thrown so we can send an ErrorResponse
            Expect.Exception<XmlException>(() => serviceUnderTest.GetFriendsOf("test"));
        }


        class GetFriendsSucceedsSecondTimeTwitterClient : ITwitterClient
        {

            public bool GetFriendsCalled { get; set; }

            public string GetFriends(string userName, int page)
            {
                #region bunch of xml
                if (!GetFriendsCalled)
                {
                    GetFriendsCalled = true;
                    // invalid
                    return @"<?xml version='1.0' encoding='UTF-8'?><users type='array'><user>  <id>33171615</id>  <name>meng he</name>  <screen_name>sophiemeng</screen_name>  <location></location>  <description></description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/146323587/D0103_0939_02_normal.jpg</profile_image_url>  <url></url>  <protected>false</protected>  <followers_count>35</followers_count>  <profile_background_color>9ae4e8</profile_background_color>  <profile_text_color>000000</profile_text_color>  <profile_link_color>0000ff</profile_link_color>  <profile_sidebar_fill_color>e0ff92</profile_sidebar_fill_color>  <profile_sidebar_border_color>87bc44</profile_sidebar_border_color>  <friends_count>57</friends_count>  <created_at>Sun Apr 19 10:02:41 +0000 2009</created_at>  <favourites_count>1</favourites_count>  <utc_offset></utc_offset>  <time_zone></time_zone>  
<profile_background_image_url>http://static.twitter.com/images/themes/theme1/bg.gif</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>7</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    <created_at>Sun May 17 05:29:08 +0000 2009</created_at>    <id>1823622220</id>    <text>I like spring</text>    <source>web</source>    <truncated>false</truncated>    <in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id></in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name></in_reply_to_screen_name>  </status></user><user>  <id>17016988</id>  <name>webovative</name>  <screen_name>webovative</screen_name>  <location></location>  <description>Providing bleeding edge online marketing services to our clients since 1995</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/106650239/twitter_normal.jpg</profile_image_url>  <url></url>  <protected>false</protected>  
<followers_count>1054</followers_count>  <profile_background_color>0099B9</profile_background_color>  <profile_text_color>3C3940</profile_text_color>  <profile_link_color>0099B9</profile_link_color>  <profile_sidebar_fill_color>95E8EC</profile_sidebar_fill_color>  <profile_sidebar_border_color>5ED4DC</profile_sidebar_border_color>  <friends_count>1109</friends_count>  <created_at>Tue Oct 28 06:52:56 +0000 2008</created_at>  <favourites_count>1</favourites_count>  <utc_offset>-28800</utc_offset>  <time_zone>Pacific Time (US &amp; Canada)</time_zone>  <profile_background_image_url>http://static.twitter.com/images/themes/theme4/bg.gif</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>7</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    <created_at>Sun Apr 19 22:00:16 +0000 2009</created_at>    <id>1560863556</id>    <text>@Debbas Lucky dog, pics or it didnt happen ;-)</text>    
<source>&lt;a href=&quot;http://desktop.seesmic.com/&quot;&gt;Seesmic Desktop&lt;/a&gt;</source>    <truncated>false</truncated>    <in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id>15195171</in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name>Debbas</in_reply_to_screen_name>  </status></user><user>  <id>11101852</id>  <name>Sebastian Johnsson</name>  <screen_name>SebastianJ</screen_name>  <location>Sydney, Australia</location>  <description>Swedish entrepreneur interested in startups, web development, blogging, internet marketing, seo and personal development.</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/40386632/sebastian_normal.jpg</profile_image_url>  <url>http://www.sebastianjohnsson.com</url>  <protected>false</protected>  <followers_count>20233</followers_count>  <profile_background_color>9AE4E8</profile_background_color>  <profile_text_color>333333</profile_text_color>  
<profile_link_color>0084B4</profile_link_color>  <profile_sidebar_fill_color>DDFFCC</profile_sidebar_fill_color>  <profile_sidebar_border_color>BDDCAD</profile_sidebar_border_color>  <friends_count>17892</friends_count>  <created_at>Wed Dec 12 20:27:12 +0000 2007</created_at>  <favourites_count>3</favourites_count>  <utc_offset>3600</utc_offset>  <time_zone>Stockholm</time_zone>  <profile_background_image_url>http://static.twitter.com/images/themes/theme1/bg.gif</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>2813</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    <created_at>Wed Jun 03 06:02:18 +0000 2009</created_at>    <id>2013584111</id>    <text>New blog post: Too early...  http://bit.ly/1yDRZm</text>    <source>&lt;a href='http://alexking.org/projects/wordpress'&gt;Twitter Tools&lt;/a&gt;</source>    <truncated>false</truncated>    
<in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id></in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name></in_reply_to_screen_name>  </status></user><user>  <id>33157975</id>  <name>MarketingQuickies3</name>  <screen_name>GetMQ3</screen_name>  <location>UK</location>  <description>Do you want to lift your web sales?</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/146248067/CASH_normal.jpg</profile_image_url>  <url>http://www.getmarketingquickies3.com</url>  <protected>false</protected>  <followers_count>557</followers_count>  <profile_background_color>9AE4E8</profile_background_color>  <profile_text_color>333333</profile_text_color>  <profile_link_color>0084B4</profile_link_color>  <profile_sidebar_fill_color>DDFFCC</profile_sidebar_fill_color>  <profile_sidebar_border_color>BDDCAD</profile_sidebar_border_color>  <friends_count>947</friends_count>  <created_at>Sun Apr 19 07:44:59 +0000 2009</created_at> 
 <favourites_count>0</favourites_count>  <utc_offset>0</utc_offset>  <time_zone>Dublin</time_zone>  <profile_background_image_url>http://s3.amazonaws.com/twitter_production/profile_background_images/9117268/GETMQ3.jpg</profile_background_image_url>  <profile_background_tile>false</profile_background_tile>  <statuses_count>2</statuses_count>  <notifications></notifications>  <following></following>  <status>    <created_at>Sun Apr 19 08:33:57 +0000 2009</created_at>    <id>1557020320</id>    <text>Sign up for the affiliate program. 50% recurring payouts...</text>    <source>web</source>    <truncated>false</truncated>    <in_reply_to_status_id></in_reply_to_status_id>    <in_reply_to_user_id></in_reply_to_user_id>    <favorited>false</favorited>    <in_reply_to_screen_name></in_reply_to_screen_name>  </status></user><user>  <id>18794485</id>  <name>Pinpointe</name>  <screen_name>pinpointe</screen_name>  <location>Silicon Valley USA</location>  
<description>On-Demand B2B Email Marketing Service (Like ConstantContact on steroids)</description>  <profile_image_url>http://s3.amazonaws.com/twitter_production/profile_images/71023613/pinpointe-twitter_normal.gif</profile_image_url>  <url>http://www.pinpointe.com/blog</url>  <protected>false</protected>  <followers_count>1965</followers_count>  <profile_background_color>1A1B1F</profile_background_color>  <profile_text_color>000000</profile_text_color>  <profile_link_color>2FC2EF</profile_link_color>  <profile_sidebar_fill_color>ffffff</profile_sidebar_fill_color>  <profile_sidebar_border_color>181A1E</profile_sidebar_border_color>  <friends_count>363</friends_count>  <created_at>Fri Jan 09 06:26:00 +0000 2009</created_at>  <favourites_count>5</favourites_count>  <utc_offset>-32400</utc_offset>  <time_zone>Alaska</time_zone>  <profile_background_image_url>http://s3.amazonaws.com/twitter_production/profile_background_images/5396491/twitter-bg4.gif</profile_background_image_url>  
<profile_background_tile>false</profile_background_tile>  <statuses_count>76</statuses_count>  <notifications>false</notifications>  <following>false</following>  <status>    
<created
";
                }
                return @"<?xml version='1.0' encoding='UTF-8'?><users><user>
  <id>12958792</id>
  <name>person</name>
  <screen_name>test</screen_name>
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
</user></users>";
                #endregion
            }

            public string GetFollowers(string userName, int page)
            {
                throw new NotImplementedException();
            }

            public string GetUser(string userName)
            {
                throw new NotImplementedException();
            }
        }
    }
}
