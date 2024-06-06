using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Tests
{
    public class BaseApplicationTest
    {
        protected async Task<ExpenseTrackerDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new ExpenseTrackerDbContext(options);

            await SeedData(dbContext);
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }

        // method to seed data
        protected async Task SeedData(ExpenseTrackerDbContext dbContext)
        {
            var users = GetDummyUsers();
            dbContext.Users.AddRange(users.ToList());

            var accounts = GetDummyAccounts(users.First().Id);
            dbContext.Accounts.AddRange(accounts.ToList());

            var categories = GetDummyCategories(users.First().Id);
            dbContext.Categories.AddRange(categories.ToList());

            await dbContext.SaveChangesAsync();
        }


        protected List<Category> GetDummyCategories(Guid userId)
        {
            // seed category entity
            var categories = new List<Category>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Category 1",
                    Description = "Category 1 description",
                    IsActive = true,
                    UserId = userId,
                    Order = 1
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Category 2",
                    Description = "Category 2 description",
                    IsActive = true,
                    UserId = userId,
                    Order = 2
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Category 3",
                    Description = "Category 3 description",
                    IsActive = true,
                    UserId = userId,
                    Order = 1
                }
            };

            return categories;
        }

        protected List<Account> GetDummyAccounts(Guid userId)
        {

            // seed data
            var accounts = new List<Account>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Account 1",
                    Description = "Account 1 description",
                    IsActive = true,
                    UserId = userId,
                    IsDefault = true
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Account 2",
                    Description = "Account 2 description",
                    IsActive = true,
                    UserId = userId,
                    IsDefault = false
                }
            };

            return accounts;
        }

        protected List<User> GetDummyUsers()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "",
                IdentityId = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe"
            };

            return new List<User> { user };
        }
    }
}
