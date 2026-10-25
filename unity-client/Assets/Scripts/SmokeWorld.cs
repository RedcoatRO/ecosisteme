using EcoSim.Core;
using EcoSim.Gameplay;
using UnityEngine;

namespace EcoSim.Client
{
    /// <summary>Diagnostic probe and clock only; no simulated ecology or performance claim.</summary>
    public sealed class SmokeWorld : MonoBehaviour
    {
        private SimulationSession session;
        private Camera sceneCamera;
        private Transform probe;
        private Material groundMaterial;
        private Material probeMaterial;
        private double accumulator;
        private bool romanian = true;
        private const float PanelHeight = 180;
        private GUIStyle labelStyle;
        private GUIStyle buttonStyle;

        private void Awake()
        {
            Application.targetFrameRate = 30; // Desired cap, not a measured guarantee.
            session = new SimulationSession(new WorldConfig(42));
            var baseMaterial = Resources.Load<Material>("SmokeMaterial");
            if (baseMaterial == null)
            {
                Debug.LogError("ECOSIM_SMOKE_BLOCKED: run EcoSim/Prepare Android smoke to generate URP assets.");
                enabled = false;
                return;
            }
            groundMaterial = new Material(baseMaterial) { color = new Color(0.28f, 0.52f, 0.29f) };
            probeMaterial = new Material(baseMaterial) { color = new Color(0.95f, 0.68f, 0.15f) };
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Diagnostic flat terrain (20 metres square)";
            ground.transform.localScale = new Vector3(2, 1, 2);
            ground.GetComponent<Renderer>().sharedMaterial = groundMaterial;
            var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            marker.name = "Simulation probe ID 1";
            marker.GetComponent<Renderer>().sharedMaterial = probeMaterial;
            probe = marker.transform;
            sceneCamera = new GameObject("Smoke Camera").AddComponent<Camera>();
            sceneCamera.tag = "MainCamera";
            sceneCamera.transform.position = new Vector3(0, 16, -16);
            sceneCamera.transform.LookAt(Vector3.zero);
            sceneCamera.backgroundColor = new Color(0.40f, 0.65f, 0.80f);
            sceneCamera.clearFlags = CameraClearFlags.SolidColor;
            var light = new GameObject("Sun").AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            light.transform.rotation = Quaternion.Euler(50, -25, 0);
            SynchronizeView();
            Debug.Log("ECOSIM_SMOKE_STARTED: seed=42, probeId=1; diagnostic scene, not ecological gameplay.");
        }

        private void Update()
        {
            if (session.Paused) accumulator = 0;
            else
            {
                accumulator += Mathf.Min(Time.unscaledDeltaTime, 0.25f);
                while (accumulator >= 0.5)
                {
                    session.Advance();
                    accumulator -= 0.5;
                }
            }
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) PlaceProbe(touch.position);
            }
            else if (Input.GetMouseButtonDown(0)) PlaceProbe(Input.mousePosition);
            SynchronizeView();
        }

        private void PlaceProbe(Vector2 screenPosition)
        {
            float uiScale = Mathf.Max(1, Screen.width / 960f);
            if (Screen.height - screenPosition.y <= PanelHeight * uiScale) return;
            var ray = sceneCamera.ScreenPointToRay(screenPosition);
            if (!new Plane(Vector3.up, Vector3.zero).Raycast(ray, out float distance)) return;
            var point = ray.GetPoint(distance);
            if (Mathf.Abs(point.x) > 10 || Mathf.Abs(point.z) > 10) return;
            session.MoveProbe(point.x, point.z);
            Debug.Log($"ECOSIM_PROBE_MOVED: id=1 x={point.x}m z={point.z}m");
        }

        private void SynchronizeView()
        {
            var state = session.Snapshot;
            probe.position = new Vector3((float)state.ProbeXMeters, 0.6f, (float)state.ProbeZMeters);
            // Diagnostic size comes from simulation state, never renderer count.
            float size = 0.8f + (state.DiagnosticSample % 5) * 0.1f;
            probe.localScale = Vector3.one * size;
        }

        private void OnGUI()
        {
            if (!enabled || session == null) return;
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 22 };
                buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 22 };
            }
            float scale = Mathf.Max(1, Screen.width / 960f);
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1));
            GUI.Box(new Rect(0, 0, Screen.width / scale, PanelHeight), "");
            var state = session.Snapshot;
            GUI.Label(new Rect(16, 8, 900, 36), romanian ? "EcoSim — scenă de diagnostic, fără model ecologic" : "EcoSim — diagnostic scene, no ecology model", labelStyle);
            GUI.Label(new Rect(16, 44, 900, 36), $"Tick {state.Tick} | {state.ElapsedSeconds:F1} s | ID {state.ProbeId}", labelStyle);
            GUI.Label(new Rect(16, 80, 900, 36), romanian ? "Atinge terenul pentru a muta sonda." : "Tap the ground to move the probe.", labelStyle);
            if (GUI.Button(new Rect(16, 122, 240, 48), session.Paused ? (romanian ? "Continuă" : "Resume") : (romanian ? "Pauză" : "Pause"), buttonStyle))
                session.SetPaused(!session.Paused);
            if (GUI.Button(new Rect(272, 122, 160, 48), romanian ? "English" : "Română", buttonStyle)) romanian = !romanian;
        }
        private void OnDestroy()
        {
            if (groundMaterial != null) Destroy(groundMaterial);
            if (probeMaterial != null) Destroy(probeMaterial);
        }
    }
}
