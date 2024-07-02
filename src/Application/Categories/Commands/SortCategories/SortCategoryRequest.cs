using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Categories.Commands.SortCategories
{
    public record SortCategoryRequest(Guid Id, int Order);
}
