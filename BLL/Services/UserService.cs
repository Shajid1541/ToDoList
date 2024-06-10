using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL;

namespace BLL.Services
{
    public class UserService
    {
        #region Fields
        private readonly DataAccessFactory dataAccessFactory;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public UserService(DataAccessFactory dataAccessFactory, IMapper mapper)
        {
            this.dataAccessFactory = dataAccessFactory;
            this.mapper = mapper;
        }
        #endregion

        #region Methods

        #region CreateUserAsync
        public async Task<UserDTO> CreateUserAsync(UserDTO UserDTO)
        {
            using var UserRepository = dataAccessFactory.CreateUserData();
            var User = mapper.Map<User>(UserDTO);
            await UserRepository.CreateAsync(User);
            return mapper.Map<UserDTO>(User);
        }
        #endregion

        #region GetUserByIdAsync
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            using var UserRepository = dataAccessFactory.CreateUserData();

            var User = await UserRepository.ReadAsync(id);
            return mapper.Map<UserDTO>(User);
        }
        #endregion

        #region GetAllUsersAsync
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            using var UserRepository = dataAccessFactory.CreateUserData();

            var Users = await UserRepository.ReadAllAsync();
            return mapper.Map<List<UserDTO>>(Users);
        }
        #endregion

        #region UpdateUserAsync
        public async Task<UserDTO> UpdateUserAsync(UserDTO UserDto)
        {
            using var UserRepository = dataAccessFactory.CreateUserData();

            var User = await UserRepository.ReadAsync(UserDto.Id);
            if (User == null)
            {
                return null;
            }

            mapper.Map(UserDto, User);
            await UserRepository.UpdateAsync(User);
            return mapper.Map<UserDTO>(User);
        }
        #endregion

        #region DeleteUserAsync
        public async Task<bool> DeleteUserAsync(int id)
        {
            using var UserRepository = dataAccessFactory.CreateUserData();

            return await UserRepository.DeleteAsync(id);
        }
        #endregion

        #endregion
    }
}
