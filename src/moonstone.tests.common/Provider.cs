﻿using moonstone.authentication;
using moonstone.core.i18n;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.core.services;
using moonstone.services;
using moonstone.sql.configs;
using moonstone.sql.context;
using moonstone.sql.repositories;
using System;

namespace moonstone.tests.common
{
    public class Provider
    {
        private const string DATABASE = "moonstone_dev_tests";
        private const string PASSWORD = "moonstone_ui";
        private const string SERVER = ".";
        private const string USER = "moonstone_ui";

        public static User GetNewUser()
        {
            var user = new User
            {
                Email = $"{Guid.NewGuid()}@schwindelig.ch",
                PasswordHash = "h4$h",
                Culture = "de-CH"
            };

            return user;
        }

        public static RepositoryHub GetRepositoryHub(SqlContext context)
        {
            return new RepositoryHub(
                new SqlUserRepository(context));
        }

        public static ServiceHub GetServiceHub(RepositoryHub repoHub)
        {
            return new ServiceHub(
                loginService: null, /* until we figure out how to get the crap owin context thingy working in nunit */
                environmentService: new EnvironmentService(new CultureNinja()));
        }

        public static SqlContext GetSqlContext()
        {
            var context = new SqlContext(DATABASE, SERVER, USER, PASSWORD);
            context.RegisterModelDescription(ModelDescriptions.User());

            return context;
        }
    }
}