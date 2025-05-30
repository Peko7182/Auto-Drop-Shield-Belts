using HarmonyLib;
using RimWorld;
using Verse;
using System.Reflection;
using AutoDropShieldBelts.Utils;

// Created with RW Mod Structure Builder
namespace AutoDropShieldBelts
{
    [StaticConstructorOnStartup]
    public static class AutoDropShieldBelts
    {
        static AutoDropShieldBelts()
        {
            // Assembly version
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            
            // Log initialization
            Log.Message($"[Auto Drop Shield Belts v{version}] Initialized");

            // Harmony instance to patch methods
            var harmony = new Harmony("com.peko.rimworld.mod.AutoDropShieldBelts");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
    
    // Harmony patch class
    [HarmonyPatch(typeof(Pawn_ApparelTracker))]
    [HarmonyPatch("Wear")]
    public static class Patch_Pawn_ApparelTracker_Wear
    {
        // Postfix to run code after the apparel is equipped
        [HarmonyPostfix]
        static void Postfix(Pawn_ApparelTracker __instance, Apparel newApparel)
        {
            // If the mod is disabled, do nothing
            if (!AutoDropShieldBeltMod.Settings.EnableMod) return;
            
            Pawn pawn = __instance.pawn;
            
            // If inventory is enabled and the apparel is a shield belt
            if (AutoDropShieldBeltMod.Settings.EnableInventory && newApparel.def == ThingDefOf.Apparel_ShieldBelt)
                // Check if the pawn has any ranged weapons equipped
                if (pawn.equipment.AllEquipmentListForReading.Any(thing => thing.def.IsRangedWeapon))
                    // If a ranged weapon is equipped, un-equip the shield belt
                    pawn.UnEquipApparel(ThingDefOf.Apparel_ShieldBelt);
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
            // If the mod is disabled, do nothing
            if (!AutoDropShieldBeltMod.Settings.EnableMod) return;
            
            Pawn pawn = __instance.pawn;

            if (AutoDropShieldBeltMod.Settings.EnableInventory)
            {
                if (newEq.def.IsRangedWeapon)
                    // If inventory is enabled, unequip the shield belt if a ranged weapon is equipped
                    pawn.UnEquipApparel(ThingDefOf.Apparel_ShieldBelt);
                else
                    // If inventory is enabled, equip the shield belt if a non-ranged weapon is equipped
                    pawn.EquipApparel(ThingDefOf.Apparel_ShieldBelt);
            }
            else
            {
                if (newEq.def.IsRangedWeapon)
                    // If inventory is disabled, drop the shield belt if a ranged weapon is equipped
                    pawn.DropApparel(ThingDefOf.Apparel_ShieldBelt);
            }
        }
    }
    
    // Harmony patch class
    [HarmonyPatch(typeof(ThingOwner))]
    [HarmonyPatch("TryAddOrTransfer", new[] { typeof(Thing), typeof(bool) })]
    public static class Patch_ThingOwner_TryAddOrTransfer
    {
        // Postfix to run code after the equipment is added or transferred
        [HarmonyPostfix]
        static void Postfix(ThingOwner __instance, Thing item, bool canMergeWithExistingStacks, ref bool __result)
        {
            // If the mod is disabled, do nothing
            if (!AutoDropShieldBeltMod.Settings.EnableMod) return;
            
            Pawn pawn = (__instance.Owner as Pawn) ?? ((__instance.Owner as Pawn_InventoryTracker)?.pawn);

            if (AutoDropShieldBeltMod.Settings.EnableInventory)
            {
                // If inventory is enabled, equip the shield belt if a ranged weapon is added
                if (item.def.IsRangedWeapon)
                {
                    pawn.EquipApparel(ThingDefOf.Apparel_ShieldBelt);
                }
            }
        }
    }
    
    // Harmony patch class
    [HarmonyPatch(typeof(Pawn_EquipmentTracker))]
    [HarmonyPatch("TryDropEquipment")]
    public static class Patch_Pawn_EquipmentTracker_TryDropEquipment
    {
        // Postfix to run code after the weapon is dropped
        [HarmonyPostfix]
        static void Postfix(Pawn_EquipmentTracker __instance, ThingWithComps eq, ThingWithComps resultingEq, IntVec3 pos, bool forbid, ref bool __result)
        {
            // If the mod is disabled, do nothing
            if (!AutoDropShieldBeltMod.Settings.EnableMod) return;

            Pawn pawn = __instance.pawn;

            // If inventory is enabled and the drop IS VALID
            if (AutoDropShieldBeltMod.Settings.EnableInventory && __result)
            {
                // If inventory is enabled, equip the shield belt if a weapon is dropped
                pawn.EquipApparel(ThingDefOf.Apparel_ShieldBelt);
            }
        }
    }
}
