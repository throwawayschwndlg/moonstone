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
    public class BankAccountService : BaseService, IBankAccountService
    {
        protected IGroupService GroupService { get; set; }

        public BankAccountService(RepositoryHub repoHub, IGroupService groupService)
            : base(repoHub)
        {
            this.GroupService = groupService;
        }

        public BankAccount CreateBankAccount(BankAccount bankAccount, decimal startingBalance)
        {
            return this.GetBankAccountById(
                this.Repositories.BankAccountRepository.Create(bankAccount));
        }

        public BankAccount GetBankAccountById(Guid id)
        {
            return this.Repositories.BankAccountRepository.GetById(id);
        }

        public IEnumerable<BankAccount> GetBankAccountsForUser(Guid userId)
        {
            return this.Repositories.BankAccountRepository.GetForUser(userId);
        }

        public BankAccount GetDefaultAccountForuser(Guid userId)
        {
            return this.GetBankAccountsForUser(userId).First();
        }
    }
}