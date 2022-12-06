using UnityEngine;

public partial class Data
{
    public struct NewData
    {
        public int? Points { get; internal set; }
        public int? Level { get; internal set; }
        public int? SelectedTankIndex { get; internal set; }

        //SignIn or SingUp
        public string Id { get; internal set; }
        public string Password { get; internal set; }
        public int? AutoSignIn { get; internal set; }
        public int? IsWindToggleOn { get; internal set; }
        public int? DifficultyLevel { get; internal set; }
        public int? RoundTime { get; internal set; }
    }

    public void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    public void SetData(NewData newData)
    {
        if (newData.Points != null) PlayerPrefs.SetInt(Keys.Points, (int)newData.Points);
        if (newData.Level != null) PlayerPrefs.SetInt(Keys.Level, (int)newData.Level);
        if (newData.SelectedTankIndex != null) PlayerPrefs.SetInt(Keys.SelectedTankIndex, (int)newData.SelectedTankIndex);
        if (newData.Id != null) PlayerPrefs.SetString(Keys.Id, newData.Id);
        if (newData.Password != null) PlayerPrefs.SetString(Keys.Password, newData.Password);
        if (newData.AutoSignIn != null) PlayerPrefs.SetInt(Keys.AutoSignIn, (int)newData.AutoSignIn);
        if (newData.IsWindToggleOn != null) PlayerPrefs.SetInt(Keys.MapWind, (int)newData.IsWindToggleOn);
        if (newData.DifficultyLevel != null) PlayerPrefs.SetInt(Keys.DifficultyLevel, (int)newData.DifficultyLevel);
        if (newData.RoundTime != null) PlayerPrefs.SetInt(Keys.RoundTime, (int)newData.RoundTime);
    }

    public void DeleteData(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
