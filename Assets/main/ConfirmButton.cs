using UnityEngine;
using TMPro;

public class ConfirmButton : MonoBehaviour
{
    public TMP_InputField nameInputField;  // Input Field اللي اللاعب هيكتب فيه الاسم
    public GameObject nameScreen;          // RawImage اللي فيها الشاشة كلها

    public void OnButtonPressed()
    {
        string playerName = nameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            // حفظ الاسم على الجهاز
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save();

            // اخفاء الـ RawImage كلها
            nameScreen.SetActive(false);

            // بدء اللعبة
            Debug.Log("Game started with player: " + PlayerPrefs.GetString("PlayerName"));
        }
        else
        {
            Debug.Log("Player must enter a name!");
        }
    }
}