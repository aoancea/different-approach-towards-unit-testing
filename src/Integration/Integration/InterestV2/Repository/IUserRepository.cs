﻿using System;

namespace Ragnar.Integration.InterestV2.Repository
{
    public interface IUserRepository
    {
        Model.User Detail(Guid userId);
    }
}