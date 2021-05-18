using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace InfoBez_Elgamal
{
    class Elgamal
    {
        Random rnd = new Random();

        public List<ulong> PrimeNumbers = new List<ulong>();
        public ulong PrimesFrom = 10000;
        public ulong PrimesTo = 20000;

        public BigInteger P = 0;
        public BigInteger G = 0;

        public BigInteger X = 0;
        public BigInteger Y = 0;

        public int K;

        public BigInteger A = 0;

        public string InputText;
        public string EncryptedText;
        public string DecrypredText;

        public void Encrypt()
        {
            A = BigInteger.ModPow(G, K, P);
            var list = new List<BigInteger>();

            for (int i = 0; i < InputText.Length; i++)
            {
                var m = (byte)InputText[i];
                list.Add(BigInteger.Remainder(BigInteger.Multiply(BigInteger.Pow(Y, K), m), P));
            }

            var bytes = new byte[4 * list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                var data = BitConverter.GetBytes((ulong)list[i]);

                for (int j = 0; j < 4; j++)
                {
                    bytes[i * 4 + j] = data[j];
                }
            }

            EncryptedText = ToHex(bytes);
        }

        public void Decrypt()
        {
            DecrypredText = "";
            var bytes = FromHex(EncryptedText);
            var count = bytes.Length / 4;

            var data = new List<BigInteger>();

            for (int i = 0; i < count; i++)
            {
                var part = new byte[4];

                for (int j = 0; j < 4; j++)
                {
                    part[j] = bytes[i * 4 + j];
                }

                data.Add(new BigInteger(part));
            }

            for (int i = 0; i < data.Count; i++)
            {
                var exponent = BigInteger.Subtract(BigInteger.Subtract(P, new BigInteger(1)), X);
                var m = BigInteger.ModPow(A, exponent, P);
                var ch = BigInteger.Remainder(BigInteger.Multiply(m, data[i]), P);
                DecrypredText += ((char)ch).ToString();
            }
        }

        public void GenerateP()
        {
            if (PrimeNumbers.Count == 0)
            {
                PrimeNumbers = GeneratePrimeNumbers(PrimesFrom, PrimesTo);
            }

            P = PrimeNumbers[rnd.Next(PrimeNumbers.Count / 2, PrimeNumbers.Count - 1)];
        }

        public void GenerateG()
        {
            if (PrimeNumbers.Count == 0)
            {
                PrimeNumbers = GeneratePrimeNumbers(PrimesFrom, PrimesTo);
            }

            G = PrimeNumbers[rnd.Next(0, PrimeNumbers.Count - 1)];

            while (G > P)
            {
                G = PrimeNumbers[rnd.Next(0, PrimeNumbers.Count - 1)];
            }
        }

        public void GenerateX()
        {
            X = PrimeNumbers[rnd.Next(0, PrimeNumbers.Count - 1)];

            while (X > P)
            {
                X = PrimeNumbers[rnd.Next(0, PrimeNumbers.Count - 1)];
            }
        }

        public void CalculateY()
        {
            Y = BigInteger.ModPow(G, X, P);
        }

        public void GenerateK()
        {
            int k = (int)(P / 10);

            if (K != 0)
            {
                k = K + 1;
            }

            while (k < P - 1)
            {
                if (FindGreatestCommonDivider(k, P - 1) == 1)
                {
                    K = k;
                    return;
                }

                k++;
            }

            throw new Exception("Не удалось найти сессионный ключ, которое было бы взаимно простым с P - 1.");
        }

        public List<ulong> GeneratePrimeNumbers(ulong from, ulong to)
        {
            var numbers = new List<ulong>();
            for (ulong i = from; i < to; i++)
            {
                if (IsPrime(i))
                {
                    numbers.Add(i);
                }
            }

            return numbers;
        }

        private bool IsPrime(ulong num)
        {
            if ((num & 1) == 0) return (num == 2);

            var limit = (ulong)Math.Sqrt(num);
            for (ulong i = 3; i <= limit; i += 2)
            {
                if (num % i == 0) return false;
            }
            return true;
        }

        static BigInteger FindGreatestCommonDivider(BigInteger num1, BigInteger num2)
        {
            while (num2 != 0)
            {
                var temp = num2;
                num2 = num1 % num2;
                num1 = temp;
            }

            return num1;
        }

        public string ToHex(ICollection<byte> input)
        {
            //var bytes = Encoding.ASCII.GetBytes(input);
            var hexString = BitConverter.ToString(input.ToArray());
            hexString = hexString.Replace("-", " ");
            return hexString;
        }

        public byte[] FromHex(string input)
        {
            input = input.Replace(" ", "");
            byte[] data = new byte[input.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }

            return data;
        }
    }
}
