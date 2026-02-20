using System;
using UnityEngine;

namespace FirebaseWebGL
{
    public static class FirebaseApp
    {
        private static bool _isInitialized;
        public static bool isInitialized => _isInitialized;

        private static IFirebaseAnalytics _analytics;
        public static IFirebaseAnalytics Analytics => _analytics;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            if (_isInitialized)
                return;

            var settings = FirebaseSettings.instance;
            if (settings == null)
                throw new Exception($"{nameof(FirebaseSettings)} file is not found in {nameof(Resources)} folder");

            if (settings.includeAuth)
            {
                //TODO: add FirebaseAuth initialization here
            }
            if (settings.includeAnalytics)
            {
                _analytics = new FirebaseAnalytics();
            }
            if (settings.includeFirestore)
            {
                //TODO: add FirebaseFirestore initialization here
            }
            if (settings.includeMessaging)
            {
                //TODO: add FirebaseMessaging initialization here
            }
            if (settings.includeRemoteConfig)
            {
                //TODO: add FirebaseRemoteConfig initialization here
            }

            _isInitialized = true;
        }
    }
}
