using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.UseCases
{
    public class CreateAvailabilityExceptionUseCase
    {
        private readonly IRegisterGateway _registerGateway;
        private readonly IAvailabilityExceptionRepository _availabilityExceptionRepository;

        public CreateAvailabilityExceptionUseCase(
            IRegisterGateway registerGateway,
            IAvailabilityExceptionRepository availabilityExceptionRepository)
        {
            _registerGateway = registerGateway;
            _availabilityExceptionRepository = availabilityExceptionRepository;
        }

        public async Task Run(CreateAvailabilityExceptionDto dto, Guid userId)
        {
            Guid VeterinarianId = await _registerGateway.GetVeterinarianIdByUserIdAsync(userId);
            if (VeterinarianId == Guid.Empty)
                throw new DomainException("Usuário não é um veterinário.");
           

            if (dto.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new DomainException("Não é possível bloquear uma data no passado.");

            var exception = new AvailabilityException(VeterinarianId, dto.Date, dto.Reason);
            await _availabilityExceptionRepository.AddAsync(exception);
        }
    }
}
