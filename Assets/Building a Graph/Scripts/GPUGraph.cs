using UnityEngine;
public class GPUGraph : MonoBehaviour
{
    const int maxResolution = 1000;

    [SerializeField] ComputeShader computeShader;
    [SerializeField, Range(10, maxResolution)] int resolution = 10;
    [SerializeField] FunctionLibrary.FunctionName function = FunctionLibrary.FunctionName.Wave;
    public enum TransitionMode { Cycle, Random }
    [SerializeField] TransitionMode transitionMode;
    [SerializeField, Min(0f)] float functionDuration = 1f, transitionDuration = 1f;
    [SerializeField] Material material;
    [SerializeField] Mesh mesh;

    float duration;
    bool transitioning;
    FunctionLibrary.FunctionName transitionFunction;

    ComputeBuffer positionsBuffer;
    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");

    private void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);//一个坐标3个float 1个float4个字节
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        if (duration > functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }
        UpdateFunctionOnGPU();
    }

    private void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);

        var kernelIndex = (int)function;
        computeShader.SetBuffer(kernelIndex, positionsId, positionsBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(kernelIndex, groups, groups, 1);

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);
        Bounds bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);
    }

    private void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(function) :
            FunctionLibrary.GetRandomFunctionName(function);
    }
}
