
using AutoFixture;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Options;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Services;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class JwtServiceTest
    {
        private const string JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMjAiLCJzdWIiOiIyUmwvNVZYOGNsdE15YnIwT2tpMVhUZGVZSUZMKzdzYjY3VjdUSVp1VHo0PSIsInJvbGUiOiIxIiwiZXhwIjoxNjUzNTg3MzM0LCJpc3MiOiJhdXRoQ29udHJvbGxlciIsImF1ZCI6IkNvbnRyb2xsZXJzIn0.O0JQ7KMd6re__XnRaGZeOWt-jyjVSBh_W5m26nkcylk";
        private const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        private const string SubClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Create<AuthOptions>());
            var sut = new JwtService(options, context, logger);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Generate_Claims()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Create<AuthOptions>());
            var sut = new JwtService(options, context, logger);
            var user = FixtureHelper.Fixture.Create<User>();
            //Act
            var actual = sut.GenerateClaims(user);
            //Assert
            Assert.Equal(3, actual.Count);
            Assert.Equal(actual[0].Value, user.Id.ToString());
            Assert.Equal(actual[1].Value, user.PassHash.PasswordHash);
            Assert.Equal(actual[2].Value, user.Role.RoleId.ToString());

        }
        [Fact]
        public void ItShould_Create_Jwt()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Create<AuthOptions>());
            var sut = new JwtService(options, context, logger);
            var claims = new List<Claim>()
                {
                    new Claim("id","20"),
                    new Claim("password","test"),
                    new Claim("role","1")
                };
            //Act
            var actual = sut.GenerateJWT(claims);
            //Assert
            Assert.NotNull(actual);
        }
        [Fact]
        public void ItShould_Get_Claims_From_Token()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Build<AuthOptions>()
                .With(x => x.Secret, "!@#123$%^456&*(789").Create());
            var sut = new JwtService(options, context, logger);
            var token = JWT;
            //Act
            var actual = sut.GetClaims(token);
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Equal("20", actual.Content.Identity.Name);
            Assert.Equal("1", actual.Content.FindFirst(x => x.Type ==
                RoleClaimType).Value);
            Assert.Equal("2Rl/5VX8cltMybr0Oki1XTdeYIFL+7sb67V7TIZuTz4="
                , actual.Content.FindFirst(x => x.Type ==
                SubClaimType).Value);
        }
        [Fact]
        public async Task ItShould_Create_RT()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            JwtServiceDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Create<AuthOptions>());
            var sut = new JwtService(options, context, logger);
            var user = context.Users.FirstOrDefault();
            //Act
            var actual = await sut.GenerateRTAsync(user.Id);
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Equal(user.RefreshToken, actual.Content);
        }
        [Fact]
        public async Task ItShould_Return_NotFound_While_Create_RT()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Create<AuthOptions>());
            var sut = new JwtService(options, context, logger);
            //Act
            var actual = await sut.GenerateRTAsync(1);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.NotNull(actual.Message);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
        }
        [Fact]
        public async Task ItShould_Refresh_Tokens()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            JwtServiceDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Build<AuthOptions>()
                .With(x => x.Secret, "!@#123$%^456&*(789").Create());
            var sut = new JwtService(options, context, logger);
            var tokenPair = new RefreshTokenDto()
            {
                AccessToken = JWT,
                RefreshToken = "edhfi$hsi@nd8i"
            };
            var user = context.Users.FirstOrDefault();
            //Act
            var actual = await sut.RefreshAsync(tokenPair);
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Equal(user.RefreshToken,actual.Content.RefreshToken);
            Assert.NotEqual(tokenPair.AccessToken, actual.Content.AccessToken);
            Assert.NotEqual(tokenPair.RefreshToken, actual.Content.RefreshToken);
        }
        [Fact]
        public async Task ItShould_Return_BadRequest_While_Refresh_Tokens()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            JwtServiceDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<JwtService>();
            var options = Options.Create(
                FixtureHelper.Fixture.Build<AuthOptions>()
                .With(x => x.Secret, "!@#123$%^456&*(789").Create());
            var sut = new JwtService(options, context, logger);
            var tokenPair = new RefreshTokenDto()
            {
                AccessToken = JWT,
                RefreshToken = "fake-token"
            };
            //Act
            var actual = await sut.RefreshAsync(tokenPair);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.NotNull(actual.Message);
            Assert.Equal(ResponseStatus.BadRequest, actual.Status);
        }
    }
}
