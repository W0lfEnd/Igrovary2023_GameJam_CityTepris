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


    public Guid   guid       { get; private set; } = Guid.NewGuid();
    public Type   figureType { get; private set; } = Type.NONE;
    public int[,] shape      { get; private set; } = new int[3,3];


    public MapFigureData( Type type = Type.NONE )
    {
        figureType = type;
        shape = getShapeByType( type );
    }

    private int[,] getShapeByType( Type type )
    {
        switch ( type )
        {
        case Type.L:    return new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 1 }
        };
        case Type.J:    return new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 1, 0 }
        };
        case Type.O:    return new int[,]
        {
            { 0, 0, 0 },
            { 0, 1, 1 },
            { 0, 1, 1 }
        };
        case Type.I:    return new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        };
        case Type.S:    return new int[,]
        {
            { 0, 0, 0 },
            { 0, 1, 1 },
            { 1, 1, 0 }
        };
        case Type.Z:    return new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 0 },
            { 0, 1, 1 }
        };

        default:        return new int[3,3];
        }
    }
}
