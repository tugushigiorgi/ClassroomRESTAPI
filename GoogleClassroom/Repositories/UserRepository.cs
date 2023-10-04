using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Database.Models;
using GoogleClassroom.Identity;
using GoogleClassroom.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace GoogleClassroom.Repositories
{
    public class UserRepository
    {


        private ClassroomDbContext dbContext;

        private UserManager<user> usrmanager;
        private IJwtService jwtService;
        public UserRepository(ClassroomDbContext db, UserManager<user> manager, IJwtService Service) { 
            dbContext = db;
            usrmanager = manager;
            jwtService = Service;

        }


        public async Task<ControllerResponse> CreateAccount(RegisterUserDto dto)
        {


            var result = await usrmanager.FindByEmailAsync(dto.Email);

            if (result != null)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = "User already registered with That Email"

                };

            }


            var newuser = new user
            {
                UserName = dto.Name,
                Email = dto.Email,
                Surname = dto.Surname,
                ProfilePhoto = dto.ProfilePhoto,

            };

            var t = await usrmanager.CreateAsync(newuser, dto.Password);
            if (t.Succeeded)
            {
                return new ControllerResponse
                {
                    code = 1,
                    message = "Succesfully created new account"
                };
            }


            else
            {
                var errors = t.Errors.Select(error => error.Description).ToList();
                var errorMessage = string.Join(", ", errors);

                return new ControllerResponse
                {
                    code = 0,
                    message =    errorMessage
                };





            }



        }



        public async Task<AuthenticationResponseDto> Login(LoginDto dto) { 
        
        var currentUser=await usrmanager.FindByEmailAsync(dto.Email);

            if (currentUser != null)
            {
                var isPasswordCorrect = await usrmanager.CheckPasswordAsync(currentUser , dto.Password);

                if(isPasswordCorrect)
                {
                  return    jwtService.CreateJwtToken(currentUser);

                }

            }
            return null;




        }

    }
}
