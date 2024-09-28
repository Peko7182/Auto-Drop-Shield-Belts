using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace AutoDropShieldBelts
{
    public static class Extensions
    {
        /// <summary>
        /// Throws a text message at the pawn's position.
        /// </summary>
        /// <param name="pawn">The pawn to throw the text at.</param>
        /// <param name="text">The text to throw.</param>
        public static void ThrowText(this Pawn pawn, string text)
        {
            if (!AutoDropShieldBeltMod.Settings.ThrowTextMessages) return;
            MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, text);
        }
        
        
        /// <summary>
        /// Equips the apparel on the pawn and removes it from its inventory.
        /// </summary>
        /// <param name="pawn">The pawn to equip the apparel on.</param>
        /// <param name="thingDef">The def of the apparel to equip.</param>
        public static void EquipApparel(this Pawn pawn, ThingDef thingDef)
        {
            // Get the apparel
            var apparel = pawn.inventory?.innerContainer.FirstOrDefault(a => a.def == thingDef);

            // Check if the apparel exists
            if (apparel == null)
            {
                Log.Warning("Apparel not found in pawn's inventory.");
                return; // If apparel not found, do nothing
            }

            // Remove the apparel from the inventory
            pawn.inventory?.innerContainer.Remove(apparel);

            // Add the apparel to the apparel tracker
            pawn.apparel?.Wear((Apparel)apparel);
            
            // Throw a text message to the player
            pawn.ThrowText($"Equipping {thingDef.label}...");
        }

        /// <summary>
        /// Un-equips the apparel from the pawn and puts it in its inventory.
        /// </summary>
        /// <param name="pawn">The pawn to un-equip the apparel from.</param>
        /// <param name="thingDef">The def of the apparel to un-equip.</param>
        public static void UnEquipApparel(this Pawn pawn, ThingDef thingDef)
        {
            // Get the apparel
            var apparel = pawn.apparel?.WornApparel.FirstOrDefault(a => a.def == thingDef);

            // Check if the apparel exists
            if (apparel == null)
            {
                return; // If apparel not found, do nothing
            }

            // Remove the apparel from the apparel tracker
            pawn.apparel?.Remove(apparel);

            // Add the apparel to the pawn's inventory
            pawn.inventory?.innerContainer.TryAdd(apparel, canMergeWithExistingStacks: false);

            // Throw a text message to the player
            pawn.ThrowText($"UnEquipping {thingDef.label}...");
        }
        
        
        /// <summary>
        /// Queues a job to remove the apparel from the pawn and drop it to the ground.
        /// </summary>
        /// <param name="pawn">The pawn to remove the apparel from.</param>
        /// <param name="thingDef">The def of the apparel to remove.</param>
        public static void DropApparel(this Pawn pawn, ThingDef thingDef)
        {
            // Find the apparel the pawn is wearing
            var apparel = pawn.apparel?.WornApparel.FirstOrDefault(a => a.def == thingDef);

            // Check if the apparel exists
            if (apparel == null)
            {
                return; // If apparel not found, do nothing
            }

            // Log the removal of the apparel
            pawn.ThrowText($"Removing {thingDef.label}...");

            // Create a job to remove the apparel
            Job removeJob = JobMaker.MakeJob(JobDefOf.RemoveApparel, apparel);
            removeJob.count = 1;  // Make sure it only removes one apparel at a time
            removeJob.haulDroppedApparel = true;

            // Assign the job to the pawn
            pawn.jobs.StartJob(removeJob, JobCondition.InterruptForced);
        }
    }
}