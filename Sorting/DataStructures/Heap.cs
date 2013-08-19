using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1.DataStructures
{
    class Heap<T> where T : IComparable
    {
        private List<T> data = new List<T>();
        private Dictionary<T, int> dataDictionary = new Dictionary<T, int>();  
 
        public void Insert(T insertElement)
        {
            data.Add(insertElement);
            int addedIndex = data.Count - 1;
            int newIndex = Heappify(addedIndex);
            dataDictionary.Add(insertElement, newIndex);
        }

        public T ExtractMin()
        {
            T returnValue = data[0];
            dataDictionary.Remove(data[0]);
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            

            Heappify(0);

            return returnValue;
        }

        public void Delete(T deleteElement) // TODO: check that there are enough elements
        {
            int deleteIndex = dataDictionary[deleteElement];
            dataDictionary.Remove(deleteElement);
            data[deleteIndex] = data[data.Count - 1];
            int newIndex = Heappify(deleteIndex);
            dataDictionary[data[data.Count - 1]] = newIndex;
        }

        private int Heappify(int index) //TODO: update dataDictionary
        {
            if(index > data.Count || index < 0)
                throw  new ArgumentOutOfRangeException("index");


            if (index != 0)
            {
                int parentIndex = (index + 1) / 2 - 1;
                if (data[index].CompareTo(data[parentIndex]) == -1) // this element is less than parent
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
                selectedChildIndex = data[firstChildIndex].CompareTo(data[secondChildIndex]) == -1
                                         ? firstChildIndex : secondChildIndex;
            }
            else if (firstChildIndex > data.Count && secondChildIndex < data.Count)
            {
                selectedChildIndex = secondChildIndex;
            }
            else if (firstChildIndex < data.Count && secondChildIndex > data.Count)
            {
                selectedChildIndex = firstChildIndex;
            }

            if (selectedChildIndex != - 1 && data[index].CompareTo(data[selectedChildIndex]) == 1)
            {
                Swap(index, selectedChildIndex);
                return Heappify(selectedChildIndex);
            }

            return index;
        }

        private void Swap(int index1, int index2)
        {
            T temp = data[index1];
            data[index1] = data[index2];
            data[index2] = temp;
        }
    }
}
