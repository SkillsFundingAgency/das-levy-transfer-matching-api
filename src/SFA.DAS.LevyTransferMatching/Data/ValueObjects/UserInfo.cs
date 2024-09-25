namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects;

public class UserInfo(string userId, string userDisplayName)
{
    public string UserId { get; } = userId;
    public string UserDisplayName { get; } = userDisplayName;

    public static UserInfo System => new UserInfo(string.Empty, string.Empty);
}