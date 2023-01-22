﻿using CleanArchMvc.API.Models;
using CleanArchMvc.Domain.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authenticate;
        private readonly IConfiguration _configuration;
        public TokenController(IAuthenticate authenticate, IConfiguration configuration)
        {
            _authenticate = authenticate;
            _configuration = configuration;
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginDto userInfo)
        {
            var result = await _authenticate.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt.");
                return BadRequest(ModelState);
            }
        }

        private UserTokenDto GenerateToken(LoginDto userInfo)
        {
            //declarações do usuário
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuvalor", "oque voce quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //gerar chave privada para assinar o token
            var privateKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            //gerar a assinatura digital
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            //definir o tempo de expiração
            var expiration = DateTime.UtcNow.AddMinutes(10);

            //gerar o token
            JwtSecurityToken token = new JwtSecurityToken(
                //emissor
                issuer: _configuration["Jwt:Issuer"],
                //audiencia
                audience: _configuration["Jwt:Audience"],
                //claims
                claims: claims,
                //data de expiracao
                expires: expiration,
                //assinatura digital
                signingCredentials: credentials
                );

            return new UserTokenDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterDto userInfo)
        {
            var result = await _authenticate.RegisterUser(userInfo.Email, userInfo.Password);

            if (result)
            {
                return Ok($"User {userInfo.Email} was created successfully");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt.");
                return BadRequest(ModelState);
            }
        }
    }
}
