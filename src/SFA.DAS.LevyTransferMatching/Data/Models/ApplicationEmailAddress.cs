using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class ApplicationEmailAddress : Entity
    {
        public ApplicationEmailAddress() {}

        public ApplicationEmailAddress(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public int Id { get; protected set; }

        public int ApplicationId { get; protected set; }

        public string EmailAddress { get; protected set; }
    }
}
