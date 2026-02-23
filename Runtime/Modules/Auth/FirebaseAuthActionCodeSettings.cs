using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthActionCodeSettings
    {
        public Android android { get; set; }
        public string dynamicLinkDomain { get; set; }
        public bool handleCodeInApp { get; set; }
        public IOS iOS { get; set; }
        public string linkDomain { get; set; }
        public string url { get; set; }

        [Serializable]
        public sealed class Android
        {
            public bool installApp { get; set; }
            public string minimumVersion { get; set; }
            public string packageName { get; set; }
        }

        [Serializable]
        public sealed class IOS
        {
            public string bundleId { get; set; }
        }
    }
}
