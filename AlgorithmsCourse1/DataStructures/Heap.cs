using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1.DataStructures
{
    /// <summary>
    /// Implementation of a Heap (priority queue) data structure.
    /// </summary>
    /// <remarks>
    /// Known issues: if you try to insert an object with a value that was alreay inserted into heap that will throw an exception.
    /// While this is not a critical issue if you are using a heap of reference type objects, it's not good if you are using 
    /// a heap of value type objects.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    class Heap<T> where T : IComparable<T>
    {
        private bool maxHeap;
        private List<T> data = new List<T>();
        private Dictionary<T, int> dataDictionary = new Dictionary<T, int>(); // to support Delete in O(logn) time

        public int Count
        {
            get { return data.Count; }
        }

        public Heap(bool maxHeap)
        {
            this.maxHeap = maxHeap;
        }

        public void Insert(T insertElement)
        {
            data.Add(insertElement);
            dataDictionary.Add(data[data.Count - 1], data.Count - 1);
            Heappify(data.Count - 1);
        }

        public T ExtractRoot()
        {
            if (data.Count == 0)
                throw new Exception("Failed to return a root. Heap is empty.");

            T returnValue = data[0];

            Swap(0, data.Count - 1);

            dataDictionary.Remove(data[data.Count - 1]);
            data.RemoveAt(data.Count - 1);

            if(data.Count != 0)
                Heappify(0);

            return returnValue;
        }

        public T PeekRoot()
        {
            if (data.Count == 0)
                throw new Exception("Failed to return a root. Heap is empty.");

            return data[0];
        }

        public void Delete(T deleteElement)
        {
            if(data.Count == 0)
                throw  new Exception("Failed to delete an element. The heap is empty.");

            int deleteIndex = dataDictionary[deleteElement];

            Swap(deleteIndex, data.Count - 1);

            dataDictionary.Remove(data[data.Count - 1]);
            data.RemoveAt(data.Count - 1);

            if(deleteIndex != data.Count)
                Heappify(deleteIndex);
        }

        public bool Contains(T element)
        {
            return dataDictionary.ContainsKey(element);
        }

        private int Heappify(int index)
        {
            if(index >= data.Count || index < 0)
                throw  new ArgumentOutOfRangeException("index");

            if (index != 0)
            {
                int parentIndex = (index + 1) / 2 - 1;
                if (data[index].CompareTo(data[parentIndex]) == (maxHeap ? 1 : -1))
                {
                   Swap(index, parentIndex);
                   return Heappify(parentIndex);
                }
            }

            int firstChildIndex = (index + 1)*2 - 1;
            int secondChildIndex = (index + 1)*2;

            int selectedChildIndex = - 1;
            if (firstChildIndex < data.Count && secondChildIndex < data.Count)
            {
                selectedChildIndex = (!maxHeap && data[firstChildIndex].CompareTo(data[secondChildIndex]) == -1)
                                     || (maxHeap && data[firstChildIndex].CompareTo(data[secondChildIndex]) == 1)
                                         ? firstChildIndex : secondChildIndex;
            }
            else if (firstChildIndex > data.Count && secondChildIndex <= data.Count)
            {
                selectedChildIndex = secondChildIndex;
            }
            else if (firstChildIndex < data.Count && secondChildIndex >= data.Count)
            {
                selectedChildIndex = firstChildIndex;
            }

            if (selectedChildIndex != - 1 && data[index].CompareTo(data[selectedChildIndex]) == (maxHeap ? -1 : 1))
            {
                Swap(index, selectedChildIndex);
                return Heappify(selectedChildIndex);
            }

            return index;
        }

        private void Swap(int index1, int index2)
        {
            if (index1 == index2)
                return;

            dataDictionary.Remove(data[index1]);
            dataDictionary.Remove(data[index2]);

            T temp = data[index1];
            data[index1] = data[index2];
            data[index2] = temp;

            dataDictionary.Add(data[index1], index1);
            dataDictionary.Add(data[index2], index2);
        }
    }
}
