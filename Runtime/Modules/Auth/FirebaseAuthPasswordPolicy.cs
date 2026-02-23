using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthPasswordPolicy
    {
        public string allowedNonAlphanumericCharacters { get; set; }
        public object customStrengthOptions { get; set; }
        public string enforcementState { get; set; }
        public bool forceUpgradeOnSignin { get; set; }

        [Serializable]
        public sealed class CustomStrengthOptions
        {
            public int? minPasswordLength { get; set; }
            public int? maxPasswordLength { get; set; }
            public bool? containsLowercaseLetter { get; set; }
            public bool? containsUppercaseLetter { get; set; }
            public bool? containsNumericCharacter { get; set; }
            public bool? containsNonAlphanumericCharacter { get; set; }
        }
    }
}