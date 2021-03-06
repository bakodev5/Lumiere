﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    /// <summary>
    /// Enum for Difficulty settings
    /// </summary>
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public Difficulty difficulty;
    public string moveUp;
    public string moveDown;
    public string moveLeft;
    public string moveRight;
    public string walk;
    public string useItem;
    public string dropItem;
    public string pickupItem;
    public string stackModifier;
    public string openInventory;
    public string openMenu;

    /// <summary>
    /// Holds the default settings.
    /// </summary>
    public Settings()
    {
        difficulty = Difficulty.Normal;
        moveUp = "W";
        moveDown = "S";
        moveLeft = "A";
        moveRight = "D";
        walk = "C";
        useItem = "E";
        dropItem = "Q";
        pickupItem = "Mouse0";
        stackModifier = "LeftShift";
        openInventory = "Tab";
        openMenu = "Escape";
    }

}
