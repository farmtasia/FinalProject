using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryTabManager : MonoBehaviour
{
    public GameObject bagTab;
    public GameObject collectionTab;
    public GameObject mapTab;

    public Button bagButton;
    public Button collectionButton;
    public Button mapButton;

    public TextMeshProUGUI bagText;
    public TextMeshProUGUI collectionText;
    public TextMeshProUGUI mapText;

    private Color selectedColor = new Color(24f / 255f, 20f / 255f, 36f / 255f, 1f);
    private Color waitColor = new Color(1f, 226f / 225f, 184f / 255f, 1f);

    private void Start()
    {
        OpenBagTab();

        bagButton.onClick.AddListener(OpenBagTab);
        collectionButton.onClick.AddListener(OpenCollectionTab);
        mapButton.onClick.AddListener(OpenMapTab);
    }

    private void OpenBagTab()
    {
        bagTab.SetActive(true);
        bagText.color = selectedColor;
        collectionTab.SetActive(false);
        collectionText.color = waitColor;
        mapTab.SetActive(false);
        mapText.color = waitColor;
    }


    private void OpenCollectionTab()
    {
        bagTab.SetActive(false);
        bagText.color = waitColor;
        collectionTab.SetActive(true);
        collectionText.color = selectedColor;
        mapTab.SetActive(false);
        mapText.color = waitColor;
    }

    private void OpenMapTab()
    {
        bagTab.SetActive(false);
        bagText.color = waitColor;
        collectionTab.SetActive(false);
        collectionText.color = waitColor;
        mapTab.SetActive(true);
        mapText.color = selectedColor;
    }
}
