using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHoverTarget : MonoBehaviour
{
    [Header("Ảnh con trỏ chuột khi rê vào vật thể này")]
    [SerializeField] private Texture2D hoverCursor;

    [Header("Ảnh con trỏ chuột mặc định lúc bình thường")]
    [SerializeField] private Texture2D defaultCursor;

    [Header("Tâm nhấp chuột (Hotspot)")]
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    private void OnMouseEnter()
    {
        // Khi con trỏ chuột đi vào vùng Collider của Nhà chính
        if (hoverCursor != null)
        {
            Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
        }
    }

    private void OnMouseExit()
    {
        // Khi con trỏ chuột rời khỏi Nhà chính -> Trả lại chuột bình thường
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    private void OnDisable()
    {
        // Đảm bảo chuột được reset nếu đối tượng bị ẩn/phá hủy
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }
}
