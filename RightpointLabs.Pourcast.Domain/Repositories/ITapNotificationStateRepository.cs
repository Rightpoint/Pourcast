namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using Models;

    public interface ITapNotificationStateRepository
    {
        void Add(TapNotificationState entity);
        void Update(TapNotificationState entity);

        TapNotificationState GetByTapId(string tapId);
    }
}