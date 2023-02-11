using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSharp.Video;

public class AL_USV_Adapter: UdonSharpBehaviour
{
	//Points to the USVPlayer so that Stream mode can be checked
    public USharpVideoPlayer videoPlayer;
	public VRCAudioLink.AudioLink audioLink;
	public AudioSource videoAudioSource;
	public AudioSource streamAudioSource;
	
	private void Start()
    {
        UnityEngine.Debug.Log("Timer started");
        Timer();
    }
    
    public void Timer()
    {
        CheckVideoMode();
        SendCustomEventDelayedSeconds(nameof(Timer), 3f);
    }
	
	//Checks if the USVPlayer is in Stream mode and outputs whether this is true or false
	public void CheckVideoMode() {
	
        if (videoPlayer.IsUsingAVProPlayer())
        {
			//Announces the state of stream mode
            Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "Video player is in stream mode");
			
			//Checks if the Audiolink source is set to the appropriate setting and changes it if not set correctly.
			if (audioLink.audioSource == streamAudioSource)
			{
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "No action necessary, the player is in stream mode and Audiolink is already set to " + audioLink.audioSource.name);
			return;;
			}

			else
			{
			audioLink.audioSource = streamAudioSource;
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "Audio Source set to " + audioLink.audioSource.name);
			}
        }
		
        else
        {
			//Announces the state of stream mode
            Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "Video player is NOT in stream mode");
						
			//Checks if the Audiolink source is set to the appropriate setting and changes it if not set correctly.
			if (audioLink.audioSource == videoAudioSource)
			{
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "No action necessary, the player is in video mode and Audiolink is already set to " + audioLink.audioSource.name);
			return;;
			}
			
			else
			{
			audioLink.audioSource = videoAudioSource;
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "Audio Source set to " + audioLink.audioSource.name);
			}
		}
    }
}

