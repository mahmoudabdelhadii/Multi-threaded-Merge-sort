using System;
using System.Threading;
using System.Diagnostics;


namespace MergeSort
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int ARRAY_SIZE = 1000;
            int[] arraySingleThread = new int[ARRAY_SIZE];
            Random Rand = new Random();
            Stopwatch stopwatch = new Stopwatch();
            int[] LA =new int []{1,3,5,7,9};
            int[] RA = new int []{2,4,6,8,10};
            int[] result_Array= new int[10];


            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = Rand.Next(-1000, 1000);
            }
            //PrintArray(arraySingleThread);


            Console.WriteLine("/////////////////////////////////////////////////////////////");

            // TODO : Use the "Random" class in a for loop to initialize an array

            // copy array by value.. You can also use array.copy()
            int[] arrayMultiThread = new int[ARRAY_SIZE];
            Array.Copy(arraySingleThread, arrayMultiThread, arraySingleThread.Length);
            //PrintArray(arrayMultiThread);
            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */


            stopwatch.Start();
            //MergeSort(arraySingleThread);
            stopwatch.Stop();
            Merge(LA,RA,result_Array);
            PrintArray(result_Array);


            //TODO: Multi Threading Merge Sort
        }
        /*********************** Methods **********************
         *****************************************************/
            /*
            implement Merge method. This method takes two sorted array and
            and constructs a sorted array in the size of combined arrays
            */
            
            static int[] Merge(int[] LA, int[] RA, int[] A)
            {
                int result_length = LA.Length + RA.Length;
                int[] result_Array = new int [result_length];
                
                int index_Right = 0; 
                int index_Result = 0; 
                int index_Left = 0;
                        // TODO :implement
                while (index_Left < LA.Length || index_Right < RA.Length)
                {
                  
                if (index_Left < LA.Length && index_Right < RA.Length)  
                {  
                   
                    if (LA[index_Left] > RA[index_Right])
                    {
                        result_Array[index_Result] = RA[index_Right];
                        index_Right++;
                        index_Result++;
                    }
                    
                    else
                    {
                        result_Array[index_Result] = LA[index_Left];
                        index_Left++;
                        index_Result++;
                    }
                }
                 else if (index_Right < RA.Length)
                {
                    result_Array[index_Result] = RA[index_Right];
                    index_Right++;
                    index_Result++;
                }  
                else if (index_Left < LA.Length)
                {
                    result_Array[index_Result] = LA[index_Left];
                    index_Left++;
                    index_Result++;
                }
                
               
            }
            return result_Array;
            }

            /*
            implement MergeSort method: takes an integer array by reference
            and makes some recursive calls to intself and then sorts the array
            */
            /*
            static int[] MergeSort(int[] A)
            {
            
            int length = A.Length;
            int Midpoint = (int) Math.Floor((double) length / 2);
            int[] left;
            int[] right;
            int[] sorted_array = new int[length];

            //Array.Copy(A,Midpoint,left,0,);

            if (length < 2)
                return A;

            // TODO :implement

          }
            */

                // a helper function to print your array
            
            static void PrintArray(int[] myArray)
            {
                Console.Write("[");
                for (int i = 0; i < myArray.Length; i++)
                {
                    Console.Write("{0} ", myArray[i]);

                }
                Console.Write("]");
                Console.WriteLine();

            }

            // a helper function to confirm your array is sorted
            // returns boolean True if the array is sorted
            static bool IsSorted(int[] a)
            {
                int j = a.Length - 1;
                if (j < 1) return true;
                int ai = a[0], i = 1;
                while (i <= j && ai <= (ai = a[i])) i++;
                return i > j;
            }



        
    }
}

