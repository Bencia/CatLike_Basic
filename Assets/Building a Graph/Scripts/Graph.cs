using UnityEngine;
public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] int resolution = 10;
    [SerializeField] FunctionLibrary.FunctionName function = FunctionLibrary.FunctionName.Wave;

    Transform[] points;
    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i] = Instantiate(pointPrefab, transform);
            point.localScale = scale;
        }
        #region Square
        //for (int i = 0; i < resolution; i++)
        //{
        //    for (int j = 0; j < resolution; j++)
        //    {
        //        Transform point = Instantiate(pointPrefab, transform);
        //        point.localScale = scale;
        //        //º¯Êý²¿·Ö
        //        position.x = (i - .5f) * step - 1f;
        //        position.y = (j - .5f) * step - 1f;
        //        point.localPosition = position;
        //    }
        //}
        #endregion
    }
    private void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z++;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            Transform point = points[i];
            point.localPosition = f(u, v, time);
        }
    }
}
