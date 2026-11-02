using UnityEngine;

public class VoicePixelsController : MonoBehaviour
{
    private string microphoneName;
    private AudioClip recordingClip;

    private int sampleRate = 11025; 

    public ParticleSystem pixelSystem; 
    public float pixelIntensity = 300f; 
    public float soundThreshold = 0.02f; 

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];

            recordingClip = Microphone.Start(microphoneName, true, 1, sampleRate);

            while (!(Microphone.GetPosition(microphoneName) > 0)) { } 
        }
        else
        {
            Debug.LogError("Микрофон не найден!");
        }
    }

    public float GetLoudnessFromMicrophone()
    {
        int microphonePosition = Microphone.GetPosition(microphoneName);
        if (microphonePosition < 0) return 0;

        int sampleCount = 64; 
        float[] waveData = new float[sampleCount];

        int startPosition = microphonePosition - sampleCount;
        if (startPosition < 0) return 0;

        recordingClip.GetData(waveData, startPosition);

        float totalLoudness = 0;
        foreach (var sample in waveData)
        {
            totalLoudness += Mathf.Abs(sample);
        }

        return totalLoudness / sampleCount;
    }
}
