//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class NumberUtilities
{
    public static async Task<List<int>> CalculatePrimesAsync(int min, int max)
    {

        var array = Enumerable.Range(min, max - min);
        var primeList = await Task.Run(() =>
        {
            var primes = from n in array.AsParallel()
                         where IsPrime(n) 
                         select n;
            return primes.ToList();
        });
        
        return primeList.OrderBy(n => n).ToList();
    }

    public static bool IsPrime(int candidate)
    {
        if ((candidate & 1) == 0)
        {
            if (candidate == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        for (int i = 3; (i * i) < candidate; i += 2)
        {
            if ((candidate % i) == 0)
            {
                return false;
            }
        }
        return candidate != 1;
    }
}

