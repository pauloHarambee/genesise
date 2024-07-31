using GenesisExchange.Data;
using GenesisExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace GenesisExchange.Controllers.sub_controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly WindowsIdentity _windowsIdentity;
        protected readonly AppDbContext _appDbContext;
        protected readonly ILogger _logger;
        protected readonly UserManager<AppUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly SignInManager<AppUser> _signInManager;

        public BaseController()
        {
        }
        public BaseController(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager) : this(userManager, roleManager, signInManager)
        {
            _appDbContext = appDbContext;
        }
        public BaseController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
          SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public BaseController(AppDbContext appDbContext, ILogger logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }
        public BaseController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public BaseController(AppDbContext appDbContext, WindowsIdentity windowsIdentity)
        {
            _appDbContext = appDbContext;
            _windowsIdentity = windowsIdentity;
        }

        public BaseController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
    }
}
