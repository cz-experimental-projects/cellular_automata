﻿using System;
using CellularAutomata.Example;
using CellularAutomata.World;

namespace CellularAutomata.Events;

public static class WorldEvents
{
    public static event Action<int, int, GameWorld>? RequestSpawnCell;
    public static event Action<int, int, GameWorld>? RequestRemoveCell;

    static WorldEvents()
    {
        RequestSpawnCell += DefaultSpawnCellHandler;
        RequestRemoveCell += DefaultSpawnCellHandler;
    }
        
    public static void OnRequestSpawnCell(int x, int y, GameWorld world)
    {
        RequestSpawnCell?.Invoke(x, y, world);
    }

    public static void OnRequestRemoveCell(int x, int y, GameWorld world)
    {
        RequestRemoveCell?.Invoke(x, y, world);
    }

    public static void RemoveDefaultHandlers()
    {
        RequestRemoveCell -= DefaultRemoveCellHandler;
        RequestSpawnCell -= DefaultSpawnCellHandler;
    }
        
    private static void DefaultSpawnCellHandler(int x, int y, GameWorld world)
    {
        world.SetCell(x, y, new SandCell());
    }

    private static void DefaultRemoveCellHandler(int x, int y, GameWorld world)
    {
        world.SetCell(x, y, null);
    }
}