using System.Collections;
using UnityEngine;

public class CubeSort : MonoBehaviour
{
    [SerializeField] private CubeGenerator _cubeGenerator;
    [SerializeField] private float _changePositionDelay = 0.5f;
    
    private Cube[] _cubes;

    #region MonoBehaviour

    void Awake()
    {
        _cubeGenerator.CubesGenerated += (cubes) => _cubes = cubes;
    }

    #endregion

    public void SortCubes() => StartCoroutine(QuickSort(_cubes,0,_cubes.Length-1));

    private IEnumerator QuickSort(Cube[] cubes, int start, int end)
    {
        int leftPivot = start;
        int rightPivot = end;
        int mainstay = cubes[(start+end)/2].Value; // опорный элемент

        // сортируем массив так, чтобы слева от опорного были элементы МЕНЬШЕ, а справа - больше
        while (leftPivot<=rightPivot)
        {

            while (cubes[leftPivot].Value < mainstay) leftPivot++; // ищем объект, который БОЛЬШЕ опорного
            while (cubes[rightPivot].Value > mainstay) rightPivot--; // ищем объект, который МЕНЬШЕ опорного
            
            // меняем найденные элементы местами
            if (leftPivot<=rightPivot)
            {
                int temp = cubes[leftPivot].Value;
                cubes[leftPivot].Value = cubes[rightPivot].Value;
                cubes[rightPivot].Value = temp;

                leftPivot++;
                rightPivot--;
                
                yield return new WaitForSeconds(_changePositionDelay);
            }
        }

        // проверяем, ножно ли дальше сортировать подмассивы
        if (start < rightPivot) yield return QuickSort(cubes, start, rightPivot);
        if (end > leftPivot) yield return QuickSort(cubes, leftPivot, end);

    }
}
