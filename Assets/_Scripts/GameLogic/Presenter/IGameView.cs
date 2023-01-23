using UT.Shared;

namespace UT.GameLogic
{
    public interface IGameView : IScreen
    {
        void StartTimer(float withSeconds);
        void StopTimer();
        void AddTimeToTimer(int newTotalSeconds);
    }
}