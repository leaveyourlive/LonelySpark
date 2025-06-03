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
        // �������� ��������� ���������� ������ (������� ���������)
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        foreach (var obj in staticObjects)
        {
            if (obj == null) continue;

            Renderer rend = obj.GetComponent<Renderer>();
            if (rend == null) continue;  // ��� ������� � �� ���������

            // �������� �������������� ���� �������
            Bounds bounds = rend.bounds;

            // ���������, ������������ �� ���� � ����������� ������
            bool isVisible = GeometryUtility.TestPlanesAABB(cameraPlanes, bounds);

            // ���������� ��� ������������ ������
            obj.SetActive(isVisible);
        }
    }
}
