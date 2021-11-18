using Microsoft.IdentityModel.Tokens;
using Ngnet.ApiModels.AuthModels;
using Ngnet.Database;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ngnet.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly NgnetDbContext database;

        public AuthService(NgnetDbContext database)
        {
            this.database = database;
        }

        public string CreateJwtToken(string userId, string username, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }

        public async Task<int> Update<T>(T model)
        {
            User mappedModel = MappingFactory.Mapper.Map<User>(model);

            User user = this.database.Users.FirstOrDefault(x => x.Id == mappedModel.Id);
            if (user == null)
            {
                return 0;
            }

            user = this.ModifyEntity(mappedModel, user);

            return await this.database.SaveChangesAsync();
        }

        public async Task<int> AddExperience(UserExperience exp)
        {
            User user = this.database.Users.FirstOrDefault(x => x.Id == exp.UserId);
            if (user == null)
            {
                return 0;
            }

            user.Experiences.Add(exp);
            return await this.database.SaveChangesAsync();
        }

        public ICollection<UserExperienceModel> GetExperiences(string UserId)
        {
            return this.database.UserExperiences.Where(x => x.UserId == UserId).To<UserExperienceModel>().ToHashSet();
        }

        private User ModifyEntity(User mappedModel, User user)
        {
            user.FirstName = mappedModel.FirstName == null ? user.FirstName : mappedModel.FirstName;
            user.LastName = mappedModel.LastName == null ? user.LastName : mappedModel.LastName;
            user.Gender = mappedModel.Gender == null ? user.Gender : mappedModel.Gender;
            user.Age = mappedModel.Age == null ? user.Age : mappedModel.Age;

            user.ModifiedOn = DateTime.UtcNow;
            user.IsDeleted = mappedModel.IsDeleted == true ? mappedModel.IsDeleted : user.IsDeleted;
            user.DeletedOn = mappedModel.IsDeleted == true ? DateTime.UtcNow : user.DeletedOn;

            return user;
        }
    }
}
