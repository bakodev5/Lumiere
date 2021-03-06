﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Map
{

    private Tile[,] tileMatrix;
    public List<Container> containers;
    public RoomProperties roomProperties;

    public int w, h;
    public int tileOffset;
    public GameObject gameObject;

    public int levelNumber;
    public float difficulty;

    public Map(int w, int h, int tileOffset, GameObject gameObject, RoomProperties roomProperties, int levelNumber, float difficulty)
    {
        this.gameObject = gameObject;
        this.w = w;
        this.h = h;
        this.tileOffset = tileOffset;
        this.tileMatrix = new Tile[h, w];
        this.containers = new List<Container>();
        this.roomProperties = roomProperties;

        this.levelNumber = levelNumber;
        this.difficulty = difficulty;
    }

    /// TODO: this description was from legacy code, change it to fit this (somewhat
    ///       similar) code
    ///  
    /// <summary>     
    /// Applies a GameObject to a coordinate in the tileMap as well as adding that tile
    /// to the Unity heirarchy as a child to a Container. Removes the tile that existed 
    /// in the specified location if a tile exists.
    /// </summary>
    /// <returns>
    /// True if the tile was placed correctly, false otherwise (such as when the tile
    /// cannot be placed in the specified coordinate due to the coordinate being invalid)
    /// </returns>
    public Tile SetTile(int x, int y, Tile tile, Container container)
    {
        if (!ValidTileSpace(x, y))
            return null;

        Tile preExistingTile = GetTile(x, y);
        if (preExistingTile != null)
            preExistingTile.Remove();

        this.tileMatrix[y, x] = tile;

        tile.SetX_Y(x, y);
        tile.SetContainer(container);
        tile.SetMap(this);

        container.AddTile(tile);

        return tile;
    }

    public Tile CreateTileAndSetTile(int x, int y, Container container, TileType tileType)
    {
        Tile tile = new Tile(x, y, this, tileType);

        return SetTile(x, y, tile, container);
    }

    public Tile CreateTileAndSetTile(Pair<int, int> pair, Container container, TileType tileType)
    {
        return CreateTileAndSetTile(pair.First, pair.Second, container, tileType);
    }

    public List<Tile> GetTiles()
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = 0; i < tileMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < tileMatrix.GetLength(1); j++)
            {
                tiles.Add(tileMatrix[i, j]);
            }
        }

        return tiles;
    }

    public void FillArea(int x, int y, int w, int h, TileType tileType, Container container)
    {
        // Force top and left of rectangle to be inside the map.
        if (x < 0)
            x = 0;
        if (y < 0)
            y = 0;

        // Force width and height of rectangle to not be outside the map.
        if (x + w > this.w)
            w = this.w - x;
        if (y + h > this.h)
            h = this.h - y;

        for (int currX = x; currX < x + w; currX++)
        {
            for (int currY = y; currY < y + h; currY++)
            {
                Tile tileToAdd =
                SetTile(currX, currY, new Tile(x, y, this, tileType), container);
            }
        }
    }

    public void FillLine(int x, int y, int length, Utilities.Direction direction, TileType tileType, Container container)
    {
        switch (direction)
        {
        case Utilities.Direction.NORTH:
            FillArea(x, y - length, 1, length, tileType, container);
            break;

        case Utilities.Direction.SOUTH:
            FillArea(x, y, 1, length, tileType, container);
            break;

        case Utilities.Direction.WEST:
            FillArea(x - length, y, length, 1, tileType, container);
            break;

        case Utilities.Direction.EAST:
            FillArea(x, y, length, 1, tileType, container);
            break;
        }
    }

    public void FillAreaWithBorder(int x, int y, int w, int h, TileType areaTileType, TileType borderTileType, Container container)
    {
        FillArea(x, y, w, h, areaTileType, container);
        FillLine(x, y, w, Utilities.Direction.EAST, borderTileType, container);
        FillLine(x, y, h, Utilities.Direction.SOUTH, borderTileType, container);
        FillLine(x, y + h - 1, w, Utilities.Direction.EAST, borderTileType, container);
        FillLine(x + w - 1, y, h, Utilities.Direction.SOUTH, borderTileType, container);
        SetTile(x + w - 1, y + h - 1, new Tile(x + w - 1, y + h - 1, this, borderTileType), container);

    }

    public void CreateBorder(int x, int y, int w, int h, TileType borderTileType, Container container)
    {
        FillLine(x, y, w, Utilities.Direction.EAST, borderTileType, container);
        FillLine(x, y, h, Utilities.Direction.SOUTH, borderTileType, container);
        FillLine(x, y + h - 1, w, Utilities.Direction.EAST, borderTileType, container);
        FillLine(x + w - 1, y, h, Utilities.Direction.SOUTH, borderTileType, container);
        SetTile(x + w - 1, y + h - 1, new Tile(x + w - 1, y + h - 1, this, borderTileType), container);
    }

    public Tile GetTile(int x, int y)
    {
        if (!ValidTileSpace(x, y))
            return null;

        return tileMatrix[y, x];
    }

    public Room GenRoom(RoomType roomType)
    {
        int roomWidth = Utilities.RandomIntInRange(roomProperties.minWidth, roomProperties.maxWidth);
        int roomHeight = Utilities.RandomIntInRange(roomProperties.minHeight, roomProperties.maxHeight);

        int x = Utilities.RandomIntInRange(0, w - roomWidth);
        int y = Utilities.RandomIntInRange(0, h - roomHeight);

        return new Room(this, x, y, roomWidth, roomHeight, roomType);
    }

    public Room GenRandomRoom()
    {
        Room room;
        do
        {
            RoomType[] roomTypes = roomProperties.roomTypes;
            RoomType roomType = roomTypes[Utilities.RandomIntInRange(0, roomTypes.Length)];
            room = GenRoom(roomType);
        }
        while (room == null);

        return room;
    }

    public void AddContainer(Container container)
    {
        this.containers.Add(container);
        container.gameObject.transform.parent = this.gameObject.transform;
    }

    public bool IsRoomAreaValid(Room room, TileType[] avoidTiles)
    {
        for (int currX = room.x; currX < room.x + room.w; currX++)
        {
            for (int currY = room.y; currY < room.y + room.h; currY++)
            {
                Tile currTileObj = GetTile(currX, currY);

                if (currTileObj != null)
                {
                    foreach (TileType avoidTile in avoidTiles)
                    {
                        if (avoidTile == currTileObj.tileType)
                            return false;
                    }
                }
            }
        }

        return true;
    }

    public bool DoesAreaContainOnlyThisTile(int x, int y, int w, int h, TileType tileType)
    {
        for (int currX = x; currX < x + w; currX++)
        {
            for (int currY = y; currY < y + h; currY++)
            {
                Tile currTileObj = GetTile(currX, currY);

                if (currTileObj != null && currTileObj.tileType != tileType)
                    return false;
            }
        }

        return true;
    }

    /// TODO: this description was from legacy code, change it to fit this (somewhat
    ///       similar) code
    ///  
    /// <summary>
    /// Checks if a coordinate is one that exists within the boundaries of the map/
    /// </summary>
    /// TODO: this belongs in a seperate script as it is not directly tied to the
    ///       concept of GeneratingTiles
    private bool ValidTileSpace(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < h && y < h);
    }

    public Container GetRanContainer(ContainerType[] ignoreContainerTypes)
    {
        Container container;
        bool isInIgnoreRoomObjTypes;
        do
        {
            isInIgnoreRoomObjTypes = false;
            container = this.containers[Utilities.RandomIntInRange(0, containers.Count)];

            foreach (ContainerType containerType in ignoreContainerTypes)
            {
                if (containerType == container.containerType)
                    isInIgnoreRoomObjTypes = true;
            }
        }
        while (isInIgnoreRoomObjTypes);

        return container;
    }

    public void RemoveContainer(Container container)
    {
        this.containers.Remove(container);
    }

    public List<Room> GetRooms()
    {
        return containers.OfType<Room>().ToList<Room>();
    }

    // For each room, fill a list of all other rooms based on distance from
    // the given room.
    public void PopulateClosestOtherRooms()
    {
        foreach (Room room in GetRooms())
        {
            List<Pair<Room, float>> closestRoomsByDistance = new List<Pair<Room, float>>();

            foreach (Room compareToRoom in GetRooms())
            {
                if (compareToRoom == room)
                    continue;

                float dist = Vector2.Distance(new Vector2(room.x, room.y), new Vector2(compareToRoom.x, compareToRoom.y));

                closestRoomsByDistance.Add(new Pair<Room, float>(compareToRoom, dist));

            }

            closestRoomsByDistance = closestRoomsByDistance.OrderBy(i => i.Second).ToList();

            room.closestOtherRooms = closestRoomsByDistance.Select(i => i.First).ToList();
        }
    }

    public void OpenDoorArea(Door door, TileType tileType)
    {
        CreateTileAndSetTile(door.x, door.y, door.container, tileType);

        Pair<int, int> currLeftOfPair = new Pair<int, int>(door.x, door.y);
        Pair<int, int> currRightOfPair = new Pair<int, int>(door.x, door.y);
        Utilities.Direction leftOf = Utilities.LeftOf(door.direction);
        Utilities.Direction rightOf = Utilities.RightOf(door.direction);

        for (int i = 0; i < door.radius; i++)
        {
            currLeftOfPair = Utilities.CordInDirection(leftOf, currLeftOfPair);
            currRightOfPair = Utilities.CordInDirection(rightOf, currRightOfPair);

            CreateTileAndSetTile(currLeftOfPair, door.container, tileType);
            CreateTileAndSetTile(currRightOfPair, door.container, tileType);
        }
    }

    public void ChangeTilesInArea(int x, int y, int radius, TileType targetTileType, TileType newTileType, Container container)
    {
        for (int currX = x - radius; currX <= x + radius; currX++)
        {
            for (int currY = y - radius; currY <= y + radius; currY++)
            {
                if (GetTile(currX, currY) != null && GetTile(currX, currY).tileType == targetTileType)
                {
                    CreateTileAndSetTile(currX, currY, container, newTileType);
                }
            }
        }
    }

    public void ChangeTilesInArea(int x, int y, int radius, TileType newTileType, Container container)
    {
        for (int currX = x - radius; currX <= x + radius; currX++)
        {
            for (int currY = y - radius; currY <= y + radius; currY++)
            {
                if (GetTile(currX, currY) != null)
                {
                    CreateTileAndSetTile(currX, currY, container, newTileType);
                }
            }
        }
    }

    public void ConnectContainers(Container a, Container b)
    {
        a.connectedContainers.Add(b);
        b.connectedContainers.Add(a);
    }

    public bool AreContainersConnected(Container startingContainer, Container endingContainer)
    {
        return AreContainersConnected(startingContainer, endingContainer, new Dictionary<Container, bool>());
    }

    private bool AreContainersConnected(
        Container startingContainer,
        Container endingContainer,
        Dictionary<Container, bool> hasSeenContainer
    )
    {
        // This container has already been searched
        if (hasSeenContainer.ContainsKey(startingContainer))
            return false;

        // This container can no longer be searched again
        hasSeenContainer.Add(startingContainer, true);

        // We have found that these two containers do connect
        if (startingContainer == endingContainer)
            return true;

        foreach (Container nextContainer in startingContainer.connectedContainers)
        {
            if (AreContainersConnected(nextContainer, endingContainer, hasSeenContainer))
                return true;
        }

        return false;
    }

    public bool AreAllContainersConnected()
    {
        foreach (Container currContainer in containers)
        {
            //Containers contain themselves
            foreach (Container compareContainer in containers)
            {
                if (!AreContainersConnected(currContainer, compareContainer))
                {
                    return false;
                }
            }
        }
        return true;
    }



    /*
    public List<List<Container>> GetConnectedContainerGroups()
    {

    }
    */
}

