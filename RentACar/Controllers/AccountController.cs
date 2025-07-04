﻿using Mailing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.DataContext.Entities;
using RentACar.Models;

namespace RentACar.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMailService _mailService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mailService = mailService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = new AppUser
        {
            UserName = registerViewModel.Username,
            Email = registerViewModel.Email,
            FullName = registerViewModel.FullName,
        };

        var result = await _userManager.CreateAsync(user, registerViewModel.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var emailConfirmLink = Url.Action("ConfirmEmail", "Account", new { emailConfirmToken, user.UserName }, Request.Scheme, Request.Host.ToString());

        try
        {
            _mailService.SendMail(new Mail
            {
                ToEmail = user.Email,
                Subject = "Email confirmation",
                TextBody = emailConfirmLink
            });
        }
        catch (Exception ex)
        {
            // Log error (optional): _logger.LogError(ex, "Email send failed");
            TempData["EmailError"] = "Email could not be sent. Please try again later.";
            // Optionally show message or continue without breaking
        }

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Username or password is incorrect");
            return View();
        }

        var existUser = await _userManager.FindByNameAsync(loginViewModel.UserName);

        if (existUser == null)
        {
            ModelState.AddModelError("", "Username or password is incorrect");
            return View();
        }
               
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(existUser);
        if (!isEmailConfirmed)
        {
            ModelState.AddModelError("", "Email not confirmed");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(
            existUser,
            loginViewModel.Password,
            loginViewModel.RememberMe,
            lockoutOnFailure: true
        );

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", $"You are banned {(existUser.LockoutEnd.Value - DateTimeOffset.UtcNow).Minutes} minutes left.");
            return View();
        }

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Username or password is incorrect");
            return View();
        }

        if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
        {
            return Redirect(loginViewModel.ReturnUrl);
        }

        return RedirectToAction("Index", "Profile");
    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _userManager.ChangePasswordAsync(user, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        return RedirectToAction(nameof(Login));
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest();

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            ModelState.AddModelError("", "Email doesnt found");
            return View();
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetLink = Url.Action("ResetPassword", "Account", new { resetToken, email }, Request.Scheme, Request.Host.ToString());

      
        _mailService.SendMail(new Mail { ToEmail = email, Subject = "Reset pas", TextBody = resetLink });

        return View(nameof(EmailSimulyasiya), resetLink);
    }

    public IActionResult EmailSimulyasiya()
    {
        return View();
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(resetPasswordViewModel);
        }

        var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);

        if (user == null)
            return BadRequest();

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.ResetToken, resetPasswordViewModel.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(resetPasswordViewModel);
        }

        return RedirectToAction(nameof(Login));
    }


    public async Task<IActionResult> ConfirmEmail(string emailConfirmToken, string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
            return BadRequest();

        var result = await _userManager.ConfirmEmailAsync(user, emailConfirmToken);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return RedirectToAction(nameof(Login));
    }
}
