using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;


namespace MultiThreadPi2
{
    class MainClass
    {
        static void Main(string[] args)
        {
            long numberOfSamples = 1000000000;
            long hits_multi = 0;
            long hits_single = 0;
            int num_threads = 12;
            long samples_per_thread = numberOfSamples / num_threads;
            Stopwatch stopwatch_singlethread = new Stopwatch();
            Stopwatch stopwatch_multithread = new Stopwatch();
            Console.WriteLine("The number of processors " +
        "on this computer is {0}.",
        Environment.ProcessorCount);
            stopwatch_singlethread.Start();
            double pi_single = EstimatePI(numberOfSamples, ref hits_single);
            stopwatch_singlethread.Stop();
            TimeSpan ts_single = stopwatch_singlethread.Elapsed;
            Console.WriteLine(" single thread - number of samples: {0}  pi estimation:{1}", numberOfSamples, pi_single);
            string elapsedTime_single = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts_single.Hours, ts_single.Minutes, ts_single.Seconds,
                ts_single.Milliseconds / 10);
            Console.WriteLine("RunTime for Single Thread " + elapsedTime_single);

            List<Thread> threads = new List<Thread>();
            stopwatch_multithread.Start();
            for (int i = 0; i < num_threads; i++)
            { 
                Thread t = new Thread(() => EstimatePI_multithread(samples_per_thread, ref hits_multi));
                threads.Add(t);
                //t.Start();
                t.Start();
                

            }
            
            foreach(Thread t in threads)
                t.Join();

            stopwatch_multithread.Stop();
            double pi_multi = (double) (4*hits_multi)/numberOfSamples;
            

           

            TimeSpan ts_multi = stopwatch_multithread.Elapsed;
            Console.WriteLine(" Multi thread - number of samples: {0}  pi estimation:{1}", numberOfSamples, pi_multi);
            string elapsedTime_multi = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
               ts_multi.Hours, ts_multi.Minutes, ts_multi.Seconds,
               ts_multi.Milliseconds / 10);
            Console.WriteLine("RunTime for MUltiThreading" + elapsedTime_multi);
           
        }

        static double EstimatePI(long numberOfSamples, ref long hits)
        {
            //implement
            //interlock counter
            //double[,] samples = new double[numberOfSamples, 2];
            //samples = GenerateSamples(numberOfSamples);
            Random rand_seed = new Random();

            Random Rand = new Random(rand_seed.Next());
            int square_points = 0;
            double x_cord;
            double y_cord;
            for (int i = 0; i < (numberOfSamples); i++)
            {
                x_cord = Rand.NextDouble() * (1 - (-1)) - 1;
                y_cord = Rand.NextDouble() * (1 - (-1)) - 1;

                if (x_cord * x_cord + y_cord * y_cord <= 1)
                    hits++; 

                //total number of samples

                square_points++;
            }
            return ((double)(4 * hits) / square_points);
        }

        static void EstimatePI_multithread(long numberOfSamples, ref long hits)
        {
            //implement
            //interlock counter
            //double[,] samples = new double[numberOfSamples, 2];
            Random rand_seed = new Random();
            
            Random Rand = new Random(rand_seed.Next());
            
            //samples = GenerateSamples(numberOfSamples);

            double x_cord;
            double y_cord;
            

            for (int i = 0; i < (numberOfSamples); i++)
            {
                x_cord = Rand.NextDouble() * (1 - (-1)) - 1;
                y_cord = Rand.NextDouble() * (1 - (-1)) - 1;

                if (x_cord * x_cord + y_cord * y_cord <= 1)
                    Interlocked.Increment(ref hits);

            }
            //return (hits);
        }

        static double[,] GenerateSamples(long numberOfSamples)
        {
            // Implement  
            Random rand_seedx = new Random();
            Random rand_seedy = new Random();
            Random Rand_x = new Random(rand_seedx.Next());
            Random Rand_y = new Random(rand_seedy.Next());
            double[,] samples = new double[numberOfSamples, 2];
            for (int i = 0; i < numberOfSamples; i++)
            {
                samples[i, 0] = Rand_x.NextDouble() * (1 - (-1)) - 1;
                samples[i, 1] = Rand_y.NextDouble() * (1 - (-1)) - 1;
            }

            return samples;
        }

    }
}