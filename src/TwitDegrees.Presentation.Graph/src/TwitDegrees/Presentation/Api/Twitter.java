
package TwitDegrees.Presentation.Api;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.logging.Logger;
import javax.xml.namespace.QName;
import javax.xml.ws.Service;
import javax.xml.ws.WebEndpoint;
import javax.xml.ws.WebServiceClient;
import javax.xml.ws.WebServiceFeature;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.1.6 in JDK 6
 * Generated source version: 2.1
 * 
 */
@WebServiceClient(name = "Twitter", targetNamespace = "http://tempuri.org/", wsdlLocation = "http://localhost:1066/Api/Twitter.svc?wsdl")
public class Twitter
    extends Service
{

    private final static URL TWITTER_WSDL_LOCATION;
    private final static Logger logger = Logger.getLogger(TwitDegrees.Presentation.Api.Twitter.class.getName());

    static {
        URL url = null;
        try {
            URL baseUrl;
            baseUrl = TwitDegrees.Presentation.Api.Twitter.class.getResource(".");
            url = new URL(baseUrl, "http://localhost:1066/Api/Twitter.svc?wsdl");
        } catch (MalformedURLException e) {
            logger.warning("Failed to create URL for the wsdl Location: 'http://localhost:1066/Api/Twitter.svc?wsdl', retrying as a local file");
            logger.warning(e.getMessage());
        }
        TWITTER_WSDL_LOCATION = url;
    }

    public Twitter(URL wsdlLocation, QName serviceName) {
        super(wsdlLocation, serviceName);
    }

    public Twitter() {
        super(TWITTER_WSDL_LOCATION, new QName("http://tempuri.org/", "Twitter"));
    }

    /**
     * 
     * @return
     *     returns ITwitter
     */
    @WebEndpoint(name = "Twitter")
    public ITwitter getTwitter() {
        return super.getPort(new QName("http://tempuri.org/", "Twitter"), ITwitter.class);
    }

    /**
     * 
     * @param features
     *     A list of {@link javax.xml.ws.WebServiceFeature} to configure on the proxy.  Supported features not in the <code>features</code> parameter will have their default values.
     * @return
     *     returns ITwitter
     */
    @WebEndpoint(name = "Twitter")
    public ITwitter getTwitter(WebServiceFeature... features) {
        return super.getPort(new QName("http://tempuri.org/", "Twitter"), ITwitter.class, features);
    }

}
