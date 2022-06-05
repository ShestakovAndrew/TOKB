﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Tests.Statistical
{
    internal class IntervalTest : IStatisticalTest
    {
        private double resultTest;

        private struct Interval
        {
            public UInt16 alpha;
            public UInt16 betta;
        }

        public IntervalTest(string sequenceNumber)
        {
            resultTest = Сomputation(sequenceNumber);
        }

        public Result GetEnumResultTest()
        {
            if (resultTest <= 0.2) return Result.Badly;
            if (resultTest > 0.2 && resultTest <= 0.7) return Result.Good;
            if (resultTest > 0.7) return Result.Great;
            return Result.None;
        }

        public double GetValueResultTest()
        {
            return resultTest;
        }

        public double Сomputation(string sequenceNumber)
        {
            Interval interval; interval.alpha = 0; interval.betta = 1;
            int freedomDegree = 10;

            List<int> sequenceIntervalLengths = GetSequenceIntervalLengths(TestTools.DecimalToBinary(sequenceNumber));
            Dictionary<int, int> counterIntervals = GetCounterIntervals(sequenceIntervalLengths, freedomDegree);
            return alglib.chisquarecdistribution(freedomDegree, ChiSquareIntervals(counterIntervals));
        }

        private List<int> GetSequenceIntervalLengths(string bitSequence)
        {
            List<int> result = new List<int>();
            int counter = 1;
            for (int i = 0; i != bitSequence.Length; i++)
            {
                if (i == 0) continue;
                if (bitSequence[i - 1] != bitSequence[i])
                {
                    result.Add((counter - 2));
                    counter = 1;
                    continue;
                }
                counter++;
                if ((i + 1) == bitSequence.Length) result.Add((counter - 2));
            }
            return result;
        }

        private Dictionary<int, int> GetCounterIntervals(List<int> sequenceIntervalLengths, int freedomDegree)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            for (int i = 0; i != freedomDegree; i++) result[i] = 0;
            foreach (int num in sequenceIntervalLengths)
            {
                if (num <= 0)
                {
                    result[0]++;
                    continue;
                }
                result[num]++;
            }
            return result;
        }

        private double ChiSquareIntervals(Dictionary<int, int> counterIntervals)
        {
            double result = 0.0;
            double totalNumberIntervals = 0.0;

            foreach (var intervalLength in counterIntervals) totalNumberIntervals += intervalLength.Value;
            foreach (var intervalLength in counterIntervals)
	        {
                double first = 1.5;
                double second = totalNumberIntervals * first;
                double third = Math.Pow(1.0 - first, intervalLength.Key);
                double four = second * third;
                result += (Math.Pow((intervalLength.Value - four), 2.0)) / four;
            }
            return Math.Abs(result);
        }
    }
}