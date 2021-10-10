using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;


namespace MergeSort
{
    public class Variables
    {
        public static int counter1;
        public static int num_arrays;
        public static int Accumulator;
        public static int ARRAY_SIZE;
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
            Variables.ARRAY_SIZE = 10000000;
            int[] arraySingleThread = new int[Variables.ARRAY_SIZE];
            Variables.workerThreads = 8;
            Random Rand = new Random();
            Stopwatch stopwatch_singlethread = new Stopwatch();
            Stopwatch stopwatch_multithread = new Stopwatch();
            int[] result_Array = new int[arraySingleThread.Length];
            Variables.Elements_per_Array = (int)Math.Ceiling((decimal)Variables.ARRAY_SIZE / (Variables.workerThreads)); //gives
            Variables.num_arrays = (int)Math.Ceiling((decimal)Variables.ARRAY_SIZE / (Variables.Elements_per_Array));
            int[][] Jagged_array = new int[Variables.num_arrays][];


            for (int i = 0; i < Variables.ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = Rand.Next((-Variables.ARRAY_SIZE), Variables.ARRAY_SIZE);
            }
            Console.WriteLine("      initial array:    __________________________________________________________________________________________");
            //PrintArray(arraySingleThread);


            // copy array by value.. You can also use array.copy()
            int[] arrayMultiThread = new int[Variables.ARRAY_SIZE];

            Array.Copy(arraySingleThread, arrayMultiThread, arraySingleThread.Length);
            //PrintArray(arrayMultiThread);

