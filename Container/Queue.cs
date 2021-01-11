using System;

namespace Container
{
    public class Queue
    {
        int elementCount = 0;
        int[] elements;
        int pushIndex = 0;
        int popIndex = 0;
        
        public int Capacity
        {
            get => elements.Length;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                if (value < elementCount) throw new InsufficientMemoryException();
                int[] biggerArray = new int[value];

                int readIndex = popIndex;
                int writeIndex = 0;

                for (int counter = 0; counter < elementCount; counter++)
                {
                    if (readIndex == elements.Length)
                    {
                        readIndex = 0;
                    }
                    biggerArray[writeIndex++] = elements[readIndex++];
                }
                
                //Array.Copy(elements, popIndex, biggerArray, 0, elements.Length - popIndex);
                //Array.Copy(elements, 0, biggerArray, elements.Length - popIndex, popIndex);
                popIndex = 0;
                pushIndex = elementCount;
                elements = biggerArray;
            }
        }

        public Queue(int InitialCapacity = 20)
        {
            elements = new int[InitialCapacity];
        }

        public bool IsEmpty()
        {
            return elementCount == 0;
        }

        public void Push(int v)
        {
            if (elements.Length == elementCount)
                Capacity *= 2;
            elementCount++;
            if (pushIndex == elements.Length) pushIndex = 0;
            elements[pushIndex] = v;
            pushIndex++;
        }

        public int Pop()
        {
            if (IsEmpty()) throw new IndexOutOfRangeException();
            //TODO : checken ob das array verkleinert werden kann
            if (elementCount < Capacity / 3) Capacity /= 2;
            elementCount--;
            if (popIndex == elements.Length) popIndex = 0;
            return elements[popIndex++];
        }
    }
}
