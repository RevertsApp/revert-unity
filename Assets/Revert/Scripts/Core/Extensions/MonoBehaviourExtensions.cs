using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using MEC;
using Revert.Initializers;

namespace Revert.Core.Extensions {
  /// <summary>
  /// This class contains extension methods for MonoBehaviours
  /// Such as a method to control rendering frames when needed
  /// </summary>
  public static class MonoBehaviourExtensions {
    /// <summary>
    /// Temporarily sets Unity to render frames normally for a specified duration, then disables frame rendering.
    /// </summary>
    /// <param name="seconds"> The number of seconds to render frames normally. </param>
    public static IEnumerator<float> RenderFrames(this MonoBehaviour monoBehaviour, int seconds) {
      PlayerLoopManager.Instance.ToggleUpdateLoop(true); // Enables the update loop for the timing to play out
      OnDemandRendering.renderFrameInterval = 1; // Enables unity to render the normal amount of frames per second
      yield return Timing.WaitForSeconds(seconds);
      OnDemandRendering.renderFrameInterval = int.MaxValue; // Disables unity from rendering frames
      PlayerLoopManager.Instance.ToggleUpdateLoop(false); // Disables the update loop
    }
    
    /// <summary>
    /// Renders exactly one frame.
    /// </summary>
    public static void RenderFrame( this MonoBehaviour monoBehaviour) {
      Timing.RunCoroutine(RenderFrames(monoBehaviour, 0));
    }
    
    /// <summary>
    /// Start Rendering frames normally 60 per second, until stop rendering is called.
    /// This method is intensive and should only be used when needed.
    /// </summary>
    public static void StartRendering(this MonoBehaviour monoBehaviour) {
      PlayerLoopManager.Instance.ToggleUpdateLoop(true); // Enables the update loop for the timing to play out
      OnDemandRendering.renderFrameInterval = 1; // Enables unity to render the normal amount of frames per second
    }
    
    /// <summary>
    /// Stops rendering frames normally. This method should be called after StartRendering, when you are done rendering frames.
    /// </summary>
    public static void StopRendering(this MonoBehaviour monoBehaviour) {
      OnDemandRendering.renderFrameInterval = int.MaxValue; // Disables unity from rendering frames
      PlayerLoopManager.Instance.ToggleUpdateLoop(false); // Disables the update loop
    }
  }
}
