using Assets.Scripts.SaveLoadSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteProgressButton : MonoBehaviour
{
    public void DeleteGameProgress()
    {
        LocalStorage.DeleteProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
