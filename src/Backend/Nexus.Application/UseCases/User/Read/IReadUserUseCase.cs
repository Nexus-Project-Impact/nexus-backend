using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.User.Read
{
    public interface IReadUserUseCase
    {
        public  Task<ResponseUserData> GetById(Guid id);
    }
}
