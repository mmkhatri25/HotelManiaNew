using UnityEngine;
using UnityEngine.SceneManagement;

public class DevManager : MonoBehaviour
{
	public void ClearAllData()
	{
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
