using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;


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
            Variables.ARRAY_SIZE = 25;
            int[] arraySingleThread = new int[Variables.ARRAY_SIZE];
            Variables.workerThreads = 5;
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

            int j;
            if (Variables.ARRAY_SIZE % Variables.workerThreads == 0 || Variables.Elements_per_Array == 1)
            {
                
                for (Variables.Accumulator = 0,  j= 0; Variables.Accumulator < Variables.ARRAY_SIZE; Variables.Accumulator += Variables.Elements_per_Array,j++)
                {
                    Jagged_array[j] = new int[Variables.Elements_per_Array];
                    Array.Copy(arrayMultiThread, Variables.Accumulator, Jagged_array[j], 0, Variables.Elements_per_Array);

                    try
                    {
                        PrintArray(Jagged_array[j]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    

                    Console.WriteLine("Accumulator: {0}, Elements per array: {1}", Variables.Accumulator, Variables.Elements_per_Array);
                }
                Variables.num_threads = j;
                Console.WriteLine("j: {0} num_threads:{1} num_arrays:{2}", j, Variables.num_threads, Variables.num_arrays);
            }
            else
            {
                
                for (Variables.Accumulator = 0, Variables.counter1 = 0; Variables.Accumulator <= (Variables.ARRAY_SIZE - Variables.Elements_per_Array ); Variables.Accumulator += Variables.Elements_per_Array, Variables.counter1++)
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
                        PrintArray(Jagged_array[Variables.counter1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    
                    Console.WriteLine("Accumulator: {0}, Elements per array: {1}", Variables.Accumulator, Variables.Elements_per_Array);
                    Console.WriteLine("Counter1: {0}", Variables.counter1);
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
                        PrintArray(Jagged_array[Variables.counter1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
                    Console.WriteLine("Accumulator: {0}, size of last array: {1}", Variables.Accumulator, Jagged_array[Variables.counter1].Length);
                    Variables.counter1++;
                }
                Variables.num_threads = Variables.counter1 ;
                Console.WriteLine("Counter1: {0} num_threads:{1} num_arrays:{2}", Variables.counter1, Variables.num_threads, Variables.num_arrays);
            }

            Console.WriteLine("Counter1 ==== after else: {0}", Variables.counter1);

            // Enumerate list
            



            stopwatch.Start();

            //Thread[] thread_array = new Thread[Variables.workerThreads];

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < Variables.workerThreads; i++)
            {
                // Start the thread with a ThreadStart.
                //Console.WriteLine(" index:{0} length of thread array: {1}", i, thread_array.Length);
                try
                {
                    Console.WriteLine(" i:{0} ", i);
                    Thread thread1 = new Thread(() => MergeSort(Jagged_array[i]));
                    thread1.Start();
                    threads.Add(thread1);
                    Console.WriteLine(" i2:{0} ", i);
                    thread1.Join();

                }
                catch (IndexOutOfRangeException ex )
                {
                    Console.WriteLine(ex);
                }
                
            }
            // Join all the threads.
            for (int i = 0; i < Variables.workerThreads; i++)
             {
                PrintArray(Jagged_array[i]);
             }
            /*
            //int[][] Jagged_array_2 = new int[(int)Math.Ceiling((double)Variables.num_threads/2)][];
            List<int[]> arrayList = new List<int[]>();
            List<Thread> threads_merge = new List<Thread>();
            for (int i = 0; i < (int)Math.Ceiling((double)Variables.num_threads / 2); i++)
            {
                
                // Start the thread with a ThreadStart.
                //Console.WriteLine(" index:{0} length of thread array: {1}", i, thread_array.Length);
                    int[] sorted_jagged_array = new int[Jagged_array[i].Length + Jagged_array[Variables.workerThreads -1 - i].Length];
                    Console.WriteLine(" i:{0} outside i: {1} ", i, (Variables.workerThreads-1 - i));
                    Thread thread2 = new Thread(() => Merge(Jagged_array[i], Jagged_array[Variables.workerThreads - 1 - i], sorted_jagged_array));
                    
                    thread2.Start();
                    threads_merge.Add(thread2);
                    Console.WriteLine(" i2:{0} ", i);
                    thread2.Join();
                    Array.Resize(ref Jagged_array[i], (Jagged_array[i].Length + Jagged_array[Variables.workerThreads - 1 - i].Length));
                    Jagged_array[i] = sorted_jagged_array;
                    arrayList.Add(Jagged_array[i]);
            }
            */

            //arrayList.ForEach(PrintArray);
            int[] finale = new int[Variables.ARRAY_SIZE];
            Merge_threads(Variables.workerThreads, Jagged_array);
            Console.WriteLine("Final array:___________________");
            finale = Jagged_array[0];
            PrintArray(Jagged_array[0]);
            if (IsSorted(finale) == true)
                Console.WriteLine("The Array is sorted");
            else
                Console.WriteLine("The Array is NOT sorted");
            
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
            else if (num_arrays %2 ==0 && num_arrays > 2)
            {
                List<int[]> arrayList = new List<int[]>();
                List<Thread> threads_merge = new List<Thread>();
                for (int i = 0; i < (num_arrays / 2); i++)
                {

                    // Start the thread with a ThreadStart.
                    //Console.WriteLine(" index:{0} length of thread array: {1}", i, thread_array.Length);
                    int[] sorted_jagged_array = new int[input_array[i].Length + input_array[num_arrays - 1 - i].Length];
                    Console.WriteLine(" i:{0} outside i: {1} ", i, (num_arrays - 1 - i));
                    Thread thread2 = new Thread(() => Merge(input_array[i], input_array[num_arrays-1 - i], sorted_jagged_array));
                    
                    thread2.Start();
                    threads_merge.Add(thread2);
                    Console.WriteLine(" i2:{0} ", i);
                    thread2.Join();
                    Array.Resize(ref input_array[i], (input_array[i].Length + input_array[num_arrays - 1 - i].Length));
                    input_array[i] = sorted_jagged_array;
                    arrayList.Add(input_array[i]);
                }
                arrayList.ForEach(PrintArray);
                Merge_threads(num_arrays / 2, input_array);

            }
            else
            {
                List<int[]> arrayList = new List<int[]>();
                List<Thread> threads_merge = new List<Thread>();
                for (int i = 0; i < (num_arrays / 2); i++)
                {

                    // Start the thread with a ThreadStart.
                    //Console.WriteLine(" index:{0} length of thread array: {1}", i, thread_array.Length);
                    int[] sorted_jagged_array = new int[input_array[i].Length + input_array[num_arrays - 2 - i].Length];
                    Console.WriteLine(" i:{0} outside i: {1} ", i, (num_arrays - 2 - i));
                    Thread thread2 = new Thread(() => Merge(input_array[i], input_array[num_arrays - 2 - i], sorted_jagged_array));
                    
                    thread2.Start();
                    threads_merge.Add(thread2);
                    Console.WriteLine(" i2:{0} ", i);
                    thread2.Join();
                    Array.Resize(ref input_array[i], (input_array[i].Length + input_array[num_arrays - 2 - i].Length));
                    input_array[i] = sorted_jagged_array;
                    arrayList.Add(input_array[i]);

                }
                int[] sorted_jagged_array_2 = new int[input_array[0].Length + input_array[num_arrays - 1].Length];
                //Merge(input_array[num_arrays-2],input_array[num_arrays - 1],sorted_jagged_array_2);
                Thread thread3 = new Thread(() => Merge(input_array[0], input_array[num_arrays - 1], sorted_jagged_array_2));
                thread3.Start();
                
                
                thread3.Join();
                //PrintArray(sorted_jagged_array_2);
                Console.WriteLine("________________________________________________");
                Array.Resize(ref input_array[0], (input_array[0].Length + input_array[num_arrays - 1].Length));
                input_array[0] = sorted_jagged_array_2;
                
                arrayList.Add(input_array[0]);
                arrayList.ForEach(PrintArray);
                //Console.WriteLine("________________________________________________");
                Merge_threads(num_arrays / 2, input_array);
            }
        }
        static void Merge(int[] LA, int[] RA, int[] A)
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
            //return A;
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
            int[] result = new int[A.Length];
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

