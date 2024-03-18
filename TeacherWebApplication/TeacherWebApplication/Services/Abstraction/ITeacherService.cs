using TeacherWebApplication.Models.EntityModels;
using TeacherWebApplication.Models.RequestModels;
using TeacherWebApplication.Models.ResponseModels;

namespace TeacherWebApplication.Services
{
    public interface ITeacherService
    {
        public Task<TeacherResponseModel> CreateTeacher(TeacherRegisterModel trqm);
        public Task<List<TeacherResponseModel>> GetAllTeacher();
        public Task<TeacherResponseModel> GetTeacherById(int id);
        public Task<Teacher> UpdateTeacher(Teacher trqm);
        public Task<bool> DeleteTeacher(int id);
        public Task<int> LoginTeacher(TeacherLoginModel tlm);

    }
}
