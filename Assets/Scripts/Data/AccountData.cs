using UnityEngine;

public partial class Data 
{
    public string Id => PlayerPrefs.GetString(Keys.Id);
    public string Password => PlayerPrefs.GetString(Keys.Password);
    public bool IsAutoSignInChecked => PlayerPrefs.HasKey(Keys.AutoSignIn);
}
