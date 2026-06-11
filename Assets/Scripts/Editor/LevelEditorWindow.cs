using UnityEditor;
using UnityEngine;
using System.Linq;

public class LevelEditorWindow : EditorWindow
{
    public LevelData currentLevel;
    public ObjectCatalog catalog;

    private CatalogEntry selectedCatalogEntry;
    private TileLayer activeLayer = TileLayer.OBJECT;
    private LevelObjectData selectedObject;
    private int objectCounter = 0;
    private Vector2 currentPlayerPosition;
    private LevelObjectData playerData;

    private int selectedTab = 0; 

    private float leftWidth = 400f;
    private float centerWidth = 1000f;
    private float rightWidth = 400f;

    private Rect leftRect, centerRect, rightRect;
    private Rect splitterLeft, splitterRight;
    private bool resizingLeft, resizingRight;

    private Vector2 centerWindowScrollPosition;

    private Vector2 prefabsScrollPosition;
    private int prefabButtonSize = 64;
    private int prefabButtonAmount = 4;

    public enum BoardViewMode {CURRENT_LAYER, ALL_LAYERS}
    private BoardViewMode viewMode = BoardViewMode.CURRENT_LAYER;


    [MenuItem("Tools/Level Editor")]
    public static LevelEditorWindow ShowWindow()
    {
        return GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnEnable() 
    {
        if (catalog == null)
        {
            string[] guids = AssetDatabase.FindAssets("t:ObjectCatalog");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                catalog = AssetDatabase.LoadAssetAtPath<ObjectCatalog>(path);
            }
        }
    }

    private void OnGUI()
    {
        float totalWidth = position.width;
        float height = position.height;

        leftRect = new Rect(0, 0, leftWidth, height);
        centerRect = new Rect(leftWidth, 0, centerWidth, height);
        rightRect = new Rect(leftWidth + centerWidth, 0, rightWidth, height);

        splitterLeft = new Rect(leftRect.xMax - 2, 0, 4, height);
        splitterRight = new Rect(centerRect.xMax - 2, 0, 4, height);

        GUILayout.BeginArea(leftRect, EditorStyles.helpBox);
        DrawLeftSection();
        GUILayout.EndArea();

        GUILayout.BeginArea(centerRect, EditorStyles.helpBox);
        DrawCenterSection();
        GUILayout.EndArea();

        GUILayout.BeginArea(rightRect, EditorStyles.helpBox);
        DrawRightSection();
        GUILayout.EndArea();

        HandleResize(totalWidth);
    }

    public void LoadLevelData(LevelData levelData)
    {
        currentLevel = levelData;
        if (currentLevel.levelObjects.Any(o => o.objectId == "Player"))
        {
            playerData = currentLevel.levelObjects.First(o => o.objectId == "Player");
            currentPlayerPosition = playerData.gridPosition;
        }
    }

