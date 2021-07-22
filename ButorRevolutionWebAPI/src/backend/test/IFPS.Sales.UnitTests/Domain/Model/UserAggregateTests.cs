using FluentAssertions;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class UserAggregateTests
    {
        [Fact]
        public void Cant_add_null_version()
        {
            var user = new User("hakapeszi@maki.hu");

            Action action = () => user.AddVersion(null);

            action.Should().ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void Create_user_no_address_success()
        {
            var user = new User("hakapeszi@maki.hu");
            var data = new UserData("Teszt Elek", "06308885556", DateTime.Now);

            user.AddVersion(data);

            Assert.Equal(user.CurrentVersion, data);
            Assert.True(user.IsActive);
            Assert.False(user.IsDeleted);
            Assert.Null(user.CurrentVersion.ContactAddress);
            Assert.Null(user.CurrentVersion.ValidTo);
        }

        [Fact]
        public void Create_user_with_address_success()
        {
            var user = new User("hakapeszi@maki.hu");
            var address = new Address(5000, "Szolnok", "Kossuth tér 1.", 1);
            var data = new UserData("Para Zita", "06308885556", DateTime.Now, address);

            user.AddVersion(data);

            Assert.Equal(user.CurrentVersion, data);
            Assert.NotNull(user.CurrentVersion.ContactAddress);
            Assert.Equal(user.CurrentVersion.ContactAddress, address);
            Assert.Null(user.CurrentVersion.ValidTo);
        }

        [Fact]
        public void Close_user_data_prev_version_success()
        {
            var user = new User("hakapeszi@maki.hu");
            var address = new Address(5000, "Szolnok", "Kossuth tér 1.", 1);
            var data = new UserData("Füty Imre", "06308885556", DateTime.Now, address);

            var newAddress = new Address(1117, "BudaPest", "Hősök tere 1.", 1);
            var newData = new UserData("Beka Kálmán", "06308885556", DateTime.Now, newAddress);

            user.AddVersion(data);
            Assert.Equal(user.CurrentVersion, data);
            Assert.NotNull(user.CurrentVersion.ContactAddress);
            Assert.Equal(user.CurrentVersion.ContactAddress, address);
            Assert.Null(data.ValidTo);

            user.AddVersion(newData);
            Assert.Equal(newData, user.CurrentVersion);
            Assert.NotNull(user.CurrentVersion.ContactAddress);
            Assert.Equal(newAddress, user.CurrentVersion.ContactAddress);
            Assert.NotNull(data.ValidTo);
            Assert.Null(newData.ValidTo);
        }

        [Fact]
        public void Check_if_lists_inited()
        {
            var user = new User("hakapeszi@maki.hu");

            Assert.NotNull(user.Roles);
            Assert.Empty(user.Roles);
            Assert.NotNull(user.Claims);
            Assert.Empty(user.Claims);
            Assert.True(user.IsActive);
        }

        [Fact]
        public void Add_new_roles_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };
            var roleIds = new List<int>() { 1, 2, 3 };

            user.AddRolesById(roleIds);

            Assert.Equal(roleIds.Count, user.Roles.Count());
            Assert.Contains(user.Roles, ur => ur.RoleId == 1);
            Assert.Contains(user.Roles, ur => ur.RoleId == 2);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3);
            Assert.All(user.Roles, ur => Assert.Equal(user.Id, ur.UserId));
        }

        [Fact]
        public void Soft_delete_roles_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };
            var roleIds = new List<int>() { 1, 2, 3 };
            user.AddRolesById(roleIds);
            Assert.Contains(user.Roles, ur => ur.RoleId == 1);
            Assert.Contains(user.Roles, ur => ur.RoleId == 2);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3);

            user.RemoveRoles(new List<int>() { 2 });
            Assert.Contains(user.Roles, ur => ur.RoleId == 1 && ur.IsDeleted == false);
            Assert.DoesNotContain(user.Roles, ur => ur.RoleId == 2 && ur.IsDeleted == false);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3 && ur.IsDeleted == false);

            user.RemoveRoles(new List<int>() { 1 });
            Assert.DoesNotContain(user.Roles, ur => ur.RoleId == 1 && ur.IsDeleted == false);
            Assert.DoesNotContain(user.Roles, ur => ur.RoleId == 2 && ur.IsDeleted == false);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3 && ur.IsDeleted == false);

            user.RemoveRoles(new List<int>() { 3 });
            Assert.DoesNotContain(user.Roles, ur => ur.RoleId == 1 && ur.IsDeleted == false);
            Assert.DoesNotContain(user.Roles, ur => ur.RoleId == 2 && ur.IsDeleted == false);
            Assert.DoesNotContain(user.Roles, ur => ur.RoleId == 3 && ur.IsDeleted == false);
        }

        [Fact]
        public void Add_roles_more_than_once_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };

            user.AddRolesById(new List<int>() { 1, 2 });
            Assert.Equal(2, user.Roles.Count());
            Assert.Contains(user.Roles, ur => ur.RoleId == 1);
            Assert.Contains(user.Roles, ur => ur.RoleId == 2);

            user.AddRolesById(new List<int>() { 2, 3 });
            Assert.Equal(3, user.Roles.Count());
            Assert.Contains(user.Roles, ur => ur.RoleId == 1);
            Assert.Contains(user.Roles, ur => ur.RoleId == 2);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3);
        }

        [Fact]
        public void Add_duplicated_roles_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };

            user.AddRolesById(new List<int>() { 1, 2, 2 });
            Assert.Equal(2, user.Roles.Count());
            Assert.Contains(user.Roles, ur => ur.RoleId == 1);
            Assert.Contains(user.Roles, ur => ur.RoleId == 2);
        }

        [Fact]
        public void Add_delete_add_role_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };
            var roleIds = new List<int>() { 1, 2, 3 };
            user.AddRolesById(roleIds);
            Assert.Contains(user.Roles, ur => ur.RoleId == 1);
            Assert.Contains(user.Roles, ur => ur.RoleId == 2);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3);

            user.RemoveRoles(new List<int>() { 2 });
            Assert.Contains(user.Roles, ur => ur.RoleId == 1 && ur.IsDeleted == false);
            Assert.DoesNotContain(user.Roles, ur => ur.RoleId == 2 && ur.IsDeleted == false);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3 && ur.IsDeleted == false);

            user.AddRolesById(new List<int>() { 2 });
            Assert.Contains(user.Roles, ur => ur.RoleId == 1 && ur.IsDeleted == false);
            Assert.Contains(user.Roles, ur => ur.RoleId == 2 && ur.IsDeleted == false);
            Assert.Contains(user.Roles, ur => ur.RoleId == 3 && ur.IsDeleted == false);
        }

        [Fact]
        public void Add_new_claims_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };
            var claimIds = new List<int>() { 1, 2, 3 };

            user.AddClaims(claimIds);

            Assert.Equal(claimIds.Count, user.Claims.Count());
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 2);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3);
            Assert.All(user.Claims, uc => Assert.Equal(user.Id, uc.UserId));
        }

        [Fact]
        public void Soft_delete_claims_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };
            var claimsId = new List<int>() { 1, 2, 3 };
            user.AddClaims(claimsId);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 2);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3);

            user.RemoveClaims(new List<int>() { 2 });
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1 && uc.IsDeleted == false);
            Assert.DoesNotContain(user.Claims, uc => uc.ClaimId == 2 && uc.IsDeleted == false);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3 && uc.IsDeleted == false);

            user.RemoveClaims(new List<int>() { 1 });
            Assert.DoesNotContain(user.Claims, uc => uc.ClaimId == 1 && uc.IsDeleted == false);
            Assert.DoesNotContain(user.Claims, uc => uc.ClaimId == 2 && uc.IsDeleted == false);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3 && uc.IsDeleted == false);

            user.RemoveClaims(new List<int>() { 3 });
            Assert.DoesNotContain(user.Claims, uc => uc.ClaimId == 1 && uc.IsDeleted == false);
            Assert.DoesNotContain(user.Claims, uc => uc.ClaimId == 2 && uc.IsDeleted == false);
            Assert.DoesNotContain(user.Claims, uc => uc.ClaimId == 3 && uc.IsDeleted == false);
        }

        [Fact]
        public void Add_claims_more_than_once_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };

            user.AddClaims(new List<int>() { 1, 2 });
            Assert.Equal(2, user.Claims.Count());
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 2);

            user.AddClaims(new List<int>() { 2, 3 });
            Assert.Equal(3, user.Claims.Count());
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 2);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3);
        }

        [Fact]
        public void Add_duplicated_claims_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };

            user.AddClaims(new List<int>() { 1, 2, 2 });
            Assert.Equal(2, user.Claims.Count());
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 2);
        }

        [Fact]
        public void Add_delete_add_claim_success()
        {
            var user = new User("hakapeszi@maki.hu") { Id = 1 };
            var claimIds = new List<int>() { 1, 2, 3 };
            user.AddClaims(claimIds);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 2);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3);

            user.RemoveClaims(new List<int>() { 2 });
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1 && uc.IsDeleted == false);
            Assert.DoesNotContain(user.Claims, uc => uc.ClaimId == 2 && uc.IsDeleted == false);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3 && uc.IsDeleted == false);

            user.AddClaims(new List<int>() { 2 });
            Assert.Contains(user.Claims, uc => uc.ClaimId == 1 && uc.IsDeleted == false);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 2 && uc.IsDeleted == false);
            Assert.Contains(user.Claims, uc => uc.ClaimId == 3 && uc.IsDeleted == false);
        }
    }
}