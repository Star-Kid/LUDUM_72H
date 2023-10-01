using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FurnitureType
{
    Refrigerator, Stove, Toilet, Sofa, Type5, Type6
}

public enum FurnitureColor
{
    Red, Green, Yellow, Orange
}

public enum CellType
{
    Furniture, Exit, Empty
}

public struct Cell
{
    public CellType cType;
    public FurnitureType fType;
    public FurnitureColor fColor;
    //info about the borders should be here too, I guess
}

public class FieldModel : MonoBehaviour
{
    const int fieldHeight = 10;
    const int fieldWidth = 10;
    char[,] rawCells;
    public Cell[,] cells { private set; get; }
    //byte[,] walls

    int numberOfColors;
    int numberOfFurnitures;
    bool[][,] furnitureForms;

    void Awake()
    {
        numberOfColors = Enum.GetNames(typeof(FurnitureColor)).Length;
        numberOfFurnitures = 4; //can be any, numberOfFurnitures should be >= numberOfColors

        FillFurnitureForms();
        GenerateRawFieldData(); //char generation for now, reading from file - later
        FillCellsFromRawData();
    }

    void FillFurnitureForms()
    {
        furnitureForms = new bool[numberOfFurnitures][,];
        //form presentation by horizontal columns, upside down
        furnitureForms[0] = new bool[,] { { true,  true , true } };
        furnitureForms[1] = new bool[,] { { true } };
        furnitureForms[2] = new bool[,] { { true } };
        furnitureForms[3] = new bool[,] { { true, true }, { false, true } };
        for (int i = 4; i < furnitureForms.Length; i++)
        {
            furnitureForms[i] = new bool[,] { { true } };
        }

        /*int debugIdx = 3;
        Debug.Log("length of furniture: " + furnitureForms[debugIdx].GetLength(0));
        Debug.Log("height of furniture: " + furnitureForms[debugIdx].GetLength(1));
        for (int j = 0; j < furnitureForms[debugIdx].GetLength(1); j++)
        {
            string form = "";
            for (int i = 0; i < furnitureForms[debugIdx].GetLength(0); i++)
                form += furnitureForms[debugIdx][i, j] ? '1' : '0';
            Debug.Log(form);
        }*/
    }

    bool CheckForEnoughSpaceForFurniture(int coord_x, int coord_y, FurnitureType f_type)
    {
        bool[,] fForm = furnitureForms[(int)f_type];
        if ((coord_x + fForm.GetLength(0) >= fieldWidth) || (coord_y + fForm.GetLength(1) >= fieldHeight))
            return false;
        for (int i=0; i < fForm.GetLength(0); i++)
            for (int j = 0; j < fForm.GetLength(1); j++)
                if (fForm[i, j])
                    if (rawCells[coord_x + i, coord_y+ j] != '0')
                        return false;
        return true;
    }

    void GenerateRawFieldData()
    {
        System.Random rnd = new System.Random();
        rawCells = new char[fieldWidth, fieldHeight];
        for (int i = 0; i < fieldWidth; i++)
            for (int j = 0; j < fieldHeight; j++)
                rawCells[i, j] = '0';

        int numberOfColors = Enum.GetNames(typeof(FurnitureColor)).Length;
        int numberOfFurnitures = 4; //can be any, numberOfFurnitures should be >= numberOfColors

        //generate furnitures (simple)
        for (int m = 0; m < numberOfFurnitures; m++) //numberOfFurnitures >= numberOfColors
            for (int k = 0; k < numberOfColors; k++)
            {
                int rndX, rndY;
                do
                {
                    rndX = rnd.Next(fieldWidth);
                    rndY = rnd.Next(fieldHeight);
                }
                while (!CheckForEnoughSpaceForFurniture(rndX, rndY, (FurnitureType)m ));

                for (int i = 0; i < furnitureForms[m].GetLength(0); i++)
                    for (int j = 0; j < furnitureForms[m].GetLength(1); j++)
                        if (furnitureForms[m][i, j])
                        {
                            rawCells[rndX+i, rndY+j] = (char)(65 + (m * numberOfFurnitures) + k);
                            //Debug.Log("Raw cell value: " + (65 + (m * numberOfFurnitures) + k).ToString());
                        }
            }

        //generate exit
        do
        {
            int rndX = rnd.Next(fieldWidth);
            int rndY = rnd.Next(fieldHeight);
            if (rawCells[rndX, rndY] == '0')
                if ((rndX == 0) || (rndY == 0) || (rndX == fieldWidth - 1) || (rndY == fieldHeight - 1))
                {
                    rawCells[rndX, rndY] = '*';
                    break;
                }
        }
        while (true);
    }

    void FillCellsFromRawData()
    {
        cells = new Cell[fieldWidth, fieldHeight];

        for (int i = 0; i < rawCells.GetLength(0); i++)
            for (int j = 0; j < rawCells.GetLength(1); j++)
                switch (rawCells[i, j])
                {
                    case '0':
                        cells[i, j].cType = CellType.Empty;
                        break;
                    case '*':
                        cells[i, j].cType = CellType.Exit;
                        break;
                    default:
                        cells[i, j].cType = CellType.Furniture;
                        //char values from 65 to 88 (ASCII) - furnitures (by type and color)
                        cells[i, j].fType = (FurnitureType)(((int)rawCells[i, j] - 65) / numberOfFurnitures);
                        cells[i, j].fColor = (FurnitureColor)(((int)rawCells[i, j] - 65) % numberOfColors);
                        break;
                }
    }

    void Update()
    {
        
    }
}
