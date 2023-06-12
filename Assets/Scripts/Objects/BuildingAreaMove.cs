using UnityEngine;

public class BuildingAreaMove : MonoBehaviour
{
    public GameObject post;
    public float rotationSpeed = 10f;

    void Update()
    {
        // Rotaciona o objeto "post" em torno do eixo Y com uma velocidade específica
        post.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
