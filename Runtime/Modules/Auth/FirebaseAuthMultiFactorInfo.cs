using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthMultiFactorInfo
    {
        public string displayName { get; set; }
        public string enrollmentTime { get; set; }
        public string factorId { get;set; }
        public string uid { get; set; }
    }
}
