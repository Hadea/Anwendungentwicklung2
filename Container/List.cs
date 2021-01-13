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
            if (v >= count || v < 0) throw new IndexOutOfRangeException();
            for (int counter = v; counter < count - 1; counter++)
            {
                elements[counter] = elements[counter + 1];
            }
            if (count * 3 < Capacity && Capacity > minSize) resize(Capacity / 2);
            count--;
        }

        public int At(int v)
        {
            if (v >= count || v < 0) throw new IndexOutOfRangeException();
            return elements[v];
        }

        public void ForEach(Action<int> p)
        {
            for (int i = 0; i < count; i++)
            {
                p(elements[i]);
            }
        }

        public int this[int index]
        {
            get
            {
                if (index >= count || index < 0) throw new IndexOutOfRangeException();
                return elements[index];
            }
            set
            {
                if (index >= count || index < 0) throw new IndexOutOfRangeException();
                elements[index] = value;
            }
        }

        public static List operator +(List firstList, List secondList)
        {
            if (firstList == null || secondList == null) throw new ArgumentNullException();
            List resultList = new(firstList.Count + secondList.Count);
            firstList.ForEach(resultList.Add);
            secondList.ForEach(resultList.Add);
            return resultList;
        }

        public static List operator !(List listToInvert)
        {
            List resultList = new(listToInvert.count);
            for (int counter = listToInvert.Count - 1; counter >= 0; counter--)
                resultList.Add(listToInvert[counter]);
            return resultList;
        }

        public static List Merge(params List[] lists)
        {
            int newLegth = 0;
            foreach (var item in lists) newLegth += item.Count;
            List resultList = new(newLegth);
            foreach (var item in lists) item.ForEach(resultList.Add);
            return resultList;
        }
    }
}
