using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Exception;
using Microsoft.EntityFrameworkCore;
using static GroupAssignment3.Controllers.UserApiController;

namespace GroupAssignment3.Services
{
    public class UserService
    {
        private DatabaseContext context;

        public UserService(DatabaseContext context)
        {
            this.context = context;
        }

        public UserEntity AuthenticateUser(UserDto loginData)
        {
            UserEntity authUser = context.User.Where(u => u.username == loginData.username && u.password == loginData.password).FirstOrDefault();
            if (authUser == null)
            {
                throw new HttpException(404, "Invalid username or password");
            }
            return authUser;
        }

        public List<UserEntity> GetAllUsers()
        {
            return context.User.ToList();
        }

        public UserEntity GetUserById(string userId)
        {
            UserEntity filteredUser = context.User.Where(u => u.userId == userId).FirstOrDefault();
            if (filteredUser == null)
            {
                throw new HttpException(404, "User ID '" + userId + "' does not exists");
            }
            return filteredUser;
        }

        public UserEntity CreateUser(UserCreateDto userCreate) 
        {
            if (this.IsUsernameExists(userCreate.username))
            {
                throw new HttpException(500, "Username '" + userCreate.username + "' already exists");
            }

            var newUser = new UserEntity()
            {
                userId = Guid.NewGuid().ToString(),
                email = userCreate.email,
                password = userCreate.password,
                username = userCreate.username,
                shippingAddress = userCreate.shippingAddress,
                isActive = true
            };

            context.Add(newUser);
            context.SaveChanges();
            return newUser;
        }

        public UserEntity UpdateUser(UserUpdateDto userUpdate)
        {
            var userToUpdate = context.User.Where(u => u.userId == userUpdate.id).FirstOrDefault();
            var userByUsername = this.GetUsernameByUsername(userUpdate.username);
            
            if (userToUpdate == null)
            {
                throw new HttpException(404, "User with ID '" + userUpdate.id + "' does not exists");
            }

            if (userByUsername != null && userByUsername.userId.CompareTo(userToUpdate.userId) != 0)
            {
                throw new HttpException(500, "Username '" + userUpdate.username + "' not valid. It is already used.");
            }

            userToUpdate.username = userUpdate.username;
            userToUpdate.email = userUpdate.email;
            userToUpdate.password = userUpdate.password;
            userToUpdate.shippingAddress = userUpdate.shippingAddress;
            context.SaveChanges();

            return userToUpdate;
        }

        public UserEntity DeleteUser(String userId)
        {
            UserEntity userToDelete = this.GetUserById(userId);
            context.User.Remove(userToDelete);
            context.SaveChanges();
            return userToDelete;
        }

        private bool IsUsernameExists(string username)
        {
            return context.User.Where(u => u.username == username).Count() > 0;
        }

        private UserEntity GetUsernameByUsername(string username)
        {
            return context.User.Where(u => u.username == username).FirstOrDefault();
        }

    }
}