            int j;
            if (Variables.ARRAY_SIZE % Variables.workerThreads == 0 || Variables.Elements_per_Array == 1)
            {

                for (Variables.Accumulator = 0, j = 0; Variables.Accumulator < Variables.ARRAY_SIZE; Variables.Accumulator += Variables.Elements_per_Array, j++)
                {
                    Jagged_array[j] = new int[Variables.Elements_per_Array];
                    Array.Copy(arrayMultiThread, Variables.Accumulator, Jagged_array[j], 0, Variables.Elements_per_Array);

                    try
                    {
                        //PrintArray(Jagged_array[j]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }


                    //Console.WriteLine("Accumulator: {0}, Elements per array: {1}", Variables.Accumulator, Variables.Elements_per_Array);
                }
                Variables.num_threads = j;
                //Console.WriteLine("j: {0} num_threads:{1} num_arrays:{2}", j, Variables.num_threads, Variables.num_arrays);
            }
            else
            {

                for (Variables.Accumulator = 0, Variables.counter1 = 0; Variables.Accumulator <= (Variables.ARRAY_SIZE - Variables.Elements_per_Array); Variables.Accumulator += Variables.Elements_per_Array, Variables.counter1++)
                {
                    try
                    {
                        Jagged_array[Variables.counter1] = new int[Variables.Elements_per_Array];
                        Array.Copy(arrayMultiThread, Variables.Accumulator, Jagged_array[Variables.counter1], 0, Variables.Elements_per_Array);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    try
                    {
                        //    PrintArray(Jagged_array[Variables.counter1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }


                    //Console.WriteLine("Accumulator: {0}, Elements per array: {1}", Variables.Accumulator, Variables.Elements_per_Array);
                    //Console.WriteLine("Counter1: {0}", Variables.counter1);
                }

                if (Variables.ARRAY_SIZE - Variables.Accumulator > 0)
                {

                    try
                    {
                        Jagged_array[Variables.counter1] = new int[Variables.ARRAY_SIZE - Variables.Accumulator];
                        Array.Copy(arrayMultiThread, Variables.Accumulator, Jagged_array[Variables.counter1], 0, (Variables.ARRAY_SIZE - Variables.Accumulator));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    try
                    {
                        //PrintArray(Jagged_array[Variables.counter1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    //Console.WriteLine("Accumulator: {0}, size of last array: {1}", Variables.Accumulator, Jagged_array[Variables.counter1].Length);
                    Variables.counter1++;
                }
                Variables.num_threads = Variables.counter1;
                //Console.WriteLine("Counter1: {0} num_threads:{1} num_arrays:{2}", Variables.counter1, Variables.num_threads, Variables.num_arrays);
            }

            //Console.WriteLine("Counter1 ==== after else: {0}", Variables.counter1);
            stopwatch_singlethread.Start();
            arraySingleThread = MergeSort_single(arraySingleThread);
            stopwatch_singlethread.Stop();
            Console.WriteLine("Single thread array:    __________________________________________________________________________________________");
            //PrintArray(arraySingleThread);
            if (IsSorted(arraySingleThread) == true)
                Console.WriteLine("The Array is sorted");
            else
                Console.WriteLine("The Array is NOT sorted");
            TimeSpan ts_single = stopwatch_singlethread.Elapsed;

            string elapsedTime_single = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts_single.Hours, ts_single.Minutes, ts_single.Seconds,
                ts_single.Milliseconds / 10);
            Console.WriteLine("RunTime for Single Thread " + elapsedTime_single);

            stopwatch_multithread.Start();

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < Math.Min (Variables.workerThreads, Variables.num_threads); i++)
            {
                try
                {
                    //Console.WriteLine(" i:{0} ", i);
                    Thread thread1 = new Thread(() => MergeSort(Jagged_array[i]));
                    thread1.Start();
                    threads.Add(thread1);
                    //Console.WriteLine(" i2:{0} ", i);
                    thread1.Join();

                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex);
                }

            }


            int[] finale = new int[Variables.ARRAY_SIZE];
            Merge_threads(Math.Min(Variables.workerThreads,Variables.num_threads), Jagged_array);

            finale = Jagged_array[0];
            stopwatch_multithread.Stop();
            // print all the threads.
            /*
            for (int i = 0; i < Variables.workerThreads; i++)
            {
                PrintArray(Jagged_array[i]);
            }
            */
            Console.WriteLine("Final multithread array:__________________________________________________________________________________________");
            //PrintArray(Jagged_array[0]);
            if (IsSorted(finale) == true)
                Console.WriteLine("The Array is sorted");
            else
                Console.WriteLine("The Array is NOT sorted");

            //PrintArray(result_Array);

            TimeSpan ts_multi = stopwatch_multithread.Elapsed;
            string elapsedTime_multi = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
               ts_multi.Hours, ts_multi.Minutes, ts_multi.Seconds,
               ts_multi.Milliseconds / 10);
            Console.WriteLine("RunTime for MUltiThreading" + elapsedTime_multi);






        }
        /*********************** Methods **********************
         *****************************************************/
        /*
        implement Merge method. This method takes two sorted array and
        and constructs a sorted array in the size of combined arrays
        */
        static void Merge_threads(int num_arrays, int[][] input_array)
        {

            if (num_arrays <= 1)
            {

            }
            else if (num_arrays == 2)
            {
                int[] final = new int[input_array[0].Length + input_array[1].Length];
                Thread thread3 = new Thread(() => Merge(input_array[0], input_array[1], final));
                thread3.Start();
                thread3.Join();
                //PrintArray(sorted_jagged_array_2);
                Array.Resize(ref input_array[0], (input_array[0].Length + input_array[1].Length));
                input_array[0] = final;



                //PrintArray(final_array);
                Merge_threads(num_arrays / 2, input_array);


            }
            else if (num_arrays % 2 == 0 && num_arrays > 2)
            {
                List<int[]> arrayList = new List<int[]>();
                List<Thread> threads_merge = new List<Thread>();
                for (int i = 0; i < (num_arrays / 2); i++)
                {


                    int[] sorted_jagged_array = new int[input_array[i].Length + input_array[num_arrays - 1 - i].Length];
                    // Console.WriteLine(" i:{0} outside i: {1} ", i, (num_arrays - 1 - i));
                    Thread thread2 = new Thread(() => Merge(input_array[i], input_array[num_arrays - 1 - i], sorted_jagged_array));

                    thread2.Start();
                    threads_merge.Add(thread2);
                    //Console.WriteLine(" i2:{0} ", i);
                    thread2.Join();
                    Array.Resize(ref input_array[i], (input_array[i].Length + input_array[num_arrays - 1 - i].Length));
                    input_array[i] = sorted_jagged_array;
                    arrayList.Add(input_array[i]);
                }
                //arrayList.ForEach(PrintArray);
                Merge_threads(num_arrays / 2, input_array);

            }
            else
            {
                List<int[]> arrayList = new List<int[]>();
                List<Thread> threads_merge = new List<Thread>();
                for (int i = 0; i < (num_arrays / 2); i++)
                {
                    int[] sorted_jagged_array = new int[input_array[i].Length + input_array[num_arrays - 2 - i].Length];
                    //Console.WriteLine(" i:{0} outside i: {1} ", i, (num_arrays - 2 - i));
                    Thread thread2 = new Thread(() => Merge(input_array[i], input_array[num_arrays - 2 - i], sorted_jagged_array));

                    thread2.Start();
                    threads_merge.Add(thread2);
                    //Console.WriteLine(" i2:{0} ", i);
                    thread2.Join();
                    Array.Resize(ref input_array[i], (input_array[i].Length + input_array[num_arrays - 2 - i].Length));
                    input_array[i] = sorted_jagged_array;
                    arrayList.Add(input_array[i]);

                }
                int[] sorted_jagged_array_2 = new int[input_array[0].Length + input_array[num_arrays - 1].Length];
                Thread thread3 = new Thread(() => Merge(input_array[0], input_array[num_arrays - 1], sorted_jagged_array_2));
                thread3.Start();


                thread3.Join();
                //PrintArray(sorted_jagged_array_2);
                //Console.WriteLine("________________________________________________");
                Array.Resize(ref input_array[0], (input_array[0].Length + input_array[num_arrays - 1].Length));
                input_array[0] = sorted_jagged_array_2;

                arrayList.Add(input_array[0]);
                //arrayList.ForEach(PrintArray);
                Merge_threads(num_arrays / 2, input_array);
            }
        }
        static void Merge(int[] LA, int[] RA, int[] A)
        {
            int index_Right = 0;
            int index_Result = 0;
            int index_Left = 0;
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
        }

        /*
        implement MergeSort method: takes an integer array by reference
        and makes some recursive calls to intself and then sorts the array
        */

        static void MergeSort(int[] A)
        {
            //used c-sharpcorder.com as reference

            int[] left;
            int[] right;
            //int[] result = new int[A.Length];
            int a = 0;
            if (A.Length <= 1)
            {

            }
            else
            {
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

                MergeSort(left);

                MergeSort(right);


                Merge(left, right, A);
            }
        }
        static int[] Merge_single(int[] LA, int[] RA, int[] A)
        {
            int index_Right = 0;
            int index_Result = 0;
            int index_Left = 0;
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

        static int[] MergeSort_single(int[] A)
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
            else
            {
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

                left = MergeSort_single(left);

                right = MergeSort_single(right);


                Merge_single(left, right, result);
                return result;
            }

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

