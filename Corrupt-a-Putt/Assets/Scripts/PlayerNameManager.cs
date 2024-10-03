using UnityEngine;
using TMPro;

public class PlayerNameFormatter : MonoBehaviour
{
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TMP_InputField inputField3;
    public TMP_InputField inputField4;
    public TMP_InputField inputField5;

    public TMP_Text outputText;  // The TMP element where the final text will be displayed

    private string playerName1;
    private string playerName2;
    private string playerName3;
    private string playerName4;
    private string playerName5;

    private void Start()
    {
        LoadPlayerNames();
        UpdateOutputText();  // Update the text on start if needed
    }

    // Save the current names and update the output text
    public void SavePlayerNames()
    {
        playerName1 = inputField1.text;
        playerName2 = inputField2.text;
        playerName3 = inputField3.text;
        playerName4 = inputField4.text;
        playerName5 = inputField5.text;

        PlayerPrefs.SetString("PlayerName1", playerName1);
        PlayerPrefs.SetString("PlayerName2", playerName2);
        PlayerPrefs.SetString("PlayerName3", playerName3);
        PlayerPrefs.SetString("PlayerName4", playerName4);
        PlayerPrefs.SetString("PlayerName5", playerName5);

        PlayerPrefs.Save();

        UpdateOutputText();  // Refresh the output text with the new names
    }

    // Load saved names from PlayerPrefs
    private void LoadPlayerNames()
    {
        playerName1 = PlayerPrefs.GetString("PlayerName1", "");
        playerName2 = PlayerPrefs.GetString("PlayerName2", "");
        playerName3 = PlayerPrefs.GetString("PlayerName3", "");
        playerName4 = PlayerPrefs.GetString("PlayerName4", "");
        playerName5 = PlayerPrefs.GetString("PlayerName5", "");

        inputField1.text = playerName1;
        inputField2.text = playerName2;
        inputField3.text = playerName3;
        inputField4.text = playerName4;
        inputField5.text = playerName5;
    }

    // Update the TMP text element with the formatted string
    private void UpdateOutputText()
    {
        string corruptedByText = "Corrupted by ";

        if (!string.IsNullOrEmpty(playerName1)) corruptedByText += playerName1;
        if (!string.IsNullOrEmpty(playerName2)) corruptedByText += " & " + playerName2;
        if (!string.IsNullOrEmpty(playerName3)) corruptedByText += " & " + playerName3;
        if (!string.IsNullOrEmpty(playerName4)) corruptedByText += " & " + playerName4;
        if (!string.IsNullOrEmpty(playerName5)) corruptedByText += " & " + playerName5;

        outputText.text = corruptedByText;  // Update the TMP_Text element
    }
}
