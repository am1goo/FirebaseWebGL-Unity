using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthActionCodeInfo
    {
        public FirebaseAuthActionCodeData data { get; set; }
        public FirebaseAuthActionCodeOperation operation { get; set; }
    }
}
