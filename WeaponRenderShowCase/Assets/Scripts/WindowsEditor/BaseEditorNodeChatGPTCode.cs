using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseEditorNodeChatGPTCode : EditorWindow
{

    private Vector2 nodeSize = new Vector2(100, 50);
    private Vector2 nodePosition = Vector2.zero;
    private List<Node> nodes = new List<Node>();

    [MenuItem("Window/Custom Editor Window")]
    static void ShowWindow()
    {
        GetWindow<BaseEditorNodeChatGPTCode>();
    }

    private void OnGUI()
    {
        // Dibuja el grid de fondo
        DrawGrid(20, 0.2f, Color.gray);

        // Dibuja los nodos
        DrawNodes();

        // Agrega un nodo con clic derecho
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 1)
        {
            Node newNode = new Node(nodePosition, nodeSize);
            nodes.Add(newNode);
        }
    }

    private void DrawNodes()
    {
        foreach (Node node in nodes)
        {
            Rect nodeRect = new Rect(node.position, node.size);
            GUI.Box(nodeRect, "Node");
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, 0f, 0f), new Vector3(gridSpacing * i, position.height, 0f));
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(0f, gridSpacing * j, 0f), new Vector3(position.width, gridSpacing * j, 0f));
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private class Node
    {
        public Vector2 position;
        public Vector2 size;

        public Node(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
    }
}
