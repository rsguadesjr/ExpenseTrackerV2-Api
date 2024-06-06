using ExpenseTracker.Application.Accounts.Commands.UpdateAccount;
using ExpenseTracker.Contracts.Account;
using Mapster;

namespace ExpenseTracker.API.Mappings
{
    public class AccountCategoryMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Guid id, CreateAccountRequest request), UpdateAccountCommand>()
                .Map(dest => dest.Id, src => src.id)
                .Map(dest => dest, src => src.request);
        }
    }
}
