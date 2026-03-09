using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> NextWaypoints = new();
    // Эта функция автоматически рисует линии в редакторе Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Цвет линий

        // Рисуем шарик на месте самого вейпоинта
        Gizmos.DrawSphere(transform.position, 0.5f);

        // Рисуем линии ко всем следующим точкам
        if (NextWaypoints != null)
        {
            foreach (var next in NextWaypoints)
            {
                if (next != null)
                {
                    Gizmos.DrawLine(transform.position, next.transform.position);
                }
            }
        }
    }
}
