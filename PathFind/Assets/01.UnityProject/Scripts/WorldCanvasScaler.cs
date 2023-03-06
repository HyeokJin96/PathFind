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

        //  카메라 사이즈와 캔버스 사이즈 사이의 크기 비를 구한다.
        //  width와 height 둘 중 하나의 값으로 비율을 결정한다.
        canvasAspect.x = camerSize.x / canvasSize.x;
        canvasAspect.y = canvasAspect.x;

        //  현재 캔버스의 로컬 스케일을 위에서 산출한 비율로 설정한다.
        worldCanvas.transform.localScale = canvasAspect;
    }   //  Start()

    void Update()
    {
        
    }
}
