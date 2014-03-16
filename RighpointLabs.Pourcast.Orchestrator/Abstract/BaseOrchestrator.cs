using AutoMapper;
using entities = RightpointLabs.Pourcast.DataModel.Entities;
namespace RighpointLabs.Pourcast.Orchestrator.Abstract
{
    public abstract class BaseOrchestrator
    {
         protected BaseOrchestrator()
         {
             Mapper.CreateMap<entities.Keg, Models.Keg>();
             Mapper.CreateMap<entities.Status, Models.Status>();
             Mapper.CreateMap<entities.TapName, Models.TapName>();
             Mapper.CreateMap<entities.Tap, Models.Tap>();
             Mapper.CreateMap<entities.Pour, Models.Pour>();
             Mapper.CreateMap<entities.Brewery, Models.Brewery>();
             Mapper.CreateMap<entities.Beer, Models.Beer>();
             
         }
    }
}