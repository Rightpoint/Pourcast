using System.Collections.Generic;
using System.Threading.Tasks;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Domain.Services
{
    public interface IMessagePoster
    {
        int PostNewMessage(string body, int[] users = null, string filename = null, string fileContentType = null, byte[] filedata = null);
        int PostReply(int repliedTo, string body, int[] users = null, string filename = null, string fileContentType = null, byte[] filedata = null);
        Task<Dictionary<string, MessageUserInfo>>  GetUsers();
    }
}