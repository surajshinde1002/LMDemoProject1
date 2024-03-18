using AdminController.Models.EntityModels;
using AdminController.Models.RequestModels;
using AdminController.Models.ResponseModels;

namespace AdminController.Services
{
    public interface IAdminServices
    {
        public Task<RegisterResponseModel> CreateAdmin(RegisterModel model);
        public Task<Admin> GetAdminById(int id);
        public Task<Admin> UpdateAdmin(Admin admin);
        public Task<bool> DeleteAdmin(int id);
        public Task<List<Admin>> GetAllAdmins();

        public Task<Admin> LoginAdmin(LoginModel model);






    }
}
