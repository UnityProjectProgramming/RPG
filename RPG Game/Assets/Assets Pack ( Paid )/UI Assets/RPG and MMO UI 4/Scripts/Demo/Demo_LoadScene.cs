using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuloGames.UI
{
    public class Demo_LoadScene : MonoBehaviour
    {
        public string scene;

        public void LoadScene()
        {
            if (!string.IsNullOrEmpty(this.scene))
            {
                int id;
                bool isNumeric = int.TryParse(this.scene, out id);

                if (isNumeric)
                {
                    SceneManager.LoadScene(id);
                }
                else
                {
                    SceneManager.LoadScene(this.scene);
                }
            }
        }
    }
}
