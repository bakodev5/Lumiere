﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenAlgo
{
    protected Map map;

    public GenAlgo(Map map)
    {
        this.map = map;
    }

    public abstract void GenerateMap(Map map);

}
