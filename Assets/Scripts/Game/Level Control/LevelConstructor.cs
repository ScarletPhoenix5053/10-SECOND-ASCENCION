using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class LevelConstructor : MonoBehaviour
    {
        protected void Awake()
        {
            // Initialize tilespace
            _gameTiles = new TileType[_levelSizeX, _levelSizeY];
            _gameTiles[5, 12] = TileType.Terrain; _gameTiles[6, 12] = TileType.Terrain; _gameTiles[7, 12] = TileType.Terrain;
            _gameTiles[5, 13] = TileType.Terrain; _gameTiles[6, 13] = TileType.Terrain; _gameTiles[7, 13] = TileType.Terrain;
            _gameTiles[5, 14] = TileType.Terrain; _gameTiles[6, 14] = TileType.Terrain; _gameTiles[7, 14] = TileType.Terrain; _gameTiles[8, 14] = TileType.Terrain; _gameTiles[9, 14] = TileType.Terrain;
            _gameTiles[5, 15] = TileType.Terrain; _gameTiles[6, 15] = TileType.Terrain; _gameTiles[7, 15] = TileType.Terrain;

            GenerateLevel();
        }

        [SerializeField]
        private int _levelSizeX = 18;
        [SerializeField]
        private int _levelSizeY = 40;
        [SerializeField]
        private Transform _instanceParent;
        private TileType[,] _gameTiles;        

        [SerializeField]
        private LevelTileSet TileSet = new LevelTileSet();

        public void SetTile(int x, int y, TileType newType)
        {
            // Ensure x and y coordinates are valid
            if (x < 0 || x >= _levelSizeX) throw new ArgumentException("Value must be less than horzontal level size", "x");
            if (y < 0 || y >= _levelSizeY) throw new ArgumentException("Value must be less than vertical level size", "y");

            // Update tile
            _gameTiles[x, y] = newType;
        }
        public void GenerateLevel()
        {
            // Ensure necessary variables are valid
            if (_instanceParent == null) throw new MissingReferenceException("Please assign a reference to " + name + "'s Instance Parent");
            if (_gameTiles == null || _gameTiles.Length == 0) throw new MissingReferenceException("Please ensure " + name + "'s GameTiles are initilaized");
            
            var halfWidth = _levelSizeX / 2f;

            // Iterate through tiles and spawn relevant prefabs according to tileset rules
            for (int x = 0; x < _levelSizeX; x++)
            {
                for (int y = 0; y < _levelSizeY; y++)
                {
                    switch (_gameTiles[x,y])
                    {
                        case TileType.Empty:
                            // Air: do nothing
                            break;

                        case TileType.Terrain:
                            // Empty above...
                            if (_gameTiles[x, y + 1] == TileType.Empty)
                            {
                                // ... & Empty on both sides: spawn double corner tile
                                if (_gameTiles[x - 1, y] == TileType.Empty && _gameTiles[x + 1, y] == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, TileSet.DoubleCorner);

                                // ... & Empty to left: spawn left corner tile
                                if (_gameTiles[x-1, y] == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, TileSet.LeftCorner);

                                // ... & Empty to right: spawn right corner tile
                                else if (_gameTiles[x+1, y] == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y +0.5f, TileSet.RightCorner);

                                // ... : spawn ground tile
                                else GenerateTile(x - halfWidth + 0.5f, y + +0.5f, TileSet.Ground);
                            }

                            // Otherwise must be...
                            else
                            {
                                // ... & Empty on both sides: spawn double wall tile
                                if (_gameTiles[x - 1, y] == TileType.Empty && _gameTiles[x + 1, y] == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, TileSet.DoubleWall);

                                // ... & Empty to left: spawn left wall tile
                                if (_gameTiles[x - 1, y] == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, TileSet.LeftWall);

                                // ... & Empty to right: spawn right wall tile
                                else if (_gameTiles[x + 1, y] == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + +0.5f, TileSet.RightWall);

                                // ... Surrounded or roof tile: spawn default tile
                                else GenerateTile(x - halfWidth + 0.5f, y + +0.5f, TileSet.Wall);
                            }

                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void GenerateTile(float x, float y, GameObject tilePrefab)
        {
            Instantiate(
                tilePrefab,
                new Vector2(x, y),
                Quaternion.identity,
                _instanceParent
                );
        }
    }
    [Serializable]
    public struct LevelTileSet
    {
        public GameObject Wall;
        public GameObject Ground;
        public GameObject LeftCorner;
        public GameObject RightCorner;
        public GameObject DoubleCorner;
        public GameObject LeftWall;
        public GameObject RightWall;
        public GameObject DoubleWall;
    }
    public enum TileType
    {
        Empty,
        Terrain
    }
}