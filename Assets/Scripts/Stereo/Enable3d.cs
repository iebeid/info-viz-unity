
using UnityEngine;
using System.IO;

public class Enable3d : MonoBehaviour
{
	public enum StereoMode
	{
		StereoOff        = 16,
		StereoVertical   = 32,
		StereoHorizontal = 64
	}
	
	public StereoMode Mode;
	
	void Reset()
	{
		Mode = StereoMode.StereoOff;
	}
	
	// Use this for initialization
	void Start()
	{
		setMode(Mode);
	}

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {
            Debug.Log("Pausing app: stereo off");
            setMode(StereoMode.StereoOff);
        }
        else
        {
            Debug.Log("Resuming app: re-enabling stereo");
            setMode(Mode);
        }
    }
	
	void OnApplicationQuit()
	{
		Debug.Log("Enable3D::OnApplicationQuit called");
		setMode(StereoMode.StereoOff);
	}
	
	private void setMode(StereoMode m)
	{
		if(Application.platform != RuntimePlatform.Android)
		{
			Debug.LogWarning("Not on Android, setting stereo mode ignored.");
			return;
		}
		
		using(FileStream fstream =
		      new FileStream("/dev/mi3d_tn_ctrl", FileMode.Open, FileAccess.Write))
		{
			byte[] value = new byte[] { (byte) m };
			
			fstream.Write(value, 0, 1);
			fstream.Flush();
			fstream.Close();
		}
	}
}
