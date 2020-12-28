﻿using UnityEngine;

public class Item {
    public int id;
    public string unidentifiedDisplayName = "";
    public string unidentifiedResourceName = "";
    public string unidentifiedDescriptionName = "";
    public string identifiedDisplayName = "";
    public string identifiedResourceName = "";
    public string identifiedDescriptionName = "";
    public int slotCount = 0;
    public int ClassNum = 0;
    public bool costume = false;

    public ItemInfo info;
    public Texture2D texture;
    public InventoryType tab;
}