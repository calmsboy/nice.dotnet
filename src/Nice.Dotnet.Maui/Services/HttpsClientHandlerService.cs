namespace Nice.Dotnet.Maui.Services
{
    public class HttpsClientHandlerService
    {
        public HttpMessageHandler GetPlatformMessageHandler()
        {

#if ANDROID
            var handler = new CustomAndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
#elif IOS
            var handler = new NSUrlSessionHandler
            {
                TrustOverrideForUrl = IsHttpsLocalhost
            };
            return handler;
#else
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender,cert,chain,sslPolicyErrors)=>{
                return true;
            };
            return handler;

            //throw new PlatformNotSupportedException("Only Android and iOS supported.");
#endif
        }

#if ANDROID
        internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
        {
            protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
                => new CustomHostnameVerifier();

            private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
            {
                public bool Verify(string hostname, Javax.Net.Ssl.ISSLSession session)
                {
                    return Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session) ||
                        hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
                }
            }
        }
#elif IOS
        public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
        {
            if (url.StartsWith("https://localhost"))
                return true;
            return false;
        }
#endif
    }
}
