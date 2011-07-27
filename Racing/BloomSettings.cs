#region File Description
//-----------------------------------------------------------------------------
// BloomSettings.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

namespace BloomPostprocess
{
    /// <summary>
    /// Class holds all the settings used to tweak the bloom effect.
    /// </summary>
    public class BloomSettings
    {
        #region Fields


        // Name of a preset bloom setting, for display to the user.
        public  string Name;


        // Controls how bright a pixel needs to be before it will bloom.
        // Zero makes everything bloom equally, while higher values select
        // only brighter colors. Somewhere between 0.25 and 0.5 is good.
        public  float BloomThreshold;


        // Controls how much blurring is applied to the bloom image.
        // The typical range is from 1 up to 10 or so.
        public  float BlurAmount;


        // Controls the amount of the bloom and base images that
        // will be mixed into the final scene. Range 0 to 1.
        public  float BloomIntensity;
        public  float BaseIntensity;


        // Independently control the color saturation of the bloom and
        // base images. Zero is totally desaturated, 1.0 leaves saturation
        // unchanged, while higher values increase the saturation level.
        public  float BloomSaturation;
        public  float BaseSaturation;


        #endregion


        /// <summary>
        /// Constructs a new bloom settings descriptor.
        /// </summary>
        public BloomSettings(string name, float bloomThreshold, float blurAmount,
                             float bloomIntensity, float baseIntensity,
                             float bloomSaturation, float baseSaturation)
        {
            Name = name;
            BloomThreshold = bloomThreshold;
            BlurAmount = blurAmount;
            BloomIntensity = bloomIntensity;
            BaseIntensity = baseIntensity;
            BloomSaturation = bloomSaturation;
            BaseSaturation = baseSaturation;
        }
        

        /// <summary>
        /// Table of preset bloom settings, used by the sample program.
        /// </summary>
        public static BloomSettings[] PresetSettings =
        {
            //                Name           Thresh  Blur Bloom  Base  BloomSat BaseSat
            new BloomSettings("Default",     0.8f,   1.5f,  0.8F,     1,    2,       4),
            new BloomSettings("OrangeZone",  0.8f,   1f,  0.5F,     0.8f,    2,       3),
            new BloomSettings("BlueZone",    1f,   2f,  1.4F,     1.2f,    1,       1),
            new BloomSettings("TunnelEntrance",   0.1f,  1.5f,   0.1f,     0.2f,    0f,       0),
            new BloomSettings("TunnelExit",    0.8f,   1f,  2F,     3f,    0,       0),
            new BloomSettings("StartFinishLine",   0.9f,   3f,  20F,     10f,    2,       4),
            new BloomSettings("Chicane",   0.7f,  2f,   0.3f,     0.3f,    0f,       0.8f),
            new BloomSettings("Crash",  0.8f,   1f,  2F,     3f,    0,       0),
        };
    }
}
