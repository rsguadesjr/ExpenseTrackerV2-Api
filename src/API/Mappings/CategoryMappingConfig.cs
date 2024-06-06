using ExpenseTracker.Application.Categories.Commands.UpdateCategory;
using ExpenseTracker.Contracts.Category;
using Mapster;

namespace ExpenseTracker.API.Mappings
{
    public class CategoryMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Guid id, CreateCategoryRequest request), UpdateCategoryCommand>()
                .Map(dest => dest.Id, src => src.id)
                .Map(dest => dest, src => src.request);
        }
    }
}
