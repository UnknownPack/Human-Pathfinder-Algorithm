using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    
    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        var nextButton = uiDocument.rootVisualElement.Q<Button>("startButton");
        if (nextButton != null)
        {
            nextButton.clicked += LoadNextScene;
        }
    }

    private void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        } 
    }
}
