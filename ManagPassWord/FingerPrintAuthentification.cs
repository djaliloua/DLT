using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace ManagPassWord
{
    public class FingerPrintAuthentification
    {
        private readonly IFingerprint _fingerprint;
        public FingerPrintAuthentification(IFingerprint fingerprint)
        {
            _fingerprint = fingerprint;
        }
        public FingerPrintAuthentification()
        {
            
        }
        public async Task<FingerprintAvailability> GetAvailabilityAsync(bool allowAlternativeAuthentication = false)
        {
            return await _fingerprint.GetAvailabilityAsync(allowAlternativeAuthentication);
        }
        public async Task Authenticate()
        {
            var request = new AuthenticationRequestConfiguration("Prove you have fingers!", "Because without it you can't have access");
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                // do secret stuff :)
            }
            else
            {
                Application.Current.Quit();
            }
        }
    }
}
