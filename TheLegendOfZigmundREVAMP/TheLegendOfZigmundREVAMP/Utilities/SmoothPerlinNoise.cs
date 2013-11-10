using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TheLegendOfZigmundREVAMP.Utilities
{
    class SmoothPerlinNoise
    {
        private Random rand = new Random();
        private float[][] noise;

        private int octaves = 0;
        private float frequency = 0f, amplitude = 0f, persistence = 0f;

        public SmoothPerlinNoise(int width, int height, int octaves, float persistence, int seed = -1)
        {
            this.octaves = octaves;
            this.persistence = persistence;
            if (seed >= 0) rand = new Random(seed);
            noise = GeneratePerlinNoise(width, height, octaves);
        }

        public SmoothPerlinNoise(int width, int height, int seed = -1)
            : this(width, height, 7, .5f) { }

        public float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }

        public T[][] GetEmptyArray<T>(int width, int height)
        {
            T[][] image = new T[width][];

            for (int i = 0; i < width; i++)
            {
                image[i] = new T[height];
            }

            return image;
        }


        public float[][] GenerateSmoothNoise(float[][] baseNoise, int octave)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][] smoothNoise = GetEmptyArray<float>(width, height);

            int samplePeriod = 1 << octave; // calculates 2 ^ k
            float sampleFrequency = frequency == 0 ? 1.0f / samplePeriod : frequency;

            for (int i = 0; i < width; i++)
            {
                //calculate the horizontal sampling indices
                int sample_i0 = (i / samplePeriod) * samplePeriod;
                int sample_i1 = (sample_i0 + samplePeriod) % width; //wrap around
                float horizontal_blend = (i - sample_i0) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {
                    //calculate the vertical sampling indices
                    int sample_j0 = (j / samplePeriod) * samplePeriod;
                    int sample_j1 = (sample_j0 + samplePeriod) % height; //wrap around
                    float vertical_blend = (j - sample_j0) * sampleFrequency;

                    //blend the top two corners
                    float top = Interpolate(baseNoise[sample_i0][sample_j0],
                        baseNoise[sample_i1][sample_j0], horizontal_blend);

                    //blend the bottom two corners
                    float bottom = Interpolate(baseNoise[sample_i0][sample_j1],
                        baseNoise[sample_i1][sample_j1], horizontal_blend);

                    //final blend
                    smoothNoise[i][j] = Interpolate(top, bottom, vertical_blend);
                }
            }

            return smoothNoise;
        }

        public float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][][] smoothNoise = new float[octaveCount][][]; //an array of 2D arrays containing

            float persistance = persistence == 0 ? 0.7f : persistence;

            //generate smooth noise
            for (int i = 0; i < octaveCount; i++)
            {
                smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
            }

            float[][] perlinNoise = GetEmptyArray<float>(width, height); //an array of floats initialised to 0

            float amplitude = this.amplitude == 0 ? 1.0f : this.amplitude;
            float totalAmplitude = 0.0f;

            //blend noise together
            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
                    }
                }
            }

            //normalisation
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i][j] /= totalAmplitude;
                }
            }

            return perlinNoise;
        }

        public float[][] GeneratePerlinNoise(int width, int height, int octaveCount)
        {
            float[][] baseNoise = GenerateWhiteNoise(width, height);

            return GeneratePerlinNoise(baseNoise, octaveCount);
        }

        public float[][] GenerateWhiteNoise(int width, int height)
        {
            float[][] image = GetEmptyArray<float>(width, height);
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image[i].Length; j++)
                {
                    image[i][j] = (float)rand.NextDouble();
                }
            }
            return image;
        }

        public float GetNoise(int x, int y)
        {
            return noise[y][x];
        }
    }
}
