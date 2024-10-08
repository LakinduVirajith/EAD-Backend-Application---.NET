﻿using EAD_Backend_Application__.NET.DTOs;
using EAD_Backend_Application__.NET.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EAD_Backend_Application__.NET.Controllers
{

    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Registers a new user in the system.</summary>
        // POST: api/v1/auth/sign-up
        [HttpPost("sign-up")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _authService.RegisterUserAsync(dto);

            if (result.Succeeded)
            {
                return Ok("Sign up successfully!");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>Authenticates a user and returns JWT and refresh tokens.</summary>
        // POST: api/v1/auth/sign-in
        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var (token, refreshToken) = await _authService.AuthenticateUserAsync(dto);

            if (token != null)
            {
                return Ok(new { Token = token, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }

        /// <summary>Generates a new JWT token and refresh token using the provided refresh token.</summary>
        // GET: api/v1/auth/refresh-token
        [HttpGet("refresh-token/{refreshToken}")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            // Validate the refresh token and generate new tokens
            var (newToken, newRefreshToken) = await _authService.RefreshTokenAsync(refreshToken);
            if (newToken != null)
            {
                return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
            }

            return Unauthorized();
        }
    }
}
