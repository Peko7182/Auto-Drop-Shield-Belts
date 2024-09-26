using RimWorld;
using Verse;

namespace AutoDropShieldBelts
{
    public static class Utils
    {
        public static void ThrowText(this Pawn pawn, string text)
        {
            MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, text);
        }
    }
}