using EBird.Application.Model.Bird;
using EBird.Domain.Entities;
using EBird.Test.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Test.MockData
{
    public class BirdMockData
    {
        public static ICollection<BirdCreateDTO> GetWrongArgBirdCreateDTOs()
        {
            dynamic birdCreateData = SeedingServices.LoadJson("BIRD_CREATE.json");
            
            var birdCreateDtoList = new List<BirdCreateDTO>();
         
            foreach(var data in birdCreateData)
            {
                birdCreateDtoList.Add(new BirdCreateDTO()
                {
                    Name = data.name,
                    Age = data.age,
                    Weight = data.weight,
                    Elo = data.elo,
                    Status = data.status,
                    Description = data.description,
                    Color = data.color,
                    BirdTypeId = Guid.NewGuid(),
                    OwnerId = Guid.NewGuid()
                });
            }
            return birdCreateDtoList;
        }

        internal static BirdRequestDTO[] GetBirdUpdateDTOs()
        {
            dynamic birdUpdateData = SeedingServices.LoadJson("BIRD_UPDATE.json");

            var birdUpdateDtoList = new List<BirdRequestDTO>();

            foreach(var data in birdUpdateData)
            {
                birdUpdateDtoList.Add(new BirdRequestDTO()
                {
                    Name = data.name,
                    Age = data.age,
                    Weight = data.weight,
                    Elo = data.elo,
                    Status = data.status,
                    Description = data.description,
                    Color = data.color,
                    BirdTypeId = Guid.NewGuid(),
                });
            }
            return birdUpdateDtoList.ToArray();
        }

        internal static ICollection<BirdCreateDTO> GetRightArgBirdCreateDTOS()
        {
            dynamic birdCreateData = SeedingServices.LoadJson("BIRD_CREATE.json");

            var birdCreateDtoList = new List<BirdCreateDTO>();

            foreach(var data in birdCreateData)
            {
                birdCreateDtoList.Add(new BirdCreateDTO()
                {
                    Name = data.name,
                    Age = data.age,
                    Weight = data.weight,
                    Elo = data.elo,
                    Status = data.status,
                    Description = data.description,
                    Color = data.color
                });
            }
            return birdCreateDtoList;
        }
    }
}
