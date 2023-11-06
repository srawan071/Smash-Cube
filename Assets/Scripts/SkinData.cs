using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "ScriptableObjects/SkinManager", order = 1)]
public class SkinData : ScriptableObject
{
    public Texture[] NormalSkin;
    public Texture[] EpicSkin;

    public Sprite[] ShopNormalSkin;
    public Sprite[] ShopEpicSkin;
    
}

