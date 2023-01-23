using UnityEngine;

/* 
 * Yellow block - Duplicates mouse speed for 10 seconds 
 * Light Blue block: Scrambles all existing items on the screen for others. 
 * Red coin: Blocks the clicking capabilities of the player for 4 seconds.
 * Green coin: Makes the game’ clock run at 1.5 times speed, but gives out double points for clicked objects. 
 */

namespace UT.GameLogic
{
    [CreateAssetMenu(menuName = "Game/PowerUps/SpeedUpTimer", fileName = "SpeedUpTimer")]
    public class SpeedUpTimer : ScriptableObject, IPowerUp
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}




