﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomObj
{


    public Vector2Int x_y;
    public Vector2Int w_h;

    public GameObject gameObject;
    protected Map map;

    private List<TileObj> tileObjs;

    public RoomObj(Map map)
    {
        this.map = map;

        tileObjs = new List<TileObj>();

        PopulateGameObject();
        gameObject.transform.parent = this.map.gameObject.transform;
    }

    virtual protected GameObject PopulateGameObject()
    {
        GameObject gameObject = new GameObject();
        this.gameObject = gameObject;
        this.gameObject.AddComponent<BaseObjectManager>();
        return gameObject;
    }

    public void AddTile(TileObj tileObj)
    {
        tileObj.gameObject.transform.parent = gameObject.transform;

        tileObjs.Add(tileObj);
    }

    public void RemoveTile(TileObj tileObj)
    {
        tileObjs.Remove(tileObj);
    }

    public enum RoomObjType
    {
        Hideout,
        Blank
    }

    public static RoomObj InstantiateRoomObj(
        RoomObjType roomObjType,
        Map map
    )
    {
        switch (roomObjType)
        {
            case RoomObjType.Hideout:
                return new HideoutRoomObj(map);
        }

        return null;
    }
}
