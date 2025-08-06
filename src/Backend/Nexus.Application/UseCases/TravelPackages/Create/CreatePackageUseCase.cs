using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.DTOs;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.Create
{
    public class CreatePackageUseCase : ICreatePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseCreatedPackage> ExecuteCreate(RequestCreatePackage request)
        {
            if (request.Image == null || request.Image.Length == 0)
                throw new ArgumentException("Imagem não fornecida ou vazia.");

            var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName);
            var filePath = Path.Combine(imagesDirectory, fileName);

            using (var stream = new FileStream  (filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            var packageDto = new Nexus.Domain.DTOs.PackageDto
            {
                Title = request.Title,
                Description = request.Description,
                Destination = request.Destination,
                Duration = request.Duration,
                DepartureDate = request.DepartureDate,
                ReturnDate = request.ReturnDate,
                Value = request.Value,
                ImageUrl = fileName
            };

            var package = _mapper.Map<TravelPackage>(packageDto);
            await _repository.AddAsync(package);
            await _unitOfWork.Commit();

            return new ResponseCreatedPackage
            {
                Title = package.Title
            };
        }
    }
}
