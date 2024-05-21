using ExpenseTracker.Application.Transactions.Commands.Common;
using ExpenseTracker.Domain.Models.Common;
using Mapster;

namespace ExpenseTracker.Application.Common.Mapping
{
    public class TransactionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Transaction, TransactionDto>()
                .Map(dest => dest.Category, src => new KeyValueItem<int, string>(22, src.Category != null ? src.Category.Name : string.Empty))
                .Map(dest => dest.Tags, src => src.TransactionTags.Select(x => x.Tag.Name))
                //.Map(dest => dest.CreatedBy, src => new KeyValueItem<Guid?, string>(src.CreatedById, src.CreatedBy != null ? $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}" : string.Empty))
                .Map(dest => dest.CreatedBy, src => src.CreatedBy != null ? new KeyValueItem<Guid?, string>(src.CreatedById, $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}") : null)
                .Map(dest => dest.ModifiedBy, src => src.ModifiedBy != null ? new KeyValueItem<Guid?, string>(src.ModifiedById, $"{src.ModifiedBy.FirstName} {src.ModifiedBy.LastName}") : null)
                ;
        }
    }
}
