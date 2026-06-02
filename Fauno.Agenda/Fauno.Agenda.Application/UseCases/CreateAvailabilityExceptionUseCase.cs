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

        public async Task Run(CreateAvailabilityExceptionDto dto)
        {
            bool vetExists = await _registerGateway.VeterinarianExists(dto.VeterinarianId);
            if (!vetExists)
                throw new DomainException("Veterinário inválido.");

            if (dto.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new DomainException("Não é possível bloquear uma data no passado.");

            var exception = new AvailabilityException(dto.VeterinarianId, dto.Date, dto.Reason);
            await _availabilityExceptionRepository.AddAsync(exception);
        }
    }
}
