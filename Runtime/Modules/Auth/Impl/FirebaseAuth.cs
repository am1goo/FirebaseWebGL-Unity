using AOT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace FirebaseWebGL
{
    internal sealed class FirebaseAuth : IFirebaseAuth
    {
        [DllImport("__Internal")]
        private static extern bool FirebaseWebGL_FirebaseAuth_initialize();
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_connectAuthEmulator(string url, string optionsAsJson);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_applyActionCode(string oobCode, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_checkActionCode(string oobCode, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_confirmPasswordReset(string oobCode, string newPassword, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_createUserWithEmailAndPassword(string email, string password, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_fetchSignInMethodsForEmail(string email, int requestId, FirebaseJsonCallbackDelegate callback);
        //getMultiFactorResolver
        //getRedirectResult
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_initializeRecaptchaConfig(int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern bool FirebaseWebGL_FirebaseAuth_isSignInWithEmailLink(string emailLink);
        //beforeAuthStateChanged
        //onAuthStateChanged
        //onIdTokenChanged
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_revokeAccessToken(string token, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_sendPasswordResetEmail(string email, string actionCodeSettingsAsJson, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_sendSignInLinkToEmail(string email, string actionCodeSettingsAsJson, int requestId, FirebaseJsonCallbackDelegate callback);
        //setPersistence
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_signInAnonymously(int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_signInWithCredential(string credentialAsJson, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_signInWithCustomToken(string customToken, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_signInWithEmailAndPassword(string email, string password, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_signInWithEmailLink(string email, string emailLink, int requestId, FirebaseJsonCallbackDelegate callback);
        //signInWithPhoneNumber
        //signInWithPopup
        //signInWithRedirect
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_signOut(int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_updateCurrentUser(int userId, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_useDeviceLanguage();
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_validatePassword(string password, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseAuth_verifyPasswordResetCode(string code, int requestId, FirebaseJsonCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern string FirebaseWebGL_FirebaseAuth_parseActionCodeURL(string link);

        private static readonly FirebaseRequests _requests = new FirebaseRequests();
        private static readonly Dictionary<int, Action<FirebaseCallback<bool>>> _onBoolCallbacks = new Dictionary<int, Action<FirebaseCallback<bool>>>();
        private static readonly Dictionary<int, Action<FirebaseCallback<string>>> _onStringCallbacks = new Dictionary<int, Action<FirebaseCallback<string>>>();
        private static readonly Dictionary<int, Action<FirebaseCallback<string[]>>> _onStringArrayCallbacks = new Dictionary<int, Action<FirebaseCallback<string[]>>>();
        private static readonly Dictionary<int, Action<FirebaseCallback<FirebaseAuthUserCredential>>> _onUserCredentialCallbacks = new Dictionary<int, Action<FirebaseCallback<FirebaseAuthUserCredential>>>();
        private static readonly Dictionary<int, Action<FirebaseCallback<FirebaseAuthActionCodeInfo>>> _onActionCodeInfoCallbacks = new Dictionary<int, Action<FirebaseCallback<FirebaseAuthActionCodeInfo>>>();
        private static readonly Dictionary<int, Action<FirebaseCallback<FirebaseAuthPasswordValidationStatus>>> _onPasswordValidationStatusCallbacks = new Dictionary<int, Action<FirebaseCallback<FirebaseAuthPasswordValidationStatus>>>();

        private bool _isInitialized;
        public bool isInitialized => _isInitialized;

        public Action<bool> onInitialized { get; set; }

        public void Initialize(Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (_isInitialized)
            {
                firebaseCallback?.Invoke(FirebaseCallback<bool>.Success(_isInitialized));
                return;
            }

            if (Application.isEditor)
            {
                firebaseCallback?.Invoke(FirebaseCallback<bool>.Success(false));
                return;
            }

            _isInitialized = FirebaseWebGL_FirebaseAuth_initialize();
            onInitialized?.Invoke(_isInitialized);
            firebaseCallback?.Invoke(FirebaseCallback<bool>.Success(_isInitialized));
        }

        public void ConnectAuthEmulator(string url, FirebaseAuthEmulatorOptions options)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var optionsAsJson = options != null ? JsonConvert.SerializeObject(options) : null;
            FirebaseWebGL_FirebaseAuth_connectAuthEmulator(url, optionsAsJson);
        }

        public void ApplyActionCode(string oobCode, Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_applyActionCode(oobCode, requestId, OnBoolCallback);
        }

        public void CheckActionCode(string oobCode, Action<FirebaseCallback<FirebaseAuthActionCodeInfo>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onActionCodeInfoCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_checkActionCode(oobCode, requestId, OnActionCodeInfoCallback);
        }

        public void ConfirmPasswordReset(string oobCode, string newPassword, Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_confirmPasswordReset(oobCode, newPassword, requestId, OnBoolCallback);
        }

        public void CreateUserWithEmailAndPassword(string email, string password, Action<FirebaseCallback<FirebaseAuthUserCredential>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onUserCredentialCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_createUserWithEmailAndPassword(email, password, requestId, OnUserCredentialCallback);
        }

        public void FetchSignInMethodsForEmail(string email, Action<FirebaseCallback<string[]>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onStringArrayCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_fetchSignInMethodsForEmail(email, requestId, OnStringArrayCallback);
        }

        public void InitializeRecaptchaConfig(string email, Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_initializeRecaptchaConfig(requestId, OnBoolCallback);
        }

        public bool IsSignInWithEmailLink(string emailLink)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            return FirebaseWebGL_FirebaseAuth_isSignInWithEmailLink(emailLink);
        }

        public void RevokeAccessToken(string token, Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_revokeAccessToken(token, requestId, OnBoolCallback);
        }

        public void SendPasswordResetEmail(string email, FirebaseAuthActionCodeSettings actionCodeSettings, Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            var actionCodeSettingsAsJson = JsonConvert.SerializeObject(actionCodeSettings);
            FirebaseWebGL_FirebaseAuth_sendPasswordResetEmail(email, actionCodeSettingsAsJson, requestId, OnBoolCallback);
        }

        public void SendSignInLinkToEmail(string email, FirebaseAuthActionCodeSettings actionCodeSettings, Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            var actionCodeSettingsAsJson = JsonConvert.SerializeObject(actionCodeSettings);
            FirebaseWebGL_FirebaseAuth_sendSignInLinkToEmail(email, actionCodeSettingsAsJson, requestId, OnBoolCallback);
        }

        public void SignInAnonymously(Action<FirebaseCallback<FirebaseAuthUserCredential>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onUserCredentialCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_signInAnonymously(requestId, OnUserCredentialCallback);
        }

        public void SignInWithCredential(FirebaseAuthUserCredential credential, Action<FirebaseCallback<FirebaseAuthUserCredential>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onUserCredentialCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            var credentialAsJson = JsonConvert.SerializeObject(credential);
            FirebaseWebGL_FirebaseAuth_signInWithCredential(credentialAsJson, requestId, OnUserCredentialCallback);
        }

        public void SignInWithCustomToken(string customToken, Action<FirebaseCallback<FirebaseAuthUserCredential>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onUserCredentialCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_signInWithCustomToken(customToken, requestId, OnUserCredentialCallback);
        }

        public void SignInWithEmailAndPassword(string email, string password, Action<FirebaseCallback<FirebaseAuthUserCredential>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onUserCredentialCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_signInWithEmailAndPassword(email, password, requestId, OnUserCredentialCallback);
        }

        public void SignInWithEmailLink(string email, string emailLink, Action<FirebaseCallback<FirebaseAuthUserCredential>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onUserCredentialCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_signInWithEmailLink(email, emailLink, requestId, OnUserCredentialCallback);
        }

        public void SignOut(Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_signOut(requestId, OnBoolCallback);
        }

        public void UpdateCurrentUser(int userId, Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onBoolCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_updateCurrentUser(userId, requestId, OnBoolCallback);
        }

        public void UseDeviceLanguage()
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            FirebaseWebGL_FirebaseAuth_useDeviceLanguage();
        }

        public void ValidatePassword(string password, Action<FirebaseCallback<FirebaseAuthPasswordValidationStatus>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onPasswordValidationStatusCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_validatePassword(password, requestId, OnPasswordValidationStatusCallback);
        }

        public void VerifyPasswordResetCode(string code, Action<FirebaseCallback<string>> firebaseCallback)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var requestId = _requests.NextId();
            _onStringCallbacks.Add(requestId, (callback) =>
            {
                firebaseCallback?.Invoke(callback);
            });

            FirebaseWebGL_FirebaseAuth_verifyPasswordResetCode(code, requestId, OnStringCallback);
        }

        public FirebaseAuthActionCodeURL ParseActionCodeURL(string link)
        {
            if (!_isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            var actionCodeUrlAsJson = FirebaseWebGL_FirebaseAuth_parseActionCodeURL(link);
            return JsonConvert.DeserializeObject<FirebaseAuthActionCodeURL>(actionCodeUrlAsJson);
        }

        [MonoPInvokeCallback(typeof(FirebaseJsonCallbackDelegate))]
        private static void OnBoolCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<bool>>(json);

            if (_onBoolCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onBoolCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        [MonoPInvokeCallback(typeof(FirebaseJsonCallbackDelegate))]
        private static void OnStringCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<string>>(json);

            if (_onStringCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onStringCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        [MonoPInvokeCallback(typeof(FirebaseJsonCallbackDelegate))]
        private static void OnStringArrayCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<string[]>>(json);

            if (_onStringArrayCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onStringArrayCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        [MonoPInvokeCallback(typeof(FirebaseJsonCallbackDelegate))]
        private static void OnActionCodeInfoCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<FirebaseAuthActionCodeInfo>>(json);

            if (_onActionCodeInfoCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onActionCodeInfoCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        [MonoPInvokeCallback(typeof(FirebaseJsonCallbackDelegate))]
        private static void OnUserCredentialCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<FirebaseAuthUserCredential>>(json);

            if (_onUserCredentialCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onUserCredentialCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        [MonoPInvokeCallback(typeof(FirebaseJsonCallbackDelegate))]
        private static void OnPasswordValidationStatusCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<FirebaseAuthPasswordValidationStatus>>(json);

            if (_onPasswordValidationStatusCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onPasswordValidationStatusCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}
