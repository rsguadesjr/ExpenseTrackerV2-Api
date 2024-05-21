using ExpenseTracker.Application.Transactions.Commands.UpdateTransaction;
using ExpenseTracker.Contracts.Transaction;
using Mapster;

namespace ExpenseTracker.API.Mappings
{
    public class TransactionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Guid id, CreateTransactionRequest request), UpdateTransactionCommand>()
                .Map(dest => dest.Id, src => src.id)
                .Map(dest => dest, src => src.request);
        }
    }
}
