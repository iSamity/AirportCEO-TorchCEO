using UnityEngine;
using TorchCEO.Config;

namespace TorchCEO.Flashlight;

/// <summary>
/// In-game only: a spotlight aimed at the floor under the cursor on the active floor plane.
/// </summary>
sealed class CursorFlashlightController : MonoBehaviour
{
    /// <summary>Unity spot light maximum practical cone angle (degrees).</summary>
    private const float SpotlightSpotAngleDegrees = 179f;

    /// <summary>Offset along world Z toward the camera from the floor hit (world units).</summary>
    private const float DepthTowardCameraFromFloor = 40f;

    private GameObject _lightGo;
    private Light _light;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DefaultConfig.CursorFlashlightEnabled.SettingChanged += OnEnabledChanged;
        DefaultConfig.CursorFlashlightIntensityBelowGround.SettingChanged += OnLightParamsChanged;
        DefaultConfig.CursorFlashlightIntensityAboveGroundDay.SettingChanged += OnLightParamsChanged;
        DefaultConfig.CursorFlashlightIntensityAboveGroundNight.SettingChanged += OnLightParamsChanged;
        DefaultConfig.CursorFlashlightRange.SettingChanged += OnLightParamsChanged;
        EnsureLightObject();
        ApplyEnabledFromConfig();
        ApplyLightParamsFromConfig();
    }

    private void OnDestroy()
    {
        DefaultConfig.CursorFlashlightEnabled.SettingChanged -= OnEnabledChanged;
        DefaultConfig.CursorFlashlightIntensityBelowGround.SettingChanged -= OnLightParamsChanged;
        DefaultConfig.CursorFlashlightIntensityAboveGroundDay.SettingChanged -= OnLightParamsChanged;
        DefaultConfig.CursorFlashlightIntensityAboveGroundNight.SettingChanged -= OnLightParamsChanged;
        DefaultConfig.CursorFlashlightRange.SettingChanged -= OnLightParamsChanged;
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
        _light.type = LightType.Spot;
        _light.shadows = LightShadows.None;
        ApplyFlashlightShapeFromConfig();
    }

    private static void ApplyFlashlightShapeToLight(Light light)
    {
        float range = Mathf.Max(0.05f, DefaultConfig.CursorFlashlightRange.Value);
        light.range = range;
        light.spotAngle = SpotlightSpotAngleDegrees;
        light.innerSpotAngle = 0f;
    }

    private void ApplyFlashlightShapeFromConfig()
    {
        if (_light == null)
            return;
        ApplyFlashlightShapeToLight(_light);
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
        // When enabled, visibility and intensity are applied in LateUpdate (above-ground 0 = stay off for bright sun).
    }

    private void ApplyLightParamsFromConfig()
    {
        if (_light == null)
            return;
        ApplyFlashlightShapeFromConfig();
        _light.intensity = IntensityAfterRangeCompensation(IntensityForFloor(FloorManager.currentFloor));
    }

    /// <summary>
    /// Matches vanilla mouse-light gating: use "night" torch sensitivity when outdoor lighting is low (sun intensity).
    /// See <see cref="LightController.OutsideLightsShouldBeTurnedOn"/>.
    /// </summary>
    private static bool ShouldUseNightTorchSensitivity()
    {
        var lc = Singleton<LightController>.Instance;
        return lc != null && lc.OutsideLightsShouldBeTurnedOn;
    }

    private static float IntensityForFloor(int floorZ)
    {
        if (floorZ < 0)
            return DefaultConfig.CursorFlashlightIntensityBelowGround.Value;
        return ShouldUseNightTorchSensitivity()
            ? DefaultConfig.CursorFlashlightIntensityAboveGroundNight.Value
            : DefaultConfig.CursorFlashlightIntensityAboveGroundDay.Value;
    }

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
        float baseIntens = IntensityForFloor(floor);
        if (baseIntens <= 0f)
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
        float depth = Mathf.Max(0.01f, DepthTowardCameraFromFloor);
        float zTowardCam = camZ == f ? -depth : Mathf.Sign(camZ - f) * depth;
        var lightPos = new Vector3(hit.x, hit.y, f + zTowardCam);
        _lightGo.transform.position = lightPos;

        var aimOnFloor = new Vector3(hit.x, hit.y, f);
        Vector3 dir = aimOnFloor - lightPos;
        if (dir.sqrMagnitude > 1e-8f)
            _lightGo.transform.rotation = Quaternion.LookRotation(dir);

        _light.intensity = IntensityAfterRangeCompensation(baseIntens);
    }

    /// <summary>
    /// Unity spot attenuation makes surfaces brighter at the same distance when range increases; scale down so large ranges do not wash out the center.
    /// </summary>
    private static float IntensityAfterRangeCompensation(float baseIntensity)
    {
        float r = Mathf.Max(0.05f, DefaultConfig.CursorFlashlightRange.Value);
        float scale = Mathf.Min(1f, DefaultConfig.CursorFlashlightRangeDefault / r);
        return baseIntensity * scale;
    }
}
