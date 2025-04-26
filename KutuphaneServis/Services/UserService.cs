﻿using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace KutuphaneServis.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IGenericRepository<User> userRepository,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public IResponse<UserCreateDto> CreateUser(UserCreateDto user)
        {
            if (user == null)
            {
                ResponseGeneric<UserCreateDto>.Error("Kullanıcı bilgileri boş olamaz.");
            }

            // Kullanıcı adı veya e-posta adresi boş olamaz
            if(string.IsNullOrEmpty(user.Username) && string.IsNullOrEmpty(user.Email))
            {
                return ResponseGeneric<UserCreateDto>.Error("Kullanıcı adı veya e-posta adresi boş olamaz.");
            }

            // Kullanıcı adı veya e-posta adresi zaten var mı kontrol et
            var existingUser = _userRepository.GetAll().FirstOrDefault(x => x.Username == user.Username || x.Email == user.Email);
            if (existingUser != null)
            {
                return ResponseGeneric<UserCreateDto>.Error("Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor.");
            }

            //Gelen şifre alanını hashle
            var hashedPassword = HashPasword(user.Password);

            //Geken DTO'yu Entity'e dönüştürüyoruz.
            var newUser = new User
            {
                Name = user.Name,
                Surname = user.Surname,
                Username = user.Username,
                Email = user.Email,
                Password = hashedPassword, // Şifreyi hashlemeden kaydediyoruz
            };
            newUser.RecordDate = DateTime.Now;
             _userRepository.Create(newUser);

            return ResponseGeneric<UserCreateDto>.Success(null, "Kullanıcı kaydı oluşturuldu.");
        }

        public IResponse<string> LoginUser(UserLoginDto user) 
        {
           if((user.Username == null || user.Email == null) && user.Password == null)
            {
                return ResponseGeneric<string>.Error("Kullanıcı adı veya e-posta adresi boş olamaz.");
            }

           var checkUser = _userRepository.GetAll().FirstOrDefault(x => (x.Username == user.Username || x.Email == user.Email) && x.Password == HashPasword(user.Password));


            if (checkUser == null)
            {
                return ResponseGeneric<string>.Error("Kullanıcı adı veya şifre hatalı.");
            }

            var generatedToken = GenerateJwtToken(checkUser);

            return ResponseGeneric<string>.Success(generatedToken, "Giriş başarılı.");

        }

        private string HashPasword(string password)
        {
            //TODO: SecretKey'i config dosyasından al
            string secretKey = "J7~Sp;zgaoUc.5[4v>>#7?0Gkbqb|K";


            using (var sha256 = SHA256.Create()) 
            { 
            var combinedPassword = password + secretKey;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                var hashedPassword = Convert.ToBase64String(bytes);
                return hashedPassword;
            }

        }

        private string GenerateJwtToken(User user) 
        { 
        var claims = new List<Claim>
        {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                               issuer: _configuration["Jwt:Issuer"],
                                              audience: _configuration["Jwt:Audience"],
                                                             claims: claims,
                                                                            expires: DateTime.Now.AddMinutes(1),
                                                                                           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
