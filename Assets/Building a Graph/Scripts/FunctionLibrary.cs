using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);
    public enum FunctionName
    {
        Wave, MultiWave, Ripple, NormalSphere,/*, VerticalBandsSphere,*/
        /*HorizontalBandsSphere, TwistedSphere, */Torus
    }
    static Function[] functions = { Wave, MultiWave, Ripple, NormalSphere,/*VerticalBandsSphere,*/
    /*HorizontalBandsSphere,TwistedSphere,*/Torus};
    public static Function GetFunction(FunctionName name) => functions[(int)name];
    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        return ((int)name < functions.Length - 1) ? name + 1 : 0;
    }
    //这个地方在处理不重复随机的时候用了个好方法 注意看
    public static FunctionName GetRandomFunctionName(FunctionName name)
    {
        FunctionName currRandom = (FunctionName)Random.Range(1, functions.Length);
        return currRandom == name ? 0 : currRandom;
    }

    public static Vector3 Morph( float u, float v, float t, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0, 1, progress));
    }

    #region Functions
    private static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }
    private static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + .5f * t));
        p.y += Sin(2 * PI * (v + t)) * .5f;
        p.y += Sin(PI * (u + v + .25f + t));
        p.y *= 1f / 2.5f;
        p.z = v;
        return p;
    }
    private static Vector3 Ripple(float u, float v, float t)//波纹
    {
        float d = Sqrt(u * u + v * v);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4 * d - t));
        p.y /= 1f + 10f * d;
        p.z = v;
        return p;
    }
    private static Vector3 NormalSphere(float u, float v, float t)
    {
        float r = .5f + .5f * Sin(PI *t);//球的半径
        float s = r * Cos(.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * .5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    private static Vector3 VerticalBandsSphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(8f * PI * u + t);//球的半径
        float s = r * Cos(.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * .5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    private static Vector3 HorizontalBandsSphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(8f * PI * v + t);//球的半径
        float s = r * Cos(.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * .5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    private static Vector3 TwistedSphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));//球的半径
        float s = r * Cos(.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * .5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    private static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    #endregion
}
