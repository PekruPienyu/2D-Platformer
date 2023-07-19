using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class InteractableTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Coin, PowerUp, Star ---------//
    public List<GameObject> itemPrefabs = new();

    public List<Sprite> tileSprites = new();
    public GameObject debrisPrefab;
    public int mySpriteIndex;
    
    private int itemCount = 1;
    private int[] items;

    public bool containsItem;
    private bool isCoin;
    private bool isPowerUp;
    private bool isStar;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!containsItem) itemCount = 0;
        items = new int[itemCount];
        spriteRenderer.sprite = tileSprites[mySpriteIndex];
    }



    #region  // CustomEditor Hide unused variables in Inspector //
    [CustomEditor(typeof(InteractableTile)), CanEditMultipleObjects]
    public class MyScriptEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var myScript = target as InteractableTile;

            base.OnInspectorGUI();

            if (myScript.containsItem)
            {
                EditorGUI.indentLevel++;
                myScript.isCoin = EditorGUILayout.Toggle("Is Coin", myScript.isCoin);
                if(myScript.isCoin)
                {
                    EditorGUI.indentLevel++;
                    myScript.itemCount = EditorGUILayout.IntField("Coin Count", myScript.itemCount);
                    EditorGUI.indentLevel--;
                }
                myScript.isPowerUp = EditorGUILayout.Toggle("Is PowerUp", myScript.isPowerUp);
                myScript.isStar = EditorGUILayout.Toggle("Is Star", myScript.isStar);
                EditorGUI.indentLevel--;
            }
            else
            {
                myScript.isCoin = false;
                myScript.isPowerUp = false;
                myScript.isStar = false;
            }
        }
    }
    #endregion
}


