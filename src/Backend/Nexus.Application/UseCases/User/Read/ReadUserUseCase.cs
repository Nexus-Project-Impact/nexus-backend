using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.User.Read
{
    public class ReadUserUseCase : IReadUserUseCase
    {
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ReadUserUseCase(
            UserManager<Domain.Entities.User> userManager,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseUserData> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                throw new KeyNotFoundException($"User com este id não foi encontrado.");

            var response = _mapper.Map<ResponseUserData>(user);

            return response;
        }
    }
}
