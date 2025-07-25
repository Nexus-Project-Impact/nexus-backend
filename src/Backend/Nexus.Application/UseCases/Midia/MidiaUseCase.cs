    using Microsoft.AspNetCore.Http;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Midia
{
    public class MidiaUseCase : IMidiaUseCase
    {
        public async Task<ResponseMessage> Execute(RequestMidia request)
        {
            if (request.Image == null || request.Image.Length == 0)
                throw new ArgumentException("Imagem não fornecida ou vazia.");

            var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            var fileName = Path.GetFileName(request.Image.FileName);
            var filePath = Path.Combine(imagesDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            return new ResponseMessage
            {
                Message = "Imagem salva com sucesso.",
            };

        }
    }
}
