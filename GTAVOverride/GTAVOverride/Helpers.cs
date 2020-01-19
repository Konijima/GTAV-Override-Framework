using System;
using System.Drawing;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Enums;
using System.Collections.Generic;

namespace GTAVOverride
{
    public static class Helpers
    {
        public static Random Random = new Random();

        public static void DrawText(Vector2 position, string text, bool centre = false, bool proportional = true, float scale = 0.35f, int r = 255, int g = 255, int b = 255, int opacity = 255, Font font = Font.ChaletLondon)
        {
            Function.Call(Hash.SET_TEXT_SCALE, scale, scale);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_CENTRE, centre);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, proportional);
            Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, opacity);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, position.X, position.Y);
        }

        public static void DrawText3D(Vector3 coords, Vector3 offset, string text, bool centre = true, bool proportional = false, float scale = 0.35f, int r = 255, int g = 255, int b = 255, int opacity = 255, Font font = Font.ChaletLondon)
        {
            Vector3 position = coords - offset;
            PointF point = Screen.WorldToScreen(position, true);

            if (point.X == 0 || point.Y == 0) return;

            Function.Call(Hash.SET_TEXT_SCALE, scale, scale);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_CENTRE, centre);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, proportional);
            Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, opacity);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, point.X / Screen.Width, point.Y / Screen.Height);
        }

        public static void DrawRectangle(float x, float y, float width, float height, int r, int g, int b, int a)
        {
            Function.Call(Hash.DRAW_RECT, x, y, width, height, r, g, b, a);
        }

        public static void ShowHelpMessageThisFrame(string text, bool beep = true)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, beep, -1);
        }

        public static string GetAreaName(Vector3 position)
        {
            int locationHash = Function.Call<int>(Hash.GET_HASH_OF_MAP_AREA_AT_COORDS, position.X, position.Y, position.Z);

            if (locationHash == 2072609373) return "Blaine County";
            else return "Los Santos";
        }

        public static bool IsPedPolice(Ped ped)
        {
            if (ped == null || !ped.IsHuman) return false;

            if (ped.Model == PedHash.Cop01SFY ||
                ped.Model == PedHash.Cop01SMY ||
                ped.Model == PedHash.Sheriff01SFY ||
                ped.Model == PedHash.Sheriff01SMY ||
                ped.Model == PedHash.Swat01SMY ||
                ped.Model == PedHash.Ranger01SFY ||
                ped.Model == PedHash.Ranger01SMY ||
                ped.Model == PedHash.RsRanger01AMO ||
                ped.Model == PedHash.Security01SMM)
                return true;

            return false;
        }

        public static bool IsVehiclePolice(Vehicle vehicle)
        {
            if (vehicle == null) return false;

            if (vehicle.Model == VehicleHash.Police ||
                vehicle.Model == VehicleHash.Police2 ||
                vehicle.Model == VehicleHash.Police3 ||
                vehicle.Model == VehicleHash.Police4 ||
                vehicle.Model == VehicleHash.Policeb ||
                vehicle.Model == VehicleHash.PoliceOld1 ||
                vehicle.Model == VehicleHash.PoliceOld2 ||
                vehicle.Model == VehicleHash.Sheriff ||
                vehicle.Model == VehicleHash.Sheriff2 ||
                vehicle.Model == VehicleHash.PoliceT)
                return true;

            return false;
        }

        public static bool CanPlayerRobPeople()
        {
            Ped playerPed = Game.Player.Character;

            if (!playerPed.IsAlive ||
                !playerPed.IsAiming ||
                playerPed.IsRagdoll ||
                Game.Player.WantedLevel > 1 ||
                Game.Player.Character.IsInMeleeCombat ||
                !CanRobPedWithThisWeapon(playerPed.Weapons.Current) ||
                (!playerPed.IsOnFoot && !playerPed.IsOnBike))
                return false;
            return true;
        }

        public static bool CanRobPedWithThisWeapon(Weapon weapon)
        {
            List<WeaponGroup> restrictedWeaponList = new List<WeaponGroup>
            {
                WeaponGroup.Unarmed,
                WeaponGroup.Thrown,
                WeaponGroup.PetrolCan,
                WeaponGroup.NightVision,
                WeaponGroup.Melee,
                WeaponGroup.FireExtinguisher,
                WeaponGroup.DigiScanner,
                WeaponGroup.Parachute,
            };

            if (restrictedWeaponList.Contains(weapon.Group)) return false;

            return true;
        }

        public static SocialClasses GetPedSocialClass(Ped ped)
        {
            List<PedHash> poorList = GetPoorSocialClassPedModels();
            if (poorList.Contains(ped.Model)) return SocialClasses.Poor;

            List<PedHash> richList = GetRichSocialClassPedModels();
            if (richList.Contains(ped.Model)) return SocialClasses.Rich;

            return SocialClasses.Regular;
        }

        public static void ClearAllBlips()
        {
            Debug.Log("Clearing all blips...");

            Blip[] blips = World.GetAllBlips();
            foreach (Blip blip in blips) blip.Delete();
        }

        public static List<PedHash> GetPoorSocialClassPedModels()
        {
            List<PedHash> list = new List<PedHash>
            {
                PedHash.MethFemale01,
                PedHash.Methhead01AMY,
                PedHash.MethMale01,
                PedHash.Downtown01AFM,
                PedHash.Downtown01AMY,
                PedHash.Soucent01AFM,
                PedHash.Soucent01AFO,
                PedHash.Soucent01AFY,
                PedHash.Soucent01AMM,
                PedHash.Soucent01AMO,
                PedHash.Soucent01AMY,
                PedHash.Soucent02AFM,
                PedHash.Soucent02AFO,
                PedHash.Soucent02AFY,
                PedHash.Soucent02AMM,
                PedHash.Soucent02AMO,
                PedHash.Soucent02AMY,
                PedHash.Soucent03AFY,
                PedHash.Soucent03AMM,
                PedHash.Soucent03AMO,
                PedHash.Soucent03AMY,
                PedHash.Soucent04AMM,
                PedHash.Soucent04AMY,
                PedHash.Soucentmc01AFM,
                PedHash.Salton01AFM,
                PedHash.Salton01AFO,
                PedHash.Salton01AMM,
                PedHash.Salton01AMO,
                PedHash.Salton01AMY,
                PedHash.Salton02AMM,
                PedHash.Salton03AMM,
                PedHash.Salton04AMM,
                PedHash.Hillbilly01AMM,
                PedHash.Hillbilly02AMM,
                PedHash.Hippie01,
                PedHash.Hippie01AFY,
                PedHash.Hippy01AMY,
                PedHash.Tramp01,
                PedHash.Tramp01AFM,
                PedHash.Tramp01AMM,
                PedHash.Tramp01AMO,
                PedHash.TrampBeac01AFM,
                PedHash.TrampBeac01AMM,
                PedHash.Skidrow01AFM,
                PedHash.Skidrow01AMM,
                PedHash.Acult01AMM,
                PedHash.Acult01AMO,
                PedHash.Acult01AMY,
                PedHash.Acult02AMO,
                PedHash.Acult02AMY
            };

            return list;
        }

        public static List<PedHash> GetRichSocialClassPedModels()
        {
            List<PedHash> list = new List<PedHash>
            {
                PedHash.Hasjew01AMM,
                PedHash.Hasjew01AMY,
                PedHash.Golfer01AFY,
                PedHash.Golfer01AMM,
                PedHash.Golfer01AMY,
                PedHash.Tennis01AFY,
                PedHash.Tennis01AMM,
                PedHash.TennisCoach,
                PedHash.TennisCoachCutscene,
                PedHash.Business01AFY,
                PedHash.Business01AMM,
                PedHash.Business01AMY,
                PedHash.Business02AFM,
                PedHash.Business02AFY,
                PedHash.Business02AMY,
                PedHash.Business03AFY,
                PedHash.Business03AMY,
                PedHash.Business04AFY,
                PedHash.Bevhills01AFM,
                PedHash.Bevhills01AFY,
                PedHash.Bevhills01AMM,
                PedHash.Bevhills01AMY,
                PedHash.Bevhills02AFM,
                PedHash.Bevhills02AFY,
                PedHash.Bevhills02AMM,
                PedHash.Bevhills02AMY,
                PedHash.Bevhills03AFY,
                PedHash.Bevhills04AFY,
                PedHash.Tourist01AFM,
                PedHash.Tourist01AFY,
                PedHash.Tourist01AMM,
                PedHash.Tourist02AFY
            };

            return list;
        }

        public static bool IsPedGameCharacter(Ped ped)
        {
            if (ped.Model == PedHash.Franklin ||
                ped.Model == PedHash.Michael ||
                ped.Model == PedHash.Trevor)
                return true;
            return false;
        }

        public static bool HasPedClearLineOfSighOfPed(Ped ped1, Ped ped2)
        {
            return Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT, ped1, ped2);
        }

        public static bool HasPedHandsUp(Ped ped)
        {
            return Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, ped, 0);
        }

        public static void StartPedHandsUp(Ped ped, Ped facePed, int duration = 2000)
        {
            Function.Call(Hash.TASK_HANDS_UP, ped, duration, facePed, -1, true);
        }

        public static void UpdatePedHandsUp(Ped ped, int duration = 2000)
        {
            Function.Call(Hash.UPDATE_TASK_HANDS_UP_DURATION, ped, duration);
        }

        public static bool IsPedSpeaking(Ped ped)
        {
            return Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, ped);
        }

        public static void PedSpeakThreathen(Ped ped)
        {
            if (IsPedSpeaking(ped)) return;

            if (IsPedGameCharacter(ped))
            {
                if (Random.Next(0, 2) == 0) Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "DRAW_GUN", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
                else Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "STAY_DOWN", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
            }
            else Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_FUCK_YOU", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
        }

        public static void PedSpeakThreathenSuccess(Ped ped)
        {
            if (IsPedSpeaking(ped)) return;

            if (IsPedGameCharacter(ped))
            {
                if (Random.Next(0, 2) == 0) Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_OUT_OF_MY_WAY", "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR");
                else Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_THANKS", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
            }
            else Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_THANKS", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
        }

        public static void PedSpeakThreathenFailed(Ped ped)
        {
            if (IsPedSpeaking(ped)) return;

            if (IsPedGameCharacter(ped)) Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_FUCK_YOU", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
            else Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_FUCK_YOU", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
        }

        public static void PedSpeakInsult(Ped ped)
        {
            if (IsPedSpeaking(ped)) return;

            Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_INSULT_HIGH", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
        }

        public static void PedSpeakShocked(Ped ped)
        {
            if (IsPedSpeaking(ped)) return;

            Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_SHOCKED_HIGH", "SPEECH_PARAMS_FORCE");
        }

        public static void PedSpeakScared(Ped ped)
        {
            if (IsPedSpeaking(ped)) return;

            if (Random.Next(0, 1) == 0) Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_FRIGHTENED_MED", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
            else Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, "GENERIC_FRIGHTENED_HIGH", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
        }
    }
}
