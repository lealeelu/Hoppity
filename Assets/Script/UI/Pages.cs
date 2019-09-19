using UnityEngine;
using UnityEngine.UI;

public class Pages : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pages;
    [SerializeField]
    private Button backPage;
    [SerializeField]
    private Button forwardPage;

    private int currentPage;

    public void Start()
    {
        backPage.onClick.AddListener(GoBackPage);
        forwardPage.onClick.AddListener(GoForwardPage);
    }

    private void GoBackPage()
    {
        if (currentPage > 0)
        {
            SetPage(currentPage - 1);
        }
    }

    private void GoForwardPage()
    {
        if (currentPage < pages.Length)
        {
            SetPage(currentPage + 1);
        }
    }

    void OnEnable()
    {
        SetPage(0);
    }

    public void SetPage(int pageNumber)
    {
        if (pageNumber < 0 || pageNumber >= pages.Length) return;

        for (int i = 0; i < pages.Length; i++)
        {
            if (i == pageNumber) pages[i].SetActive(true);
            else pages[i].SetActive(false);
        }
        
        //activate nav buttons based on page
        if (pageNumber == 0) backPage.gameObject.SetActive(false);
        else backPage.gameObject.SetActive(true);

        if (pageNumber == pages.Length - 1) forwardPage.gameObject.SetActive(false);
        else forwardPage.gameObject.SetActive(true);

        currentPage = pageNumber;
    }
}
