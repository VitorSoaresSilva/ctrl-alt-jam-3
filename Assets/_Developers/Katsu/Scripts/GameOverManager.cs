using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameOverManager : MonoBehaviour
{
    public string carTag = "Player"; // Tag do carro
    public CarDamage car; // Refer�ncia ao script CarDamage do carro
    public Image gameOverImage; // Imagem de Game Over
    public AudioClip gameOverMusic; // M�sica de Game Over
    public AudioMixerGroup audioMixer; // AudioMixer para controlar o volume da m�sica
    public float delayBeforeMenu = 10f; // Atraso antes de voltar ao menu

    private AudioSource audioSource;

    void Start()
    {
        // Crie um novo AudioSource para tocar a m�sica de Game Over
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameOverMusic;
        audioSource.outputAudioMixerGroup = audioMixer;
        audioSource.Play();

        Invoke(nameof(GoToMenu), delayBeforeMenu);
    }

    void GoToMenu()
    {
        Debug.Log("GoToMenu");
        SceneControl.instance.ChangeScene("MainMenu");
    }

    void Update()
    {
        //// Verifique se a sa�de do carro � 0 ou menos, ou se os pontos s�o 0 ou menos
        //if (car != null && (car.health <= 0 || CareerPoints.instance.Points <= 0))
        //{
        //    // O jogo acabou, fa�a algo aqui (por exemplo, exibir uma tela de Game Over)
        //    gameOverImage.enabled = true; // Mostre a imagem de Game Over
        //    audioSource.Play(); // Toque a m�sica de Game Over

        //    // Reset os pontos
        //    CareerPoints.instance.SetPoints(0);

        //    // Aguarde alguns segundos e vai para a cena de game over
        //    GoToGameOverScreen();
        //}
    }

    //void GoToGameOverScreen()
    //{
    //    SceneControl.instance.ChangeScene("GameOver");
    //}
}
