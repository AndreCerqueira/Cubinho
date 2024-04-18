using System.Collections;
using UnityEngine;

public enum DayTime
{
    Day,
    Night
}

public class SkyboxColorTransition : MonoBehaviour
{
    public float score => ScoreCounter.instance.GetScore();
    public Material skyboxMaterial; // Referência para o material do céu em uso
    public Material daySkyboxMaterial; // Referência para o material do céu para o dia
    public Material nightSkyboxMaterial; // Referência para o material do céu para a noite
    public Light directionalLight; // Referência para a luz direcional
    public float transitionDuration = 10f; // Duração da transição em segundos
    private DayTime currentDayTime; // Estado atual do tempo

    void Start()
    {
        currentDayTime = DayTime.Day;
        ForceChangeToDay();
        StartCoroutine(ChangeSkyboxColor());
    }

    IEnumerator ChangeSkyboxColor()
    {
        while (true)
        {
            yield return ChangeToDayNight();
        }
    }

    IEnumerator ChangeToDayNight()
    {
        float targetScore = score + 500;
        while (score < targetScore)
        {
            yield return null;
        }

        if (currentDayTime == DayTime.Day)
        {
            yield return ChangeToNight();
        }
        else
        {
            yield return ChangeToDay();
        }
    }

    IEnumerator ChangeToDay()
    {
        float elapsedTime = 0f;
        Color startSunDiscColor = skyboxMaterial.GetColor("_SunDiscColor");
        Color startSunHaloColor = skyboxMaterial.GetColor("_SunHaloColor");
        Color startHorizonLineColor = skyboxMaterial.GetColor("_HorizonLineColor");
        Color startSkyGradientTop = skyboxMaterial.GetColor("_SkyGradientTop");
        Color startSkyGradientBottom = skyboxMaterial.GetColor("_SkyGradientBottom");
        float startLightIntensity = directionalLight.intensity;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            Color newSunDiscColor = Color.Lerp(startSunDiscColor, daySkyboxMaterial.GetColor("_SunDiscColor"), t);
            Color newSunHaloColor = Color.Lerp(startSunHaloColor, daySkyboxMaterial.GetColor("_SunHaloColor"), t);
            Color newHorizonLineColor = Color.Lerp(startHorizonLineColor, daySkyboxMaterial.GetColor("_HorizonLineColor"), t);
            Color newSkyGradientTop = Color.Lerp(startSkyGradientTop, daySkyboxMaterial.GetColor("_SkyGradientTop"), t);
            Color newSkyGradientBottom = Color.Lerp(startSkyGradientBottom, daySkyboxMaterial.GetColor("_SkyGradientBottom"), t);
            float newLightIntensity = Mathf.Lerp(startLightIntensity, 1f, t); // Intensidade máxima para o dia

            SetSkyboxColors(newSunDiscColor, newSunHaloColor, newHorizonLineColor, newSkyGradientTop, newSkyGradientBottom);
            directionalLight.intensity = newLightIntensity;

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        SetSkyboxColors(daySkyboxMaterial.GetColor("_SunDiscColor"), daySkyboxMaterial.GetColor("_SunHaloColor"),
                        daySkyboxMaterial.GetColor("_HorizonLineColor"), daySkyboxMaterial.GetColor("_SkyGradientTop"),
                        daySkyboxMaterial.GetColor("_SkyGradientBottom"));
        directionalLight.intensity = 1f; // Garantir que a intensidade final da luz seja definida corretamente
        currentDayTime = DayTime.Day;
    }

    IEnumerator ChangeToNight()
    {
        float elapsedTime = 0f;
        Color startSunDiscColor = skyboxMaterial.GetColor("_SunDiscColor");
        Color startSunHaloColor = skyboxMaterial.GetColor("_SunHaloColor");
        Color startHorizonLineColor = skyboxMaterial.GetColor("_HorizonLineColor");
        Color startSkyGradientTop = skyboxMaterial.GetColor("_SkyGradientTop");
        Color startSkyGradientBottom = skyboxMaterial.GetColor("_SkyGradientBottom");
        float startLightIntensity = directionalLight.intensity;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            Color newSunDiscColor = Color.Lerp(startSunDiscColor, nightSkyboxMaterial.GetColor("_SunDiscColor"), t);
            Color newSunHaloColor = Color.Lerp(startSunHaloColor, nightSkyboxMaterial.GetColor("_SunHaloColor"), t);
            Color newHorizonLineColor = Color.Lerp(startHorizonLineColor, nightSkyboxMaterial.GetColor("_HorizonLineColor"), t);
            Color newSkyGradientTop = Color.Lerp(startSkyGradientTop, nightSkyboxMaterial.GetColor("_SkyGradientTop"), t);
            Color newSkyGradientBottom = Color.Lerp(startSkyGradientBottom, nightSkyboxMaterial.GetColor("_SkyGradientBottom"), t);
            float newLightIntensity = Mathf.Lerp(startLightIntensity, 0.25f, t); // Intensidade mínima para a noite

            SetSkyboxColors(newSunDiscColor, newSunHaloColor, newHorizonLineColor, newSkyGradientTop, newSkyGradientBottom);
            directionalLight.intensity = newLightIntensity;

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        SetSkyboxColors(nightSkyboxMaterial.GetColor("_SunDiscColor"), nightSkyboxMaterial.GetColor("_SunHaloColor"),
                        nightSkyboxMaterial.GetColor("_HorizonLineColor"), nightSkyboxMaterial.GetColor("_SkyGradientTop"),
                        nightSkyboxMaterial.GetColor("_SkyGradientBottom"));
        directionalLight.intensity = 0.25f; // Garantir que a intensidade final da luz seja definida corretamente
        currentDayTime = DayTime.Night;
    }

    void SetSkyboxColors(Color sunDiscColor, Color sunHaloColor, Color horizonLineColor, Color skyGradientTop, Color skyGradientBottom)
    {
        skyboxMaterial.SetColor("_SunDiscColor", sunDiscColor);
        skyboxMaterial.SetColor("_SunHaloColor", sunHaloColor);
        skyboxMaterial.SetColor("_HorizonLineColor", horizonLineColor);
        skyboxMaterial.SetColor("_SkyGradientTop", skyGradientTop);
        skyboxMaterial.SetColor("_SkyGradientBottom", skyGradientBottom);
    }

    void ForceChangeToDay()
    {
        SetSkyboxColors(daySkyboxMaterial.GetColor("_SunDiscColor"), daySkyboxMaterial.GetColor("_SunHaloColor"),
                        daySkyboxMaterial.GetColor("_HorizonLineColor"), daySkyboxMaterial.GetColor("_SkyGradientTop"),
                        daySkyboxMaterial.GetColor("_SkyGradientBottom"));
        directionalLight.intensity = 1f;
        currentDayTime = DayTime.Day;
    }
}
