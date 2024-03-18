using AdminController.Data;
using AdminController.Models.EntityModels;
using AdminController.Models.RequestModels;
using AdminController.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AdminController.Services
{
    public class AdminServices : IAdminServices
    {
        
        AdminDbContext _db;
        public AdminServices(AdminDbContext db) 
        { 
            _db = db;
        }


        public async Task<RegisterResponseModel> CreateAdmin(RegisterModel model)
        {
            
            Admin admin = new Admin
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Role = "Admin",
            };
           
            if (admin != null) 
            { 
                _db.Admins.Add(admin);
                await _db.SaveChangesAsync();
                
            }
            return new RegisterResponseModel
            {
                
                Email = admin.Email,
                Username = admin.Username,
                Id = admin.Id,
                Role = admin.Role,
            };

        }

        public async Task<Admin> GetAdminById(int id)
        {
            Admin admin = await _db.Admins.FindAsync(id);
           if(admin != null)
            {
                return admin;
            }
            return null;
                
           
        }

        public async Task<bool> DeleteAdmin(int id)
        {
            Admin admin = await _db.Admins.FindAsync(id);
                
            if(admin != null)
            {
                _db.Admins.Remove(admin);
                _db.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<List<Admin>> GetAllAdmins()
        {

            List<Admin> allAdmins = await _db.Admins.ToListAsync();
            if(allAdmins.Count > 0) { return allAdmins; }
            return null;
        }

        public async Task<Admin> UpdateAdmin(Admin demoAdmin)
        {
            Admin _admin = await _db.Admins.FindAsync(demoAdmin.Id);
            if(_admin == null) 
            {
                return null;
            }

            if(demoAdmin != null)
            {

                if(demoAdmin.Username != _admin.Username)
                {
                    _admin.Username = demoAdmin.Username;
                }
                if(demoAdmin.Email != _admin.Email)
                {
                    _admin.Email = demoAdmin.Email;
                }
                if (demoAdmin.Password != _admin.Password)
                {
                    _admin.Password = demoAdmin.Password;
                }
               
            }
           await _db.SaveChangesAsync();
            return _admin;
        }




        public async Task<Admin> LoginAdmin(LoginModel model)
        {
            var _admin = await _db.Admins.ToListAsync();
            var user = _db.Admins.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if(user != null)
            {
                return user;
            }
            return null;
            //foreach (var item in _admin)
            //{
            //    if(item.Email == model.Email )
            //    {
            //        if(item.Password != model.Password)
            //        {
            //            return  2;
            //        }
                    
            //    }
            //    else if(item.Password == model.Password)
            //    {
            //        if(item.Email != model.Email)
            //        {
            //            return 1;
            //        }
            //    }
                                
            //}
            
        }


        
    }
}
