using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyInternetShop.Data.Entities;
using MyInternetShop.Models;

namespace MyInternetShop.Data.EntityFactories
{
    public class ClientFactory :IClientFactory
    {
        private readonly IMapper _mapper;
        protected class ClientInformation
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public ClientFactory(IMapper mapper)
        {
            _mapper = mapper;
        }
        public  Client ReturnClient (object clientInformation)
        {
             var _clientInformation = (ClientInformation)clientInformation;
             return new Client { ClientId = _clientInformation.Id, FullName = _clientInformation.Name };
        }

        public  async Task<Client> ReturnClientAsync (int ClientId, string FullName = null)
        {
            var _clientInformation = new ClientInformation { Id = ClientId, Name = FullName };
            return await Task<Client>.Run(()=>ReturnClient(_clientInformation));
        }

        public ClientModel ReturnClientModel (int Id, string Name = null)
        {
            var client= new Client { ClientId = Id, FullName = Name };
            return _mapper.Map<ClientModel>(client);
        }

    }
}
