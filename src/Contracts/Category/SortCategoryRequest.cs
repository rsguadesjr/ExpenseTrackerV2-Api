namespace ExpenseTracker.Contracts.Category
{
    public class SortCategoryRequest
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
    }
}
