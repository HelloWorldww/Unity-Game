using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class CameraScroll : Singleton<CameraScroll>
    {
        [SerializeField] private Camera cam;

        [SerializeField] private SpriteRenderer mapRenderer;

        private float mapMinY, mapMaxY;

        private Vector3 dragOrigin;

        public static Vector3 cameraPos;

        protected override void Awake()
        {
            base.Awake();

            mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
            mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;

            cameraPos = cam.transform.localPosition;
        }

        void Update()
        {
            if (!WorkManagerController.managersOpened && !OffersManager.offersOpened)
                PanCamera();
        }

        private void PanCamera()
        {
            if (Input.GetMouseButtonDown(0))
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                // difference.Set(0, difference.y, difference.z);

                //cam.transform.position += difference;
                cam.transform.position = ClampCamera(cam.transform.position + difference);
            }
        }

        private Vector3 ClampCamera(Vector3 targetPosition)
        {
            float camHeight = cam.orthographicSize;

            float minY = mapMinY + camHeight;
            float maxY = mapMaxY - camHeight;

            float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

            return new Vector3(0, newY, targetPosition.z);
        }

        public void ExpandMapRenderer()
        {
            float increment = 3.7f - 0.1f;

            mapRenderer.transform.localScale = new Vector3(mapRenderer.transform.localScale.x, mapRenderer.transform.localScale.y + increment, mapRenderer.transform.localScale.z);
            mapRenderer.transform.localPosition = new Vector3(mapRenderer.transform.localPosition.x, mapRenderer.transform.localPosition.y - ((increment / 2) / 2), mapRenderer.transform.localPosition.z);

            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y - (increment / 2), cam.transform.localPosition.z);

            mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
            mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
        }
    }
}