﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{

    public bool GenMapOnStart = false;

    private int levelNumber = 1;
    private const float difficulty = 1.5f;
    private Text levelText;

    private const int initalMapSize = 40;
    private const float mapSizeIncreaseFactor = 3;


    public ComplexGenAlgo complexGenAlgo;
    public RoomProperties roomProperties;

    private GameObject currMap;

    public GameObject ExitDoor;

    void Start()
    {
        levelNumber = 1;
        levelText = GameObject.FindGameObjectWithTag ("UILevel").GetComponent<Text>();

        if(GenMapOnStart)
        {
            GenMap();
        }

    }


    public void GenMap()
    {
        UpdateLevelText ();
        currMap = new GameObject();

        int width = (int)(initalMapSize + 1 * mapSizeIncreaseFactor);
        int height = (int)(initalMapSize + 1 * mapSizeIncreaseFactor);

        complexGenAlgo.roomAttempts = 1000;
        complexGenAlgo.spaceBetweenRooms = 5;

        Map map = new Map(width, height, 1, currMap, roomProperties, levelNumber, difficulty);
        complexGenAlgo.GenerateMap(map, ExitDoor);

        levelNumber++;

    }

    public void DestroyMap()
    {
        //Default layer = 0
        //DroppedItems layer = 8
        //Enemy layer = 9

        GameObject[] gameObjectArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach(GameObject gObject in gameObjectArray)
        {
            if(gObject.layer == LayerMask.NameToLayer("DroppedItems") || gObject.layer == LayerMask.NameToLayer("Default") || gObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Destroy(gObject);
            }
        }
    }

    /// <summary>
    /// Updates the level text.
    /// </summary>
    private void UpdateLevelText()
    {
        if (levelText == null) 
        {
            levelText = GameObject.FindGameObjectWithTag ("UILevel").GetComponent<Text>();
        }

        levelText.text = "Current level: " + levelNumber;
    }
}