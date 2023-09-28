using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int ZoomLevel = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 双曲函数
    /// </summary>
    /// <returns></returns>
    private float Sinh(float x)
    {
        float ax = (Mathf.Exp(x) - Mathf.Exp(-1.0f * x)) / 2;
        return ax;
    }

    /// <summary>
    /// 根据经纬度获取目标瓦片位置
    /// </summary>
    /// <param name="longtitude">经度</param>
    /// <param name="latitude">纬度</param>
    /// <returns></returns>
    private Vector2 CalculateTileIndex(double longtitude, double latitude)
    {
        int x = (int)(Mathf.Pow(2, ZoomLevel - 1) * (longtitude / 180 + 1));
        int y = (int)(Mathf.Pow(2, ZoomLevel - 1) * (1 - Mathf.Log((Mathf.Tan(Mathf.PI * (float)latitude / 180) + 1 / Mathf.Cos(Mathf.PI * (float)latitude / 180)), 2.7182818f) / Mathf.PI));
        return new Vector2(x, y);
    }

    /// <summary>
    /// 根据瓦片编号获取经纬度信息
    /// </summary>
    /// <param name="xtile">瓦片x编号</param>
    /// <param name="ytile">瓦片y编号</param>
    /// <returns></returns>
    private Vector2 CalculateLongAndLa(int xtile, int ytile)
    {
        float n = Mathf.Pow(2, ZoomLevel);
        float longtitude = xtile / n * 360.0f - 180.0f;
        float latitude = Mathf.Atan(Sinh(Mathf.PI * (1 - 2 * ytile / n))) * 180.0f / Mathf.PI;
        return new Vector2(longtitude, latitude);
    }

    /// <summary>
    /// 经纬度坐标转像素坐标
    /// </summary>
    /// <param name="longtitude">经度</param>
    /// <param name="latitude">纬度</param>
    /// <returns>像素坐标值</returns>
    private Vector2 LongAndLaToPixel(float longtitude, float latitude)
    {
        float pixelX = (longtitude + 180 / 360) * Mathf.Pow(2, ZoomLevel) * 256 % 256;
        float pixelY = (1 - Mathf.Log(Mathf.Tan(latitude * Mathf.PI / 180)
            + 1 / (Mathf.Cos(latitude * Mathf.PI / 180)), 2.7182818f) / (2 * Mathf.PI)) * Mathf.Pow(2, ZoomLevel) * 256 % 256;
        return new Vector2(pixelX, pixelY);
    }

    /// <summary>
    /// 根据瓦片上某一点的像素坐标得到经纬度坐标
    /// </summary>
    /// <param name="tileX">瓦片X编号</param>
    /// <param name="tileY">瓦片Y编号</param>
    /// <param name="pixelX">目标点X像素坐标</param>
    /// <param name="pixelY">目标点Y像素坐标</param>
    /// <returns></returns>
    private Vector2 PixelToLongAndLa(float tileX, float tileY, float pixelX, float pixelY)
    {
        float longtitude = (tileX + pixelX / 256) / (Mathf.Pow(2, ZoomLevel)) * 360 - 180.0f;
        float latitude = (Mathf.Asin(Sinh(Mathf.PI - 2 * Mathf.PI * (tileY + pixelY / 256) / (Mathf.Pow(2, ZoomLevel))))) * 180 / Mathf.PI;
        return new Vector2(longtitude, latitude);
    }
}