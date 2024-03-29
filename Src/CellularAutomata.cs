﻿using System;
using CellularAutomata.Events;
using CellularAutomata.Utils;
using CellularAutomata.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CellularAutomata;

public class CellularAutomata : Game
{
    protected GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch = null!;
    protected Camera Camera = null!;
    protected GameWorld GameWorld = null!;

    private bool _paused = true;

    private readonly int _worldWidth;
    private readonly int _worldHeight;
    private readonly int _cellSize;
    private readonly int _gridBorderSize;

    private Texture2D _worldTexture = null!;

    public static int UpdateDelay = 1;
        
    public CellularAutomata(int worldWidth, int worldHeight, int cellSize, int gridBorderSize = 0)
    {
        _worldWidth = worldWidth;
        _worldHeight = worldHeight;
        _cellSize = cellSize;
        _gridBorderSize = gridBorderSize;
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        GameWorld = new GameWorld(GraphicsDevice, _worldWidth, _worldHeight, _cellSize, _gridBorderSize);
        Camera = new Camera();

        var w = _worldWidth * _cellSize + _worldWidth * _gridBorderSize;
        var h = _worldHeight * _cellSize + _worldWidth * _gridBorderSize;
        _worldTexture = new Texture2D(GraphicsDevice, w, h);
        var colors = new Color[w*h];
        Array.Fill(colors, Color.White);
        _worldTexture.SetData(colors);
    }

    protected override void Update(GameTime gameTime)
    {
        if (!_paused)
        {
            GameWorld.PerformCellUpdate();
        }
        Camera.Controls();

        var ms = Mouse.GetState();
        if (ms.LeftButton == ButtonState.Pressed)
        {
            var mp = Camera.ScreenToWorldPos(ms.Position.ToVector2());
            GameWorld.GetCellPosFromWorldPos(mp, out var x, out var y);
            WorldEvents.OnRequestSpawnCell(x, y, GameWorld);
        } 
        else if (ms.RightButton == ButtonState.Pressed)
        {
            var mp = Camera.ScreenToWorldPos(ms.Position.ToVector2());
            GameWorld.GetCellPosFromWorldPos(mp, out var x, out var y);
            WorldEvents.OnRequestRemoveCell(x, y, GameWorld);
        }

        var ks = Keyboard.GetState();
        if (ks.IsKeyDown(Keys.Space))
        {
            _paused = !_paused;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(25, 25, 25, 255));
        SpriteBatch.Begin(transformMatrix: Camera.GetTransform());
        SpriteBatch.Draw(_worldTexture, new Vector2(0, 0), Color.Black);
        GameWorld.RenderWorld(SpriteBatch);
        SpriteBatch.End();
    }
}