using UnityEngine;

namespace FirebaseWebGL
{
    [CreateAssetMenu(fileName = nameof(FirebaseSettings), menuName = "Firebase/WebGL Settings")]
    public class FirebaseSettings : ScriptableObject
    {
        [Header("General")]
        [SerializeField]
        private string _apiKey;
        public string apiKey => _apiKey;
        [SerializeField]
        private string _authDomain;
        public string authDomain => _authDomain;
        [SerializeField]
        private string _projectId;
        public string projectId => _projectId;
        [SerializeField]
        private string _storageBucket;
        public string storageBucket => _storageBucket;
        [SerializeField]
        private string _messagingSenderId;
        public string messagingSenderId => _messagingSenderId;
        [SerializeField]
        private string _appId;
        public string appId => _appId;
        [SerializeField]
        private string _measurementId;
        public string measurementId => _measurementId;
        [Header("Modules")]
        [SerializeField]
        private bool _includeAuth;
        public bool includeAuth => _includeAuth;
        [SerializeField]
        private bool _includeAnalytics;
        public bool includeAnalytics => _includeAnalytics;
        [SerializeField]
        private bool _includeFirestore;
        public bool includeFirestore => _includeFirestore;
        [SerializeField]
        private bool _includeMessaging;
        public bool includeMessaging => _includeMessaging;
        [SerializeField]
        private bool _includeRemoteConfig;
        public bool includeRemoteConfig => _includeRemoteConfig;

        private static FirebaseSettings _instance;
        public static FirebaseSettings instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<FirebaseSettings>(nameof(FirebaseSettings));
                }
                return _instance;
            }
        }
    }
}
