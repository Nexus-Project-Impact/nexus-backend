using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nexus.Communication.Requests;

namespace Nexus.Application.UseCases.TravelPackages.Register
{
    public class RegisterPackageValidator : AbstractValidator<RequestRegisterPackageJson>
    {
    }
}
