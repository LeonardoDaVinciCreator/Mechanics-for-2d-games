using UnityEngine;

public class WallBase : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("ActiveBird")) return;
        gameObject.tag = "ActiveWall";
    }

    //типы:
    //подъём/спуск, 
    // уничтожающиеся, уничтожающиеся после прыжка с них, 
    // появляющиеся при приближении(доп колайдер больше чем сама стена) и исчезающая при выходе из поля
    //с аниматором или движение через код
    //уничтожающие персонажа
    //переворачивающие персонажа

    protected float timeSec;
    protected float speed;
    protected float radiusDetection;//радиус для колайдера обнаружения персонажа
}
