using ExpenseTracker.Application.Transactions.Common;
using ExpenseTracker.Domain.Models.Common;
using Mapster;

namespace ExpenseTracker.Application.Common.Mapping
{
    public class TransactionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Transaction, TransactionDto>()
                .Map(dest => dest.Category, src => src.Category != null ? new KeyValueItem<Guid, string>(src.Category.Id, src.Category.Name) : null)
                .Map(dest => dest.Tags, src => src.TransactionTags.Select(x => x.Tag.Name));
        }
    }
}
