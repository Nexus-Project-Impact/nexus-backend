using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review.Delete
{
    public interface IDeleteReviewUseCase
    {
        public Task<bool> ExecuteDelete(int id);
    }
}
