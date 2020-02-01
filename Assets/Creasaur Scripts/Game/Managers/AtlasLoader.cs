using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlasLoader
{
    public Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

    //Creates new Instance only, Manually call the loadSprite function later on 
    public AtlasLoader(int atlasIndex)
    {
        LoadSprite("Sprites/Atlas/" + "Atlas" + atlasIndex);
    }

    //Loads the provided sprites
    public void LoadSprite(string spriteBaseName)
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>(spriteBaseName);
        if (allSprites == null || allSprites.Length <= 0)
        {
            Debug.LogError("The Provided Base-Atlas Sprite '" + spriteBaseName + "' does not exist!");
            return;
        }

        for (int i = 0; i < allSprites.Length; i++)
        {
            spriteDic.Add(allSprites[i].name, allSprites[i]);
        }
    }

    public Sprite GetAtlas(string atlasName)
    {
        Sprite tempSprite;

        if (!spriteDic.TryGetValue(atlasName, out tempSprite))
        {
            Debug.LogError("The Provided atlas '" + atlasName + "' does not exist!");
            return null;
        }
        return tempSprite;
    }

    public int AtlasCount()
    {
        return spriteDic.Count;
    }
}
