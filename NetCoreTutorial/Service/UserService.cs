using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NetCoreTutorial.Domain;
using NetCoreTutorial.Helpers;
using NetCoreTutorial.Model;
using NetCoreTutorial.Repository;

namespace NetCoreTutorial.Service
{
    public interface IUserService
    {
        Task<AppUser> GetByIdFromCache(int userId);
        Task<LoginResponse> Login(LoginRequest request);
        Task<LoginResponse> SignUp(SignupRequest request);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork<GenericBaseEntityRepository<User>> _unitOfWork;
        private readonly IJwtHelper _jwtHelper;

        // cache
        private static string CacheKeyById(long id) => "User_Id" + id;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

        public UserService(IUnitOfWork<GenericBaseEntityRepository<User>> unitOfWork, IJwtHelper jwtHelper,
            IMemoryCache memoryCache,
            IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
            _memoryCache = memoryCache;

            _memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(appSettings.Value.CacheExpiryMinute),
                Priority = CacheItemPriority.Normal
            };
        }

        public async Task<AppUser> GetByIdFromCache(int id)
        {
            if (_memoryCache.TryGetValue(CacheKeyById(id), out AppUser appUser))
            {
                return appUser;
            }

            var user = await _unitOfWork.Repository.GetFirst(x => x.Id == id);
            if (user != null)
            {
                appUser = new AppUser(user);
                _memoryCache.Set(CacheKeyById(id), appUser, _memoryCacheEntryOptions);
            }

            return appUser;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _unitOfWork.Repository.GetSingle(x =>
                x.Username == request.Username);

            if (user != null)
            {
                if (HashHelper.ValidateHash(request.Password, user.PasswordHash))
                {
                    var token = _jwtHelper.GenerateJwtToken(new AppUser(user));
                    return new LoginResponse(user, token);
                }
            }

            throw new AppException("incorrect username or password");
        }

        public async Task<LoginResponse> SignUp(SignupRequest request)
        {
            var userByUsername = await _unitOfWork.Repository.GetSingle(x => x.Username == request.Username);
            if (userByUsername != null)
            {
                throw new AppException($"{request.Username} already registered");
            }

            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashHelper.HashCreate(request.Password),
                Email = request.Email
            };
            await _unitOfWork.Repository.Add(user);
            await _unitOfWork.SaveChanges();

            var token = _jwtHelper.GenerateJwtToken(new AppUser(user));
            return new LoginResponse(user, token);
        }

        // call when updated user or deleted user 
        private void ClearUserCache(AppUser appUser)
        {
            if (_memoryCache.TryGetValue(CacheKeyById(appUser.Id), out AppUser _))
            {
                _memoryCache.Remove(CacheKeyById(appUser.Id));
            }
        }
    }
}