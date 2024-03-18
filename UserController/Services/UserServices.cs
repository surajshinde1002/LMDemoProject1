using Microsoft.EntityFrameworkCore;
using UserController.data;
using UserController.Models.EntityModel;
using UserController.Models.RequestModel;
using UserController.Models.ResponseModel;

namespace UserController.Services
{
    public class UserServices : IUserServices
    {
        //Dependency injection
        UserDbContext _db;

        //Injecting DBContext Class
        public UserServices(UserDbContext db)
        {
            _db = db;
        }

        //Servise for creating/registering user .
        public async Task<RegisterResponseModel> CreateUser(RegisterModel model)
        {
            //Copying register model data in original user model add in database
            User user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Role = "User",
                Standard = model.Standard,
                Roll = model.Roll,
                DOB = model.DOB,
                Flag = true,
            };


            if (user != null)//No issue 
            {

                _db.Users.Add(user); // Adding in the database
                await _db.SaveChangesAsync(); //Saving the changes which is mandatory or changes won't reflect in the database

            }
            //for the response copying the original user model into response model and return to controller
            return new RegisterResponseModel
            {

                Email = user.Email,
                Username = user.Username,
                Id = user.Id,
                Role = user.Role,
                Standard = user.Standard,
                Roll = user.Roll,
                DOB = user.DOB
            };
        }


        //Service for getting user by Id(Primary key) .
        public async Task<User> GetById(int id)
        {
            User user = await _db.Users.FindAsync(id); //Finding data by Id(Primary key)
            if (user != null)//User found then returns the user
            {
                return user;
            }
            return null; // User not found
        }


        //Service for getting list of users by Standard .
        public async Task<List<User>> GetAllByStd(int std)
        {
            // Retrieve users from the database based on the standard
            List<User> users = await _db.Users.Where(u => u.Standard == std).ToListAsync();

            return users; // Return filtered list directly
        }


        //Service for getting list of all active users by Standard .
        public async Task<List<User>> GetActiveStdentsByStd(int std)
        {
            // Retrieve active users from the database based on the standard
            List<User> users = await _db.Users.Where(u => u.Standard == std && u.Flag).ToListAsync();

            return users; // Return filtered list directly
        }



        //Removing the user by id(Soft Delete) .
        public async Task<bool> Deleteuser(int id)
        {
            User user = await _db.Users.FindAsync(id);

            if (user != null)
            {
                user.Flag = false; // Mark user as inactive
                await _db.SaveChangesAsync(); // Save changes to the database(Mandatory Line or the changes won't reflect in database)
                return true; // Successful update
            }
            return false; // User not found
        }


        //Service for getting list of users .
        public async Task<List<User>> GetAllUsers()
        {
            List<User> allUsers = await _db.Users.ToListAsync();
            return allUsers; // Return the list of users
        }


        //Service for getting list of all active users .
        public async Task<List<User>> GetAllActiveUsers()
        {
            // Retrieve active users directly from the database
            List<User> activeUsers = await _db.Users.Where(u => u.Flag).ToListAsync();

            return activeUsers; // Return the list of active users
        }


        //Service for updating the user .
        public async Task<User> UpdateUser(User demoUser)
        {
            // Retrieve the user from the database
            User _user = await _db.Users.FindAsync(demoUser.Id);

            if (_user == null)
            {
                return null; // User not found, return null
            }

            // Update user properties if the corresponding values are different
            if (demoUser.Username != null && demoUser.Username != _user.Username)
            {
                _user.Username = demoUser.Username;
            }

            if (demoUser.Email != null && demoUser.Email != _user.Email)
            {
                _user.Email = demoUser.Email;
            }

            if (demoUser.Password != null && demoUser.Password != _user.Password)
            {
                _user.Password = demoUser.Password;
            }

            if (demoUser.DOB != _user.DOB)
            {
                _user.DOB = demoUser.DOB;
            }

            if (demoUser.Standard != _user.Standard)
            {
                _user.Standard = demoUser.Standard;
            }

            if (demoUser.Roll != _user.Roll)
            {
                _user.Roll = demoUser.Roll;
            }

            await _db.SaveChangesAsync(); // Save changes to the database
            return _user; // Return the updated user
        }


        //Service for login the user.
        public async Task<int> LoginUser(LoginModel model)
        {
            // Query the database to find the user with the provided email and password
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                if (user.Flag) // Check if the user is active
                {
                    return 1; // User found and is active
                }
                else
                {
                    return 2; // User found but is not active
                }
            }
            else
            {
                return 0; // User not found with the provided email and password
            }


        }

        public async Task<bool> Activate(string name, int id)
        {
            List<User> list = await _db.Users.ToListAsync();
            foreach (var item in list)
            {
                if(item.Username == name && item.Id == id)
                {
                    item.Flag = true;
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
