using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthActionCodeData
    {
        public string email { get; set; }
        public FirebaseAuthMultiFactorInfo multiFactorInfo { get; set; }
        public string previousEmail { get; set; }
    }
}
