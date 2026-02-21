using AOT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace FirebaseWebGL
{
    internal sealed class FirebaseMessaging : IFirebaseMessaging
    {
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseMessaging_initialize(int requestId, FirebaseCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseMessaging_getToken(int requestId, FirebaseCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseMessaging_deleteToken(int requestId, FirebaseCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseMessaging_onMessage(FirebaseCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void FirebaseWebGL_FirebaseMessaging_experimentalSetDeliveryMetricsExportedToBigQueryEnabled(bool enabled);

        private readonly FirebaseRequests _requests = new FirebaseRequests();
        private readonly Dictionary<long, Action<FirebaseCallback<bool>>> _onInitializedCallbacks = new Dictionary<long, Action<FirebaseCallback<bool>>>();
        private readonly Dictionary<long, Action<FirebaseCallback<string>>> _onGetTokenCallbacks = new Dictionary<long, Action<FirebaseCallback<string>>>();
        private readonly Dictionary<long, Action<FirebaseCallback<bool>>> _onDeleteTokenCallbacks = new Dictionary<long, Action<FirebaseCallback<bool>>>();

        private static FirebaseMessaging _instance;

        private bool _isInitializing = false;

        private bool _isInitialized;
        public bool isInitialized => _isInitialized;

        public Action<bool> onInitialized { get; set; }
        public Action<string> onMessageReceived { get; set; }

        private string _token;

        public FirebaseMessaging()
        {
            _instance = this;
        }

        public void Initialize(Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (_isInitializing)
                return;

            if (isInitialized)
                return;

            if (Application.isEditor)
            {
                firebaseCallback?.Invoke(new FirebaseCallback<bool>(false));
                return;
            }

            var requestId = _requests.NextId();
            _onInitializedCallbacks.Add(requestId, firebaseCallback);

            FirebaseWebGL_FirebaseMessaging_initialize(requestId, OnInitializationCallback);
        }

        [MonoPInvokeCallback(typeof(FirebaseCallbackDelegate))]
        private static void OnInitializationCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<bool>>(json);
            _instance?.OnInitialized(firebaseCallback);
        }

        private void OnInitialized(FirebaseCallback<bool> firebaseCallback)
        {
            _isInitializing = false;

            if (_onInitializedCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onInitializedCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            if (firebaseCallback.success == false)
            {
                //do nothing
                return;
            }

            _isInitialized = firebaseCallback.result;
            onInitialized?.Invoke(_isInitialized);
        }

        public void GetToken(Action<FirebaseCallback<string>> firebaseCallback)
        {
            if (!isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            if (_token != null)
            {
                firebaseCallback?.Invoke(new FirebaseCallback<string>(_token));
                return;
            }

            var requestId = _requests.NextId();
            _onGetTokenCallbacks.Add(requestId, firebaseCallback);

            FirebaseWebGL_FirebaseMessaging_getToken(requestId, OnGetTokenCallback);
        }

        [MonoPInvokeCallback(typeof(FirebaseCallbackDelegate))]
        private static void OnGetTokenCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<string>>(json);
            _instance?.OnGetToken(firebaseCallback);
        }

        private void OnGetToken(FirebaseCallback<string> firebaseCallback)
        {
            if (_onGetTokenCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onGetTokenCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            if (firebaseCallback.success == false)
            {
                //do nothing
                return;
            }

            _token = firebaseCallback.result;
        }

        public void DeleteToken(Action<FirebaseCallback<bool>> firebaseCallback)
        {
            if (!isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            if (_token == null)
            {
                firebaseCallback?.Invoke(new FirebaseCallback<bool>(true));
                return;
            }

            var requestId = _requests.NextId();
            _onDeleteTokenCallbacks.Add(requestId, firebaseCallback);

            FirebaseWebGL_FirebaseMessaging_deleteToken(requestId, OnDeleteTokenCallback);
        }

        [MonoPInvokeCallback(typeof(FirebaseCallbackDelegate))]
        private static void OnDeleteTokenCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<bool>>(json);
            _instance?.OnDeleteToken(firebaseCallback);
        }

        private void OnDeleteToken(FirebaseCallback<bool> firebaseCallback)
        {
            if (_onDeleteTokenCallbacks.TryGetValue(firebaseCallback.requestId, out var callback))
            {
                _onDeleteTokenCallbacks.Remove(firebaseCallback.requestId);
                try
                {
                    callback?.Invoke(firebaseCallback);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            if (firebaseCallback.success == false)
            {
                //do nothing
                return;
            }

            if (firebaseCallback.result == false)
            {
                //do nothing
                return;
            }

            _token = null;
        }

        public void OnMessage(Action<string> onMessageReceived)
        {
            if (!isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            this.onMessageReceived = onMessageReceived;
            FirebaseWebGL_FirebaseMessaging_onMessage(OnMessageCallback);
        }

        [MonoPInvokeCallback(typeof(FirebaseCallbackDelegate))]
        private static void OnMessageCallback(string json)
        {
            var firebaseCallback = JsonConvert.DeserializeObject<FirebaseCallback<string>>(json);
            _instance?.OnMessage(firebaseCallback);
        }

        private void OnMessage(FirebaseCallback<string> firebaseCallback)
        {
            if (firebaseCallback.success == false)
            {
                //do nothing
                return;
            }

            try
            {
                onMessageReceived?.Invoke(firebaseCallback.result);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public void ExperimentalSetDeliveryMetricsExportedToBigQueryEnabled(bool enabled)
        {
            if (!isInitialized)
                throw new FirebaseModuleNotInitializedException(this);

            FirebaseWebGL_FirebaseMessaging_experimentalSetDeliveryMetricsExportedToBigQueryEnabled(enabled);
        }
    }
}
