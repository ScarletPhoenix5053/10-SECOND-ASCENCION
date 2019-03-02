using UnityEngine;
using UnityEditor;
using Sierra.AGPW.TenSecondAscencion;

[CustomEditor(typeof(LevelConstructor))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var levelConstructor = target as LevelConstructor;

        if (GUILayout.Button("Edit Tiles"))
        {
            LevelEditorWindow window = (LevelEditorWindow)EditorWindow.GetWindow(typeof(LevelEditorWindow));

            var windowXSize = window.TileSize * levelConstructor.LevelSize.x + 14;
            window.minSize = new Vector2(windowXSize, 200);
            window.maxSize = new Vector2(windowXSize, 2000);
            window.SetLevelContstructorTo(levelConstructor);
            window.Show();
        }

        if (GUILayout.Button("Regenerate Level"))
        {
            levelConstructor.GenerateLevel();
            levelConstructor.GenerateLevel();
            levelConstructor.GenerateLevel();
            levelConstructor.GenerateLevel();
        }

        if (GUILayout.Button("Reset Tiles"))
        {
            levelConstructor.ResetTiles();
        }
    }
}
public class LevelEditorWindow : EditorWindow
{
    private GUIStyle _tileEditorStyle;
    private GUIStyle _tileEditorButtonStyle;
    private RectOffset _tileEditorSpacing;
    private const int _tileGridSize = 18;

    private Vector2 _scrollPos = Vector2.zero;

    public int TileSize { get { return _tileGridSize; } }

    private void Awake()
    {
        _tileEditorStyle = new GUIStyle();
        _tileEditorButtonStyle = new GUIStyle(GUI.skin.button);
        _tileEditorSpacing = new RectOffset(0, 0, 0, 0);

        _tileEditorStyle.margin = _tileEditorSpacing;
        _tileEditorButtonStyle.margin = _tileEditorSpacing;        
    }
    private void OnGUI()
    {
        GUILayout.Label("Tile Editor", EditorStyles.boldLabel);

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        GUILayout.BeginHorizontal(_tileEditorStyle);
        for (int x = 0; x < _levelConstructor.LevelSize.x; x++)
        {
            GUILayout.BeginVertical();
            for (int y = _levelConstructor.LevelSize.y-1; y >= 0; y--)
            {
                if (_levelConstructor.GetTile(x,y) == TileType.Terrain) GUI.backgroundColor = Color.red; 
                else GUI.backgroundColor = Color.white;

                if (GUILayout.Button("", _tileEditorButtonStyle, GUILayout.Width(_tileGridSize), GUILayout.Height(_tileGridSize)))
                {
                    if (_levelConstructor.GetTile(x, y) == TileType.Terrain)
                    {
                        _levelConstructor.SetTile(x, y, TileType.Empty);
                    }
                    else
                    {
                        _levelConstructor.SetTile(x, y, TileType.Terrain);
                    }
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();


        _levelConstructor.GenerateLevel();
        _levelConstructor.GenerateLevel();
        _levelConstructor.GenerateLevel();
        _levelConstructor.GenerateLevel();
    }

    private LevelConstructor _levelConstructor;

    public void SetLevelContstructorTo(LevelConstructor levelConstructor)
    {
        _levelConstructor = levelConstructor;
    }
}