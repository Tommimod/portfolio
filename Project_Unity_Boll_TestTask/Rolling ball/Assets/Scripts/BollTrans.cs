using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BollTrans : MonoBehaviour {

    [SerializeField]
    private List<Vector2> position_list; //список всех позиций шара
    [SerializeField]
    private List<Vector2> point_list; //список всех не пройденых позиций
    private Vector2 startPos; //начальная позиция
    private Vector2 mousePos; //позиция клика
    private Vector2 endPos; //последняя позиция
    private float speed; //скорость
    private LineRenderer lines; //линия траектории
    private Coroutine MoveIE;
    private bool go_back;
    private int minusCount;
    public Weather weather_sc;

    public Slider slider;


    // Use this for initialization
    void Start ()
    {
        lines = GetComponent<LineRenderer>();
        startPos = transform.position; 
        position_list.Add(startPos);
        endPos = transform.position;
        go_back = false;
        minusCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        speed = slider.value;

        if (Input.GetKeyDown(KeyCode.Mouse0)) //движение по ЛКМ
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                point_list.Add(mousePos);
            }
        }

        for (int i = 0; i < point_list.Count; i++)
        {
            MoveIE = StartCoroutine(Moving(i));
            return;
        }

        if (go_back == true)
        {
            transform.position = Vector2.Lerp(
            transform.position, 
            position_list[position_list.Count - minusCount], 
            speed / 100f);

            if (new Vector2(transform.position.x, transform.position.y) == 
                position_list[position_list.Count - minusCount])
            {
                minusCount+=1;
            }

            if (position_list.Count - minusCount < 0)
            {
                position_list.Clear();
                startPos = transform.position;
                position_list.Add(startPos);
                go_back = false;
                minusCount = 1;
                weather_sc.StartCoroutine("Start_cor");
            }
        }
    }

    IEnumerator Moving(int i)
    {
        if (new Vector2(transform.position.x, transform.position.y) != point_list[i])
        {
            transform.position = Vector2.Lerp(transform.position, point_list[i], speed/100f);
            endPos = transform.position;
            lines.SetPosition(0, endPos);
            lines.SetPosition(1, point_list[i]);
        }

        if (endPos == point_list[i])
        {
            point_list.Remove(point_list[i]);
            position_list.Add(endPos);
            weather_sc.StartCoroutine("Start_cor");
            yield return null;
        }
    }

    public void Button_Time()
    {
        go_back = true;

    }
}
