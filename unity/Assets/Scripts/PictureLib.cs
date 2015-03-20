﻿using System.Collections.Generic;
using UnityEngine;

public class PictureLib : MonoBehaviour
{

    Material bckg;

    public List<WWW> www = new List<WWW>();
    public List<GameObject> stand = new List<GameObject>();
    public int pages_num;//current_layout
    int pics_per_page;//current_layout
    public int current_page;//current_layout
    public int cl;//current_layout

    public int[] layout_x = { 1, 2, 1, 2, 3, 1, 3, 2, 3, 4, 2, 4, 3, 4 };

    public int[] layout_y = { 1, 1, 2, 2, 1, 3, 2, 3, 3, 2, 4, 3, 4, 4 };


    float w_max, h_max;

    //создать стенд, а точнее, группу из трех объектов: задний фон back с материалом bckg,
    //место под картинку pic и в нижнем правом углу текст txt с порядковым номером картинки
    GameObject createStand(Vector3 scale, Vector3 pos)
    {
        GameObject obj, pn, back, pic, txt;
        obj = new GameObject("pic_group");
        pn = transform.parent.transform.Find("PicNumber").gameObject;
        back = (GameObject)Instantiate(gameObject);
        pic = (GameObject)Instantiate(gameObject);
        txt = (GameObject)Instantiate(pn.gameObject);
        back.name = "back"; pic.name = "pic"; txt.name = "txt";

        obj.transform.parent = transform.parent.transform;
        back.transform.parent = obj.transform;
        pic.transform.parent = obj.transform;
        txt.transform.parent = obj.transform;

        obj.transform.localPosition = pos;
        obj.transform.localRotation = Quaternion.identity;
        back.transform.localPosition = Vector3.zero - new Vector3(0.01f, 0, 0);
        pic.transform.localPosition = Vector3.zero - new Vector3(0.02f, 0, 0);
        txt.transform.localPosition = Vector3.zero - new Vector3(0.02f, 0, 0);

        back.transform.localScale = scale; pic.transform.localScale = scale;
        back.renderer.material = bckg; pic.renderer.material = bckg;
        txt.transform.localPosition.y -= back.transform.localScale.z / 2 * 10 - 0.3;
        txt.transform.localPosition.z -= back.transform.localScale.x / 2 * 10 - 0.2;

        obj.AddComponent<BoxCollider>();
        obj.GetComponent<BoxCollider>().size = new Vector3(0.1f, scale.z * 10, scale.x * 10);
        obj.AddComponent("PictureLib_Scale");
        obj.GetComponent<PictureLib_Scale>().Wall = gameObject;

        return obj;
    }

    //загрузить картинку на ранее созданный стенд obj, отмасштабировав ее так,
    //чтобы она уместилась в области w_max на h_maх и в то же время сохранила пропорции
    public void loadPicture(GameObject obj, Texture2D texture, float w_max, float h_max)
    {
        obj.renderer.material.mainTexture = texture;
        var w = texture.width;
        var h = texture.height;
        if ((w / w_max) >= (h / h_max))
        {
            obj.transform.localScale.x = w_max;
            obj.transform.localScale.z = w_max / w * h;
        }
        else
        {
            obj.transform.localScale.x = h_max / h * w;
            obj.transform.localScale.z = h_max;
        }
    }

    //создать компоновку x на y стендов
    public void CreateLayout(int x, int y)
    {
        var w_all = this.transform.localScale.x - 0.1;
        var h_all = this.transform.localScale.z - 0.1;
        var w = w_all / x;
        var h = h_all / y;
        var x_start = w_all * 10 / 2; //домножаем на 10, т.к. масштаб плейна - это обычные координаты 1-к-10
        var y_start = h_all * 10 / 2;

        var k = 0;
        for (var j = 0; j < y; j++)
            for (var i = 0; i < x; i++)
            {
                stand[k] = createStand(new Vector3((float)(w - 0.01), 1, (float)(h - 0.01)),
                                       new Vector3(0, (float)(y_start - h * 10 / 2 - j * h * 10), (float)(x_start - w * 10 / 2 - i * w * 10)));
                k++;
            }

        w_max = (float)(w - 0.01);
        h_max = (float)(h - 0.01);
        pics_per_page = x * y;
        pages_num = (int)Mathf.Ceil((float)(www.Count / 1.0 / (x * y)));
        //у деления на 1.0 есть глубокий функциональный смысл: он добавляет float в операцию трех int'ов
        //если его убрать, то результат тоже будет целым (например, 14/(2*2)=3), и никакой Ceil не поможет
    }

    //загрузить на текущую компоновку картинки, соответствующие номеру страницы page
    //например, при компоновке 1-на-1 и странице №12 надо загрузить картинку №12
    //при компоновке 2-на-2 и странице №2 - картинки 5,6,7,8
    public void loadPics(int page)
    {
        int i, stand_length = layout_x[cl] * layout_y[cl];
        if (page != pages_num)
        {
            for (i = 0; i < stand_length; i++)
            {
                stand[i].SetActive(true);
                loadPicture(stand[i].transform.Find("pic").gameObject,
                            www[(page - 1) * pics_per_page + i].texture,
                            w_max, h_max);
                stand[i].transform.Find("txt").GetComponent<TextMesh>().text = "" + ((page - 1) * pics_per_page + i + 1);
            }
            //последняя страница уникальна тем, что может быть неполной
            //(например, компоновка 2-на-2 и всего 14 картинок - тогда на последней будут картинки 13 и 14,
            //а место под 15 и 16 останется пустым)
            //поэтому ей и выделен отдельный else
        }
        else
        {
            int pics_on_last_page = www.Count - (pages_num - 1) * pics_per_page;
            for (i = 0; i < pics_on_last_page; i++)
            {
                loadPicture(stand[i].transform.Find("pic").gameObject,
                            www[(page - 1) * pics_per_page + i].texture,
                            w_max, h_max);
                stand[i].transform.Find("txt").GetComponent<TextMesh>().text = "" + ((page - 1) * pics_per_page + i + 1);
            }
            for (i = pics_on_last_page; i < stand_length; i++) stand[i].SetActive(false);
        }
    }

    void DisplayPictures(List<DataStructures.Picture> input)
    {
        if (input.Count == 0)
        {
            transform.parent.transform.Find("LayoutButton").gameObject.active = false;
            transform.parent.transform.Find("Tip").gameObject.active = false;
            transform.parent.transform.Find("Left Arrow").gameObject.active = false;
            transform.parent.transform.Find("Right Arrow").gameObject.active = false;
            transform.parent.transform.Find("Zoom").gameObject.SetActive(false);
        }
        else
        {
            //суть >>>
            for (var i = 0; i < input.Count; i++)
            {
                www[i] = new WWW(input[i].path);
                //yield www[i];
            }
            CreateLayout(1, 1); cl = 0;
            loadPics(1); current_page = 1;
            // <<< суть
        }
    }

    void UndisplayPictures()
    {
        for (var i = 0; i < stand.Count; i++) Destroy(stand[i]);
        transform.parent.transform.Find("Zoom").gameObject.SetActive(false);
    }
}
