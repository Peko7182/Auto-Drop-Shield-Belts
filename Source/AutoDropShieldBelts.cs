using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System.Reflection;

// Created with RW Mod Structure Builder
namespace AutoDropShieldBelts
{
    [StaticConstructorOnStartup]
    public static class AutoDropShieldBelts
    {
        static AutoDropShieldBelts()
        {
            // Log initialization
            Log.Message("[Auto Drop Shield Belts v0.0.0.1] Initialized");

            // Harmony instance to patch methods
            var harmony = new Harmony("com.peko.rimworld.mod.AutoDropShieldBelts");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    // Harmony patch class
    [HarmonyPatch(typeof(Pawn_EquipmentTracker))]
    [HarmonyPatch("AddEquipment")]
    public static class Patch_Pawn_EquipmentTracker_AddEquipment
    {
        // Postfix to run code after the weapon is equipped
        [HarmonyPostfix]
        static void Postfix(Pawn_EquipmentTracker __instance, ThingWithComps newEq)
        {
            // Check if the thing equipped is a ranged weapon
            if (newEq.def.IsRangedWeapon)
            {
                Pawn pawn = __instance.pawn; // Get the pawn who equipped the weapon

                QueueShieldBeltRemovalJob(pawn);
            }
        }

        // Queue a job to remove the shield belt slowly
        private static void QueueShieldBeltRemovalJob(Pawn pawn)
        {
            // Find the shield belt the pawn is wearing
            var shieldBelt = pawn.apparel?.WornApparel.FirstOrDefault(a => a.def == ThingDefOf.Apparel_ShieldBelt);
            if (shieldBelt != null)
            {
                // Log the removal of the shield belt
                pawn.ThrowText("Removing shield belt...");

                // Create a job to remove the shield belt
                Job removeJob = JobMaker.MakeJob(JobDefOf.RemoveApparel, shieldBelt);
                removeJob.count = 1;  // Make sure it only removes one shield belt at a time
                removeJob.haulDroppedApparel = true;

                // Assign the job to the pawn
                pawn.jobs.StartJob(removeJob, JobCondition.InterruptForced);
            }
        }
    }
}
