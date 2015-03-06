namespace RightpointLabs.Pourcast.Domain.Services
{
    public interface IFaceRecognitionService
    {
        string[] ProcessImage(string rawDataUrl, out string intermediateUrl, out string finalUrl);
    }
}