package TwitDegrees.Presentation;

import TwitDegrees.Presentation.Api.Twitter;

/**
 * Created by IntelliJ IDEA.
 * User: trasa
 * Date: Jun 10, 2009
 * Time: 11:56:05 PM
 * To change this template use File | Settings | File Templates.
 */

public class TwitterClient {

    Twitter service;

    public TwitterClient() {
        service = new Twitter();
    }

    public String getUserGraphML(String name) {
        return service.getTwitter().getUserGraphML(name);
    }

    public static void main(String[] argv) {
        TwitterClient client = new TwitterClient();
        System.out.println("Graph Output:");
        System.out.println(client.getUserGraphML("trasa"));

      // well, it works, sortof...
//      System.out.println(user.getName().getValue());
//      System.out.println(user.getFriends().getValue().getTwitterUser().isEmpty());
//      System.out.println(((TwitterUserProxy)user.getFriends().getValue().getTwitterUser().toArray()[0]).getName().getValue());
  }
}