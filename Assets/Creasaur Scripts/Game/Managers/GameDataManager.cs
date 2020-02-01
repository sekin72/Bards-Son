using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Managers
{
    public class GameDataManager
    {
        private AtlasLoader _productAtlasLoader;
        private AtlasLoader _lockedProductAtlasLoader;

        public GameDataManager()
        {
            _productAtlasLoader = new AtlasLoader(1);
            _lockedProductAtlasLoader = new AtlasLoader(2);
        }

        public Sprite[] GetAtlas(string name, int atlasIndex)
        {
            var _sprites = new Sprite[10];
            var index = 0;

            if (atlasIndex == 1)
            {
                for (int i = 1; i < 11; i++)
                {
                    _sprites[index] = _productAtlasLoader.GetAtlas(atlasIndex + name + i);
                    index++;
                }
            }
            else
            {
                for (int i = 1; i < 11; i++)
                {
                    _sprites[index] = _lockedProductAtlasLoader.GetAtlas(atlasIndex + name + i);
                    index++;
                }
            }

            return _sprites;
        }

    }
}
