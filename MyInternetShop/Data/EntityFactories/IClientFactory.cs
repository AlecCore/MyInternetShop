using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyInternetShop.Data.Entities;
using MyInternetShop.Models;

namespace MyInternetShop.Data.EntityFactories
{
    public interface IClientFactory
    {

        public  Client ReturnClient(object clientInformation);
        public  Task<Client> ReturnClientAsync(int ClientId, string FullName = null);

        public ClientModel ReturnClientModel(int Id, string FullName = null);
    }
}
