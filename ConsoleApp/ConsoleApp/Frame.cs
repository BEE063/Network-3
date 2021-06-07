using ConsoleApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PP_lab1
{
    public class Frame
    {
        public static BitArray[] GenerateData(string message)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(message);
            BitArray messageArray = new BitArray(bytes);

            int[] messageIntArray = new int[messageArray.Length];
            for (int i = 0; i < messageIntArray.Length; i++)
            {
                if (messageArray[i] == true)
                {
                    messageIntArray[i] = 1;
                }
                else
                {
                    messageIntArray[i] = 0;
                }
            }

            var checkStr = string.Join(" ", Array.ConvertAll(VerticalSum(ToTwoDimensionalArray(messageIntArray, bytes.Length, 8)), x => x.ToString()));

            BitArray bitArray = new BitArray(8);
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (VerticalSum(ToTwoDimensionalArray(messageIntArray, bytes.Length, 8))[i] == 1)
                {
                    bitArray[i] = true;
                }
                else
                {
                    bitArray[i] = false;
                }
            }


            BitArray total = new BitArray(bitArray.Length + messageArray.Length);

            for (int i = 0; i < bitArray.Length; i++)
            {
                total[i] = bitArray[i];
            }
            for (int i = bitArray.Length; i < messageArray.Length; i++)
            {
                total[i] = messageArray[i - bitArray.Length];
            }


            double package = messageArray.Length / 56;
            BitArray[] bitArrayMatrix = new BitArray[(int)(Math.Ceiling(package))];

            if (total.Length > 56)
            {
                Packages(total, 2);
            }

            return Packages(total, (int)Math.Ceiling(package));
        }
        public static int[,] ToTwoDimensionalArray(int[] array, int height, int width)
        {
            int[,] a = new int[height, width];
            for (int i = 0, c = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++, c++)
                {
                    if (c == array.Length) return a;
                    a[i, j] = array[c];
                }
            }
            return a;
        }
        public static BitArray[] Packages(BitArray array, int height)
        {

            BitArray[] bitArray = new BitArray[height];
            for (int i = 0, c = 0; i < height; i++)
            {
                BitArray ba = new BitArray(array.Count / height);
                for (int j = 0; j < array.Count / height; j++, c++)
                {
                    ba.Set(j, array[c]);
                }
                bitArray[i] = ba;
            }
            return bitArray;
        }

        public static int[] VerticalSum(int[,] array)
        {
            int[] sumColumn = new int[8];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sumColumn[j] += array[i, j];
                }

            }
            for (int i = 0; i < sumColumn.Length; i++)
            {
                if (sumColumn[i] % 2 == 0)
                {
                    sumColumn[i] = 1;
                }
                else
                {
                    sumColumn[i] = 0;
                }
            }
            return sumColumn;
        }

        public static BitArray[] GenerateErrorData(string message)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(message);
            BitArray messageArray = new BitArray(bytes);
            int checkSum = 0;
            for (int i = 0; i < messageArray.Length; i += 2)
            {
                if (messageArray[i] == true)
                {
                    checkSum += 1;
                }
            }

            string temp = checkSum.ToString() + message;
            byte[] bytesTemp = System.Text.Encoding.Unicode.GetBytes(temp);
            BitArray array = new BitArray(bytesTemp);
            for (int i = 50; i < 64; i++)
            {
                if (array[i] == true)
                {
                    array[i] = false;
                }
                else
                {
                    array[i] = true;
                }
            }


            return Packages(array, 84); ;
        }

        private static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static void GenerateReceipt(BitArray[] array, bool isValid)
        {
            BitArray bt = new BitArray(1);
            if (isValid == true)
            {
                bt[0] = true;
                array[0] = bt;
            }
            else
            {
                bt[0] = false;
                array[0] = bt;
            }
        }
        public static void GenerateRequest(BitArray[] array, bool isValid)
        {
            if (isValid == true)
            {
                array[0][0] = true;
            }
            else
            {
                array[0][0] = false;
            }
        }


    }
}

