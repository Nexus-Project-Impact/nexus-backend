using FluentValidation;
using Nexus.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.Create
{
    public class CreateReservationValidator : AbstractValidator<RequestRegisterReservationJson>
    {
    }
}
