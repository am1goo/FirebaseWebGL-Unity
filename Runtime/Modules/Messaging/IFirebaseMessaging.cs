using System;

namespace FirebaseWebGL
{
    public interface IFirebaseMessaging : IFirebaseModule
    {
        void Initialize(Action<FirebaseCallback<bool>> callback);
        void GetToken(Action<FirebaseCallback<string>> callback);
        void DeleteToken(Action<FirebaseCallback<bool>> callback);
        void ExperimentalSetDeliveryMetricsExportedToBigQueryEnabled(bool enabled);

        void OnMessage(Action<string> onMessageReceived);
    }
}
