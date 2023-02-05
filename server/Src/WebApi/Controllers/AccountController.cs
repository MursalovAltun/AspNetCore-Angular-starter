using System.Threading.Tasks;
using Application.Auth;
using Application.Auth.Account.Commands.CreateAccount;
using Application.Auth.Account.DTO;
using Application.Auth.DTO;
using Application.Auth.Identity;
using Application.Providers.CurrentUserProvider;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IAuthenticationResultProvider _authenticationResultProvider;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;

        public AccountController(ISender sender,
            IAuthenticationResultProvider authenticationResultProvider,
            ICurrentUserProvider currentUserProvider,
            IMapper mapper,
            IUserManager userManager)
        {
            _sender = sender;
            _authenticationResultProvider = authenticationResultProvider;
            _currentUserProvider = currentUserProvider;
            _mapper = mapper;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthenticateResult> Create([FromBody] CreateAccountCommand request)
        {
            var user = await _sender.Send(request);

            return _authenticationResultProvider.Get(user);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> EmailIsTaken(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet]
        public async Task<UserDto> Me()
        {
            var user = await _currentUserProvider.GetUserAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}