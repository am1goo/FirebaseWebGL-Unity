using System;

namespace FirebaseWebGL
{
    public interface IFirebaseModule
    {
        bool isInitialized { get; }
        Action<bool> onInitialized { get; set; }
    }
}
