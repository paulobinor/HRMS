namespace hrms_be_backend_common.Configuration
{
    public class PassowordConfig
    {
        public int MaxNumberOfFailedAttemptsToLogin { get; set; }
        public int MinutesBeforeResetAfterFailedAttemptsToLogin { get; set; }
        public int CharacterLengthMin { get; set; }
        public int CharacterLengthMax { get; set; }
        public int MustContainSpecialCharacter { get; set; }
        public int MustContainUppercase { get; set; }
        public int MustContainLowercase { get; set; }
        public int MustContainNumber { get; set; }
    }
}
