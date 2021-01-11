using System;

namespace Container
{
    public class Queue
    {
        int count = 0;
        int[] elements;
        int pushIndex=0;
        int popIndex=0;

        public int Capacity { get => elements.Length; }

        public Queue(int InitialCapacity = 20)
        {
            elements = new int[InitialCapacity];
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public void Push(int v)
        {
            if (elements.Length == count)
                resize(elements.Length * 2);
            count++;
            if (pushIndex == elements.Length) pushIndex = 0;
            elements[pushIndex] = v;
            pushIndex++;
        }

        private void resize(int v)
        {
            int[] biggerArray = new int[v];
            Array.Copy(elements, popIndex, biggerArray,0 , elements.Length - popIndex);
            Array.Copy(elements,0, biggerArray, elements.Length - popIndex, popIndex);
            popIndex = 0;
            pushIndex = elements.Length;
            elements = biggerArray;
        }

        public int Pop()
        {
            if (IsEmpty()) throw new IndexOutOfRangeException();
            //TODO : checken ob das array verkleinert werden kann
            //if (count < Capacity / 2) resize(Capacity / 2);
            count--;
            if (popIndex == elements.Length) popIndex = 0;
            return elements[popIndex++];
        }
    }
}
