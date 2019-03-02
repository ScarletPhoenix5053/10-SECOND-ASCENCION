using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class LevelConstructor : MonoBehaviour
    {
        protected void Reset()
        {
            Debug.Log("Don't even fuck with me unity");
        }
        protected void Awake()
        {
            // Initialize tilespace
            //ResetTiles();

            //GenerateLevel();
        }

        [SerializeField]
        private int _levelSizeX = 18;
        [SerializeField]
        private int _levelSizeY = 40;
        [SerializeField]
        private Transform _instanceParent;
        private TileType[,] _gameTiles;
        [SerializeField]
        private int _maxScalableEdgeHeight = 2;

        [SerializeField]
        private LevelPreset _preset;
        [SerializeField]
        private LevelTileSet _tileSet = new LevelTileSet();

        public Vector2Int LevelSize { get { return new Vector2Int(_levelSizeX, _levelSizeY); } }

        public TileType GetTile(int x, int y)
        {
            // Ensure scriptable object is in place
            if (_preset == null) throw new MissingReferenceException(name + " needs a LevelPreset reference to work.");

            // Ensure x and y coordinates are valid            
            if (x < 0 || x >= _levelSizeX) return TileType.Terrain; // throw new ArgumentException("Value must be less than horzontal level size", "x");
            if (y < 0 || y >= _levelSizeY) return TileType.Terrain; // throw new ArgumentException("Value must be less than vertical level size", "y");

            // Ensure there are tiles to use
            if (_preset.Tiles == null) ResetTiles();

            // Return tile
            return _preset.Tiles[x, y];
        }
        public void SetTile(int x, int y, TileType newType)
        {
            // Ensure scriptable object is in place
            if (_preset == null) throw new MissingReferenceException(name + " needs a LevelPreset reference to work.");

            // Ensure x and y coordinates are valid
            if (x < 0 || x >= _levelSizeX) throw new ArgumentException("Value must be less than horzontal level size", "x");
            if (y < 0 || y >= _levelSizeY) throw new ArgumentException("Value must be less than vertical level size", "y");

            // Update tile
            _preset.Tiles[x, y] = newType;
        }
        public void GenerateLevel()
        {
            // Ensure necessary variables are valid
            if (_instanceParent == null) throw new MissingReferenceException("Please assign a reference to " + name + "'s Instance Parent");
            if (_preset.Tiles == null || _preset.Tiles.Length == 0) throw new MissingReferenceException("Please ensure " + name + "'s GameTiles are initilaized");
            
            var halfWidth = _levelSizeX / 2f;

            // Clear transform
            for (int i = 0; i < _instanceParent.childCount; i++)
            {
                DestroyImmediate(_instanceParent.GetChild(i).gameObject);
            }

            // Iterate through tiles and spawn relevant prefabs according to tileset rules
            for (int x = 0; x < _levelSizeX; x++)
            {
                for (int y = 0; y < _levelSizeY; y++)
                {
                    switch (GetTile(x,y))
                    {
                        /* !!!IMPORTANT!!!
                         * This method/algorythm/ruleset - WHATEVER - is not optimized for ideal performance and contains bugs.
                         * 
                         * 1)   A scalable tile with air on both sides will generate as a double wall, even if one
                         *      side is underneath a solid tile, creating a scalable wall where one shouldn't exist.
                         *      
                         * 2)   I'm 100% sure there's a more effecient iteration pattern than this.
                         */

                        case TileType.Empty:
                            // Air: do nothing
                            break;

                        case TileType.Terrain:
                            // Empty above...
                            if (GetTile(x, y + 1) == TileType.Empty)
                            {
                                // ... & Empty on both sides: spawn double corner tile
                                if (GetTile(x - 1, y) == TileType.Empty && GetTile(x + 1, y) == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, _tileSet.DoubleCorner);
                                
                                // ... & Empty to left: spawn left corner tile
                                else if(GetTile(x - 1, y) == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, _tileSet.LeftCorner);

                                // ... & Empty to right: spawn right corner tile
                                else if (GetTile(x + 1, y) == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, _tileSet.RightCorner);

                                // ... : spawn ground tile
                                else GenerateTile(x - halfWidth + 0.5f, y + +0.5f, _tileSet.Ground);
                            }

                            // Is scalable... (at most a certain number of tiles beneath a corner)
                            else if (TileIsScalable(x, y))
                            {
                                // ... & Empty on both sides: spawn double wall tile
                                if (GetTile(x - 1, y) == TileType.Empty && GetTile(x + 1, y) == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, _tileSet.DoubleWall);

                                // ... & Empty to left: spawn left wall tile
                                else if(GetTile(x - 1, y) == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + 0.5f, _tileSet.LeftWall);

                                // ... & Empty to right: spawn right wall tile
                                else if (GetTile(x + 1, y) == TileType.Empty) GenerateTile(x - halfWidth + 0.5f, y + +0.5f, _tileSet.RightWall);

                                // ... Surrounded or roof tile: spawn default tile
                                else GenerateTile(x - halfWidth + 0.5f, y + +0.5f, _tileSet.Wall);
                            }

                            // Must be surrounded or non-scalable: spawn default tile
                            else GenerateTile(x - halfWidth + 0.5f, y + +0.5f, _tileSet.Wall);

                            break;

                        default:
                            break;
                    }
                }
            }
        }
        public void ResetTiles()
        {
            _preset.Tiles = new TileType[_levelSizeX, _levelSizeY];
            GenerateLevel();
            GenerateLevel();
            GenerateLevel();
            GenerateLevel();
        }

        private bool TileIsScalable(int x, int y)
        {
            var cornerInRange = false;
            var isScalable = false;
            var distBelowCorner = 0;

            // Iterate upwards and check each tile to see if it is a corner
            for (int i = 1; i <= _maxScalableEdgeHeight; i++)
            {
                if (GetTile(x, y + i + 1) == TileType.Empty &&
                    (GetTile(x - 1, y + i) == TileType.Empty || GetTile(x + 1, y + i) == TileType.Empty))
                {
                    cornerInRange = true;
                    distBelowCorner = i;
                    break;
                }
            }

            if (cornerInRange)
                // Iterate upwards and check each tile to see if it is a wall
                for (int i = 1; i <= distBelowCorner; i++)
                {
                    if (GetTile(x - 1, y + i) == TileType.Empty || GetTile(x + 1, y + i) == TileType.Empty)
                    {
                        if (i == distBelowCorner) isScalable = true;
                        else continue;
                    }
                    else break;
                }

            return isScalable;
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