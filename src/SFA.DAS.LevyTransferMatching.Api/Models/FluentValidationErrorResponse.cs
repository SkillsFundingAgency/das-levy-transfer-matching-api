using FluentValidation.Results;

namespace SFA.DAS.LevyTransferMatching.Api.Models;

public class FluentValidationErrorResponse
{
    public IEnumerable<ValidationFailure> Errors { get; set; }
}