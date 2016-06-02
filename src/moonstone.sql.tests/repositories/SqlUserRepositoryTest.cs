using FluentAssertions;
using moonstone.core.models;
using moonstone.sql.context;
using moonstone.sql.repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.sql.tests.repositories
{
    [TestFixture]
    public class SqlUserRepositoryTest
    {
        protected SqlContext Context { get; set; }
        protected SqlUserRepository UserRepository { get; set; }

        [Test]
        public void Create_Can_Insert_New_User()
        {
            var user = GetNewUser();
            bool doesNotExistBeforeCreation = false;
            bool existsAfterCreation = false;

            doesNotExistBeforeCreation = this.UserRepository.GetByEmail(user.Email) == null;
            this.UserRepository.Create(user);
            existsAfterCreation = this.UserRepository.GetByEmail(user.Email) != null;

            Assert.IsTrue(doesNotExistBeforeCreation);
            Assert.IsTrue(existsAfterCreation);
        }

        [Test]
        public void Create_Returns_Id()
        {
            var user = GetNewUser();

            var id = this.UserRepository.Create(user);

            id.Should().NotBe(default(Guid));
        }

        [Test]
        public void GetByEmail_Can_Find_User()
        {
            var user = GetNewUser();

            this.UserRepository.Create(user);

            var foundUser = this.UserRepository.GetByEmail(user.Email);

            foundUser.Should().NotBeNull();
            foundUser.Email.ShouldBeEquivalentTo(user.Email);
        }

        [Test]
        public void GetById_Can_Find_User()
        {
            var user = GetNewUser();

            var id = this.UserRepository.Create(user);

            var foundUser = this.UserRepository.GetById(id);

            foundUser.Should().NotBeNull();
            foundUser.Email.ShouldBeEquivalentTo(user.Email);
        }

        [SetUp]
        public void Setup()
        {
            this.Context = new SqlContext("moonstone_dev_tests", ".");
            this.UserRepository = new SqlUserRepository(this.Context);
            this.Context.RegisterModelDescription(SqlModelDescription<User>.Auto("core", "users"));
        }

        private static User GetNewUser()
        {
            var user = new User
            {
                Email = $"{Guid.NewGuid()}@schwindelig.ch"
            };

            return user;
        }
    }
}