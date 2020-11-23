using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Identity
{
    public class ClientSet :IClient
    {
       private readonly int numberOfBuckets;
       private readonly int bucketSize;
       private Client[][] Clients;
       public ClientSet(in int bucketCount, in int sizeofBuckets) 
       {
           numberOfBuckets = bucketCount;
           bucketSize = sizeofBuckets;
           Clients = new Client [bucketCount][];
           for (int i = 0; i < Clients.Length; i++)
           {
               Clients[i] = new Client[bucketSize];
           }
       }
       private string BinarySearch(Client clients) 
       {
           for (int i = 0; i < numberOfBuckets; i++) {
               var bucket = Clients[i];
               int max = bucket.Length - 1;
               int min = 0;
               while (min <= max) {
                   int medium = (min + max) / 2;
                   if (bucket[medium] != null)
                   {
                       if (bucket[medium].GetHashCode() == clients.GetHashCode())
                            return $"{i},{medium}";
                        else if (clients.GetHashCode() < bucket[medium].GetHashCode())
                            max = medium - 1;
                        else 
                            min = medium + 1;
                   }
                   else {
                       max = medium - 1;
                   }
               }
           }
           return null;
       }
       public bool Remove(Client p)
       {
           if (Contains(p)) 
           {
               string[] tokens = BinarySearch(p).Split(',');
               int bucketPosition = int.Parse(tokens[0]);
               int positionInBucket = int.Parse(tokens[0]);
               var tmp = CloneBucketArray(Clients[bucketPosition], positionInBucket + 1);
               Clients[bucketPosition][positionInBucket] = null;
               Clients[bucketPosition] = MergeBucketArray(tmp, Clients[bucketPosition], positionInBucket);
               return true;
           }
           return false;
       }
       public bool Contains(Client Client)
       {
        if (BinarySearch(Client) != null)
            return true;
        return false;
        }

        public bool Replace(Client pOld, Client pNew) 
        {
            if (Contains(pOld))
            {
                string[] tokens = BinarySearch(pOld).Split(',');
                Clients[int.Parse(tokens[0])][int.Parse(tokens[1])] = pNew;
                return true;
            }
            return false;
        }
        public Client Get(Client p)
        {
            if (Contains(p))
            {
                return GetClientFromBinarySearch(p);
            }
            return null;
        }
        private Client GetClientFromBinarySearch(Client p)
        {
            string[]tokens = BinarySearch(p).Split(',');
            return Clients [int.Parse(tokens[0])][int.Parse(tokens[1])];
        }
        public Client[] ToSortedArray() {
            Client[][] buckets = new Client[Clients.Length][];
            Client[] newClientArray = new Client[bucketSize * numberOfBuckets];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = CloneBucketArray(Clients[i], 0);
                var shrinkFrom = buckets[i].Length - GetNullValuesOrElementsToRightInBucket(Clients[i]);
                if (buckets[i].Length != shrinkFrom)
                    if(shrinkFrom == 0)
                        buckets[i] = null;
                    else 
                        buckets[i]= ShrinkBucket(Clients[i], shrinkFrom);
                newClientArray = MergeBucketArray(buckets[i], newClientArray);
            }
            return ShrinkBucket(newClientArray);
        }
        public bool Add(Client Client){
            bool isSaved = false;
            if (Contains(Client))
                return isSaved;
            int bucketPosition = Client.GetHashCode() % numberOfBuckets;
            int position = 0;
            bool isPositionNull = false, isArrayEmpty = true, isArrayFull = false;
            Client[] tmp = new Client[bucketSize];
            Client[] bucket = Clients[bucketPosition];
            for (int i = 0; i < bucketSize; i++) {
                if (bucket[i] != null) {
                    isArrayEmpty = false;
                    if (Client.CompareTo(Clients[bucketPosition][i]) < 0)
                    {
                        position = i;
                        isArrayFull = false;
                        break;
                    }
                    else if (Client.CompareTo(Clients[bucketPosition][i]) > 0 && ((bucket.Length - 1 ) >= i + 1 && Clients[bucketPosition][i + 1] == null)) {
         
                        position = i + 1;
                        isPositionNull = true;
                        isArrayFull = false;
                        break;
                    }
                    else if (Client.CompareTo(Clients[bucketPosition][i]) > 0 && ((bucket.Length - 1) >= i + 1 && Clients[bucketPosition][i + 1] != null))
                    {
                        isArrayFull = true;
                    }
                }
            }
            if ((!isPositionNull) && (!isArrayEmpty)) 
            {
                tmp = CloneBucketArray(bucket, position);
                bucket[position] = Client;
                bucket = MergeBucketArray(tmp, bucket, position + 1);
                Clients[bucketPosition] = bucket;
            }
            else if (isArrayFull)
            {
                var oldLength = bucket.Length;
                bucket = ExtendBucket(bucket);
                bucket[oldLength] = Client;
            }
            else 
            {
                Clients[bucketPosition][position] = Client;
            }
            isSaved = true;
            return isSaved;
        }
        public static Client[] ExtendBucket(Client[] people)
        {
            var oldBucketSize = people.Length;
            Client[] tmp = new Client[(int)(oldBucketSize * 1.5)];
            for (int i = 0; i < oldBucketSize; i++)
            {
                tmp[i] = people[i];
            }
            return tmp;
        }
        internal static Client[] ShrinkBucket(Client[] people, int? startShrinkIndex = null)
        {
            var newBucketSize = people.Length - GetNullValuesOrElementsToRightInBucket(people, startShrinkIndex);
            Client[] tmp = new Client[newBucketSize];
            for (int i = 0; i < newBucketSize; i++)
            {
                if (startShrinkIndex.HasValue)
                {
                    if (i > startShrinkIndex)
                        break;
                    tmp[i] = people[i];
                }
                else
                {
                    if (people[i] == null)
                        break;
                    tmp[i] = people[i];
                }
            }
            return tmp;
        }
        internal static int GetNullValuesOrElementsToRightInBucket (Client [] people, int? startPosition = null) {
            int availableSpaces = 0;
            for (int i = people.Length - 1; i >= 0; i--)
            {
                 if (startPosition.HasValue)
                {
                    if (i >= startPosition)
                        availableSpaces++;
                }
                else
                {
                    if (people[i] == null)
                        availableSpaces++;
                }
            }
            return availableSpaces;
        }
         internal static Client[] CloneBucketArray(Client[] people, int startPosition)
        {
            Client[] tmpArray = new Client[people.Length];
            int cont = 0;
            for (int i = 0; i < people.Length; i++)
            {
                if (i >= startPosition && people[i] != null)
                {
                    tmpArray[cont] = people[i];
                    cont++;
                }
            }
            return tmpArray;
        }
        internal Client[] MergeBucketArray(Client[] fromArray, Client[] toArray, int? destinationIndex = null) {
            if (fromArray != null)
            {
                if (!destinationIndex.HasValue)
                    destinationIndex = toArray.Length - GetNullValuesOrElementsToRightInBucket(toArray, destinationIndex); 
                while (true)
                {
                    var availableSpaces = GetNullValuesOrElementsToRightInBucket(toArray, destinationIndex);
                    var ClientCount = fromArray.Length - GetNullValuesOrElementsToRightInBucket(fromArray);
                    if (availableSpaces < ClientCount)
                    {
                        toArray = ExtendBucket(toArray);
                    }
                    else
                        break;
                }
                int cont = 0;
                for (int i = destinationIndex.Value; i < toArray.Length; i++)
                {
                    if (cont < fromArray.Length)
                    {
                        toArray[i] = fromArray[cont];
                        cont++;
                    }
                }
            }
            return toArray;
        }
    }
}