﻿using System.Collections;
using CellularAutomata.Cells;
using CellularAutomata.World;
using Microsoft.Xna.Framework;

namespace CellularAutomata.Example;

// ReSharper disable once InconsistentNaming
public class SandCell : Cell
{
    public override IEnumerator Update(GameWorld world, int thisX, int thisY)
    {
        yield return base.Update(world, thisX, thisY);
        if (world.TryMove(this, thisX, thisY, thisX, thisY + 1)) yield break;
        if (world.TryMove(this, thisX, thisY, thisX - 1, thisY + 1)) yield break;
        world.TryMove(this, thisX, thisY, thisX + 1, thisY + 1);
    }

    public override Color CellColor()
    {
        return Color.Yellow;
    }

    public override Cell Clone()
    {
        return new SandCell();
    }
}