using FluentValidation.Results;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class FluentValidationErrorResponse
    {
        public IEnumerable<ValidationFailure> Errors { get; set; }
    }
}