﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Graphics
{
    public class TexturePool
    {
        public Game Game { get; set; }

        public bool loaded = false;

        public Dictionary<string, Texture2D> Textures;

        private static TexturePool _instance;
        public static TexturePool Instance
        {
            get
            {
                if (_instance == null) _instance = new TexturePool();
                return _instance;
            }
        }

        private TexturePool()
        {
            Textures = new Dictionary<string, Texture2D>();
        }

        public bool LoadAll()
        {
            if (Game == null) return false;
            if (loaded) return false;

            //LoadTexture(TextureNames.TEST_1);
            //LoadTexture(TextureNames.TEST_2);
            //LoadTexture(TextureNames.TEST_3);
            //LoadTexture(TextureNames.TEST_4);

            LoadTexture(TextureNames.PLAYER_S);
            LoadTexture(TextureNames.PLAYER_W);
            LoadTexture(TextureNames.PLAYER_C);
            LoadTexture(TextureNames.PLAYER_J);

            loaded = true;

            return true;
        }

        private void LoadTexture(string name)
        {
            Textures.Add(name, Game.Content.Load<Texture2D>(name));
        }
    }
}
