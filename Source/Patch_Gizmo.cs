using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;

namespace LoadFromStockpileZone
{
    [HarmonyPatch(typeof(CompTransporter), "CompGetGizmosExtra")]
    public class Patch_Gizmo
    {
        public static void Postfix(ref IEnumerable<Gizmo> __result, CompTransporter __instance)
        {
            List<Gizmo> gizmos = __result.ToList();

            AddGizmos(__instance, ref gizmos); 
            __result = gizmos;

        }

        private static void AddGizmos(CompTransporter __instance, ref List<Gizmo> gizmos)
        {
            gizmos.Add(CreateGizmo_LoadStockpile(__instance));
        }

        private static Gizmo CreateGizmo_LoadStockpile(CompTransporter transporter)
        {
            List<CompTransporter> group = transporter.TransportersInGroup(transporter.parent.Map);
            string label;
            if (group != null)
            {
                label = ((group?.Count > 1) ? TranslatorFormattedStringExtensions.Translate("Gizmo_LoadStockpileGroup", group.Count) : Translator.Translate("Gizmo_LoadStockpile"));
            }
            else
            {
                label = Translator.Translate("Gizmo_LoadStockpile");
            }

            return new Command_LoadStockpile
            {
                defaultLabel = label,
                defaultDesc = Translator.Translate("Gizmo_LoadStockpileDesc"),
                icon = ContentFinder<Texture2D>.Get("UI/LoadStockpile", false),
                transporter = transporter,
                group = group
            };
        }


    }
}
