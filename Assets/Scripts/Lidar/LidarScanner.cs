using System;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class LidarScanner : MonoBehaviour
{
    [SerializeField] private VisualEffect vfxGraph;
    [SerializeField] private int raysPerFrame = 500;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private VoicePixelsController voiceContoller;

    [SerializeField] private Vector3[] positions;

    public float pixelIntensity = 300f;
    public float soundThreshold = 0.02f;
    private const int MaxPoints = 131072;

    private GraphicsBuffer _positionBuffer;
    private Ray[] _rayCache = null;
    private int _bufferIndex = 0;
    
    void Start()
    {
        _rayCache = new Ray[raysPerFrame];
        positions = new Vector3[MaxPoints];
        _positionBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, MaxPoints, sizeof(float) * 3);
        
        vfxGraph.SetGraphicsBuffer("Positions", _positionBuffer);
    }

    void Update()
    {
        int rayCount;

        float loudness = voiceContoller.GetLoudnessFromMicrophone();

        if (loudness > soundThreshold)
        {
            rayCount = Mathf.RoundToInt(loudness * pixelIntensity);
            if (rayCount > raysPerFrame)
                rayCount = raysPerFrame;
        }

        else
        {
            rayCount = 0;
        }
        var pointsAdded = false;

        var originPos = transform.position;
        var myTransform = transform; 
        
        for (var i = 0; i < rayCount; i++)
        {
            var randomDir = Random.onUnitSphere;
            var rayDirection = myTransform.TransformDirection(randomDir).normalized;

            var ray = _rayCache[i];
            ray.origin = originPos;
            ray.direction = rayDirection;
           
            if (!Physics.Raycast(ray, out var hit, maxDistance))
                continue;
            
            positions[_bufferIndex] = hit.point;
            
            _bufferIndex = (_bufferIndex + 1) % MaxPoints;
            pointsAdded = true;
        }
        
        if (!pointsAdded)
            return;
        else 
        _positionBuffer.SetData(positions);
    }

    private void OnDestroy()
    {
        if (_positionBuffer != null)
            _positionBuffer.Release();
    }
}
    