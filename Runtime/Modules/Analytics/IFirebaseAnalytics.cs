using System;
using System.Collections.Generic;

namespace FirebaseWebGL
{
    public interface IFirebaseAnalytics : IFirebaseModule
    {
        void Initialize(Action<FirebaseCallback<bool>> callback);
        void GetGoogleAnalyticsClientId(Action<FirebaseCallback<string>> callback);
        void SetAnalyticsCollectionEnabled(bool enabled);
        void SetUserId(string userId);
        void SetUserProperties(Dictionary<string, string> properties);
        void SetDefaultEventParameters(Dictionary<string, string> parameters);
        void SetConsent(Dictionary<FirebaseAnalyticsConsentType, FirebaseAnalyticsConsentValue> consent);
        void LogEvent(string eventName);
        void LogEvent(string eventName, Dictionary<string, object> eventParams);
    }
}
