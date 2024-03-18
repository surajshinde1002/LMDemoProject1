using Microsoft.EntityFrameworkCore;
using TeacherWebApplication.Data;
using TeacherWebApplication.Models.EntityModels;
using TeacherWebApplication.Models.RequestModels;
using TeacherWebApplication.Models.ResponseModels;

namespace TeacherWebApplication.Services
{
    public class TeacherService : ITeacherService
    {
        //Dependency injection
        TeacherDbContext _db;

        //Injecting DBContext Class
        public TeacherService(TeacherDbContext db)
        {
            _db = db;
        }

        //Servise for creating/registering user .
        public async Task<TeacherResponseModel> CreateTeacher(TeacherRegisterModel trqm)
        {
            //Copying register model data in original teacher register model add in database
            Teacher teacher = new Teacher()
            {

                Email = trqm.Email,
                Name = trqm.Name,
                Password = trqm.Password,
                PhoneNumber = trqm.PhoneNumber,
                Standard = trqm.Standard,

            };
            if (teacher != null)//No issue
            {
                await _db.TeacherTable.AddAsync(teacher);// Adding in the database .
                await _db.SaveChangesAsync(); //Saving the changes which is mandatory or changes won't reflect in the database .
            }

            //for the response copying the original user model into response model and return to controller
            return new TeacherResponseModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                Standard = teacher.Standard,
                Role = teacher.Role,
                PhoneNumber = teacher.PhoneNumber,
            };

        }


        //Service for getting user by Id(Primary key) .
        public async Task<TeacherResponseModel> GetTeacherById(int id)
        {
            Teacher teacher = await _db.TeacherTable.FindAsync(id);
            if (teacher != null)//User found then returns the user
            {
                return new TeacherResponseModel
                {
                    Email = teacher.Email,
                    Name = teacher.Name,
                    Id = teacher.Id,
                    Standard = teacher.Standard,
                    Role = teacher.Role,
                    PhoneNumber = teacher.PhoneNumber
                };
            }
            return null;// User not found
        }


        //Removing the user by id(Hard Delete) .
        public async Task<bool> DeleteTeacher(int id)
        {
            Teacher teacher = await _db.TeacherTable.FindAsync(id);
            if (teacher != null)
            {
                _db.TeacherTable.Remove(teacher); // Removing the teacher data from database
                await _db.SaveChangesAsync();// Save changes to the database(Mandatory Line or the changes won't reflect in database)
                return true; // Successful update
            }
            return false; // User not found
        }


        //Service for getting list of users .
        public async Task<List<TeacherResponseModel>> GetAllTeacher()
        {
            List<Teacher> list = await _db.TeacherTable.ToListAsync();

            List<TeacherResponseModel> listModel = new List<TeacherResponseModel>();
            foreach (Teacher teacher in list)
            {
                listModel.Add(new TeacherResponseModel
                {
                    Id = teacher.Id,

                    Email = teacher.Email,
                    Name = teacher.Name,
                    Standard = teacher.Standard,
                    Role = teacher.Role,
                    PhoneNumber = teacher.PhoneNumber,

                });
            }
            if (list.Count > 0)
            {
                return listModel; // Return the list of users
            }
            return null;
        }



        //Service for login the user.
        public async Task<int> LoginTeacher(TeacherLoginModel tlm)
        {
            // Query the database to find the user with the provided email and password
            var _teacher = await _db.TeacherTable.ToListAsync();
            //var user = _db.TeacherTable.FirstOrDefault(u => u.Email == tlm.Email && u.Password == tlm.Password);
            foreach (var teacher in _teacher)
            {
                if (tlm.Email == teacher.Email && tlm.Password == teacher.Password)
                {
                    return 1;
                }
                else if (tlm.Email == teacher.Email)
                {
                    if (tlm.Password != teacher.Password)
                    {
                        return 2;
                    }
                }

            }
            return 0;

        }


        //Service for updating the user .
        public async Task<Teacher> UpdateTeacher(Teacher trqm)
        {

            // Retrieve the teacher from the database
            Teacher _teacher = await _db.TeacherTable.FindAsync(trqm.Id);

            if (_teacher == null)
            {
                return null; // Teacher not found, return null
            }

            // Update teacher properties if the corresponding values are different
            if (trqm.Name != null && trqm.Name != _teacher.Name)
            {
                _teacher.Name = trqm.Name;
            }

            if (trqm.Email != null && trqm.Email != _teacher.Email)
            {
                _teacher.Email = trqm.Email;
            }

            if (trqm.Password != null && trqm.Password != _teacher.Password)
            {
                _teacher.Password = trqm.Password;
            }

            if (trqm.PhoneNumber != _teacher.PhoneNumber)
            {
                _teacher.PhoneNumber = trqm.PhoneNumber;
            }

            await _db.SaveChangesAsync(); // Save changes to the database
            return _teacher; // Return the updated user

        }


    }
}
