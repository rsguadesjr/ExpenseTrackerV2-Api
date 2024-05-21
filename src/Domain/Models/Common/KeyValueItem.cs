using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Domain.Models.Common
{
    public class KeyValueItem<TId, TValue>(TId id, TValue value)
    {
        public TId Id { get; set; } = id;
        public TValue Value { get; set; } = value;
    }
}
