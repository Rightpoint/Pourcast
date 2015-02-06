namespace RightpointLabs.Pourcast.Domain.Services
{
    public interface IMessagePoster
    {
        int PostNewMessage(string body, string filename = null, string fileContentType = null, byte[] filedata = null);
        int PostReply(int repliedTo, string body, string filename = null, string fileContentType = null, byte[] filedata = null);
    }
}