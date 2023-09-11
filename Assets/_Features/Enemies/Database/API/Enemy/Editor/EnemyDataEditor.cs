using Codice.CM.Client.Differences.Graphic;
using Enemies.Instance.API.Movement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEditor.Sprites;

namespace Enemies.Database.API.Enemy.Editor
{
    [CustomEditor(typeof(EnemyData))]
    [CanEditMultipleObjects]
    public class EnemyDataEditor : UnityEditor.Editor
    {
        private class EnemyMovementIntKeyAndName
        {
            public readonly int SelectionKey;
            public readonly string Name;

            public EnemyMovementIntKeyAndName(int selectionKey, string name)
            {
                SelectionKey = selectionKey;
                Name = name;
            }
        }

        private EnemyData m_EnemyData;

        private Dictionary<Type, EnemyMovementIntKeyAndName> m_AvailableEnemyMovementTypes;

        private static int s_SelectedDataType;

        private void OnEnable()
        {
            m_EnemyData = serializedObject.targetObject as EnemyData;
            getEnemyMovementTypes();
        }

        private void getEnemyMovementTypes()
        {
            m_AvailableEnemyMovementTypes = new Dictionary<Type, EnemyMovementIntKeyAndName>();
            int i = 0;
            Type[] enemyMovementTypes = typeof(EnemyMovement).Assembly.GetTypes();
            foreach (Type enemyMovementType in enemyMovementTypes)
            {
                if (typeof(EnemyMovement).IsAssignableFrom(enemyMovementType)
                    && enemyMovementType.IsInterface == false
                    && enemyMovementType.IsAbstract == false
                    && enemyMovementType != typeof(EnemyMovement))
                {
                    m_AvailableEnemyMovementTypes.Add(enemyMovementType,
                        new EnemyMovementIntKeyAndName(i++, enemyMovementType.Name.Replace("EnemyMovement", string.Empty)));
                }
            }

            if (m_EnemyData.EnemyMovement == null)
            {
                m_EnemyData.SetEnemyMovement(new LinearEnemyMovement());
            }

            if (m_EnemyData.EnemyMovement != null)
            {
                s_SelectedDataType = m_AvailableEnemyMovementTypes[m_EnemyData.EnemyMovement.GetType()].SelectionKey;
            }
        }

        public override void OnInspectorGUI()
        {
            showEnemyMovementSelection();
            base.OnInspectorGUI();
            showCardGraphic();

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        private void showEnemyMovementSelection()
        {
            string[] possibleSelections = new string[m_AvailableEnemyMovementTypes.Count];
            foreach (var keyValuePair in m_AvailableEnemyMovementTypes)
            {
                possibleSelections[keyValuePair.Value.SelectionKey] = keyValuePair.Value.Name;
            }

            GUILayout.Space(5);
            int selection = EditorGUILayout.Popup(s_SelectedDataType, possibleSelections);
            GUILayout.Space(5);

            if (selection != s_SelectedDataType)
            {
                s_SelectedDataType = selection;

                Type selectedType = m_AvailableEnemyMovementTypes.
                    First(x => x.Value.SelectionKey == selection).Key;
                ConstructorInfo constructor = (selectedType.GetConstructor(Type.EmptyTypes));
                m_EnemyData.SetEnemyMovement(constructor?.Invoke(null) as EnemyMovement);
            }
        }

        private void showCardGraphic()
        {
            if (m_EnemyData.EnemyModel != null)
            {
                SpriteRenderer spriteRenderer = m_EnemyData.EnemyModel.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                {
                    spriteRenderer = m_EnemyData.EnemyModel.GetComponentInChildren<SpriteRenderer>();
                }

                if (spriteRenderer != null && spriteRenderer.sprite != null)
                {
                    GUILayout.Space(5);
                    GUILayout.Label("Artwork Preview:");
                    
                    GUILayout.Label(spriteRenderer.sprite.texture,
                        GUILayout.MaxHeight(200),
                        GUILayout.MaxWidth(200));
                }
            }
        }
    }
}
