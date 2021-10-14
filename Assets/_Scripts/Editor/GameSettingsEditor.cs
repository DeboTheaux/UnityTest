﻿using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    GameSettings t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;
    FigureSpawnProbability figureTarget;
    SerializedObject FigureGetTarget;
    SerializedProperty ThisFigure;

    void OnEnable()
    {
        t = (GameSettings)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("difficulty"); // Find the List in our script and create a refrence of it
    }

    public override void OnInspectorGUI()
    {
        //Update our list

        GetTarget.Update();
        EditorGUILayout.LabelField("Game Difficulties", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        ListSize = ThisList.arraySize;

        if (ListSize != ThisList.arraySize)
        {
            while (ListSize > ThisList.arraySize)
            {
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            }
            while (ListSize < ThisList.arraySize)
            {
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        }

        //Display our list to the inspector window

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            SerializedProperty listRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty difficultyName = listRef.FindPropertyRelative("name");
            SerializedProperty totalGameSeconds = listRef.FindPropertyRelative("totalGameSeconds");
            SerializedProperty spawnRateMin = listRef.FindPropertyRelative("spawnRateMin");
            SerializedProperty spawnRateMax = listRef.FindPropertyRelative("spawnRateMax");
            SerializedProperty listOfChances = listRef.FindPropertyRelative("chances");
           
            difficultyName.stringValue = EditorGUILayout.TextField("Difficulty Name", difficultyName.stringValue, GUILayout.MaxWidth(400));
            totalGameSeconds.floatValue = EditorGUILayout.FloatField("Total Game Seconds", totalGameSeconds.floatValue, GUILayout.MaxWidth(400));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Spawn Rate Min");
            spawnRateMin.floatValue = EditorGUILayout.Slider(spawnRateMin.floatValue, 1, totalGameSeconds.floatValue, GUILayout.MaxWidth(400));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Spawn Rate Max");
            spawnRateMax.floatValue = EditorGUILayout.Slider(spawnRateMax.floatValue, 1, totalGameSeconds.floatValue, GUILayout.MaxWidth(400));
            EditorGUILayout.EndHorizontal();

            // Array fields with remove at index
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Figure Spawn Probability", EditorStyles.boldLabel);

            //recorro los elementos del array 

            for (int a = 0; a < listOfChances.arraySize; a++)
            {

                SerializedProperty listChancesRef = listOfChances.GetArrayElementAtIndex(a);
                SerializedProperty figureId = listChancesRef.FindPropertyRelative("FigureId");
                SerializedProperty figureIdName = figureId.FindPropertyRelative("id");
                SerializedProperty probability = listChancesRef.FindPropertyRelative("probability");

                EditorGUILayout.BeginHorizontal();

                bool deleteButton = GUILayout.Button("-", GUILayout.MaxWidth(15), GUILayout.MaxHeight(15));                

                EditorGUILayout.LabelField($"{figureIdName.stringValue} Probability");

                EditorGUILayout.EndHorizontal();

                figureIdName.stringValue = EditorGUILayout.TextField("Figure Id", figureIdName.stringValue, GUILayout.MaxWidth(400));
                probability.floatValue = EditorGUILayout.FloatField("Probability", probability.floatValue, GUILayout.MaxWidth(400));

                if (deleteButton)
                {
                    listOfChances.DeleteArrayElementAtIndex(a);
                }

            }

            if (GUILayout.Button("Add new Figure Spawn Probability", GUILayout.MaxWidth(400), GUILayout.MaxHeight(20)))
            {
                //Al hacer click en el boton agreggo un elemento
                listOfChances.InsertArrayElementAtIndex(listOfChances.arraySize);
            }

            EditorGUILayout.Space();

            //Remove this index from the List
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button($"Remove '{difficultyName.stringValue}'", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20)))
            {
                ThisList.DeleteArrayElementAtIndex(i);
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add new Game Diffculty"))
        {
            t.difficulty.Add(new Difficulty());
        }


        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();
    }
}
