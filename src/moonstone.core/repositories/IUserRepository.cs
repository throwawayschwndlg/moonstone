﻿using moonstone.core.models;
using System;

namespace moonstone.core.repositories
{
    public interface IUserRepository
    {
        void Create(User user);

        User GetByEmail(string email);

        User GetById(Guid id);
    }
}