using UnityEngine;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    public string[] lines;
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    private int index = 0;

    public void TriggerDialog()
    {
        if (dialogPanel == null || dialogText == null) return;

        if (!dialogPanel.activeSelf)
        {
            index = 0;
            dialogPanel.SetActive(true);
            dialogText.text = lines.Length > 0 ? lines[0] : "";
        }
        else
        {
            index++;
            if (index < lines.Length) dialogText.text = lines[index];
            else dialogPanel.SetActive(false);
        }
    }
}

