using System.Collections.Generic;
using UnityEngine;

public class YSortManager : MonoBehaviour
{
    public Transform player;
    public Transform feetPoint;

    public float detectionRadius = 10f;
    public int sortingPrecision = 100;
    public int sortingOffset = 200;

    GameObject GoodTree;
    SpriteRenderer goodTreeRenderer;

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    void Start()
    {
        SpriteRenderer[] allRenderers = FindObjectsOfType<SpriteRenderer>();
        foreach (var sr in allRenderers)
        {
            if ((sr.gameObject.layer != LayerMask.NameToLayer("IgnoreLayer")) && (sr.enabled))
            {
                spriteRenderers.Add(sr);
            }
        }

        GoodTree = GameObject.FindWithTag("GoodTree");
        if (GoodTree != null)
            goodTreeRenderer = GoodTree.GetComponent<SpriteRenderer>();

        if (player == null || feetPoint == null)
        {
            Debug.LogError("YSortManager: ������� Player � Feet Point � ����������.");
        }
    }

    void LateUpdate()
    {
        if (player == null || feetPoint == null) return;

        float playerFootY = feetPoint.position.y;

        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer != null)
        {
            playerRenderer.sortingOrder = Mathf.RoundToInt(-playerFootY * sortingPrecision) + sortingOffset;
        }

        // ��������� GoodTree
        if (GoodTree != null && goodTreeRenderer != null)
        {
            float distanceToGoodTree = Vector2.Distance(player.position, GoodTree.transform.position);
            float detectionRadiusForGoodTree = detectionRadius * 2f;

            if (distanceToGoodTree <= detectionRadiusForGoodTree)
            {
                float objectBottomY = goodTreeRenderer.bounds.min.y;
                float objectTopY = goodTreeRenderer.bounds.max.y;

                float bufferZone = 1f; // ����� �������� �� ���������

                int currentOrder = GoodTree.GetComponent<SpriteRenderer>().sortingOrder;
                int newOrderAbove = Mathf.RoundToInt(-objectBottomY * sortingPrecision) + sortingOffset - 2;
                int newOrderBelow = Mathf.RoundToInt(-objectBottomY * sortingPrecision) + sortingOffset + 3;

                if (playerFootY > objectBottomY + bufferZone)
                {
                    // ����� ���� ���� � �������
                    GoodTree.GetComponent<SpriteRenderer>().sortingOrder = newOrderAbove;
                }
                else if (playerFootY <= objectBottomY - bufferZone)
                {
                    // ����� ���� ���� � ������ ����� �������
                    GoodTree.GetComponent<SpriteRenderer>().sortingOrder = newOrderBelow;
                }
                // ����� � ����� �� �������, �� ������ sortingOrder

            }
        }

        // ��������� ��������� ��������
        foreach (var sr in spriteRenderers)
        {
            if (sr == null) continue;

            float distance = Vector2.Distance(player.position, sr.transform.position);
            if (distance <= detectionRadius)
            {
                float objectBottomY = sr.bounds.min.y;

                if (playerFootY > objectBottomY)
                {
                    sr.sortingOrder = Mathf.RoundToInt(-objectBottomY * sortingPrecision) + sortingOffset;
                }
                else
                {
                    sr.sortingOrder = Mathf.RoundToInt(-objectBottomY * sortingPrecision) + sortingOffset - 1;
                }
            }
        }
    }
}