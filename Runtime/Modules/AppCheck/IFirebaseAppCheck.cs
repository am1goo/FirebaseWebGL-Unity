using System;

namespace FirebaseWebGL
{
    public interface IFirebaseAppCheck : IFirebaseModule
    {
        void Initialize(Action<FirebaseCallback<bool>> firebaseCallback);
        void GetLimitedUseToken(Action<FirebaseCallback<string>> firebaseCallback);
        void GetToken(bool forceRefresh, Action<FirebaseCallback<string>> firebaseCallback);
        void OnTokenChanged(Action<string> onTokenChanged);
        void SetTokenAutoRefreshEnabled(bool isTokenAutoRefreshEnabled);
    }
}
