﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 
namespace WindowsFormsApp4
{
    public class MiscellaneousTesting
    {
        public MiscellaneousTesting()
        {            
        }

        public void Test1()
        {
            double[] myarray = new double[] { -1.2, 2.4, 67.1, 54, 243, 212, -23.4, 353.1, -44, 347.7, 656.1, -464 };
            Console.WriteLine("My old array length is " + myarray.Length);
            for (int i = 0; i < myarray.Length; i++)
            {
                Console.Write("\t " + myarray[i]);
            }

            Array.Copy(myarray, 3, myarray, 0, myarray.Length - 3);
            int countcount = myarray.Length - 3;
            myarray[countcount++] = 32321;
            myarray[countcount++] = -31.32;
            myarray[countcount++] = 233.1;
            Console.WriteLine("\n My new array length is " + myarray.Length + " first element is " + myarray[0] + " and count = " + countcount);
            for (int i = 0; i < myarray.Length; i++)
            {
                Console.Write("\t " + myarray[i]);
            }
            Console.WriteLine("\nWasup");
        }
        
        public void Test2()
        {
            string filepath = @"C:\Users\Towle\Desktop\Tuan\general_towle\data\mystft.csv";
            int num = 200;
            double[] col0 = new double[num];
            double[] col1 = new double[num];
            double[] col2 = new double[num];
            double[] col3 = new double[num]; 
            for (int i = 0; i < num; i++)
            {
                col0[i] = i * 0.1;
                col1[i] = i;
                col2[i] = i * (i + 1) - 0.1;
                col3[i] = Math.Log(i+1); 
            }
            File.WriteAllText(filepath, "C0\n");
            File.AppendAllLines(filepath,
                col0.Select(d => d.ToString()));
            var next_col = File.ReadLines(filepath)
                .Select((line, index) => index == 0 
                ? line + ";Col1" 
                : line + ";" + col1[index-1].ToString())
                .ToList();
            File.WriteAllLines(filepath, next_col);
            next_col = File.ReadLines(filepath)
                .Select((line, index) => index == 0
                ? line + ";Col2"
                : line + ";" + col2[index-1].ToString())
                .ToList();
            File.WriteAllLines(filepath, next_col);
            next_col = File.ReadLines(filepath)
                .Select((line, index) => index == 0
                ? line + ";Col3"
                : line + ";" + col3[index-1].ToString())
                .ToList();
            File.WriteAllLines(filepath, next_col);
        }
    }
}