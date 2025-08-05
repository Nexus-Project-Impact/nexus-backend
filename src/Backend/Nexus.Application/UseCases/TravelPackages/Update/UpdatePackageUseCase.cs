using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.Update
{
    public class UpdatePackageUseCase : IUpdatePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponsePackage?> ExecuteUpdate(int id, RequestUpdatePackage register)
        {
            var package = await _repository.GetByIdAsync(id);

            if (package == null)
            {
                return null;
            }

            // Handle image upload if provided
            string imageFileName = package.ImageUrl; // Keep existing image if no new one provided
            
            if (register.Image != null && register.Image.Length > 0)
            {
                var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(imagesDirectory))
                    Directory.CreateDirectory(imagesDirectory);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(register.Image.FileName);
                var filePath = Path.Combine(imagesDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await register.Image.CopyToAsync(stream);
                }

                // Delete old image file if it exists and is different
                if (!string.IsNullOrEmpty(package.ImageUrl) && package.ImageUrl != fileName)
                {
                    var oldFilePath = Path.Combine(imagesDirectory, package.ImageUrl);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                imageFileName = fileName;
            }

            // Update package properties
            package.Title = register.Title;
            package.Description = register.Description;
            package.Destination = register.Destination;
            package.Duration = register.Duration;
            package.DepartureDate = register.DepartureDate;
            package.ReturnDate = register.ReturnDate;
            package.Value = register.Value;
            package.ImageUrl = imageFileName;

            await _repository.UpdateAsync(package);

            var packagesJson = _mapper.Map<ResponsePackage>(package);

            await _unitOfWork.Commit();

            return packagesJson;
        }
    }
}
