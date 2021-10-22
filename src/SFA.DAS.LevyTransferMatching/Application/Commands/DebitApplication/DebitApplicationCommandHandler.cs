using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication
{
    public class DebitApplicationCommandHandler : IRequestHandler<DebitApplicationCommand, DebitApplicationCommandResult>
    {
        private readonly IApplicationRepository _repository;
        private readonly ILogger<DebitApplicationCommandHandler> _logger;

        public DebitApplicationCommandHandler(IApplicationRepository repository, ILogger<DebitApplicationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DebitApplicationCommandResult> Handle(DebitApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _repository.Get(null, request.ApplicationId, null);

            var success = application.Debit(request.NumberOfApprentices, request.Amount, UserInfo.System);

            await _repository.Update(application);

            return new DebitApplicationCommandResult
            {
                IsSuccess = success
            };
        }
    }
}
