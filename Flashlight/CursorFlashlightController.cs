using UnityEngine;
using TorchCEO.Config;

namespace TorchCEO.Flashlight;

/// <summary>
/// In-game only: a point light that follows the mouse on the active floor plane.
/// </summary>
sealed class CursorFlashlightController : MonoBehaviour
{
    private const float LightRange = 100f;
    /// <summary>
    /// Offset along world Z toward the camera, keeping perpendicular distance to the floor plane constant so the lit disk size matches on every floor.
    /// </summary>
    private const float DepthTowardCameraFromFloor = 1f;

    private GameObject _lightGo;
    private Light _light;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DefaultConfig.CursorFlashlightEnabled.SettingChanged += OnEnabledChanged;
        DefaultConfig.CursorFlashlightIntensityBelowGround.SettingChanged += OnLightParamsChanged;
        DefaultConfig.CursorFlashlightIntensityAboveGround.SettingChanged += OnLightParamsChanged;
        EnsureLightObject();
        ApplyEnabledFromConfig();
        ApplyLightParamsFromConfig();
    }

    private void OnDestroy()
    {
        DefaultConfig.CursorFlashlightEnabled.SettingChanged -= OnEnabledChanged;
        DefaultConfig.CursorFlashlightIntensityBelowGround.SettingChanged -= OnLightParamsChanged;
        DefaultConfig.CursorFlashlightIntensityAboveGround.SettingChanged -= OnLightParamsChanged;
    }

    private void OnEnabledChanged(object sender, System.EventArgs e) => ApplyEnabledFromConfig();

    private void OnLightParamsChanged(object sender, System.EventArgs e) => ApplyLightParamsFromConfig();

    private void EnsureLightObject()
    {
        if (_lightGo != null)
            return;

        _lightGo = new GameObject("UICeoCursorFlashlight");
        _lightGo.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(_lightGo);
        _light = _lightGo.AddComponent<Light>();
        _light.type = LightType.Point;
        _light.shadows = LightShadows.None;
        _light.range = LightRange;
    }

    private void ApplyEnabledFromConfig()
    {
        bool on = DefaultConfig.CursorFlashlightEnabled.Value;
        if (!on)
        {
            if (_lightGo != null)
                _lightGo.SetActive(false);
            if (_light != null)
                _light.enabled = false;
            return;
        }
        // When enabled, visibility and intensity are applied in LateUpdate (above-ground 0 = stay off).
    }

    private void ApplyLightParamsFromConfig()
    {
        if (_light == null)
            return;
        _light.intensity = IntensityForFloor(FloorManager.currentFloor);
    }

    private static float IntensityForFloor(int floorZ) =>
        floorZ < 0
            ? DefaultConfig.CursorFlashlightIntensityBelowGround.Value
            : DefaultConfig.CursorFlashlightIntensityAboveGround.Value;

    private void LateUpdate()
    {
        if (!DefaultConfig.CursorFlashlightEnabled.Value)
            return;

        var cc = Singleton<CameraController>.Instance;
        if (cc == null || cc.mainCamera == null)
        {
            if (_lightGo != null)
                _lightGo.SetActive(false);
            return;
        }

        int floor = FloorManager.currentFloor;
        float intens = IntensityForFloor(floor);
        if (intens <= 0f)
        {
            if (_lightGo != null)
                _lightGo.SetActive(false);
            return;
        }

        EnsureLightObject();
        _lightGo.SetActive(true);
        _light.enabled = true;

        Vector3 hit = cc.GetWorldPosFromMousePos(floor);
        float f = floor;
        float camZ = cc.mainCamera.transform.position.z;
        float zTowardCam = camZ == f ? -DepthTowardCameraFromFloor : Mathf.Sign(camZ - f) * DepthTowardCameraFromFloor;
        _lightGo.transform.position = new Vector3(hit.x, hit.y, f + zTowardCam);

        _light.intensity = intens;
    }
}
