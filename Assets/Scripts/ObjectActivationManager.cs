using UnityEngine;
using System.Collections.Generic;

public class ObjectActivationManager : MonoBehaviour
{
    private Camera mainCamera;
    private List<GameObject> staticObjects = new List<GameObject>();
    private Plane[] cameraPlanes;

    void Start()
    {
        mainCamera = Camera.main;
        GameObject[] found = GameObject.FindGameObjectsWithTag("StaticObject");
        staticObjects.AddRange(found);
    }

    void Update()
    {
        // Получаем плоскости фрустрации камеры (область видимости)
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        foreach (var obj in staticObjects)
        {
            if (obj == null) continue;

            Renderer rend = obj.GetComponent<Renderer>();
            if (rend == null) continue;  // Нет рендера — не проверяем

            // Получаем ограничивающий бокс объекта
            Bounds bounds = rend.bounds;

            // Проверяем, пересекается ли бокс с фрустрацией камеры
            bool isVisible = GeometryUtility.TestPlanesAABB(cameraPlanes, bounds);

            // Активируем или деактивируем объект
            obj.SetActive(isVisible);
        }
    }
}
