using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Categories.Commands.SortCategories
{
    public class SortCategoriesValidator : AbstractValidator<SortCategoriesCommand>
    {
        public SortCategoriesValidator()
        {
            RuleForEach(x => x.SortRequests)
                .Must(x => x.Order >= 0);

            RuleForEach(x => x.SortRequests)
                .Must(x => x.Id != Guid.Empty);
        }
    }
}
