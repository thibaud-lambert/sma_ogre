﻿using Mogre;
using System;

namespace sma_ogre
{
	class UnkownConfigKeyException : Exception {}
	class UnkownConfigSectionException : Exception {}

    class WorldConfig
    {
        private static WorldConfig singleton;

        private Random rnd;

        private int    mInitialGoodAgentsNumber;
        private int    mInitialBadAgentsNumber;
        private float  mGroundWidth;
        private float  mGroundLength;
        private float  mGroundBorderWidth;
        private string mOgreMesh;

        private ColourValue mAmbientLightOn;
        private ColourValue mAmbientLightOff;
        private bool        mAmbientLightIsOn;

        private WorldConfig()
        {
            rnd = new Random();
            mAmbientLightIsOn = true;

            try
            {
                DefineConfig();
            }
            catch (UnkownConfigKeyException e)
            {
                Console.WriteLine(e);
            }
            catch (UnkownConfigSectionException e)
            {
                Console.WriteLine(e);
            }
        }

        public static WorldConfig Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new WorldConfig();
                }
                return singleton;
            }
        }

		public int RandInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

		public float RandFloat(float min, float max)
        {
            return (float)rnd.NextDouble() * (max - min) + min;
        }

		// Properties to get configuration

        public float GroundWidth
        {
            get { return mGroundWidth; }
        }

        public float GroundLength
        {
            get { return mGroundLength; }
        }

        public float GroundBorderWidth
        {
            get { return mGroundBorderWidth; }
        }

        public int InitialGoodAgentsNumber
        {
            get { return mInitialGoodAgentsNumber; }
        }

        public int InitialBadAgentsNumber
        {
            get { return mInitialBadAgentsNumber; }
        }

        public ColourValue AmbientLightOn
        {
            get { return mAmbientLightOn; }
        }

        public ColourValue AmbientLightOff
        {
            get { return mAmbientLightOff; }
        }

        public string OgreMesh
        {
            get { return mOgreMesh; }
        }

        public ColourValue SwitchedLight()
        {
            if (mAmbientLightIsOn)
            {
                mAmbientLightIsOn = false;
                return mAmbientLightOff;
            }
			else
            {
                mAmbientLightIsOn = true;
                return mAmbientLightOn;
            }
        }

        private void DefineConfig()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load("world.cfg", "\t:=", true);
            var section = cf.GetSectionIterator();
            while (section.MoveNext())
            {
                foreach (var line in section.Current)
                {
                    if (section.CurrentKey.Equals("General"))
                    {
                        LoadGeneralConfig(line);
                    }
                    else if (section.CurrentKey.Equals("Ground"))
                    {
                        LoadGroundConfig(line);
                    }
                    else if (section.CurrentKey.Equals("Meshes"))
                    {
                        LoadMeshesConfig(line);
                    }
					else
                    {
                        throw new UnkownConfigSectionException();
                    }
                }
            }
        }

        private void LoadGeneralConfig(System.Collections.Generic.KeyValuePair<string,string> line)
        {
            if (line.Key.Equals("InitialGoodAgentsNumber"))
            {
                mInitialGoodAgentsNumber = int.Parse(line.Value);
            }
			else if (line.Key.Equals("InitialBadAgentsNumber"))
            {
                mInitialBadAgentsNumber = int.Parse(line.Value);
            }
			else if (line.Key.Equals("AmbientLightOn"))
            {
                string[] lightOnColors = line.Value.Split(',');
                mAmbientLightOn = new ColourValue(float.Parse(lightOnColors[0], System.Globalization.CultureInfo.InvariantCulture),
                                                  float.Parse(lightOnColors[1], System.Globalization.CultureInfo.InvariantCulture),
                                                  float.Parse(lightOnColors[2], System.Globalization.CultureInfo.InvariantCulture));
            }
			else if (line.Key.Equals("AmbientLightOff"))
            {
                string[] lightOffColors = line.Value.Split(',');
                mAmbientLightOff = new ColourValue(float.Parse(lightOffColors[0], System.Globalization.CultureInfo.InvariantCulture),
                                                   float.Parse(lightOffColors[1], System.Globalization.CultureInfo.InvariantCulture),
                                                   float.Parse(lightOffColors[2], System.Globalization.CultureInfo.InvariantCulture));
            }
			else {
                throw new UnkownConfigKeyException();
            }
        }

        private void LoadGroundConfig(System.Collections.Generic.KeyValuePair<string,string> line)
        {
            if (line.Key.Equals("Width"))
            {
                mGroundWidth = int.Parse(line.Value);
            }
			else if (line.Key.Equals("Length"))
            {
                mGroundLength = int.Parse(line.Value);
            }
			else if (line.Key.Equals("BorderWidth"))
            {
                mGroundBorderWidth = int.Parse(line.Value);
            }
			else {
                throw new UnkownConfigKeyException();
            }
        }

        private void LoadMeshesConfig(System.Collections.Generic.KeyValuePair<string,string> line)
        {
            if (line.Key.Equals("Ogre"))
            {
                mOgreMesh = line.Value;
            }
			else {
                throw new UnkownConfigKeyException();
            }
        }
    }
}
