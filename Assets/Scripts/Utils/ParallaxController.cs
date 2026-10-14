using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class ParallaxController : MonoBehaviour
    {
        [SerializeField] private List<ParallaxLayer> layers;
        
        private Vector3 _lastCameraPosition;
        private Camera _mainCamera;
        private float _backgroundSize;
    
        [System.Serializable]
        public class ParallaxLayer
        {
            public Transform layer;
            [Range(0f, 1f)]
            public float speedFactor;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            
            if (_mainCamera != null) 
                _lastCameraPosition = _mainCamera.transform.position;
            
            // Background Size
            // Ojo con esto porque estamos asumiendo que tenemos las capas ya agregadas al GameObject en la jerarquia
            // Podemos pasarle el tamaño o comprobar al menos que existe el layers[0]
            if (layers[0] != null)
            {
                _backgroundSize = layers[0].layer.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
            }
            else
                _backgroundSize = 20f;
        }

        private void LateUpdate()
        {
            Vector3 cameraDelta = _mainCamera.transform.position - _lastCameraPosition;
        
            foreach (var layer in layers)
            {
                var moveX = cameraDelta.x * layer.speedFactor;
                var moveY = cameraDelta.y * layer.speedFactor;
                
                layer.layer.position += new Vector3(moveX, moveY, 0);

                CheckForLooping(_mainCamera.transform.position.x, layer.layer);
            }
        
            _lastCameraPosition = _mainCamera.transform.position;
        }
        
        private void CheckForLooping(float cameraXPosition, Transform layerTransform)
        {
            // Horizontal
            if (cameraXPosition - layerTransform.position.x > _backgroundSize)
            {
                layerTransform.position = new Vector3(layerTransform.position.x + _backgroundSize, layerTransform.position.y, layerTransform.position.z);
            }
            else if (cameraXPosition - layerTransform.position.x < 0)
            {
                layerTransform.position = new Vector3(layerTransform.position.x - _backgroundSize, layerTransform.position.y, layerTransform.position.z);
            }
        }
    }
}