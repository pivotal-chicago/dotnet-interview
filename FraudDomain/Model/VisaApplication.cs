using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using FraudDomain.DTOs;

namespace FraudDomain.Model
{
    public class VisaApplication
    {
        public ResidentialAddress Address { get; set; }
        public string Id { get; set; }

        public static VisaApplication FromDto(VisaApplicationDto dto)
        {
            var objectModel = new VisaApplication {Id = dto.Id};
            var newAddress = new ResidentialAddress
            {
                City = dto.City,
                ZIP = dto.ZIP,
                State = dto.State
            };

            var addressComponents = dto.Street.Split(' ', 2);
            newAddress.StreetNumber = addressComponents[0];
            newAddress.Street = addressComponents[1];

            objectModel.Address = newAddress;

            return objectModel;
        }
    }


}
