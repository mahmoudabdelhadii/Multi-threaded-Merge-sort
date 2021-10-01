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
            int[] LA =new int []{1,3,5,7,9,11,13,15};
            int[] RA = new int []{2,4,6,8,10,12,14,16};
            int[] result_Array = new int[arraySingleThread.Length];


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
            result_Array = MergeSort(arraySingleThread);
            PrintArray(result_Array);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);


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
                        A[index_Result] = RA[index_Right];
                        index_Result++;
                        index_Right++;
                        
                    }
                    
                    else
                    {
                        A[index_Result] = LA[index_Left];
                        index_Result++;
                        index_Left++;
                    }
                }
                 else if (index_Right < RA.Length)
                {
                    A[index_Result] = RA[index_Right];
                    index_Result++;
                    index_Right++;
                }  
                else if (index_Left < LA.Length)
                {
                    A[index_Result] = LA[index_Left];
                    index_Result++;
                    index_Left++;
                }
            }
            return A;
            }

            /*
            implement MergeSort method: takes an integer array by reference
            and makes some recursive calls to intself and then sorts the array
            */
            
            static int[] MergeSort(int[] A)
            {
            //used c-sharpcorder.com as reference
            int[] left;
            int[] right;
            int[] result = new int[A.Length];
            int a = 0;
            if (A.Length <= 1)
                return A;

            int midPoint = A.Length / 2;
            left = new int[midPoint];

            if (A.Length % 2 == 1)
                right = new int[midPoint+1];
            else
                right = new int[midPoint];
          
            for (int i = midPoint; i < A.Length; i++)
            {
                right[a] = A[i];
               a++;
            }
            for (int i = 0; i < midPoint; i++)
                left[i] = A[i];
            
            left = MergeSort(left);
            
            right = MergeSort(right);

            Merge(left, right, result);
            return result;
        }
            
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

