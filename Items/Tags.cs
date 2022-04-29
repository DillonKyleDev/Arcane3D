using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    public bool Collectable;
    public bool Player;
    public bool Enemy;
    public bool Weapon;
    public bool Projectile;
    public bool Attackable;
    public bool Slime;
    public bool Slime_Ball;
    public bool TerrainItem;
    public bool Ground;
    public bool Cast_Resource;
    public bool Sm_Resource;
    public bool Med_Resource;
    public bool Lg_Resource;

    List<KeyValuePair<string, bool>> tagList = new List<KeyValuePair<string, bool>>();
    
    private void Awake() {
        KeyValuePair<string, bool> tag1 = new KeyValuePair<string, bool>("Collectable", Collectable);
        KeyValuePair<string, bool> tag2 = new KeyValuePair<string, bool>("Player", Player);
        KeyValuePair<string, bool> tag3 = new KeyValuePair<string, bool>("Enemy", Enemy);
        KeyValuePair<string, bool> tag4 = new KeyValuePair<string, bool>("Weapon", Weapon);
        KeyValuePair<string, bool> tag5 = new KeyValuePair<string, bool>("Projectile", Projectile);
        KeyValuePair<string, bool> tag6 = new KeyValuePair<string, bool>("Attackable", Attackable);
        KeyValuePair<string, bool> tag7 = new KeyValuePair<string, bool>("Slime", Slime);
        KeyValuePair<string, bool> tag8 = new KeyValuePair<string, bool>("Slime_Ball", Slime_Ball);
        KeyValuePair<string, bool> tag9 = new KeyValuePair<string, bool>("TerrainItem", TerrainItem);
        KeyValuePair<string, bool> tag10 = new KeyValuePair<string, bool>("Ground", Ground);
        KeyValuePair<string, bool> tag11 = new KeyValuePair<string, bool>("Cast_Resource", Cast_Resource);
        KeyValuePair<string, bool> tag12 = new KeyValuePair<string, bool>("Sm_Resource", Sm_Resource);
        KeyValuePair<string, bool> tag13 = new KeyValuePair<string, bool>("Med_Resource", Med_Resource);
        KeyValuePair<string, bool> tag14 = new KeyValuePair<string, bool>("Lg_Resource", Lg_Resource);
        
        tagList.Add(tag1);
        tagList.Add(tag2);
        tagList.Add(tag3);
        tagList.Add(tag4);
        tagList.Add(tag5);
        tagList.Add(tag6);
        tagList.Add(tag7);
        tagList.Add(tag8);
        tagList.Add(tag9);
        tagList.Add(tag10);
        tagList.Add(tag11);
        tagList.Add(tag12);
        tagList.Add(tag13);
        tagList.Add(tag14);
    }

    public bool HasTag(string tag) {
        bool hasTag = false;
        for(int i = 0; i < tagList.Count; i++) {
            if(tagList[i].Key == tag) {
                hasTag = tagList[i].Value;
                return hasTag;
            } else {
                hasTag = false;
            }
        }
        return hasTag;
    }

    public void AddTag(string tag) {
        for(int i = 0; i < tagList.Count; i++) {
            if(tagList[i].Key == tag) {
                var newEntry = new KeyValuePair<string, bool>(tagList[i].Key, true);
            }
        }
    }
     public void RemoveTag(string tag) {
        for(int i = 0; i < tagList.Count; i++) {
            if(tagList[i].Key == tag) {
                var newEntry = new KeyValuePair<string, bool>(tagList[i].Key, false);
            }
        }
    }
}
