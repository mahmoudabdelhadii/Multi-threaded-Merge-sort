using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace MergeSort
{
    public class Variables
    {
        public static int counter1;
        public static int num_arrays;
        public static int Accumulator;
        public static int ARRAY_SIZE ;
        public static int Elements_per_Array;
        public static int workerThreads;
        public static int portThreads;
        //public static int[][] Jagged_array;
        public static int[][] sorted_array;
        public static int num_threads;
      

    }
    public class Program
    {
        public static void Main(string[] args)
        {
            Variables.ARRAY_SIZE = 100;
            int[] arraySingleThread = new int[Variables.ARRAY_SIZE];
            Variables.workerThreads = 10;
            Random Rand = new Random();
            Stopwatch stopwatch = new Stopwatch();
            int[] result_Array = new int[arraySingleThread.Length];
            Variables.Elements_per_Array = (int)Math.Ceiling((decimal)Variables.ARRAY_SIZE / (Variables.workerThreads)); //gives
            Variables.num_arrays = (int)Math.Ceiling((decimal)Variables.ARRAY_SIZE / (Variables.Elements_per_Array));
            int[][] Jagged_array = new int[Variables.num_arrays][];

           

            for (int i = 0; i < Variables.ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = Rand.Next((-Variables.ARRAY_SIZE), Variables.ARRAY_SIZE);
            }

            //PrintArray(arraySingleThread);
            // TODO : Use the "Random" class in a for loop to initialize an array

            // copy array by value.. You can also use array.copy()
            int[] arrayMultiThread = new int[Variables.ARRAY_SIZE];

            Array.Copy(arraySingleThread, arrayMultiThread, arraySingleThread.Length);
            PrintArray(arrayMultiThread);
            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */
            
            
            if (Variables.ARRAY_SIZE % Variables.workerThreads == 0 || Variables.Elements_per_Array == 1)
            {
                int j = 0;
                for (Variables.Accumulator = 0; Variables.Accumulator < Variables.ARRAY_SIZE; Variables.Accumulator += Variables.Elements_per_Array)
                {
                    Variables.Jagged_array[j] = new int[Variables.Elements_per_Array];
                    Array.Copy(arrayMultiThread, Variables.Accumulator, Variables.Jagged_array[j], 0, Variables.Elements_per_Array);

                    try
                    {
                        //PrintArray(Variables.Jagged_array[j]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    j++;

                    Console.WriteLine("Accumulator: {0}, Elements per array: {1}", Variables.Accumulator, Variables.Elements_per_Array);
                }
                Variables.num_threads = j;
                Console.WriteLine("Counter1: {0} num_threads:{1} num_arrays:{2}", j, Variables.num_threads, Variables.num_arrays);
            }
            else
            {
                Variables.counter1 = 0;
                for (Variables.Accumulator = 0; Variables.Accumulator <= (Variables.ARRAY_SIZE - Variables.Elements_per_Array ); Variables.Accumulator += Variables.Elements_per_Array)
                {
                    try
                    {
                        Variables.Jagged_array[Variables.counter1] = new int[Variables.Elements_per_Array];
                        Array.Copy(arrayMultiThread, Variables.Accumulator, Variables.Jagged_array[Variables.counter1], 0, Variables.Elements_per_Array);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    try
                    {
                        PrintArray(Variables.Jagged_array[Variables.counter1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    Variables.counter1++;
                    Console.WriteLine("Accumulator: {0}, Elements per array: {1}", Variables.Accumulator, Variables.Elements_per_Array);
                    Console.WriteLine("Counter1: {0}", Variables.counter1);
                }

                if (Variables.ARRAY_SIZE - Variables.Accumulator > 0)
                {

                    try
                    {
                        Variables.Jagged_array[Variables.counter1] = new int[Variables.ARRAY_SIZE - Variables.Accumulator];
                        Array.Copy(arrayMultiThread, Variables.Accumulator, Variables.Jagged_array[Variables.counter1], 0, (Variables.ARRAY_SIZE - Variables.Accumulator));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    //Variables.Jagged_array[Variables.counter1] = temp2_array;
                    //PrintArray(temp2_array);
                    try
                    {
                        PrintArray(Variables.Jagged_array[Variables.counter1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    Variables.counter1++;
                    Console.WriteLine("Accumulator: {0}, size of last array: {1}", Variables.Accumulator, Variables.Jagged_array[Variables.counter1].Length);
                }
                Variables.num_threads = Variables.counter1;
                Console.WriteLine("Counter1: {0} num_threads:{1} num_arrays:{2}", Variables.counter1, Variables.num_threads, Variables.num_arrays);
            }

            Console.WriteLine("Counter1 ==== after else: {0}", Variables.counter1);





            stopwatch.Start();

            Thread[] thread_array = new Thread[Variables.num_threads];
            for (int i = 0; i < thread_array.Length; i++)
            {
                // Start the thread with a ThreadStart.
                Console.WriteLine("length of thread array: {0}", thread_array.Length);
                thread_array[i] = new Thread(() => MergeSort(Variables.Jagged_array[i]));
                thread_array[i].Start();
            }
            // Join all the threads.
            for (int i = 0; i < thread_array.Length; i++)
            {
                thread_array[i].Join();
            }
            Console.WriteLine("DONE");
            PrintArray(Variables.Jagged_array[0]);
            PrintArray(Variables.Jagged_array[1]);
            PrintArray(Variables.Jagged_array[2]);
            PrintArray(Variables.Jagged_array[3]);
            PrintArray(Variables.Jagged_array[4]);
            PrintArray(Variables.Jagged_array[5]);
            PrintArray(Variables.Jagged_array[6]);
            PrintArray(Variables.Jagged_array[7]);
            PrintArray(Variables.Jagged_array[8]);
            PrintArray(Variables.Jagged_array[9]);

            stopwatch.Stop();
            //PrintArray(result_Array);

            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);





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
            {
                return A;
            }
            int midPoint = A.Length / 2;
            left = new int[midPoint];

            if (A.Length % 2 == 1)
                right = new int[midPoint + 1];
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
            if (myArray == null)
            {
                throw new Exception("printing array is null");
            }
            else
            {
                Console.Write("[");
                for (int i = 0; i < myArray.Length; i++)
                {
                    Console.Write("{0} ", myArray[i]);

                }
                Console.Write("]");
                Console.WriteLine();
            }
        }

        static void PrintArray_obj(object[] myArray)
        {
            if (myArray == null)
            {
                throw new Exception("printing array is null");
            }
            else
            {
                Console.Write("[");
                for (int i = 0; i < myArray.Length; i++)
                {
                    Console.Write("{0} ", myArray[i]);

                }
                Console.Write("]");
                Console.WriteLine();
            }
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