    private void DrawLeftSection()
    {
        EditorGUILayout.LabelField("Herramientas", EditorStyles.boldLabel);

        catalog = (ObjectCatalog)EditorGUILayout.ObjectField("Catalogo de objetos", catalog, typeof(ObjectCatalog), false);

        EditorGUILayout.LabelField("Nivel", EditorStyles.boldLabel);

        if (GUILayout.Button("Nuevo Nivel"))
        {
            string path = EditorUtility.SaveFilePanelInProject("Crear Nuevo Nivel", "NewLevelData", "asset", "Selecciona ubicación para el nuevo nivel");
            if (!string.IsNullOrEmpty(path))
            {
                var newLevel = ScriptableObject.CreateInstance<LevelData>();
                AssetDatabase.CreateAsset(newLevel, path);
                AssetDatabase.SaveAssets();
                currentLevel = newLevel;
            }
        }

        if (GUILayout.Button("Cargar Nivel"))
        {
            string path = EditorUtility.OpenFilePanel("Cargar Nivel", "Assets", "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = FileUtil.GetProjectRelativePath(path);
                currentLevel = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                if (currentLevel.levelObjects.Any(o => o.objectId == "Player"))
                {
                    playerData = currentLevel.levelObjects.First(o => o.objectId == "Player");
                    currentPlayerPosition = playerData.gridPosition;
                }
            }
        }

        EditorGUILayout.Space();

        string[] tabs = { "Parámetros del Nivel", "Modo Edición" };
        selectedTab = GUILayout.Toolbar(selectedTab, tabs);

        EditorGUILayout.Space();

        if (currentLevel == null) return;

        if (selectedTab == 0)
        {
            EditorGUILayout.LabelField("Map Settings", EditorStyles.boldLabel);
            currentLevel.width = EditorGUILayout.IntField("Width", currentLevel.width);
            currentLevel.height = EditorGUILayout.IntField("Height", currentLevel.height);
            currentLevel.tileSize = EditorGUILayout.FloatField("Tile Size", currentLevel.tileSize);
            currentLevel.origin = EditorGUILayout.Vector3Field("Origin", currentLevel.origin);
        }
        else
        {
            EditorGUILayout.LabelField("Ajustes del jugador", EditorStyles.boldLabel);
            currentLevel.playerSpawnPosition = EditorGUILayout.Vector2IntField("Posición", currentLevel.playerSpawnPosition);
            GUILayout.Space(10);

            if (catalog != null && catalog.entries.Count > 0)
            {
                if (selectedCatalogEntry == null)
                    selectedCatalogEntry = catalog.entries[0];

                EditorGUILayout.LabelField("Prefabs disponibles:");

                prefabsScrollPosition = EditorGUILayout.BeginScrollView(prefabsScrollPosition, GUILayout.Height(200));
                EditorGUILayout.BeginHorizontal();
                int count = 0;

                foreach (var entry in catalog.entries)
                {
                    Texture2D preview = entry.previewSprite != null ? entry.previewSprite.texture : null;

                    if (preview != null)
                    {
                        if (GUILayout.Button(preview, GUILayout.Width(prefabButtonSize), GUILayout.Height(prefabButtonSize)))
                            selectedCatalogEntry = entry;
                    }
                    else
                    {
                        if (GUILayout.Button(entry.prefab.name, GUILayout.Width(prefabButtonSize), GUILayout.Height(prefabButtonSize)))
                            selectedCatalogEntry = entry;
                    }

                    count++;
                    if (count % prefabButtonAmount == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
            }

            viewMode = (BoardViewMode)EditorGUILayout.EnumPopup("Modo de vista", viewMode);
            activeLayer = (TileLayer)EditorGUILayout.EnumPopup("Capa actual", activeLayer);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Guardar Nivel"))
        {
            EditorUtility.SetDirty(currentLevel);
            AssetDatabase.SaveAssets();
        }
    }

    private void DrawCenterSection()
    {
        if (currentLevel == null) return;

        EditorGUILayout.LabelField("Grilla de edición", EditorStyles.boldLabel);

        if (selectedCatalogEntry != null)
        {
            if (selectedCatalogEntry.previewSprite != null)
            {
                GUILayout.Label(selectedCatalogEntry.previewSprite.texture, GUILayout.Width(64), GUILayout.Height(64));
            }
            else
            {
                GUILayout.Label("Seleccionado: " + selectedCatalogEntry.prefab.name);
            }
        }

        centerWindowScrollPosition = EditorGUILayout.BeginScrollView(centerWindowScrollPosition);

        int cellSize = 50;
        int rowLabelWidth = 30;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(rowLabelWidth);
        for (int x = 0; x < currentLevel.width; x++)
        {
            GUILayout.Label(x.ToString(), GUILayout.Width(cellSize), GUILayout.Height(20));
        }
        EditorGUILayout.EndHorizontal();

        for (int y = currentLevel.height - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(y.ToString(), GUILayout.Width(rowLabelWidth), GUILayout.Height(cellSize));

            for (int x = 0; x < currentLevel.width; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Rect tileRect = GUILayoutUtility.GetRect(cellSize, cellSize, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                bool isPlayerSpawn = gridPos == currentPlayerPosition;

                if (playerData == null)
                {
                    PlacePlayer(currentLevel.playerSpawnPosition);
                }
                else if (currentPlayerPosition != currentLevel.playerSpawnPosition)
                {
                    currentLevel.levelObjects.RemoveAll(o => o.objectId == "Player");
                    PlacePlayer(currentLevel.playerSpawnPosition);
                }

                LevelObjectData obj = null;

                if (viewMode == BoardViewMode.CURRENT_LAYER)
                {
                    obj = currentLevel.levelObjects.Find(o => o.gridPosition == gridPos && o.layer == activeLayer);
                }
                else if (viewMode == BoardViewMode.ALL_LAYERS)
                {
                    for (int l = (int)TileLayer.ENTITY; l >= (int)TileLayer.BASE_TILE; l--)
                    {
                        obj = currentLevel.levelObjects.Find(o => o.gridPosition == gridPos && (int)o.layer == l);
                        if (obj != null) break;
                    }
                }

                if (obj != null)
                {
                    if (isPlayerSpawn)
                    {
                        if (catalog.playerEntry.previewSprite != null)
                        {
                            GUI.Box(tileRect, "", GUI.skin.button);
                            GUI.DrawTexture(tileRect, catalog.playerEntry.previewSprite.texture, ScaleMode.ScaleToFit);
                        }
                        else
                        {
                            GUI.Box(tileRect, catalog.playerEntry.prefab.name, GUI.skin.button);
                        }
                    }
                    else
                    {
                        var entry = catalog.entries.FirstOrDefault(e => e.prefab == obj.prefab);
                        if (entry != null && entry.previewSprite != null)
                        {
                            GUI.Box(tileRect, "", GUI.skin.button);
                            GUI.DrawTexture(tileRect, entry.previewSprite.texture, ScaleMode.ScaleToFit);
                        }
                        else
                        {
                            GUI.Box(tileRect, obj.objectId, GUI.skin.button);
                        }
                    }
                }
                else
                {
                    GUI.Box(tileRect, "", GUI.skin.button);
                }

                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && tileRect.Contains(Event.current.mousePosition))
                {

                    if (viewMode == BoardViewMode.CURRENT_LAYER)
                    {

                        if (obj != null)
                            selectedObject = obj;
                        else if (selectedCatalogEntry != null && !isPlayerSpawn)
                            PlaceObject(gridPos);
                    }
                    else
                    {
                        var objectsAtPos = currentLevel.levelObjects.FirstOrDefault(o => o.layer == activeLayer && o.gridPosition == gridPos);
                        if (objectsAtPos != null)
                            selectedObject = objectsAtPos;
                        else if (selectedCatalogEntry != null && !isPlayerSpawn)
                            PlaceObject(gridPos);
                    }
                    Event.current.Use();
                }

                if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && tileRect.Contains(Event.current.mousePosition))
                {
                    if (activeLayer == TileLayer.OBJECT && isPlayerSpawn)
                        continue;

                    if (viewMode == BoardViewMode.CURRENT_LAYER)
                    {
                        if (obj != null)
                        {
                            currentLevel.levelObjects.Remove(obj);
                            if (selectedObject == obj) selectedObject = null;
                            Event.current.Use();
                        }
                    }
                    else
                    {
                        var objectsAtPos = currentLevel.levelObjects.FirstOrDefault(o => o.layer == activeLayer && o.gridPosition == gridPos);
                        if (objectsAtPos != null)
                        {
                            currentLevel.levelObjects.Remove(objectsAtPos);
                            if (selectedObject == objectsAtPos) selectedObject = null;
                            Event.current.Use();
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawRightSection()
    {
        if (selectedObject != null)
        {
            EditorGUILayout.LabelField("Inspector de Objeto", EditorStyles.boldLabel);
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID:", selectedObject.objectId);
            if (GUILayout.Button("Copiar ID", GUILayout.Width(70)))
            {
                EditorGUIUtility.systemCopyBuffer = selectedObject.objectId;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Prefab:", selectedObject.prefab.name);
            EditorGUILayout.LabelField("Posición:", $"{selectedObject.gridPosition.x}, {selectedObject.gridPosition.y}");
            EditorGUILayout.LabelField("Capa:", selectedObject.layer.ToString());
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Dirección", EditorStyles.boldLabel);
            selectedObject.orientation = (Direction)EditorGUILayout.EnumPopup("Dirección", selectedObject.orientation);
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Movimiento", EditorStyles.boldLabel);
            selectedObject.parameters.colliderLevel = (ColliderLevel)EditorGUILayout.EnumPopup("Nivel de colisión", selectedObject.parameters.colliderLevel);
            selectedObject.parameters.movableType = (MovableType)EditorGUILayout.EnumPopup("Movimiento", selectedObject.parameters.movableType);
            if (selectedObject.parameters.movableType == MovableType.MOVABLE)
                selectedObject.parameters.movementType = (MovementType)EditorGUILayout.EnumPopup("Nivel de movimiento", selectedObject.parameters.movementType);
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Comportamiento de luz", EditorStyles.boldLabel);
            selectedObject.parameters.lightBlockType = (LightBlockType)EditorGUILayout.EnumPopup("Bloqueo de luz", selectedObject.parameters.lightBlockType);
            selectedObject.parameters.lightShape = (LightShape)EditorGUILayout.EnumPopup("Forma de luz", selectedObject.parameters.lightShape);
            if (selectedObject.parameters.lightShape != LightShape.NONE)
                selectedObject.parameters.lightRange = EditorGUILayout.IntField("Rango de luz", selectedObject.parameters.lightRange);
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Interacción", EditorStyles.boldLabel);
            selectedObject.parameters.interactionType = (InteractionType)EditorGUILayout.EnumPopup("Tipo de interacción", selectedObject.parameters.interactionType);
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Activación", EditorStyles.boldLabel);
            selectedObject.parameters.activationType = (ActivationType)EditorGUILayout.EnumPopup("Tipo de activación", selectedObject.parameters.activationType);
            selectedObject.parameters.isPowered = EditorGUILayout.Toggle("Es fuente de energia", selectedObject.parameters.isPowered);

            for (int i = 0; i < selectedObject.parameters.linkedElements.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                selectedObject.parameters.linkedElements[i] = EditorGUILayout.TextField($"Element {i}", selectedObject.parameters.linkedElements[i]);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    selectedObject.parameters.linkedElements.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Agregar Elemento"))
            {
                selectedObject.parameters.linkedElements.Add("");
            }

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Estados", EditorStyles.boldLabel);
            selectedObject.parameters.hasStates = EditorGUILayout.Toggle("Tiene estados", selectedObject.parameters.hasStates);
            selectedObject.parameters.initialState = EditorGUILayout.Toggle("Estado inicial", selectedObject.parameters.initialState);
            GUILayout.Space(5);
        }
        else
        {
            EditorGUILayout.LabelField("No hay objeto seleccionado.");
        }
    }

    private void HandleResize(float totalWidth)
    {
        EditorGUIUtility.AddCursorRect(splitterLeft, MouseCursor.ResizeHorizontal);
        EditorGUIUtility.AddCursorRect(splitterRight, MouseCursor.ResizeHorizontal);

        if (Event.current.type == EventType.MouseDown && splitterLeft.Contains(Event.current.mousePosition))
            resizingLeft = true;
        if (Event.current.type == EventType.MouseDown && splitterRight.Contains(Event.current.mousePosition))
            resizingRight = true;

        if (Event.current.type == EventType.MouseDrag && resizingLeft)
        {
            leftWidth = Mathf.Clamp(Event.current.mousePosition.x, 150, totalWidth - rightWidth - 150);
            centerWidth = totalWidth - leftWidth - rightWidth;
            Repaint();
        }
        if (Event.current.type == EventType.MouseDrag && resizingRight)
        {
            rightWidth = Mathf.Clamp(totalWidth - Event.current.mousePosition.x, 150, totalWidth - leftWidth - 150);
            centerWidth = totalWidth - leftWidth - rightWidth;
            Repaint();
        }

        if (Event.current.type == EventType.MouseUp)
        {
            resizingLeft = false;
            resizingRight = false;
        }
    }

    private void PlacePlayer(Vector2Int gridPos)
    {
        currentLevel.levelObjects.RemoveAll(o => o.gridPosition == gridPos && o.layer == TileLayer.OBJECT);

        LevelObjectData objData = new LevelObjectData
        {
            objectId = $"Player",
            prefab = catalog.playerEntry.prefab,
            gridPosition = gridPos,
            layer = TileLayer.OBJECT,
            orientation = Direction.NORTH,
            parameters = catalog.playerEntry.defaultParameters
        };
        playerData = objData;
        currentPlayerPosition = gridPos;
        PlaceObject(gridPos, objData);
    }

    private void PlaceObject(Vector2Int gridPos)
    {
        LevelObjectData objData = new LevelObjectData
        {
            objectId = $"{selectedCatalogEntry.prefab.name}_{objectCounter}", // ID único
            prefab = selectedCatalogEntry.prefab,
            gridPosition = gridPos,
            layer = activeLayer,
            orientation = Direction.NORTH,
            parameters = selectedCatalogEntry.defaultParameters
        };

        PlaceObject(gridPos, objData);
    }

    private void PlaceObject(Vector2Int gridPos, LevelObjectData data)
    {
        if (currentLevel.levelObjects.Exists(o => o.gridPosition == gridPos && o.layer == activeLayer))
            return;

        objectCounter++;
        currentLevel.levelObjects.Add(data);
        selectedObject = data;
    }
}