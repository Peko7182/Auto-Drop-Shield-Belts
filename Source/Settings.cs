using Verse;

namespace AutoDropShieldBelts
{
    public class AutoDropShieldBeltSettings : ModSettings
    {
        // Setting to enable/disable mod
        public bool EnableMod = true;
        
        // Setting to put Shield Belts in the inventory, instead of dropping them
        public bool EnableInventory = false;
        
        // Setting to Throw Text Messages
        public bool ThrowTextMessages = true;

        // Method to save and load the settings
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref EnableMod, "enableMod", true);
            Scribe_Values.Look(ref EnableInventory, "enableInventory", false);
            Scribe_Values.Look(ref ThrowTextMessages, "throwTextMessages", true);
        }
    }
}