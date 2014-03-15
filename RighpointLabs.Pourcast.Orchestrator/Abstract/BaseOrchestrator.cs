using AutoMapper;
using entities = RightpointLabs.Pourcast.DataModel.Entities;
namespace RighpointLabs.Pourcast.Orchestrator.Abstract
{
    public abstract class BaseOrchestrator
    {
         protected BaseOrchestrator()
         {
             Mapper.CreateMap<entities.Brewery, Models.Brewery>();
         }
    }
}