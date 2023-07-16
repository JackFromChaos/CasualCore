using UnityEngine;

public static class Ext2d 
{
    //вычисляет локальный Rect объекта относительно родительского объекта
    public static Rect GetRectRelativeTo(RectTransform objTransform, Transform parentTransform)
    {
        // Получить глобальные позиции углов прямоугольника объекта
        Vector3[] objCorners = new Vector3[4];
        objTransform.GetWorldCorners(objCorners);

        // Преобразовать глобальные позиции углов прямоугольника в локальные позиции относительно родительского объекта
        for (int i = 0; i < objCorners.Length; i++)
        {
            objCorners[i] = parentTransform.InverseTransformPoint(objCorners[i]);
        }

        // Получить координаты прямоугольника в локальных координатах относительно родительского объекта
        float xMin = Mathf.Min(objCorners[0].x, objCorners[1].x, objCorners[2].x, objCorners[3].x);
        float xMax = Mathf.Max(objCorners[0].x, objCorners[1].x, objCorners[2].x, objCorners[3].x);
        float yMin = Mathf.Min(objCorners[0].y, objCorners[1].y, objCorners[2].y, objCorners[3].y);
        float yMax = Mathf.Max(objCorners[0].y, objCorners[1].y, objCorners[2].y, objCorners[3].y);

        // Создать и вернуть прямоугольник
        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }
    //вычисляет локальную позицию объекта относительно родительского объекта
    public static Vector3 GetLocalPositionRelativeTo(Transform objTransform, Transform parentTransform)
    {
        // Получить глобальные позиции объекта и родительского объекта
        Vector3 objWorldPosition = objTransform.position;
        Vector3 parentWorldPosition = parentTransform.position;

        // Преобразовать глобальную позицию объекта в локальную позицию относительно родительского объекта
        Vector3 localPosition = parentTransform.InverseTransformPoint(objWorldPosition);

        return localPosition;
    }
    public static bool CheckDistance2D(this Vector3 from, Vector3 to, float dist)
    {
        dist = dist * dist;
        float a = from.x - to.x;
        float b = from.z - to.z;
        float cur = a * a + b * b;
        return cur < dist;
    }
    public static float Distance2Dsqr(this Vector3 from, Vector3 to)
    {
        float a = from.x - to.x;
        float b = from.z - to.z;
        return a * a + b * b;
    }
    public static float Distance2D(this Vector3 from, Vector3 to)
    {
        float a = from.x - to.x;
        float b = from.z - to.z;
        return Mathf.Sqrt(a * a + b * b);
    }

}
