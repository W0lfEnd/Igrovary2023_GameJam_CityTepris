using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFigureData
{
    public enum Type
    {
        NONE = 0,

        L     = 1,
        FIRST = L,
        J = 2,
        O = 3,
        I = 4, 
        S = 5,
        Z = 6,
        
        AFTER_LAST
    }


    public Guid            guid       { get; private set; } = Guid.NewGuid();
    public Type            figureType { get; private set; } = Type.NONE;
    public List<List<int>> shape      { get; private set; } = new List<List<int>>();


    public MapFigureData( Type type = Type.NONE )
    {
        figureType = type;
        shape =getShapeByType( type );
    }

    private List<List<int>> getShapeByType( Type type )
    {
        switch ( type )
        {
        case Type.L:    return new List<List<int>>
        {
           new() { 1, 1, 1 }, //1, 0, 0
           new() { 1, 0, 0 }, //1, 0, 0
           new() { 0, 0, 0 }  //1, 1, 0
        };
        case Type.J:    return new List<List<int>>
        {
           new() { 1, 0, 0 }, //0, 1, 0
           new() { 1, 1, 1 }, //0, 1, 0
           new() { 0, 0, 0 }, //1, 1, 0
        };
        case Type.O:    return new List<List<int>>
        {
           new() { 1, 1, 0 }, //0, 0, 0
           new() { 1, 1, 0 }, //1, 1, 0
           new() { 0, 0, 0 }, //1, 1, 0
        };
        case Type.I:    return new List<List<int>>
        {
           new() { 1, 1, 1 }, //1, 0, 0
           new() { 0, 0, 0 }, //1, 0, 0
           new() { 0, 0, 0 }, //1, 0, 0
        };
        case Type.S:    return new List<List<int>>
        {
           new() { 1, 0, 0 }, //0, 0, 0
           new() { 1, 1, 0 }, //0, 1, 1
           new() { 0, 1, 0 }, //1, 1, 0
        };
        case Type.Z:    return new List<List<int>>
        {
           new() { 0, 1, 0 }, //0, 0, 0
           new() { 1, 1, 0 }, //1, 1, 0
           new() { 1, 0, 0 }, //0, 1, 1
        };

        default:        return new List<List<int>>
        {
            new() { 0, 0, 0 },
            new() { 0, 0, 0 },
            new() { 0, 0, 0 }
        };
        }
    }
}
