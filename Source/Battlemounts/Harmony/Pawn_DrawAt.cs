using Battlemounts.Jobs;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(Pawn), "DrawAt")]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(bool) })]
    static class Pawn_DrawAt
    {

        static bool Prefix(Pawn __instance, Vector3 drawLoc, bool flip = false)
        {
            var extendedDataStore = Battlemounts.Instance.GetExtendedDataStorage();
            var pawnData = extendedDataStore.GetExtendedDataFor(__instance);
            if (pawnData.mount != null)
            {
                drawLoc = pawnData.mount.Drawer.DrawPos;
                var mountData = extendedDataStore.GetExtendedDataFor(pawnData.mount);

                //TODO: move this to a more appropriate place
                if (pawnData.mount.ageTracker.CurLifeStageIndex == pawnData.mount.RaceProps.lifeStageAges.Count -1 && mountData.drawOffset == -1){

                    PawnKindLifeStage curKindLifeStage = pawnData.mount.ageTracker.CurKindLifeStage;
                    Texture2D unreadableTexture = curKindLifeStage.bodyGraphicData.Graphic.MatSide.mainTexture as Texture2D;
                    Texture2D t = getReadableTexture(unreadableTexture);
                    int backHeight = getBackHeight(t);
                    float backHeightRelative = (float)backHeight / (float)t.height;

                    float textureHeight = curKindLifeStage.bodyGraphicData.drawSize.y;
                    //If animal texture does not fit in a tile, take this into account
                    float extraOffset = textureHeight > 1f ? (textureHeight - 1f) / 2f : 0;
                    //Small extra offset, you don't want to draw pawn exactly on back
                    extraOffset += (float)textureHeight / 20f;
                    mountData.drawOffset = (textureHeight * backHeightRelative - extraOffset);
                    mountData.hasLongNeckOrHorns = hasLongNeckOrHorns(t, backHeight, 6);
                }
                if (mountData.drawOffset != -1)
                {
                    drawLoc.z = pawnData.mount.Drawer.DrawPos.z + mountData.drawOffset;
                }



                __instance.Drawer.DrawAt(drawLoc);
                return false;
            }
            return true;
        }

        /*
         * Attempt of automatic long neck or horns detection that could be used to decide if pawns should be
         * drawn in front or behind the mount in frontal view. 
         * While it does often detect necks and horns, it doesn't really serve its purpose as necks and
         * horns that are long when viewed from the side, are often not in front of the pawn in frontal view. 
         * Might still prove usable for non-vanilla animals
         */
        static bool hasLongNeckOrHorns(Texture2D t, int backHeight, int fraction)
        {
            int bodyPixels = 0;
            int checkFrom = backHeight + 10;
            int middle = t.width / 2;

            if (checkFrom >= t.height)
            {
                return false;
            }

            int totalPixelsChecked = (t.height - checkFrom) * middle;
            int minPixelsForNeckOrHorns = totalPixelsChecked / fraction;
            Log.Message("minPixelsForNeckOrHorns: " + minPixelsForNeckOrHorns + ", totalPixels  " + totalPixelsChecked);

            for (int i = checkFrom; i < t.height; i++)
            {
                for(int j = middle; j < t.width; j++)
                {
                    Color c = t.GetPixel(j, i);
                    if (c.a > 0)
                    {
                        bodyPixels++;
                        if(bodyPixels > minPixelsForNeckOrHorns)
                        {
                            Log.Message("has enough pixels for neck or horns");
                            return true;
                        }
                    }
                }
            }
            Log.Message("has not enough pixels for neck or horns, nPixels: " + bodyPixels + ", totalPixels: " + totalPixelsChecked);

            return false;
        }

        static int getBackHeight(Texture2D t)
        {

            int middle = t.width / 2;
            int backHeight = 0;
            bool inBody = false;


            for (int i = 0; i < t.height; i++)
            {
                Color c = t.GetPixel(middle, i);

                if (inBody && c.a < 1)
                {
                    backHeight = i;
                    break;
                }
                if (c.a == 1)
                {
                    inBody = true;
                }
            }
            return backHeight;
        }

        static Texture2D getReadableTexture(Texture2D texture)
        {
            // Create a temporary RenderTexture of the same size as the texture
            RenderTexture tmp = RenderTexture.GetTemporary(
                                texture.width,
                                texture.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);

            // Blit the pixels on texture to the RenderTexture
            Graphics.Blit(texture, tmp);
            // Backup the currently set RenderTexture
            RenderTexture previous = RenderTexture.active;
            // Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;
            // Create a new readable Texture2D to copy the pixels to it
            Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
            // Copy the pixels from the RenderTexture to the new Texture
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            // Reset the active RenderTexture
            RenderTexture.active = previous;
            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);
            return myTexture2D;
        }
    }
}
