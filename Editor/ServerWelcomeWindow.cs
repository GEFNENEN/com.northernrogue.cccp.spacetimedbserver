using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

// Displays the welcome window with information about the asset and how to get started ///

namespace NorthernRogue.CCCP.Editor {
    
public class ServerWelcomeWindow : EditorWindow
{
    private const string WelcomeWindowShownKey = "ServerWelcomeWindow_WelcomeWindowShown";

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // Check if the welcome window has been shown before
        bool hasShown = EditorPrefs.GetBool(WelcomeWindowShownKey, false);
        
        if (!hasShown)
        {
            // Schedule opening the window after Unity is fully initialized
            EditorApplication.delayCall += () =>
            {
                ShowWindow();
                // Mark the window as shown
                EditorPrefs.SetBool(WelcomeWindowShownKey, true);
            };
        }
    }
    
    [MenuItem("SpacetimeDB/Welcome Window", priority = -10002)]
    public static void ShowWindow()
    {
        ServerWelcomeWindow window = GetWindow<ServerWelcomeWindow>("Welcome to Cosmos Cove");
        window.minSize = new Vector2(450f, 530f);
        window.maxSize = new Vector2(450f, 530f);
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        
        // Load and display the logo image
        Texture2D logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.northernrogue.cccp.spacetimedbserver/Editor/cosmos_logo.png");
        if (logoTexture != null)
        {
            float maxHeight = 70f;
            float aspectRatio = (float)logoTexture.width / logoTexture.height;
            float width = maxHeight * aspectRatio;
            float height = maxHeight;
            
            // Center the image
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(logoTexture, GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10);
        }
        else
        {
            // Fallback if image not found
            GUIStyle titleStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.fontSize = 18;
            GUILayout.Label("Cosmos Cove Control Panel", titleStyle);
            EditorGUILayout.Space(10);
        }
        
        // Welcome text
        GUIStyle textStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.fontSize = 12;
        
        string welcomeText = 
        "Cosmos Cove Control Panel is a Windows WSL integration of SpacetimeDB within Unity, allowing you to develop MMO experiences in Unity faster and easier than ever.\n\n" +
        "The menu item SpacetimeDB has been added to your toolbar.\n" +
        "There you will find all the main windows of Cosmos Cove Control Panel.\n\n" +
        "Please begin by opening the Server Installer Window to check if you have the pre-requisites installed and there you can also install them directly (alpha).\n\n" +
        "The main window is the Server Manager Panel. From there you can start your server and launch all other functionality by entering your server information in the pre-requisites.";
        
        EditorGUILayout.LabelField(welcomeText, textStyle);
        
        EditorGUILayout.Space(20);
        
        // Buttons
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Server Installer Window", GUILayout.Height(30), GUILayout.Width(200)))
        {
            ServerInstallerWindow.ShowWindow();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Server Manager Panel", GUILayout.Height(30), GUILayout.Width(200)))
        {
            ServerWindow.ShowWindow();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Documentation", GUILayout.Height(30), GUILayout.Width(200)))
        {
            Application.OpenURL(ServerWindow.Documentation);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        Type githubClass = Type.GetType("NorthernRogue.CCCP.Editor.Github");
        if (githubClass != null)
        {
            MethodInfo methodInfo = githubClass.GetMethod("WelcomeWindow");
            if (methodInfo != null)
            {
                EditorGUILayout.BeginVertical();
                methodInfo.Invoke(null, null);
                EditorGUILayout.EndVertical();
            }
        }
        
        EditorGUILayout.EndVertical();
    }
} // Class
} // Namespace

// made by Mathias Toivonen at Northern Rogue Games