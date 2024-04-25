using BepInEx;
using System.Security.Permissions;
using UnityEngine;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace SpawnResetter;

[BepInPlugin("alduris.spawnresetter", "Spawn Resetter", "1.0")]
sealed class Plugin : BaseUnityPlugin
{
    public bool needsToReset;

    public void OnEnable()
    {
        On.RainWorldGame.Update += RainWorldGame_Update;
        On.WorldLoader.GeneratePopulation += WorldLoader_GeneratePopulation;
    }

    private void RainWorldGame_Update(On.RainWorldGame.orig_Update orig, RainWorldGame self)
    {
        orig(self);
        if (self.devToolsActive && !needsToReset && Input.GetKey(KeyCode.Backspace))
        {
            needsToReset = true;
        }
        if (needsToReset && self.cameras[0].room != null)
        {
            self.devToolsLabel.text += " : Resetting spawns next region load";
        }
    }

    private void WorldLoader_GeneratePopulation(On.WorldLoader.orig_GeneratePopulation orig, WorldLoader self, bool fresh)
    {
        if (!fresh && needsToReset)
        {
            foreach (AbstractRoom abstractRoom in self.abstractRooms)
            {
                if (abstractRoom.shelter || (ModManager.MSC && abstractRoom.isAncientShelter))
                    continue;

                abstractRoom.creatures.Clear();
                abstractRoom.entitiesInDens.Clear();
            }
            fresh = true;
            Logger.LogInfo("Reset spawns!");
        }
        if (needsToReset)
        {
            needsToReset = false;
        }
        orig(self, fresh);
    }
}
