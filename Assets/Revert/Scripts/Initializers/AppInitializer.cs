using System.Collections.Generic;
using MEC;
using Revert.Core.Extensions;
using UnityEngine;
using UnityEngine.Rendering;

namespace Revert.Initializers {
  public class AppInitializer : MonoBehaviour {
    void Awake() {
      Application.targetFrameRate = 60; // Set the target frame rate to 60, this means update loops will run 60 times per second
      OnDemandRendering.renderFrameInterval = int.MaxValue; // Disables unity from rendering frames
      this.RenderFrame(); // Render the initial frame
    }

    /// <summary>
    /// This method should be called from native code when the screen is touched
    /// </summary>
    public void OnScreenTouch() {
      Debug.Log("Screen Touch");
      this.StartRendering();
      Timing.RunCoroutine(TrackTouch());
    }
    
    private IEnumerator<float> TrackTouch() {
      float touchStartTime = Time.time;
      bool toucheEnded = false;
      
      // Wait until the touch ends
      while (!toucheEnded) {
#if !UNITY_EDITOR
        if (Input.touchCount == 0) {
          toucheEnded = true;
        }
#else
        if (Input.GetMouseButtonUp(0)) {
          toucheEnded = true;
        }
#endif
        yield return Timing.WaitForOneFrame;
      }

      float touchDuration = Time.time - touchStartTime;
      this.StopRendering(); // TODO: Smoothly stop rendering instead of stopping it abruptly
    }
  }
}