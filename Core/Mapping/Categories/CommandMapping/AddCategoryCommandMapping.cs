using Core.Features.Categories.Commands.Models;
using Core.Features.Categories.Queries.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Categories
{
    public partial class CategoryProfile
    {
        public void AddCategoryCommandMapping()
        {
            CreateMap<AddCategoryCommand, Category>();
        }
    }
}
