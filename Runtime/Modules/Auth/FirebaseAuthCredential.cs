using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthCredential
    {
        public string providerId { get; set; }
        public FirebaseAuthSignInMethod signInMethod { get; set; }
    }
}
