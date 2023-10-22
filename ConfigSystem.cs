using UnityEngine;
using PugMod;
using Unity.Entities;

namespace MapExtras {
    public class ConfigSystem { //  Config File Location:  C:\Users\<user>\AppData\LocalLow\Pugstorm\Core Keeper\Steam\<number>\mods\MapExtras\...
        public static string GetWorldSectionName(World world) {
            return "World" + world.SequenceNumber;
        }
        
        public static bool GetString(string section, string key, ref string value, string defaultValue) {
            if (API.Config.TryGet(MapExtrasMod.MOD_ID, section, key, out string output)) {
                value = output;
                return true;
            }
            else {
                API.Config.Set(MapExtrasMod.MOD_ID, section, key, defaultValue);
                value = defaultValue;
                return false;
            }
        }
        
        public static bool GetFloat(string section, string key, ref float value, float defaultValue) {
            if (API.Config.TryGet(MapExtrasMod.MOD_ID, section, key, out string valueString)) {
                if (float.TryParse(valueString, out float output)) {
                    value = output;
                    return true;
                }
                else {
                    MapExtrasMod.Log("The config value entered for \"" + section + "/" + key + "\" is not the right type (it should be a number).");
                    return false;
                }
            }
            else {
                API.Config.Set(MapExtrasMod.MOD_ID, section, key, defaultValue);
                value = defaultValue;
                return false;
            }
        }
        
        public static bool GetInt(string section, string key, ref int value, int defaultValue) {
            if (API.Config.TryGet(MapExtrasMod.MOD_ID, section, key, out string valueString)) {
                if (int.TryParse(valueString, out int output)) {
                    value = output;
                    return true;
                }
                else {
                    MapExtrasMod.Log("The config value entered for \"" + section + "/" + key + "\" is not the right type (it should be a number).");
                    return false;
                }
            }
            else {
                API.Config.Set(MapExtrasMod.MOD_ID, section, key, defaultValue);
                value = defaultValue;
                return false;
            }
        }
        
        public static bool GetBool(string section, string key, ref bool value, bool defaultValue) {
            if (API.Config.TryGet(MapExtrasMod.MOD_ID, section, key, out string valueString)) {
                if (bool.TryParse(valueString, out bool output)) {
                    value = output;
                    return true;
                }
                else {
                    MapExtrasMod.Log("The config value entered for \"" + section + "/" + key + "\" is not the right type (it should be true/false).");
                    return false;
                }
            }
            else {
                API.Config.Set(MapExtrasMod.MOD_ID, section, key, defaultValue);
                value = defaultValue;
                return false;
            }
        }
        
        public static bool GetColor(string section, string key, ref Color value, Color defaultValue) {
            string colorString = "";
            bool foundString = GetString(section, key, ref colorString, defaultValue.r + " " + defaultValue.g + " " + defaultValue.b + " " + defaultValue.a);

            if (foundString) {
                string[] rgbaStrings = colorString.Trim().Split(' ');

                if (rgbaStrings.Length == 4) {
                    float[] rgbaFloats = new float[4];

                    for (int i = 0; i < rgbaStrings.Length; i++) {
                        if (float.TryParse(rgbaStrings[i], out float output)) {
                            rgbaFloats[i] = output;
                        }
                        else {
                            MapExtrasMod.Log("The config value entered for \"" + section + "/" + key + "\" is not the right type (all of the values need to be floats).");
                            return false;
                        }
                    }
                    
                    value = new Color(rgbaFloats[0], rgbaFloats[1], rgbaFloats[2], rgbaFloats[3]);
                    return true;
                }
                else {
                    MapExtrasMod.Log("The config value entered for \"" + section + "/" + key + "\" is not the right format (it should be \"<r> <g> <b> <a>\").");
                    return false;
                }
            }
            else {
                return false;
            }
        }
        
        public static void SetString(string section, string key, string value) {
            API.Config.Set(MapExtrasMod.MOD_ID, section, key, value);
        }
        
        public static void SetBool(string section, string key, bool value) {
            API.Config.Set(MapExtrasMod.MOD_ID, section, key, value);
        }
        
        public static void SetInt(string section, string key, int value) {
            API.Config.Set(MapExtrasMod.MOD_ID, section, key, value);
        }
        
        public static void SetFloat(string section, string key, float value) {
            API.Config.Set(MapExtrasMod.MOD_ID, section, key, value);
        }
    }
}