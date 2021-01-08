using System;

namespace Container
{
    public class Stack
    {
        uint elementCounter = 0;
        int[] elements;

        public Stack()
        {
            elements = new int[5];
        }

        public Stack(int Capacity)
        {
            elements = new int[Capacity];
        }

        public bool IsEmpty()
        {
            return elementCounter == 0;
        }

        public void Push(int v)
        {
            if (elementCounter == elements.Length)
            {
                //int[] newArray = new int[elements.Length *2];
                //for (int i = 0; i < elements.Length; i++)
                //{
                //    newArray[i] = elements[i];
                //}
                //elements = newArray;
                Array.Resize(ref elements, elements.Length * 2);
            }
            elements[elementCounter++] = v;
        }

        public int Pop()
        {
            return elements[--elementCounter];
        }
    }
}
