using FiveBabbittGames;
using UnityEngine;

public interface IDetectionStrategy
{
   bool Execute(Transform player, Transform detector, CountdownTimer timer);

}
