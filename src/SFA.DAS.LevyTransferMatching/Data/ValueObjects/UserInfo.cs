namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects
{
    public class UserInfo
    {
        public UserInfo(string userId, string userDisplayName)
        {
            UserId = userId;
            UserDisplayName = userDisplayName;
        }

        public string UserId { get; }
        public string UserDisplayName { get; }
    }
}
