using UnityEngine;


namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {

        const string DEFAULT_SAVE_FILE = "save";

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
            }

         
        }
    }

}
