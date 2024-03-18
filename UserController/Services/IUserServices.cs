using UserController.Models.EntityModel;
using UserController.Models.RequestModel;
using UserController.Models.ResponseModel;

namespace UserController.Services
{
    public interface IUserServices
    {
        public Task<RegisterResponseModel> CreateUser(RegisterModel model);
        public Task<User> GetById(int id);
        public Task<User> UpdateUser(User user);
        public Task<bool> Deleteuser(int id);
        public Task<List<User>> GetAllUsers();

        public Task<List<User>> GetAllActiveUsers();
        public Task<List<User>> GetAllByStd(int std);

        public Task<List<User>> GetActiveStdentsByStd(int std);

        public Task<int> LoginUser(LoginModel model);

        public Task<bool> Activate(string name, int id);

    }
}
