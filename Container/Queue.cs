using System;

namespace Container
{
    public class Queue
    {
        int elementCount = 0;
        int[] elements;
        int pushIndex = 0;
        int popIndex = 0;
        int minimumSize = 0;

        public int Capacity
        {
            get => elements.Length;
            set
            {
                minimumSize = value;
                resize(value);
            }
        }

        private void resize(int value)
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

            popIndex = 0;
            pushIndex = elementCount;
            elements = biggerArray;
        }

        public Queue(int InitialCapacity = 20)
        {
            elements = new int[InitialCapacity];
            minimumSize = InitialCapacity;
        }

        public bool IsEmpty()
        {
            return elementCount == 0;
        }

        public void Push(int v)
        {
            if (elements.Length == elementCount)
                resize(Capacity * 2);
            elementCount++;
            if (pushIndex == elements.Length) pushIndex = 0;
            elements[pushIndex] = v;
            pushIndex++;
        }

        public int Pop()
        {
            if (IsEmpty()) throw new IndexOutOfRangeException();
            if (elementCount < Capacity / 3 && Capacity > minimumSize) resize(Capacity / 2);
            elementCount--;
            if (popIndex == elements.Length) popIndex = 0;
            return elements[popIndex++];
        }

        public void ForEach(Action<int> Method)
        {
            while (elementCount > 0)
            {
                Method(Pop());
            }
        }
    }
}
