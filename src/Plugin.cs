using BepInEx;
using System.Security.Permissions;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace SpawnResetter;

[BepInPlugin("alduris.spawnresetter", "Spawn Resetter", "1.0")]
sealed class Plugin : BaseUnityPlugin
{
    bool init;

    public void OnEnable()
    {
        On.WorldLoader.GeneratePopulation += WorldLoader_GeneratePopulation;
    }

    private void WorldLoader_GeneratePopulation(On.WorldLoader.orig_GeneratePopulation orig, WorldLoader self, bool fresh)
    {
        orig(self, fresh);
    }
}
