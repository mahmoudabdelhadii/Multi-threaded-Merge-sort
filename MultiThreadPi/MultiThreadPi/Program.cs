using System;

namespace MultiThreadPi
{
    class MainClass
    {
        static void Main(string[] args)
        {
            long numberOfSamples = 100000;
            long hits = 0;
            double pi = EstimatePI(numberOfSamples, ref hits);
            Console.WriteLine(" number of samples: {0}  pi pi estimation:{1}", numberOfSamples, pi);
        }

        static double EstimatePI(long numberOfSamples, ref long hits)
        {
            //implement
            //interlock counter
            double[,] samples = new double[numberOfSamples, 2];
            //double[] x_cord = new double[numberOfSamples];
            //double[] y_cord = new double[numberOfSamples];
            samples = GenerateSamples(numberOfSamples);

            //x_cord = samples[,0];
            int square_points = 0;
            double x_cord;
            double y_cord;
            for (int i = 0; i < (numberOfSamples); i++)
            {
                x_cord = samples[i , 0];
                y_cord = samples[i, 1];

                if (x_cord * x_cord + y_cord * y_cord <= 1)
                    hits++;

                //total number of 

                square_points++;
            }
            return ((4*hits)/square_points);
        }

        static double[,] GenerateSamples(long numberOfSamples)
        {
            // Implement  
            Random rand_seedx = new Random();
            Random rand_seedy = new Random();
            Random Rand_x = new Random(rand_seedx.Next());
            Random Rand_y = new Random(rand_seedy.Next());
            double[,] samples = new double[numberOfSamples,2];
            for (int i = 0; i < numberOfSamples; i++)
            {
                samples[i, 0] = Rand_x.NextDouble() * (1 -(- 1)) - 1;
                samples[i, 1] = Rand_y.NextDouble() * (1 - (-1)) - 1;
            }

            return samples;
        }
        
    }
}


