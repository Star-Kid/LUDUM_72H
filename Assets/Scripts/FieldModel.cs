using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//numberOfFurnitures should be >= numberOfColors
public enum FurnitureType
{
    Coach, Stove, Toilet, DoubleBed, Bookshelf, Lamp, MagazineTable
}

public enum FurnitureColor
{
    Apple, GoldenDream, WellRead, Finn, EastSide, PoloBlue, FountainBlue
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
    int fieldWidth;
    int fieldHeight;

    char[,] rawCells;
    public Cell[,] cells { private set; get; }
    //byte[,] walls

    public int numberOfColors { private set; get; }
    public int numberOfFurnitures { private set; get; }

    public bool[][,] furnitureForms { private set; get; }

    public TextAsset levelsData;

    void Awake()
    {
        //numberOfColors = Enum.GetNames(typeof(FurnitureColor)).Length;
        numberOfColors = 4;
        numberOfFurnitures = 4; //can be any, numberOfFurnitures should be >= numberOfColors

        FillFurnitureForms();

        LoadLevelDataRandom();
    }

    public void LoadLevelDataRandom()
    {
        GenerateRawFieldData();
        FillCellsFromRawData();
    }

    public void LoadLevelDataFixed(int level_idx)
    {
        ReadLevelDataFromFileByIndex(level_idx);
        FillCellsFromRawData();
    }

    public void LoadLevelDataEditor(int width, int height)
    {
        GenerateEmptyFieldData(width, height);
        FillCellsFromRawData();
    }

    void FillFurnitureForms()
    {
        furnitureForms = new bool[numberOfFurnitures][,];
        for (int i = 0; i < furnitureForms.Length; i++)
        {
            furnitureForms[i] = new bool[,] { { true } };
        }
        //Describe/change furniture forms here
        //form presentation by vertical columns, upside down
        furnitureForms[0] = new bool[,] { { true,  true } }; //coach
        furnitureForms[3] = new bool[,] { { true, true }, { true, true } }; //double bed

    }

    bool CheckForEnoughSpaceForFurniture(int coord_x, int coord_y, FurnitureType f_type)
    {
        bool[,] fForm = furnitureForms[(int)f_type];
        if ((coord_x + fForm.GetLength(0) > fieldWidth) || (coord_y + fForm.GetLength(1) > fieldHeight))
            return false;
        for (int i=0; i < fForm.GetLength(0); i++)
            for (int j = 0; j < fForm.GetLength(1); j++)
                if (fForm[i, j])
                    if (rawCells[coord_x + i, coord_y+ j] != '~')
                        return false;
        return true;
    }

