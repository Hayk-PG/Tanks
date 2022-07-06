using UnityEngine;


public class Toggle_Wind : BaseToggleCheck
{
    private Tab_SelectMaps _tabSelectMaps;


    protected override void Awake()
    {
        base.Awake();

        _tabSelectMaps = FindObjectOfType<Tab_SelectMaps>();
    }

    private void OnEnable()
    {
        _tabSelectMaps.OnTabOpened += OnTabOpened;
    }

    private void OnDisable()
    {
        _tabSelectMaps.OnTabOpened -= OnTabOpened;
    }

    private void OnTabOpened()
    {
        SimulateToggle(PlayerPrefs.HasKey(Keys.MapWind) && PlayerPrefs.GetInt(Keys.MapWind) == 1);
    }

    public override void ToggleValueChanged(bool isOn)
    {
        if (isOn)
            Data.Manager.IsWindOn = true;
        else
            Data.Manager.IsWindOn = false;

        Data.Manager.SetData(new Data.NewData { IsWindToggleOn = isOn ? 1 : 0 });
    }
}
