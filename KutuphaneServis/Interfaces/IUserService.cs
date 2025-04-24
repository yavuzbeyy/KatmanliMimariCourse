using KutuphaneCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Interfaces
{
    public interface IUserService
    {
        public IResponse<IEnumerable<User>> ListAll();
        public IResponse<User> GetById(int id);
        public Task<IResponse<User>> Create(User user);
        public Task<IResponse<User>> Update(User user);
        public IResponse<User> Delete(int id);
        public IResponse<IEnumerable<User>> GetByName(string name);
    }
}
