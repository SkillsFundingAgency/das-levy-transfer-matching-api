using System;

namespace SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions
{
    [Serializable]
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException() { }

        public AggregateNotFoundException(string msg)
            : base($"Record not found. {msg}")
        {

        }
    }
}
