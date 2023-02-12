
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSharp.Video;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class AL_USV_Adapter : UdonSharpBehaviour
{
	//Points to the USVPlayer so that Stream mode can be checked
    public USharpVideoPlayer videoPlayer;
	
	//Connects to the Audiolink component
	public VRCAudioLink.AudioLink audioLink;
	
	//Connects to the Video audio source
	public AudioSource videoAudioSource;
	
	//Connects to the Stream audio source
	public AudioSource streamAudioSource;
	
	//Declares a synced variable, used to check the audio settings and set them if necessary.
	[UdonSynced] private bool isStreamMode;
	
	//Registers to the USharpVideoPlayers events
	public void Start()
	{
		videoPlayer.RegisterCallbackReceiver(this);
	}
	
	//Calls the CheckVidPlayer method any time a video begins loading.
	public void OnUSharpVideoLoadStart()
	{
		CheckVidPlayerMode();
	}
	//Checks if the USVPlayer is in Stream mode, and adjusts the Audiolink audiosource if necessary
	public void CheckVidPlayerMode()
	{
		//Checks if the USVPlayer is in Stream mode
		if (videoPlayer.IsUsingAVProPlayer())
		{
			//Announces the state of stream mode
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "Video player is in stream mode");
			
			//Sends a network event informing players what state the player is in
			SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,"StreamModeActive");
        }
		else
		{
			//Announces that the player is NOT in stream mode
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "Video player is NOT in stream mode");
		
			//Sets streamMode bool to false
			isStreamMode = false;
			
			//Sends a network event informing players what state the player is in
			SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,"VideoModeActive");
		}
	}
	public void StreamModeActive()
	{
		//Sets streamMode bool to true
		isStreamMode = true;
		
		//Checks if the Audiolink source is set to the appropriate setting and changes it if not set correctly.
		if (audioLink.audioSource == streamAudioSource)
		{
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "No action necessary, the player is in video mode and Audiolink is already set to " + audioLink.audioSource.name);
			return;;
		}
		else
		{
			audioLink.audioSource = streamAudioSource;
			Debug.Log("[<color=#ff00c6>USV_Audiolink_Adapter</color>]" + "Audio Source set to " + audioLink.audioSource.name);
		}
	}
	public void VideoModeActive()
	{
		//Sets streamMode bool to false
		isStreamMode = false;

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
	public override void OnDeserialization()
	{
		if (isStreamMode == true)
		{
			StreamModeActive();
		}
		else
		{
			VideoModeActive();
		}
	}
}
