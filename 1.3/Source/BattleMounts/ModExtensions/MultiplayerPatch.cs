using Verse;
using Multiplayer.API;

namespace Battlemounts.ModExtensions
{
    [StaticConstructorOnStartup]
    public static class MultiplayerPatch
    {
        static MultiplayerPatch()
        {
            if (MP.enabled)
            {
                MP.RegisterAll();
            }
        }
    }
}
