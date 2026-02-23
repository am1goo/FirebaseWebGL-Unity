using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthUserCredential
    {
        public string operationType{get;set;}
        public string providerId { get; set; }
    }
}
