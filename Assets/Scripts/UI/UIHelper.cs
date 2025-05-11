using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelper
{
    // relativeTransform 설정될 경우 해당 객체를 기준으로 위치 변환 필요
    public static Vector2 GetRelativeTransformedPosition(Vector2 relativePosition, Vector3 relativeForward)
    {
        float resultAngle = Vector2.SignedAngle(relativePosition, Vector2.up);
        Vector3 resultVector3 = Quaternion.AngleAxis(resultAngle, Vector3.up) * new Vector3(relativeForward.x, 0, relativeForward.z);
        resultVector3 = resultVector3.normalized * relativePosition.magnitude;
        return new Vector2(resultVector3.x, resultVector3.z);
    }

    private static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = CreateWhiteTexture();
            }
            return _whiteTexture;
        }
    }

    private static Texture2D CreateWhiteTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        return texture;
    }

    public static Rect GetRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;

        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static void DrawRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawBorder(Rect rect, float thickness, Color color)
    {
        DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color); // Top
        DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color); // Left
        DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color); // Right
        DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color); // Bottom
    }

    public static Bounds GetViewportBounds(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        Vector3 v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        Vector3 v2 = Camera.main.ScreenToViewportPoint(screenPosition2);

        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);

        min.z = Camera.main.nearClipPlane;
        max.z = Camera.main.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }
}
