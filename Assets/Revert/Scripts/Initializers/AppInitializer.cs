using UnityEngine;

namespace Revert.Initializers {
  public class AppInitializer : MonoBehaviour {
    void Awake() {
      Application.targetFrameRate = 60; // Set the target frame rate to 60, this means update loops will run 60 times per second
    }
  }
}