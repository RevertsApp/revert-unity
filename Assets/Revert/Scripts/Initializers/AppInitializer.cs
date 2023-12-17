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
  }
}