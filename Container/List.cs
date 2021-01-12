using System;

namespace Container
{
    public class List
    {
        int[] elements;
        int minSize;

        public List(int MinimumSize = 16)
        {
            elements = new int[MinimumSize];
            minSize = MinimumSize;
        }

        public int Capacity
        {
            get => elements.Length;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                minSize = value;
                resize(value);
            }
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public int Count
        {
            get { return count; }
        }
        private int count;

        public void Add(int v)
        {
            if (elements.Length == count)
                resize(elements.Length * 2);
            elements[count] = v;
            count++;
        }

        private void resize(int v)
        {
            if (v < count) throw new InsufficientMemoryException();
            Array.Resize(ref elements, v);
        }

        public void Remove(int v)
        {
            if (v >= count) throw new IndexOutOfRangeException();
            for (int counter = v; counter < count-1; counter++)
            {
                elements[counter] = elements[counter + 1];
            }
            if (count * 3 < Capacity && Capacity > minSize) resize(Capacity / 2);
            count--;
        }

        public int At(int v)
        {
            return elements[v];
        }

        public void ForEach(Action<int> p)
        {
            for (int i = 0; i < count; i++)
            {
                p(elements[i]);
            }
        }
    }
}
