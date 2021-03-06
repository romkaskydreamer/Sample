﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.UserManagement.Contract
{
    public interface IUserManagementService
    {
        Task<IList<UserDto>> Get();
        Task<UserDto> CreateUser(string firstName, string lastName, long age);
        Task<UserDto> ChangeUserFirstName(Guid aggregateId, string firstName);
        Task<UserDto> ChangeUserLastName(Guid aggregateId, string lastName);
        Task<UserDto> ChangeUserAge(Guid aggregateId, int age);
        Task<UserDto> ThrowException(Guid aggregateId);
    }
}