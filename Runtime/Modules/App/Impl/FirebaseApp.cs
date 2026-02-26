using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace FirebaseWebGL
{
    public class FirebaseApp : IFirebaseApp, IDisposable
    {
        [DllImport("__Internal")]
        private static extern bool FirebaseWebGL_FirebaseApp_initalize();
        [DllImport("__Internal")]
        private static extern string FirebaseWebGL_FirebaseApp_installedModules();
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseApp_deleteApp();
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseApp_setLogLevel(int logLevel);

        private bool _isInitialized;
        public bool isInitialized => _isInitialized;

        public Action<bool> onInitialized { get; set; }

        private bool _isDisposed;
        public bool isDisposed => _isDisposed;

        private IFirebaseAuth _auth;
        public IFirebaseAuth Auth => _auth;

        private IFirebaseAnalytics _analytics;
        public IFirebaseAnalytics Analytics => _analytics;

        private IFirebaseAppCheck _appCheck;
        public IFirebaseAppCheck AppCheck => _appCheck;

        private IFirebaseFunctions _functions;
        public IFirebaseFunctions Functions => _functions;

        private IFirebaseMessaging _messaging;
        public IFirebaseMessaging Messaging => _messaging;

        private IFirebaseRemoteConfig _remoteConfig;
        public IFirebaseRemoteConfig RemoteConfig => _remoteConfig;

        private IFirebaseInstallations _installations;
        public IFirebaseInstallations Installations => _installations;

        private IFirebasePerformance _performance;
        public IFirebasePerformance Performance => _performance;

        private IFirebaseStorage _storage;
        public IFirebaseStorage Storage => _storage;

        ~FirebaseApp()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            OnDispose(disposing);
            _isDisposed = true;
        }

        private void OnDispose(bool disposing)
        {
            if (Application.isEditor)
                return;

            try
            {
                FirebaseWebGL_FirebaseApp_deleteApp();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static FirebaseApp DefaultInstance()
        {
            return new FirebaseApp();
        }

        public FirebaseApp()
        {
            if (Application.isEditor)
            {
                _isInitialized = false;
                onInitialized?.Invoke(_isInitialized);
                return;
            }

            _isInitialized = FirebaseWebGL_FirebaseApp_initalize();
            onInitialized?.Invoke(_isInitialized);

            if (!_isInitialized)
            {
                Debug.LogError("FirebaseApp is not initialized, modules registration no needed anymore.");
                return;
            }

            var installedModules = GetInstalledModules();
            if (Contains(installedModules, FirebaseModuleNames.auth))
            {
                _auth = new FirebaseAuth();
            }
            if (Contains(installedModules, FirebaseModuleNames.analytics))
            {
                _analytics = new FirebaseAnalytics();
            }
            if (Contains(installedModules, FirebaseModuleNames.appCheck))
            {
                _appCheck = new FirebaseAppCheck();
            }
            if (Contains(installedModules, FirebaseModuleNames.functions))
            {
                _functions = new FirebaseFunctions();
            }
            if (Contains(installedModules, FirebaseModuleNames.messaging))
            {
                var enableServiceWorker = Contains(installedModules, FirebaseModuleNames.messagingSw);
                _messaging = new FirebaseMessaging(enableServiceWorker);
            }
            if (Contains(installedModules, FirebaseModuleNames.remoteConfig))
            {
                _remoteConfig = new FirebaseRemoteConfig();
            }
            if (Contains(installedModules, FirebaseModuleNames.installations))
            {
                _installations = new FirebaseInstallations();
            }
            if (Contains(installedModules, FirebaseModuleNames.performance))
            {
                _performance = new FirebasePerformance();
            }
            if (Contains(installedModules, FirebaseModuleNames.storage))
            {
                _storage = new FirebaseStorage();
            }
        }

        public string[] GetInstalledModules()
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var installedModulesAsJson = FirebaseWebGL_FirebaseApp_installedModules();
            if (installedModulesAsJson == null)
                return Array.Empty<string>();

            return JsonConvert.DeserializeObject<string[]>(installedModulesAsJson);
        }

        public void SetLogLevel(FirebaseAppLogLevel logLevel)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            FirebaseWebGL_FirebaseApp_setLogLevel((int)logLevel);
        }

        private static bool Contains<T>(T[] array, T value)
        {
            return Array.IndexOf(array, value) >= 0;
        }
    }
}
