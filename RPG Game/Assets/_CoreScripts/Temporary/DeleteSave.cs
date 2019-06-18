using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
using RPG.Saving;

public class DeleteSave : MonoBehaviour
{
    public void RemoveSaveFile()
    {
        FindObjectOfType<SavingWrapper>().DeleteSave();
    }

}
