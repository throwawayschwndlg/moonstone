using moonstone.core.exceptions.serviceExceptions;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services
{
    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(RepositoryHub repoHub)
            : base(repoHub)
        {
        }

        public Transaction CreateExpense(decimal amount, Guid categoryId, Guid createUserId, string currency, string description, Guid groupId, Guid sourceBankAccountId, string title, DateTime valueDateUtc)
        {
            try
            {
                return this.CreateTransaction(new Transaction
                {
                    Amount = amount,
                    CategoryId = categoryId,
                    CreateUserId = createUserId,
                    CreationDateUtc = DateTime.UtcNow,
                    Currency = currency,
                    Description = description,
                    GroupId = groupId,
                    SourceBankAccountId = sourceBankAccountId,
                    Title = title,
                    ValueDateUtc = valueDateUtc
                });
            }
            catch (Exception e)
            {
                throw new CreateExpenseException(
                    $"Failed to create expense.", e);
            }
        }

        public Transaction CreateIncome(decimal amount, Guid categoryId, Guid createUserId, string currency, string description, Guid groupId, Guid destinationBankAccountId, string title, DateTime valueDateUtc)
        {
            try
            {
                return this.CreateTransaction(new Transaction
                {
                    Amount = amount,
                    CategoryId = categoryId,
                    CreateUserId = createUserId,
                    CreationDateUtc = DateTime.UtcNow,
                    Currency = currency,
                    Description = description,
                    DestinationBankAccountId = destinationBankAccountId,
                    GroupId = groupId,
                    Title = title,
                    ValueDateUtc = valueDateUtc
                });
            }
            catch (Exception e)
            {
                throw new CreateExpenseException(
                    $"Failed to create expense.", e);
            }
        }

        public Transaction CreateTransaction(Transaction transaction)
        {
            try
            {
                // TODO: Validate all foreign keys (bankAccountId, groupId, categoryId, etc.)

                return this.GetTransactionById(
                    this.Repositories.TransactionRepository.Create(transaction));
            }
            catch (Exception e)
            {
                throw new CreateTransactionException(
                    $"Failed to create transaction.", e);

                throw;
            }
        }

        public Transaction GetTransactionById(Guid id)
        {
            return this.Repositories.TransactionRepository.GetById(id);
        }
    }
}