using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasScaler : MonoBehaviour
{
    private Canvas worldCanvas = default;
    private Vector2 camerSize = default;

    [SerializeField] private Vector2 canvasAspect = default;

    void Start()
    {
        worldCanvas = gameObject.GetComponentMust<Canvas>();
        Vector2 canvasSize = worldCanvas.gameObject.GetRectSizeDelta();
        camerSize = GFunc.GetCameraSize();

        //  ī�޶� ������� ĵ���� ������ ������ ũ�� �� ���Ѵ�.
        //  width�� height �� �� �ϳ��� ������ ������ �����Ѵ�.
        canvasAspect.x = camerSize.x / canvasSize.x;
        canvasAspect.y = canvasAspect.x;

        //  ���� ĵ������ ���� �������� ������ ������ ������ �����Ѵ�.
        worldCanvas.transform.localScale = canvasAspect;
    }   //  Start()

    void Update()
    {
        
    }
}