    void GenerateRawFieldData()
    {
        System.Random rnd = new System.Random();
        fieldWidth = rnd.Next(9, 12); //leesser values than 9 can procude endless cycle
        fieldHeight = rnd.Next(9, 12);

        rawCells = new char[fieldWidth, fieldHeight];
        for (int i = 0; i < fieldWidth; i++)
            for (int j = 0; j < fieldHeight; j++)
                rawCells[i, j] = '~';

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
            if (rawCells[rndX, rndY] == '~')
                if ((rndX == 0) || (rndY == 0) || (rndX == fieldWidth - 1) || (rndY == fieldHeight - 1))
                {
                    rawCells[rndX, rndY] = '!';
                    break;
                }
        }
        while (true);
    }

    void GenerateEmptyFieldData(int width, int height)
    {
        fieldWidth = width;
        fieldHeight = height;

        rawCells = new char[fieldWidth, fieldHeight];
        for (int i = 0; i < fieldWidth; i++)
            for (int j = 0; j < fieldHeight; j++)
                rawCells[i, j] = '~';
    }

    void FillCellsFromRawData()
    {
        cells = new Cell[fieldWidth, fieldHeight];

        for (int i = 0; i < rawCells.GetLength(0); i++)
            for (int j = 0; j < rawCells.GetLength(1); j++)
                switch (rawCells[i, j])
                {
                    case '~':
                        cells[i, j].cType = CellType.Empty;
                        break;
                    case '!':
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

    public void ReadLevelDataFromFileByIndex(int level_idx)
    {
        string[] data = levelsData.text.Split(Environment.NewLine);
        int currentLevelIdx = 0;
        int currentRowCounter = 0;
        int currentMaxLenghOfString = 0;

        foreach (string s in data)
        {
            //Debug.Log(s);
            if (String.IsNullOrWhiteSpace(s))
            {
                if (currentLevelIdx == level_idx)
                    break;
                currentLevelIdx++;
                currentRowCounter = 0;
                currentMaxLenghOfString = 0;
            }
            else
            {
                if (s.Length > currentMaxLenghOfString)
                    currentMaxLenghOfString = s.Length;
                currentRowCounter++;
            }
        }
        //Debug.Log("Level #" + level_idx + "reading has eneded. It has " + currentRowCounter + " rows");
        //Debug.Log("Max Length of row is " + currentMaxLenghOfString);

        fieldWidth = currentMaxLenghOfString;
        fieldHeight = currentRowCounter;

        rawCells = new char[fieldWidth, fieldHeight];

        int idxStringStart = -1;
        currentLevelIdx = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (String.IsNullOrEmpty(data[i]))
                if (idxStringStart == -1)
                    currentLevelIdx++;
                else
                    break;

            if (idxStringStart == -1)
            {
                if ((currentLevelIdx == level_idx) && !String.IsNullOrEmpty(data[i]))
                    idxStringStart = i;
            }
            if (idxStringStart != -1)
            {
                for (int k = 0; k < data[i].Length; k++)
                    rawCells[k, i - idxStringStart] = data[i][k];
                for (int k = data[i].Length; k< rawCells.GetLength(0); k++)
                    rawCells[k, i - idxStringStart] = '~';
            }
        }
    }

    public void TryToPlaceNewFurnitureToCoords(int x, int y, FurnitureType f_type, FurnitureColor f_color)
    {
        if (CheckForEnoughSpaceForFurniture(x, y, f_type))
        {
            int idx_type = (int)f_type;
            int idx_color = (int)f_color;

            for (int i = 0; i < furnitureForms[idx_type].GetLength(0); i++)
                for (int j = 0; j < furnitureForms[idx_type].GetLength(1); j++)
                    if (furnitureForms[idx_type][i, j])
                    {
                        rawCells[x + i, y + j] = (char)(65 + (idx_type * numberOfFurnitures) + idx_color);
                        Debug.Log("Setting cell: " + (x + i) + "," + (y + j) + " to value: " + rawCells[x + i, y + j]);
                    }
            FillCellsFromRawData();
        }
        else
            Debug.Log("Not Enough place for new furniture!");
    }

    public bool ClearCellOrFurnitureCellsIfNotEmpty(int x, int y)
    {
        bool isNotEmpty = (cells[x,y].cType != CellType.Empty);
        if (isNotEmpty)
        {
            if (cells[x, y].cType == CellType.Exit)
            {
                rawCells[x, y] = '~';
            }
            else
            {
                FurnitureType fType = cells[x, y].fType;
                FurnitureColor fColor = cells[x, y].fColor;
                int fLeftmostX = cells.GetLength(0);
                int fUpmostY = cells.GetLength(1);
                for (int i = 0; i < cells.GetLength(0); i++)
                    for (int j = 0; j < cells.GetLength(1); j++)
                        if ((cells[i, j].fType == fType) && (cells[i, j].fColor == fColor) && (cells[i, j].cType == CellType.Furniture))
                        {
                            if (isThereIsÑontinuosWayFromPossibleFurniturePointToClickedPoint(i, j, x, y, fType, fColor))
                            {
                                //åñëè íà ïîëå äâå îäèíàêîâûå ìåáåëè (ïî öâåòó è ôîðìå, áóäåò ãëþê - ìîæåò óäàëèòüñÿ íå òî
                                if (i < fLeftmostX) fLeftmostX = i;
                                if (j < fUpmostY) fUpmostY = j;
                            }
                        }

                int fFormIndex = (int)fType;
                for (int i = 0; i < furnitureForms[fFormIndex].GetLength(0); i++)
                    for (int j = 0; j < furnitureForms[fFormIndex].GetLength(1); j++)
                        if (furnitureForms[fFormIndex][i, j])
                        {
                            rawCells[fLeftmostX + i, fUpmostY + j] = '~';
                        }
            }
            FillCellsFromRawData();
        }
        return isNotEmpty;
    }

    bool isThereIsÑontinuosWayFromPossibleFurniturePointToClickedPoint(int point_start_x, int point_start_y, int point_finish_x, int point_finish_y, FurnitureType f_type, FurnitureColor f_color)
    {
        if ((point_start_x == point_finish_x) && (point_start_y == point_finish_y))
            return true;

        bool noWayAtTheRight = false;
        if (point_start_x + 1 < cells.GetLength(0))
        {
            if ((point_start_x + 1 == point_finish_x) && (point_start_y == point_finish_y))
                return true;

            if ((cells[point_start_x + 1, point_start_y].cType == CellType.Furniture) &&
                (cells[point_start_x + 1, point_start_y].fType == f_type) &&
                (cells[point_start_x + 1, point_start_y].fColor == f_color))
            {
                noWayAtTheRight = !isThereIsÑontinuosWayFromPossibleFurniturePointToClickedPoint(point_start_x + 1, point_start_y, point_finish_x, point_finish_y, f_type, f_color);
            }
            else
                noWayAtTheRight = true;
        }
        else
            noWayAtTheRight = true;

        if (!noWayAtTheRight)
            return true;

        if (point_start_y + 1 < cells.GetLength(1))
        {
            if ((point_start_x == point_finish_x) && (point_start_y + 1 == point_finish_y))
                return true;

            if ((cells[point_start_x, point_start_y + 1].cType == CellType.Furniture) &&
                (cells[point_start_x, point_start_y + 1].fType == f_type) &&
                (cells[point_start_x, point_start_y + 1].fColor == f_color))
            {
                return isThereIsÑontinuosWayFromPossibleFurniturePointToClickedPoint(point_start_x, point_start_y + 1, point_finish_x, point_finish_y, f_type, f_color);
            }
            else
                return false;
        }
        else
            return false;
    }

    public string ExtractCurrentFieldRawData()
    {
        string res = "";
        
        for (int j = 0; j < fieldHeight; j++)
        {
            for (int i = 0; i < fieldWidth; i++)
                res += rawCells[i, j];
            res += Environment.NewLine;
        }
        return res;
    }
}

public static class ColorConverter
{
    public static Color GetColorFromFurnitureColor(FurnitureColor f_color)
    {
        //return new[] { SomeEnum.Item1, SomeEnum.Item2 };
        Color clr = Color.magenta;
        switch (f_color)
        {
            case FurnitureColor.Apple:
                clr = new Color32(0x6F, 0xA1, 0x42, 0xFF);
                break;
            case FurnitureColor.GoldenDream:
                clr = new Color32(0xF1, 0xCD, 0x38, 0xFF);
                break;
            case FurnitureColor.WellRead:
                clr = new Color32(0xB5, 0x41, 0x31, 0xFF);
                break;
            case FurnitureColor.Finn:
                clr = new Color32(0x6D, 0x31, 0x4D, 0xFF);
                break;
            case FurnitureColor.EastSide:
                clr = new Color32(0xB3, 0x84, 0xD2, 0xFF);
                break;
            case FurnitureColor.PoloBlue:
                clr = new Color32(0x83, 0x9B, 0xD1, 0xFF);
                break;
            case FurnitureColor.FountainBlue:
                clr = new Color32(0x64, 0xBB, 0xC3, 0xFF);
                break;
        }
        return clr;
    }
}