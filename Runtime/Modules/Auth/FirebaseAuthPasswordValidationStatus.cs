using System;

namespace FirebaseWebGL
{
    [Serializable]
    public sealed class FirebaseAuthPasswordValidationStatus
    {
        public bool containsLowercaseLetter { get; set; }
        public bool containsNonAlphanumericCharacter { get; set; }
        public bool containsNumericCharacter { get; set; }
        public bool containsUppercaseLetter { get; set; }
        public bool isValid { get; set; }
        public bool meetsMaxPasswordLength { get; set; }
        public bool meetsMinPasswordLength { get; set; }
        public FirebaseAuthPasswordPolicy passwordPolicy { get; set; }
    }
}