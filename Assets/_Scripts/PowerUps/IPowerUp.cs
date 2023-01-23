/* Target - Doesn’t give points, but the next 3 items are coins 
 * Green block: Gives 4 seconds of extra time plus 1 second per 20 points the player has.
 * Yellow block - Duplicates mouse speed for 10 seconds 
 * Light Blue block: Scrambles all existing items on the screen for others. 
 * Red coin: Blocks the clicking capabilities of the player for 4 seconds.
 * Green coin: Makes the game’ clock run at 1.5 times speed, but gives out double points for clicked objects. 
 */

namespace UT.GameLogic
{
    public interface IPowerUp
    {
        void Initialize();
        void Execute();
        void Dispose();
    }
}




