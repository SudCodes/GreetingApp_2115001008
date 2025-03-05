using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        public string GetGreetingBL()
        {
            return "Hello World";
        }
        public string GetGreetingBL(GreetingRequestModel greetingRequest)
        {
            // Both first and last name provided
            if (!string.IsNullOrEmpty(greetingRequest.FirstName) && !string.IsNullOrEmpty(greetingRequest.LastName))
            {
                return $"Hello {greetingRequest.FirstName} {greetingRequest.LastName}";
            }
            // Only first name provided
            else if (!string.IsNullOrEmpty(greetingRequest.FirstName))
            {
                return $"Hello {greetingRequest.FirstName}";
            }
            // Only last name provided
            else if (!string.IsNullOrEmpty(greetingRequest.LastName))
            {
                return $"Hello {greetingRequest.LastName}";
            }
            // No names provided
            else
            {
                return "Hello World";
            }
        }

        public GreetEntity SaveGreetingBL(GreetingModel greetingModel)
        {
            var result = _greetingRL.SaveGreetingRL(greetingModel);
            return result;
        }

        public GreetingModel GetGreetingByIdBL(int Id)
        {
            return _greetingRL.GetGreetingByIdRL(Id);
        }


        //UC6

        public List<GreetingModel> GetAllGreetingsBL()
        {
            var entityList = _greetingRL.GetAllGreetingsRL();  // Calling Repository Layer
            if (entityList != null)
            {
                return entityList.Select(g => new GreetingModel
                {
                    Id = g.Id,
                    Message = g.Message
                }).ToList();  // Converting List of Entity to List of Model
            }
            return null;
        }

        //UC7
        public GreetingModel EditGreetingBL(int id, GreetingModel greetingModel)
        {
            var result = _greetingRL.EditGreetingRL(id, greetingModel); // Calling Repository Layer
            if (result != null)
            {
                return new GreetingModel()
                {
                    Id = result.Id,
                    Message = result.Message
                };
            }
            return null;
        }

        //UC8
        public bool DeleteGreetingBL(int id)
        {
            var result = _greetingRL.DeleteGreetingRL(id);
            if (result)
            {
                return true; // Successfully Deleted
            }
            return false; // Not Found
        }
    }
}
