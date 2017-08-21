using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters; // so we can detect by type.

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour 
    {
        // INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(96, 96);

        const int POTENTIALLY_WALKABLE_LAYER = 8;
        const int POTENTIALLY_ENEMY_LAYER = 9;
        float maxRaycastDepth = 5000f; // Hard coded value

        Rect screenRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);

        //Seting up delegate for EnemyAI 
        public delegate void OnMouseOverEnemy(EnemyAI enemy); // Declearing new Delegate Type.
        public event OnMouseOverEnemy onMouseOverEnemy;

        //Seting up delegate for terrian.
        public delegate void OnMouseOverTerrain(Vector3 destination); // Declearing new Delegate Type.
        public event OnMouseOverTerrain onMouseOverpotentiallyWalkable;

        // Event means (you can't override this class accidenlty , YOU CAN ONLY ADD TO IT).
        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Implement UI interaction.
                // Stop looking for other objects
            }
            else
            {
                PerformRaycasts();
            }
        }

        void PerformRaycasts()
        {
            if (screenRect.Contains(Input.mousePosition))
            {
                // creating the ray.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //specify layer priorities below.
                if (RaycastForEnemy(ray)) { return; }
                if (RaycastForPotentiallyWalkable(ray)) { return; }
            }
        }

        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            GameObject gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<EnemyAI>();
            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }

        private bool RaycastForPotentiallyWalkable(Ray ray)
        {
            // TODO , remove , Potentially walkable means it is only walkable when the  navmesh extends to the place i wanna go to.
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverpotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }        
    }
}