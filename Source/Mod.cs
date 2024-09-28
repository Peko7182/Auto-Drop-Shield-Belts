using UnityEngine;
using Verse;

namespace AutoDropShieldBelts
{
    public class AutoDropShieldBeltMod : Mod
    {
        // Reference to the settings
        public static AutoDropShieldBeltSettings Settings;

        public AutoDropShieldBeltMod(ModContentPack content) : base(content)
        {
            // Initialize the settings
            Settings = GetSettings<AutoDropShieldBeltSettings>();
        }

        // This is where the settings window UI is drawn
        public override void DoSettingsWindowContents(Rect inRect)
        {
            // Create a listing for UI elements
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            // Checkboxes for the settings
            listingStandard.CheckboxLabeled("Enable Mod", ref Settings.EnableMod, "Enables the mod.");
            listingStandard.CheckboxLabeled("Enable Inventory", ref Settings.EnableInventory, "Instead of dropping shield belts, put them in the inventory. Compatible with Simple Sidearms.");
            listingStandard.CheckboxLabeled("Throw Text Messages", ref Settings.ThrowTextMessages, "Throw text messages when interacting with shield belts.");

            listingStandard.End();
        }

        // The name displayed in the mod settings menu
        public override string SettingsCategory()
        {
            return "Auto Drop Shield Belts";
        }
    }
}