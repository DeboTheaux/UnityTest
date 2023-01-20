using UT.Shared;

namespace UT.GameLogic
{
    public interface IGameResultPopUpView : IPopUp
    {
        void ShowPopUpWithResult(string result);
    }
}
