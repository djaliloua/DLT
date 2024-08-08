using ManagPassWord.CustomClasses;
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
            await Task.Delay(2000);
            if (!DeviceUtility.IsVirtual)
            {
                var request = new AuthenticationRequestConfiguration("Prove you have fingers!", "Because without it you can't have access");
                request.AllowAlternativeAuthentication = true;
                var result = await CrossFingerprint.Current.AuthenticateAsync(request);
                if (!result.Authenticated)
                {
                    Application.Current.Quit();
                }
            }
        }
    }
}
