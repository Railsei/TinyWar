using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RTSCameraController : MonoBehaviour
{
   [Header("Kéo đối tượng MapBounds vào đây")]
    [SerializeField] private BoxCollider2D mapBounds;

    [Header("Zoom (Thu phóng)")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 4f;
    [SerializeField] private float maxZoom = 12f;

    private Camera cam;
    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleZoom();
        HandleMouseDrag();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize = Mathf.Clamp(
                cam.orthographicSize - scroll * zoomSpeed,
                minZoom,
                maxZoom
            );

            // Cập nhật lại vị trí camera ngay sau khi zoom để không bị văng ra mép
            ClampCameraPosition(transform.position);
        }
    }

    private void HandleMouseDrag()
    {
        // 1. Khi vừa nhấn chuột trái xuống
        if (Input.GetMouseButtonDown(1))
        {
            // Tránh kéo map nếu đang bấm vào thanh UI (BottomBar, nút bấm...)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        // 2. Trong lúc đang giữ chuột trái và di chuyển
        if (Input.GetMouseButton(1) && isDragging)
        {
            Vector3 currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPos;

            // Camera di chuyển theo hướng ngược lại với tay kéo
            Vector3 targetPosition = transform.position + difference;
            ClampCameraPosition(targetPosition);
        }

        // 3. Khi thả chuột trái ra
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }
    }

    // Hàm khóa vị trí chuẩn xác theo MapBounds
    private void ClampCameraPosition(Vector3 targetPos)
    {
        if (mapBounds != null)
        {
            Bounds b = mapBounds.bounds;
            float camHeight = cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;

            // Kiểm tra chiều ngang: Nếu map rộng hơn màn hình thì kẹp biên, ngược lại thì khóa tâm
            if (b.size.x > camWidth * 2f)
            {
                targetPos.x = Mathf.Clamp(targetPos.x, b.min.x + camWidth, b.max.x - camWidth);
            }
            else
            {
                targetPos.x = b.center.x;
            }

            // Kiểm tra chiều dọc: Tương tự
            if (b.size.y > camHeight * 2f)
            {
                targetPos.y = Mathf.Clamp(targetPos.y, b.min.y + camHeight, b.max.y - camHeight);
            }
            else
            {
                targetPos.y = b.center.y;
            }
        }

        targetPos.z = -10f; // Luôn giữ nguyên trục Z cho 2D
        transform.position = targetPos;
    }
}
