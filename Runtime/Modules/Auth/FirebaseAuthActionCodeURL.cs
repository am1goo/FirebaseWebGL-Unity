using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthActionCodeURL
    {
        public string apiKey { get; set; }
        public string code { get; set; }
        public string continueUrl { get; set; }
        public string languageCode { get; set; }
        public string operation { get; set; }
        public string tenantId { get; set; }
    }
}
