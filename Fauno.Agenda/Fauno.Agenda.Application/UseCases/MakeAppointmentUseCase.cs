using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Application.Interfaces.Repositories;
using Fauno.Agenda.Domain.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace Fauno.Agenda.Application.UseCases
{
    public class MakeAppointmentUseCase
    {
        private readonly IRegisterGateway _registerGateway;
        private readonly IAppointmentRepository _appointmentRepository;

        public MakeAppointmentUseCase(IRegisterGateway registerGateway, IAppointmentRepository appointmentRepository) {
            _registerGateway = registerGateway;
            _appointmentRepository = appointmentRepository;
        }


        public async void RunAsync(AppointmentDto appointmentDto)
        {

            bool ownerExisted = await _registerGateway.OwnerExists(appointmentDto.OwnerId);
            bool petExisted = await _registerGateway.PetExists(appointmentDto.OwnerId, appointmentDto.PetId);
            if (!ownerExisted)
                throw new Exception("Dono de pet inválido");
            if (!petExisted)
                throw new Exception("Pet inválido");
            
            Appointment NewAppointment = new Appointment(
                appointmentDto.Description,
                appointmentDto.Title,
                appointmentDto.OwnerId,
                appointmentDto.PetId,
                appointmentDto.Start,
                appointmentDto.End);
            _appointmentRepository.Add(NewAppointment);
        }
        
    }
}
